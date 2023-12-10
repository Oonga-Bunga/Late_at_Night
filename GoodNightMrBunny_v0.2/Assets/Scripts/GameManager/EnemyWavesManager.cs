using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    public float TimeDelay;
    public float TimePerEnemy;
    public int NEvilBunny;
    public int NZanybell;
    public int NKitestinger;
}

public class EnemyWavesManager : MonoBehaviour
{
    public enum EnemyTypes
    {
        EvilBunny,
        Zanybell,
        Kitestinger
    }

    private static EnemyWavesManager _instance;
    public static EnemyWavesManager Instance => _instance;

    [SerializeField] private TextAsset _enemyWavesJsonFile; // Json con los datos de las oleadas de enemigos
    private List<EnemyWave> _enemyWaveList = new List<EnemyWave>(); // Lista en la que se guarda la informaci�n de las oleadas de enemigos extraida del Json

    [SerializeField] private List<GameObject> _groundEnemyList; // Lista de enemigos terrestres
    [SerializeField] private List<GameObject> _flyingEnemyList; // Lista de enemigos voladores
    [SerializeField] private List<GameObject> _ceilingEnemyList; // Lista de enemigos terrestres
    private List<Vector3> _groundEnemySpawnLocations = new List<Vector3>(); // Lista de puntos de spawn de enemigos terrestres
    private List<Vector3> _flyingEnemySpawnLocations = new List<Vector3>(); // Lista de puntos de spawn de enemigos voladores
    private List<Vector3> _ceilingEnemySpawnLocations = new List<Vector3>(); // Lista de puntos de spawn de enemigos que reptan por el techo
    public List<Vector3> GroundEnemySpawnLocations { set => _groundEnemySpawnLocations = value; }
    public List<Vector3> FlyingEnemySpawnLocations{ set => _flyingEnemySpawnLocations = value; }
    public List<Vector3> CeilingEnemySpawnLocations{ set => _ceilingEnemySpawnLocations = value; }

    private int _aliveEnemies = 0; // N�mero de enemigos con vida
    public event Action OnAllEnemiesDefeated; // Invocado cuando todos los enemigos han muerto o desaparecido
    public event Action OnAllWavesDefeated; // Invocado cuando todas las oleadas han sido derrotadas

    [SerializeField] private GameObject _spawnEffect;
    [SerializeField] private AudioSource _waveStartedSound;

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
        // Comienza a cargar la informaci�n del Json
        StartCoroutine(LoadEnemyWavesFromJson());

        // Se suscribe al evento del game manager para comenzar a spawnear enemigos una vez comience el nivel
        GameManager.Instance.OnGameStarted += () => StartCoroutine(EnemyWavesProcessing());

        // Obtiene el json de LevelJsons si existe, y se coge su EnemyWavesJsonFile
        if (LevelJsons.Instance != null)
        {
            _enemyWavesJsonFile = LevelJsons.Instance.EnemyWavesJsonFile;
        }
    }

    private IEnumerator LoadEnemyWavesFromJson()
    {
        // Lee el Json como una cadena
        string json = _enemyWavesJsonFile.text;

        // Convierte la cadena Json a objetos EnemyWave
        EnemyWaveData waveList = JsonUtility.FromJson<EnemyWaveData>(json);
        List<EnemyWave> enemyWaves = waveList.EnemyWaves;

        // Por cada oleada la a�ade a la lista _enemyWaveList
        foreach (EnemyWave wave in enemyWaves)
        {
            _enemyWaveList.Add(wave);
        }

        Debug.Log("Finished loading enemies");

        yield return null;
    }

    /// <summary>
    /// Procesa cada oleada de enemigos seg�n sus datos
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyWavesProcessing()
    {
        Debug.Log("Started enemy spawning");

        OnAllEnemiesDefeated += () => OnAllWavesDefeated?.Invoke();

        foreach (EnemyWave enemyWave in _enemyWaveList)
        {
            yield return new WaitForSeconds(enemyWave.TimeDelay);
            _waveStartedSound.Play();

            List<EnemyTypes> enemyList = new List<EnemyTypes>();

            AddEnemiesToList(EnemyTypes.EvilBunny, enemyList, enemyWave);
            AddEnemiesToList(EnemyTypes.Zanybell, enemyList, enemyWave);
            AddEnemiesToList(EnemyTypes.Kitestinger, enemyList, enemyWave);

            int randomIndex;
            EnemyTypes enemy;

            while (enemyList.Count != 0)
            {
                randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
                enemy = enemyList[randomIndex];
                SpawnEnemy(enemy);
                enemyList.RemoveAt(randomIndex);
                
                if (enemyList.Count != 0)
                {
                    yield return new WaitForSeconds(enemyWave.TimePerEnemy);
                }
            }
        }

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
            case EnemyTypes.EvilBunny:
                randomSpawn = UnityEngine.Random.Range(0, _groundEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_groundEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(LevelGenerator.Instance.LevelHolder.transform);
                enemyInstance.transform.localPosition = _groundEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<NavMeshAgent>().Warp(enemyInstance.transform.position);
                enemyInstance.GetComponent<EvilBunny>().OnDied += UpdateAliveEnemies;
                _aliveEnemies++;
                Instantiate(_spawnEffect, enemyInstance.transform.position+Vector3.up, quaternion.identity);
                break;
            case EnemyTypes.Zanybell:
                randomSpawn = UnityEngine.Random.Range(0, _flyingEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_flyingEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(LevelGenerator.Instance.LevelHolder.transform);
                enemyInstance.transform.localPosition = _flyingEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<Shadow>().OnDied += UpdateAliveEnemies;
                _aliveEnemies++;
                Instantiate(_spawnEffect, enemyInstance.transform.position+Vector3.up, quaternion.identity);
                break;
            case EnemyTypes.Kitestinger:
                randomSpawn = UnityEngine.Random.Range(0, _ceilingEnemySpawnLocations.Count);
                enemyInstance = Instantiate(_ceilingEnemyList[0], Vector3.zero, Quaternion.identity);
                enemyInstance.transform.SetParent(LevelGenerator.Instance.LevelHolder.transform);
                enemyInstance.transform.localPosition = _ceilingEnemySpawnLocations[randomSpawn];
                enemyInstance.GetComponent<NavMeshAgent>().Warp(enemyInstance.transform.position);
                enemyInstance.GetComponent<Kitestinger>().OnDied += UpdateAliveEnemies;
                _aliveEnemies++;
                Instantiate(_spawnEffect, enemyInstance.transform.position+Vector3.down, quaternion.identity);
                break;
        }

        Debug.Log("Enemy spawned");
    }

    /// <summary>
    /// A�ade los enemigos de tipo type de una oleada a enemyList
    /// </summary>
    /// <param name="type"></param>
    /// <param name="enemyList"></param>
    /// <param name="enemyWave"></param>
    private void AddEnemiesToList(EnemyTypes type, List<EnemyTypes> enemyList, EnemyWave enemyWave)
    {
        int n = 0;

        switch (type)
        {
            case EnemyTypes.EvilBunny:
                n = enemyWave.NEvilBunny;
                break;
            case EnemyTypes.Zanybell:
                n = enemyWave.NZanybell;
                break;
            case EnemyTypes.Kitestinger:
                n = enemyWave.NKitestinger;
                break;
        }
        
        for (int i = 0; i < n; i++)
        {
            enemyList.Add(type);
        }
        Debug.Log(enemyList.Count);
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
