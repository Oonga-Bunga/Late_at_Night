using eDmitriyAssets.NavmeshLinksGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    #region Attributes

    private static LevelGenerator _instance;
    public static LevelGenerator Instance => _instance;

    [SerializeField] private TextAsset _levelJsonFile; // Referencia al Json con los datos del nivel
    [SerializeField] private GameObject _levelHolder; // Objeto que contiene los elementos del nivel
    public GameObject LevelHolder => _levelHolder;

    [SerializeField] private List<GameObject> _prefabsList; // Lista de prefabs que pueden ser instanciados al generar el nivel
    private Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>(); // Diccionario que relaciona los prefabs de la lista de prefabs con sus nombres

    private List<Vector3> _switchSpawnLocations = new List<Vector3>(); // Puntos de spawn de los interruptores
    private List<Vector3> _flyingEnemySpawnLocations = new List<Vector3>(); // Puntos de spawn de enemigos voladores
    private List<Vector3> _groundedEnemySpawnLocations = new List<Vector3>(); // Puntos de spawn de enemigos terrestres
    private Transform _playerSpawnPoint; // Punto de spawn del jugador

    public event Action OnLevelLoaded; // Invocado cuando termina de generarse el nivel

    #endregion

    #region Initialization

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
        // Comprueba si hay un JsonGenerator que va a generar un json a partir de la escena para no cargar el nivel
        if (JsonGenerator.Instance != null)
        {
            if (JsonGenerator.Instance.GenerateJson)
            {
                Debug.Log("Can't generate scene because a JsonGenerator is creating a new json from the scene");
                return;
            }
        }

        // Obtiene el json de LevelJsons si existe, y se coge su SceneJsonFile
        if (LevelJsons.Instance != null)
        {
            _levelJsonFile = LevelJsons.Instance.SceneJsonFile;
        }

        // Comienza la corrutina para generar el nivel
        StartCoroutine(LoadLevelFromJson());
    }

    #endregion

    #region LevelLoading

    /// <summary>
    /// Corrutina que genera el nivel a partir del json
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadLevelFromJson()
    {
        Debug.Log("Loading prefabs");

        foreach (var propPrefab in _prefabsList)
        {
            _prefabDictionary[propPrefab.name] = propPrefab;
        }

        string jsonString = _levelJsonFile.text;

        SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonString);

        Debug.Log("Generating: ");

        // Ahora puedes acceder a los datos como objetos C#
        foreach (var sceneObject in sceneData.SceneObjects)
        {
            Vector3[] infoArray = sceneObject.GetInfoAsVector3List();

            if (_prefabDictionary.ContainsKey(sceneObject.ID))
            {
                Debug.Log("Generating: " + sceneObject.ID);

                GameObject sceneObjectPrefab = _prefabDictionary[sceneObject.ID];
                GenerateObject(sceneObjectPrefab, infoArray[0], infoArray[1], infoArray[2]);
            }
            else
            {
                Debug.Log("No corresponding prefab in dictionary");
            }
        }

        Debug.Log("Reading switchNodes");

        foreach (var switchNode in sceneData.SwitchNodes)
        {
            _switchSpawnLocations.Add(new Vector3(switchNode.X, switchNode.Y, switchNode.Z));
        }

        Debug.Log("Reading zanybellNodes");

        foreach (var zanybellNode in sceneData.ZanybellNodes)
        {
            _flyingEnemySpawnLocations.Add(new Vector3(zanybellNode.X, zanybellNode.Y, zanybellNode.Z));
        }

        Debug.Log("Reading evilBunnyNodes");

        foreach (var evilBunnyNode in sceneData.EvilBunnyNodes)
        {
            _groundedEnemySpawnLocations.Add(new Vector3(evilBunnyNode.X, evilBunnyNode.Y, evilBunnyNode.Z));
        }

        Debug.Log("Reading _player spawn point");

        var playerSpawnPointData = sceneData.PlayerSpawnPoint;
        Vector3[] playerInfoArray = playerSpawnPointData.GetInfoAsVector3List();

        GameObject emptyGameObject = GenerateObject(new GameObject("EmptyGameObject"), playerInfoArray[0], playerInfoArray[1], playerInfoArray[2]);
        _playerSpawnPoint = emptyGameObject.transform;

        Debug.Log("Sending data to game manager");

        GameManager gameManager = GameManager.Instance;
        EnemyWavesManager wavesManager = EnemyWavesManager.Instance;

        if (GameManager.Instance != null)
        {
            gameManager.PlayerSpawnPoint = _playerSpawnPoint;

            if (wavesManager != null)
            {
                wavesManager.FlyingEnemySpawnLocations = _flyingEnemySpawnLocations;
                wavesManager.GroundedEnemySpawnLocations = _groundedEnemySpawnLocations;
            }
            else 
            {
                Debug.Log("There is no EnemyWavesManager in the scene");
            }
        }
        else
        {
            Debug.Log("There is no GameManager in the scene");
        }

        Debug.Log("Generating switches");

        int nSwitches = Mathf.Min(_switchSpawnLocations.Count, sceneData.TotalSwitches);
        int randomPos;

        for (int i = 0; i < nSwitches; i++)
        {
            randomPos = UnityEngine.Random.Range(0, _switchSpawnLocations.Count);
            GenerateObject(_prefabDictionary["Switch"], _switchSpawnLocations[randomPos], Vector3.zero, Vector3.one);
            _switchSpawnLocations.RemoveAt(randomPos);
        }

        Debug.Log("Baking the NavMesh");

        _levelHolder.GetComponent<NavMeshSurface>().BuildNavMesh();
        _levelHolder.GetComponent<NavMeshLinks_AutoPlacer>().Generate();

        Debug.Log("Scene generation finished");
        yield return new WaitForSeconds(2f);
        OnLevelLoaded?.Invoke();

        yield return null;
    }

    /// <summary>
    /// Instancia el objeto go dentro del _levelHolder, en la posición local pos, la rotación local rot y 
    /// la escala local scale
    /// </summary>
    /// <param name="go"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    private GameObject GenerateObject(GameObject go, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        GameObject objectInstance = Instantiate(go, Vector3.zero, Quaternion.identity, _levelHolder.transform);
        objectInstance.transform.localScale = scale;
        objectInstance.transform.localPosition = pos;
        objectInstance.transform.localRotation = Quaternion.Euler(rot);

        return objectInstance;
    }

    #endregion
}
