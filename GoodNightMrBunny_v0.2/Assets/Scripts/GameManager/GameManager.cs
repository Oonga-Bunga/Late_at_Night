using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;


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
    public List<MyVector3> CatNodes;
}

public class GameManager : MonoBehaviour
{
    #region Attributes

    // Interruptores
    private int _totalSwitches = 0;
    private int _currentActivatedSwitches = 0;
    private List<Vector3> _possibleSwitchLocationList = new List<Vector3>();
    private static List<Switch> _switchListInstance = new List<Switch>();
    public static List<Switch> SwitchListInstance => _switchListInstance;

    // UI
    [Header("UI")]

    [SerializeField] private TextMeshProUGUI _upperText;
    [SerializeField] private TextMeshProUGUI _winLoseText;
    [SerializeField] private GameObject _mobileControls;

    // Json generation and loading
    [Header("Json")]

    [SerializeField] private TextAsset _jsonFile;

    [SerializeField] private List<GameObject> _prefabsList;
    private Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject _sceneHolder;

    [SerializeField] private bool _generateJson;

    // Tiempo de supervivencia
    [Header("Time")]

    [SerializeField] private float _maxTime = 60f;
    private float _currentTime;
    public EventHandler<float> OnTimeChanged;

    // Spawn de enemigos
    [Header("Enemies")]

    [SerializeField]  private List<GameObject> _enemyList;
    [SerializeField]  private float _spawnRate = 15f;
    private float _enemySpawnCooldown = 0f;
    [SerializeField] private Transform _enemySpawnPoint;

    private List<Vector3> _catWanderingNodesList = new List<Vector3>();

    // Otros
    [Header("Other")]

    [SerializeField] private PauseManager _pauseManager;
    private bool _isInGame;

    #endregion

    #region Initialization

    void Start()
    {
        // Inicialización de valores y estados

        _currentTime = _maxTime;
        _winLoseText.gameObject.SetActive(false);

        if (_generateJson)
        {
            CreateJson();
        }

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

        // Generación de la escena
        
        LoadPrefabs();
        LoadSceneFromJson();

        // Una vez generada hacer cosas con ciertos objetos

        List<Switch> tempSwitchList = new List<Switch>();

        foreach (Switch interruptor in FindObjectsOfType<Switch>())
        {
            interruptor.OnTurnedOnOrOff += SwitchChangedState;
            _totalSwitches++;
            tempSwitchList.Add(interruptor);
        }

        _switchListInstance = tempSwitchList;

        Baby baby = FindObjectOfType<Baby>();

        if (baby != null)
        {
            baby.HealthChanged += BabyDied;
        }

        // Empezar nivel

        _isInGame = true;
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
        
        // Ahora puedes acceder a los datos como objetos C#
        foreach (var sceneObject in sceneData.SceneObjects)
        {   
            Vector3 position = new Vector3(sceneObject.Position.X, sceneObject.Position.Y, sceneObject.Position.Z);
            Vector3 rotation = new Vector3(sceneObject.Rotation.X, sceneObject.Rotation.Y, sceneObject.Rotation.Z);
            Vector3 scale = new Vector3(sceneObject.Scale.X, sceneObject.Scale.Y, sceneObject.Scale.Z);

            if (_prefabDictionary.ContainsKey(sceneObject.ID))
            {
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

        foreach (var switchNode in sceneData.SwitchNodes)
        {
            _possibleSwitchLocationList.Add(new Vector3(switchNode.X, switchNode.Y, switchNode.Z));
        }

        foreach (var catNode in sceneData.CatNodes)
        {
            _catWanderingNodesList.Add(new Vector3(catNode.X, catNode.Y, catNode.Z));
        }
    }

    private void CreateJson()
    {
        List<Object> sceneObjectList = new List<Object>();
        List<MyVector3> switchNodeList = new List<MyVector3>();
        List<MyVector3> catNodeList = new List<MyVector3>();

        GameObject[] sceneObjects = GameObject.FindGameObjectsWithTag("SceneObject");
        GameObject[] switchNodes = GameObject.FindGameObjectsWithTag("SwitchNode");
        GameObject[] catNodes = GameObject.FindGameObjectsWithTag("CatNode");

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

        foreach (GameObject catNode in catNodes)
        {
            Vector3 position = catNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            catNodeList.Add(data);
        }

        // Crear un objeto JSON que contiene las listas de props y objects
        var sceneData = new
        {
            SceneObjects = sceneObjectList,
            SwitchNodes = switchNodeList,
            CatNodes = catNodeList
        };

        // Convertir el objeto JSON a una cadena JSON
        string jsonData = JsonConvert.SerializeObject(sceneData, Newtonsoft.Json.Formatting.Indented);

        // Guardar la cadena JSON en un archivo
        File.WriteAllText(Application.dataPath + "/Scripts/GameManager/LevelJsons/newSceneData.json", jsonData);

        Debug.Log("Datos de la escena guardados como JSON.");
    }

    #endregion

    #region Update

    void Update()
    {
        if (_pauseManager.isPaused) return;

        if (_isInGame)
        {
            _enemySpawnCooldown += Time.deltaTime;

            if (_enemySpawnCooldown >= _spawnRate)
            {
                _enemySpawnCooldown = 0f;
                Instantiate(_enemyList[0], _enemySpawnPoint.position, Quaternion.identity);
            }

            _currentTime -= Time.deltaTime;
            OnTimeChanged?.Invoke(this, _currentTime);

            if (_currentTime < 0)
            {
                PlayerWon();
            }
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
