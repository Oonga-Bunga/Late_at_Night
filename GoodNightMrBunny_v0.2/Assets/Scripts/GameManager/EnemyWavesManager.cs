using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyWaveData
{
    public List<EnemyWave> EnemyWaves;
}

[System.Serializable]
public class EnemyWave
{
    public bool NeedsEnemiesKilled;
    public float TimeDelay;
    public float TimePerEnemy;
    public int NZanybell;
    public int NEvilBunny;
}

public class EnemyWavesManager : MonoBehaviour
{
    public enum EnemyTypes
    {
        Zanybell,
        EvilBunny
    }

    private static EnemyWavesManager _instance;
    public static EnemyWavesManager Instance => _instance;

    [SerializeField] private TextAsset _enemyWavesJsonFile;
    public TextAsset EnemyWavesJsonFile
    {
        get { return _enemyWavesJsonFile; }
        set { _enemyWavesJsonFile = value; }
    }

    private List<EnemyWave> _enemyWaveList = new List<EnemyWave>();
    [SerializeField] private List<GameObject> _flyingEnemyList;
    [SerializeField] private List<GameObject> _groundEnemyList;
    private List<Vector3> _flyingEnemySpawnLocations = new List<Vector3>();
    private List<Vector3> _groundedEnemySpawnLocations = new List<Vector3>();
    public List<Vector3> FlyingEnemySpawnLocations
    {
        get { return _flyingEnemySpawnLocations; }
        set { _flyingEnemySpawnLocations = value; }
    }
    public List<Vector3> GroundedEnemySpawnLocations
    {
        get { return _groundedEnemySpawnLocations; }
        set { _groundedEnemySpawnLocations = value; }
    }

    private int _aliveEnemies = 0;
    public static event Action AllEnemiesDefeated;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += (object sender, bool value) => StartCoroutine(EnemyWavesProcessing());

        if (LevelJsons.Instance != null)
        {
            _enemyWavesJsonFile = LevelJsons.Instance.EnemyWavesJsonFile;
        }

        StartCoroutine(LoadEnemyWavesFromJson());
    }

    private IEnumerator LoadEnemyWavesFromJson()
    {
        // Lee el JSON como una cadena
        string json = _enemyWavesJsonFile.text;

        // Convierte la cadena JSON a objetos EnemyWaveList
        EnemyWaveData waveList = JsonUtility.FromJson<EnemyWaveData>(json);

        if (waveList != null && waveList.EnemyWaves != null)
        {
            // Aquí tienes tu lista de oleadas de enemigos
            List<EnemyWave> enemyWaves = waveList.EnemyWaves;

            // Puedes iterar sobre las oleadas y hacer lo que necesites con ellas
            foreach (EnemyWave wave in enemyWaves)
            {
                _enemyWaveList.Add(wave);
            }
        }
        else
        {
            Debug.LogError("Error parsing JSON or waveList is null.");
        }

        yield return null;
    }

    private IEnumerator EnemyWavesProcessing()
    {
        foreach (EnemyWave enemyWave in _enemyWaveList)
        {
            if (enemyWave.NeedsEnemiesKilled)
            {
                bool enemiesDefeated = false;

                AllEnemiesDefeated += () => enemiesDefeated = true;

                yield return new WaitForSeconds(enemyWave.TimeDelay);
                yield return new WaitUntil(() => enemiesDefeated);

                AllEnemiesDefeated -= () => enemiesDefeated = true;
            }
            else
            {
                yield return new WaitForSeconds(enemyWave.TimeDelay);
            }

            StartCoroutine(SpawnEnemyWave(enemyWave));
        }

        yield return null;
    }

    private IEnumerator SpawnEnemyWave(EnemyWave enemyWave)
    {
        List<EnemyTypes> enemyList = new List<EnemyTypes>();

        StartCoroutine(AddEnemiesToList(EnemyTypes.Zanybell, enemyList, enemyWave));
        StartCoroutine(AddEnemiesToList(EnemyTypes.EvilBunny, enemyList, enemyWave));

        int randomIndex;
        EnemyTypes enemy;

        for (int i = 0; i < enemyList.Count; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
            enemy = enemyList[randomIndex];
            StartCoroutine(SpawnEnemy(enemy));
            enemyList.RemoveAt(randomIndex);

            yield return new WaitForSeconds(enemyWave.TimePerEnemy);
        }

        yield return null;
    }

    private IEnumerator SpawnEnemy(EnemyTypes enemy)
    {
        int randomSpawn;
        GameObject enemyInstance;

        switch (enemy)
        {
            case EnemyTypes.Zanybell:
                randomSpawn = UnityEngine.Random.Range(0, _flyingEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_flyingEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(SceneGenerator.Instance.SceneHolder.transform);
                enemyInstance.transform.localPosition = FlyingEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<AMonster>().Died += UpdateAliveEnemies;
                _aliveEnemies++;
                break;
            case EnemyTypes.EvilBunny:
                randomSpawn = UnityEngine.Random.Range(0, _groundedEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_groundEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(SceneGenerator.Instance.SceneHolder.transform);
                enemyInstance.transform.localPosition = _groundedEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<NavMeshAgent>().Warp(enemyInstance.transform.position);
                enemyInstance.GetComponent<AMonster>().Died += UpdateAliveEnemies;
                _aliveEnemies++;
                break;
        }

        yield return null;
    }

    private IEnumerator AddEnemiesToList(EnemyTypes type, List<EnemyTypes> enemyList, EnemyWave enemyWave)
    {
        int n = 0;

        switch (type)
        {
            case EnemyTypes.Zanybell:
                n = enemyWave.NZanybell;
                break;
            case EnemyTypes.EvilBunny:
                n = enemyWave.NEvilBunny;
                break;
        }

        for (int i = 0; i < n; i++)
        {
            enemyList.Add(type);
        }

        yield return null;
    }

    private void UpdateAliveEnemies(object sender, bool value)
    {
        _aliveEnemies--;

        if (_aliveEnemies == 0)
        {
            AllEnemiesDefeated?.Invoke();
        }
    }
}
