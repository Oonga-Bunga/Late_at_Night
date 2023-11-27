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
    public float InitialTimeDelay;
    public float DelayAfterEnemiesKilled;
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

    [SerializeField] private TextAsset _enemyWavesJsonFile; // Json con los datos de las oleadas de enemigos
    private List<EnemyWave> _enemyWaveList = new List<EnemyWave>(); // Lista en la que se guarda la información de las oleadas de enemigos extraida del Json
    
    [SerializeField] private List<GameObject> _flyingEnemyList; // Lista de enemigos voladores
    [SerializeField] private List<GameObject> _groundEnemyList; // Lista de enemigos terrestres
    private List<Vector3> _flyingEnemySpawnLocations = new List<Vector3>(); // Lista de puntos de spawn de enemigos voladores
    private List<Vector3> _groundedEnemySpawnLocations = new List<Vector3>(); // Lista de puntos de spawn de enemigos terrestres
    public List<Vector3> FlyingEnemySpawnLocations{ set => _flyingEnemySpawnLocations = value; }
    public List<Vector3> GroundedEnemySpawnLocations{ set => _groundedEnemySpawnLocations = value; }

    private int _aliveEnemies = 0; // Número de enemigos con vida
    public event Action OnAllEnemiesDefeated; // Invocado cuando todos los enemigos han muerto o desaparecido
    public event Action OnAllWavesDefeated; // Invocado cuando todas las oleadas han sido derrotadas

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
        // Se suscribe al evento del game manager para comenzar a spawnear enemigos una vez comience el nivel
        GameManager.Instance.OnGameStarted += () => StartCoroutine(EnemyWavesProcessing());

        // Obtiene el json de LevelJsons si existe, y se coge su EnemyWavesJsonFile
        if (LevelJsons.Instance != null)
        {
            _enemyWavesJsonFile = LevelJsons.Instance.EnemyWavesJsonFile;
        }

        // Comienza a cargar la información del Json
        StartCoroutine(LoadEnemyWavesFromJson());
    }

    private IEnumerator LoadEnemyWavesFromJson()
    {
        // Lee el Json como una cadena
        string json = _enemyWavesJsonFile.text;

        // Convierte la cadena Json a objetos EnemyWave
        EnemyWaveData waveList = JsonUtility.FromJson<EnemyWaveData>(json);
        List<EnemyWave> enemyWaves = waveList.EnemyWaves;

        // Por cada oleada la añade a la lista _enemyWaveList
        foreach (EnemyWave wave in enemyWaves)
        {
            _enemyWaveList.Add(wave);
        }

        yield return null;
    }

    /// <summary>
    /// Procesa cada oleada de enemigos según sus datos
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyWavesProcessing()
    {
        foreach (EnemyWave enemyWave in _enemyWaveList)
        {
            if (enemyWave.NeedsEnemiesKilled)
            {
                bool enemiesDefeated = false;

                OnAllEnemiesDefeated += () => enemiesDefeated = true;

                yield return new WaitForSeconds(enemyWave.InitialTimeDelay);

                yield return new WaitUntil(() => enemiesDefeated);
                OnAllEnemiesDefeated -= () => enemiesDefeated = true;

                yield return new WaitForSeconds(enemyWave.DelayAfterEnemiesKilled);
            }
            else
            {
                yield return new WaitForSeconds(enemyWave.InitialTimeDelay);
            }

            List<EnemyTypes> enemyList = new List<EnemyTypes>();

            AddEnemiesToList(EnemyTypes.Zanybell, enemyList, enemyWave);
            AddEnemiesToList(EnemyTypes.EvilBunny, enemyList, enemyWave);

            int randomIndex;
            EnemyTypes enemy;

            for (int i = 0; i < enemyList.Count; i++)
            {
                randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
                enemy = enemyList[randomIndex];
                SpawnEnemy(enemy);
                enemyList.RemoveAt(randomIndex);

                if (i != enemyList.Count - 1)
                {
                    yield return new WaitForSeconds(enemyWave.TimePerEnemy);
                }
            }
        }

        OnAllEnemiesDefeated += () => OnAllWavesDefeated?.Invoke();

        yield return null;
    }

    /// <summary>
    /// Spawnea a un enemigo de cierto tipo en un punto de spawn correspondiente aleatorio
    /// </summary>
    /// <param name="enemy"></param>
    private void SpawnEnemy(EnemyTypes enemy)
    {
        int randomSpawn;
        GameObject enemyInstance;

        switch (enemy)
        {
            case EnemyTypes.Zanybell:
                randomSpawn = UnityEngine.Random.Range(0, _flyingEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_flyingEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(LevelGenerator.Instance.LevelHolder.transform);
                enemyInstance.transform.localPosition = _flyingEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<AMonster>().OnDied += UpdateAliveEnemies;
                _aliveEnemies++;
                break;
            case EnemyTypes.EvilBunny:
                randomSpawn = UnityEngine.Random.Range(0, _groundedEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_groundEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(LevelGenerator.Instance.LevelHolder.transform);
                enemyInstance.transform.localPosition = _groundedEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<NavMeshAgent>().Warp(enemyInstance.transform.position);
                enemyInstance.GetComponent<AMonster>().OnDied += UpdateAliveEnemies;
                _aliveEnemies++;
                break;
        }
    }

    /// <summary>
    /// Añade los enemigos de tipo type de una oleada a enemyList
    /// </summary>
    /// <param name="type"></param>
    /// <param name="enemyList"></param>
    /// <param name="enemyWave"></param>
    private void AddEnemiesToList(EnemyTypes type, List<EnemyTypes> enemyList, EnemyWave enemyWave)
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
    }

    /// <summary>
    /// Se invoca cuando un enemigo muere, actualiza el numero de enemigos vivos, y si llega a 0 se invoca
    /// el evento OnAllEnemiesDefeated
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="value"></param>
    private void UpdateAliveEnemies()
    {
        _aliveEnemies--;

        if (_aliveEnemies == 0)
        {
            OnAllEnemiesDefeated?.Invoke();
        }
    }
}
