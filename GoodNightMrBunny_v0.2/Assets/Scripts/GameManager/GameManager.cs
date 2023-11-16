using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

[System.Serializable]
public class MyVector3
{
    public float X;
    public float Y;
    public float Z;
}

[System.Serializable]
public class Object
{
    public string ID;
    public MyVector3 Position;
    public MyVector3 Rotation;
    public MyVector3 Scale;
}

[System.Serializable]
public class SceneData
{
    public List<Object> SceneObjects;
    public List<MyVector3> SwitchNodes;
    public List<MyVector3> ZanybellNodes;
    public List<MyVector3> EvilBunnyNodes;
    public Object PlayerSpawnPoint;
}

public class GameManager : MonoBehaviour
{
    #region Attributes

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    //------------------------------------------------------
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private TextMeshProUGUI _loadingText;

    [SerializeField] private bool _generateJson;

    [SerializeField] private TextAsset _jsonFile;
    [SerializeField] private GameObject _sceneHolder;
    [SerializeField] private List<GameObject> _prefabsList;
    private Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();

    private List<Vector3> _switchSpawnLocationsList = new List<Vector3>();
    private List<Vector3> _zanybellSpawnLocationsList = new List<Vector3>();
    private List<Vector3> _evilBunnySpawnLocationsList = new List<Vector3>();

    [SerializeField] private int _totalSwitches = 3;
    [SerializeField] private GameObject _switchPrefab;

    private static List<Switch> _switchListInstance = new List<Switch>();
    public static List<Switch> SwitchListInstance => _switchListInstance;

    private static List<FlashlightRechargeStation> _rechargeStationListInstance = new List<FlashlightRechargeStation>();
    public static List<FlashlightRechargeStation> RechargeStationListInstance => _rechargeStationListInstance;

    [SerializeField] private GameObject _playerPrefab;
    private Transform _playerSpawnPoint;

    [SerializeField] private GameObject _mobileControls;
    [SerializeField] private TextMeshProUGUI _winLoseText;
    [SerializeField] private float _maxTime = 60f;
    private float _currentTime;

    private PauseManager _pauseManager;
    private bool _isInGame = false;
    private int _currentActivatedSwitches = 0;
    [SerializeField] private TextMeshProUGUI _upperText;
    public EventHandler<float> OnTimeChanged;
    [SerializeField] private List<GameObject> _groundEnemyList;
    [SerializeField] private List<GameObject> _flyingEnemyList;
    //------------------------------------------------------

    // Interruptores

    // UI
    //[Header("UI")]


    // Json generation and loading
    //[Header("Json")]


    // Tiempo de supervivencia
    //[Header("Time")]


    // Spawn de enemigos
    //[Header("Enemies")]

    // Otros
    //[Header("Other")]


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
        // Mostrar la pantalla de carga
        _loadingScreen.SetActive(true);

        // Iniciar la generación del nivel en una corrutina
        StartCoroutine(GenerateLevel());
    }

    IEnumerator GenerateLevel()
    {
        //crear json a partir de los objetos de la escena y return si generateJson es true

        _loadingText.text = "Beggin loading";
        Debug.Log("Beggin loading");

        if (_generateJson) 
        { 
            CreateJson();

            _loadingText.text = "Json generated";
            Debug.Log("Json generated");

            yield break;
        }

        //generación de objetos a partir del json

        _loadingText.text = "Loading prefabs";
        Debug.Log("Loading prefabs");

        LoadPrefabs();

        _loadingText.text = "Reading json";
        Debug.Log("Reading json");

        LoadSceneFromJson();

        //Generar interruptores

        _loadingText.text = "Instantiating switches";
        Debug.Log("Instantiating switches");

        List<Switch> tempSwitchList = new List<Switch>();
        int nSwitches = Mathf.Min(_switchSpawnLocationsList.Count, _totalSwitches);

        for (int i = 0; i < nSwitches; i++)
        {
            int randomPos = UnityEngine.Random.Range(0, _switchSpawnLocationsList.Count);
            GameObject switchInstance = Instantiate(_switchPrefab, Vector3.zero, Quaternion.identity);
            switchInstance.transform.SetParent(_sceneHolder.transform);
            switchInstance.transform.localPosition = _switchSpawnLocationsList[randomPos];
            Switch switchComponent = switchInstance.GetComponent<Switch>();
            switchComponent.OnTurnedOnOrOff += SwitchChangedState;
            tempSwitchList.Add(switchComponent);

            _switchSpawnLocationsList.RemoveAt(randomPos);
        }

        _switchListInstance = tempSwitchList;

        //generar nav mesh

        _loadingText.text = "Generating NavMesh";
        Debug.Log("Generating NavMesh");

        //buscar ciertos objetos y añadirlos a las listas

        _loadingText.text = "Searching flashlight recharge stations";
        Debug.Log("Searching flashlight recharge stations");

        List<FlashlightRechargeStation> tempRechargeStationList = new List<FlashlightRechargeStation>();

        foreach (FlashlightRechargeStation rechargeStation in FindObjectsOfType<FlashlightRechargeStation>())
        {
            tempRechargeStationList.Add(rechargeStation);
        }

        _rechargeStationListInstance = tempRechargeStationList;

        _loadingText.text = "Searching baby";
        Debug.Log("Searching baby");

        Baby baby = FindObjectOfType<Baby>();

        if (baby != null)
        {
            baby.HealthChanged += BabyDied;
        }
        else
        {
            Debug.Log("No baby found");
        }

        //detectar si el juego es en movil o pc y modificar lo necesario, como los elementos de la UI

        _loadingText.text = "Checking type of device";
        Debug.Log("Checking type of device");

        if (Application.isMobilePlatform)
        {
            _mobileControls.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _mobileControls.SetActive(false);
        }

        //inicializar variables del nivel

        _loadingText.text = "Initializing level variables";
        Debug.Log("Initializing level variables");

        _currentTime = _maxTime;
        _winLoseText.gameObject.SetActive(false);

        _pauseManager = PauseManager.Instance;

        //instanciar al jugador

        _loadingText.text = "Instantiating player";
        Debug.Log("Instantiating player");

        GameObject playerInstance = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.Euler(_playerSpawnPoint.rotation.eulerAngles));

        //Desactivar la pantalla de carga y cambiar la main cámara a la del jugador

        _loadingText.text = "Loading finished!";
        Debug.Log("Loading finished!");

        _loadingScreen.SetActive(false);

        //iniciar el transcurso del juego

        _isInGame = true;

        yield return null;
    }

    private void CreateJson()
    {
        List<Object> sceneObjectList = new List<Object>();
        List<MyVector3> switchNodeList = new List<MyVector3>();
        List<MyVector3> zanybellNodeList = new List<MyVector3>();
        List<MyVector3> evilBunnyNodeList = new List<MyVector3>();
        Object playerSpawnPointData = new Object();

        GameObject[] sceneObjects = FindChildObjectsWithTag(_sceneHolder, "SceneObject");
        GameObject[] switchNodes = FindChildObjectsWithTag(_sceneHolder, "SwitchNode");
        GameObject[] zanybellNodes = FindChildObjectsWithTag(_sceneHolder, "ZanybellNode");
        GameObject[] evilBunnyNodes = FindChildObjectsWithTag(_sceneHolder, "EvilBunnyNode");
        GameObject playerSpawnPoint = FindChildObjectsWithTag(_sceneHolder, "PlayerSpawnPoint")[0];

        foreach (GameObject sceneObject in sceneObjects)
        {
            Vector3 position = sceneObject.transform.localPosition;
            Vector3 rotation = sceneObject.transform.localRotation.eulerAngles;
            Vector3 scale = sceneObject.transform.localScale;
            Object data = new Object
            {
                ID = sceneObject.name,
                Position = new MyVector3 { X = position.x, Y = position.y, Z = position.z },
                Rotation = new MyVector3 { X = rotation.x, Y = rotation.y, Z = rotation.z },
                Scale = new MyVector3 { X = scale.x, Y = scale.y, Z = scale.z }
            };
            sceneObjectList.Add(data);
        }

        foreach (GameObject switchNode in switchNodes)
        {
            Vector3 position = switchNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            switchNodeList.Add(data);
        }

        foreach (GameObject zanybellNode in zanybellNodes)
        {
            Vector3 position = zanybellNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            zanybellNodeList.Add(data);
        }

        foreach (GameObject evilBunnyNode in evilBunnyNodes)
        {
            Vector3 position = evilBunnyNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            evilBunnyNodeList.Add(data);
        }

        Vector3 playerPosition = playerSpawnPoint.transform.position;
        Vector3 playerRotation = playerSpawnPoint.transform.rotation.eulerAngles;
        Vector3 playerScale = playerSpawnPoint.transform.localScale;
        Object playerData = new Object
        {
            ID = playerSpawnPoint.name,
            Position = new MyVector3 { X = playerPosition.x, Y = playerPosition.y, Z = playerPosition.z },
            Rotation = new MyVector3 { X = playerRotation.x, Y = playerRotation.y, Z = playerRotation.z },
            Scale = new MyVector3 { X = playerScale.x, Y = playerScale.y, Z = playerScale.z }
        };
        playerSpawnPointData = playerData;

        // Crear un objeto JSON que contiene las listas de props y objects
        var sceneData = new
        {
            SceneObjects = sceneObjectList,
            SwitchNodes = switchNodeList,
            ZanybellNodes = zanybellNodeList,
            EvilBunnyNodes = evilBunnyNodeList,
            PlayerSpawnPoint = playerSpawnPointData
        };

        // Convertir el objeto JSON a una cadena JSON
        string jsonData = JsonConvert.SerializeObject(sceneData, Newtonsoft.Json.Formatting.Indented);

        // Guardar la cadena JSON en un archivo
        File.WriteAllText(Application.dataPath + "/Scripts/GameManager/LevelJsons/newSceneData.json", jsonData);

        Debug.Log("Datos de la escena guardados como JSON.");
    }

    private GameObject[] FindChildObjectsWithTag(GameObject parent, string tag)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true); // Incluye componentes inactivos

        // Filtra los hijos por la etiqueta
        GameObject[] childrenWithTag = System.Array.FindAll(children, child => child != parent && child.CompareTag(tag))
            .Select(child => child.gameObject)
            .ToArray();

        return childrenWithTag;
    }

    private void LoadPrefabs()
    {
        foreach (var propPrefab in _prefabsList)
        {
            _prefabDictionary[propPrefab.name] = propPrefab;
        }
    }

    private void LoadSceneFromJson()
    {
        string jsonString = _jsonFile.text;

        SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonString);

        _loadingText.text = "Generating: ";
        Debug.Log("Generating: ");

        // Ahora puedes acceder a los datos como objetos C#
        foreach (var sceneObject in sceneData.SceneObjects)
        {   
            Vector3 position = new Vector3(sceneObject.Position.X, sceneObject.Position.Y, sceneObject.Position.Z);
            Vector3 rotation = new Vector3(sceneObject.Rotation.X, sceneObject.Rotation.Y, sceneObject.Rotation.Z);
            Vector3 scale = new Vector3(sceneObject.Scale.X, sceneObject.Scale.Y, sceneObject.Scale.Z);

            if (_prefabDictionary.ContainsKey(sceneObject.ID))
            {
                _loadingText.text = "Generating: " + sceneObject.ID;
                Debug.Log("Generating: " + sceneObject.ID);

                GameObject sceneObjectPrefab = _prefabDictionary[sceneObject.ID];
                GameObject sceneObjectInstance = Instantiate(sceneObjectPrefab, Vector3.zero, Quaternion.identity, _sceneHolder.transform);
                sceneObjectInstance.transform.localScale = scale;
                sceneObjectInstance.transform.localPosition = position;
                sceneObjectInstance.transform.localRotation = Quaternion.Euler(rotation);
            }
            else
            {
                Debug.Log("No hay prefab");
            }
        }

        _loadingText.text = "Reading switchNodes";
        Debug.Log("Reading switchNodes");

        foreach (var switchNode in sceneData.SwitchNodes)
        {
            _switchSpawnLocationsList.Add(new Vector3(switchNode.X, switchNode.Y, switchNode.Z));
        }

        _loadingText.text = "Reading zanybellNodes";
        Debug.Log("Reading zanybellNodes");

        foreach (var zanybellNode in sceneData.ZanybellNodes)
        {
            _zanybellSpawnLocationsList.Add(new Vector3(zanybellNode.X, zanybellNode.Y, zanybellNode.Z));
        }

        _loadingText.text = "Reading evilBunnyNodes";
        Debug.Log("Reading evilBunnyNodes");

        foreach (var evilBunnyNode in sceneData.EvilBunnyNodes)
        {
            _evilBunnySpawnLocationsList.Add(new Vector3(evilBunnyNode.X, evilBunnyNode.Y, evilBunnyNode.Z));
        }

        _loadingText.text = "Reading player spawn point";
        Debug.Log("Reading player spawn point");

        var playerSpawnPointData = sceneData.PlayerSpawnPoint;

        Vector3 playerPosition = new Vector3(playerSpawnPointData.Position.X, playerSpawnPointData.Position.Y, playerSpawnPointData.Position.Z);
        Vector3 playerRotation = new Vector3(playerSpawnPointData.Rotation.X, playerSpawnPointData.Rotation.Y, playerSpawnPointData.Rotation.Z);
        Vector3 playerScale = new Vector3(playerSpawnPointData.Scale.X, playerSpawnPointData.Scale.Y, playerSpawnPointData.Scale.Z);

        GameObject emptyGameObject = new GameObject("EmptyGameObject");
        emptyGameObject.transform.localScale = playerScale;
        emptyGameObject.transform.position = playerPosition;
        emptyGameObject.transform.rotation = Quaternion.Euler(playerRotation);

        _playerSpawnPoint = emptyGameObject.transform;
    }

    #endregion

    #region Update

    void Update()
    {
        if (!_isInGame) return;

        if (_pauseManager.isPaused) return;

        /*
        _enemySpawnCooldown += Time.deltaTime;

        if (_enemySpawnCooldown >= _spawnRate)
        {
            _enemySpawnCooldown = 0f;
            Instantiate(_enemyList[0], _enemySpawnPoint.position, Quaternion.identity);
        }
        */
        _currentTime -= Time.deltaTime;
        OnTimeChanged?.Invoke(this, _currentTime);

        if (_currentTime < 0)
        {
            PlayerWon();
        }
    }

    #endregion

    #region Methods

    private void BabyDied(object sender, float babyHealth)
    {
        if (babyHealth == 0)
        {
            PlayerLost();
        }
    }

    private void SwitchChangedState(object sender, bool isOn)
    {
        if (isOn)
        {
            _currentActivatedSwitches++;
            _upperText.text = $"{_currentActivatedSwitches}/{_totalSwitches} interruptores";
            _upperText.GetComponent<Animator>().SetTrigger("ShowText");
            if (_currentActivatedSwitches == _totalSwitches)
            {
                PlayerWon();
            }
        }
        else
        {
            _currentActivatedSwitches--;
            _upperText.text = $"{_currentActivatedSwitches}/{_totalSwitches} interruptores";
            _upperText.GetComponent<Animator>().SetTrigger("ShowText");
        }
    }

    private void PlayerWon()
    {
        _isInGame = false;
        _winLoseText.text = "You won!";
        _winLoseText.gameObject.SetActive(true);
        SceneManager.LoadScene("FinalScene");
    }

    private void PlayerLost()
    {
        _isInGame = false;
        _winLoseText.text = "You lost...";
        _winLoseText.gameObject.SetActive(true);
        SceneManager.LoadScene("FinalScene");
    }

    #endregion
}
