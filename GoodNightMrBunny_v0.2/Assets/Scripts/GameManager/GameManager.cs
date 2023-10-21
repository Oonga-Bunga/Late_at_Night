using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Object
{
    public string ID;
    public Position Rotation;
    public Position Position;
}

[System.Serializable]
public class Position
{
    public float X;
    public float Y;
    public float Z;
}

[System.Serializable]
public class Data
{
    public Object[] Props;
    public Object[] Objects;
}

public class GameManager : MonoBehaviour
{

    // Interruptores
    private int totalSwitches;
    private int currentActivatedSwitches;
    private List<float[,]> possibleSwitchLocationList = new List<float[,]>();
    [SerializeField] private TextMeshProUGUI upperText;
    [SerializeField] private TextMeshProUGUI winLoseText;
    [SerializeField] private GameObject mobileControls;
    [SerializeField] private GameObject blocking;

    // Tiempo de supervivencia
    [SerializeField] private float maxTime;
    private float currentTime;
    public EventHandler<float> TimerEvent;

    // Spawn de enemigos
    [SerializeField]  private List<GameObject> enemyList;
    [SerializeField]  private float spawnRate;
    private float enemySpawnCooldown = 0f;
    [SerializeField] private Transform enemySpawnPoint;

    // Spawn de props y objetos
    public GameObject[] propPrefabs;
    public GameObject[] objectPrefabs;

    private Dictionary<string, GameObject> propDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

    [SerializeField] private TextAsset jsonFile;

    // Otros
    private bool inGame;
    private PauseManager pauseManager;

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            mobileControls.SetActive(true);
        }
        else
        {
            mobileControls.SetActive(true);
        }

        currentTime = maxTime;
        currentActivatedSwitches = 0;
        totalSwitches = 0;
        pauseManager = FindObjectOfType<PauseManager>();
        winLoseText.gameObject.SetActive(false);

        // Create switches

        foreach (Switch interruptor in FindObjectsOfType<Switch>())
        {
            interruptor.OnTurnedOnOrOff += SwitchChangedState;
            totalSwitches++;
        }

        // Create baby?

        //FindObjectOfType<Baby>().HealthChanged += BabyDied;

        // Create objects
        
        LoadPrefabs();

        string jsonString = jsonFile.text;
        Data datos = JsonUtility.FromJson<Data>(jsonString);

        foreach (Object prop in datos.Props)
        {
            Vector3 position = new Vector3(prop.Position.X, prop.Position.Y, prop.Position.Z);
            Vector3 rotation = new Vector3(prop.Rotation.X, prop.Rotation.Y, prop.Rotation.Z);

            if (propDictionary.ContainsKey(prop.ID))
            {
                GameObject propPrefab = propDictionary[prop.ID];
                GameObject propInstance = Instantiate(propPrefab, Vector3.zero, Quaternion.identity);
                propInstance.transform.parent = blocking.transform;
                propInstance.transform.localPosition = position;
                propInstance.transform.localRotation = Quaternion.Euler(rotation);
            }
            else
            {
                Debug.Log("No hay prefab");
            }
        }

        foreach (Object obj in datos.Objects)
        {
            Vector3 position = new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z);
            Vector3 rotation = new Vector3(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z);

            if (objectDictionary.ContainsKey(obj.ID))
            {
                GameObject objPrefab = objectDictionary[obj.ID];
                GameObject objInstance = Instantiate(objPrefab, Vector3.zero, Quaternion.identity);
                objInstance.transform.parent = blocking.transform;
                objInstance.transform.localPosition = position;
                objInstance.transform.localRotation = Quaternion.Euler(rotation);
            }
            else
            {
                Debug.Log("No hay prefab");
            }
        }

        // Enemies?

        // Activar cámara de jugador

        inGame = true;
    }

    private void LoadPrefabs()
    {
        foreach (var propPrefab in propPrefabs)
        {
            propDictionary[propPrefab.name] = propPrefab;
        }

        foreach (var objectPrefab in objectPrefabs)
        {
            objectDictionary[objectPrefab.name] = objectPrefab;
        }
    }

    void Update()
    {
        if (pauseManager.isPaused) return;

        if (inGame)
        {
            enemySpawnCooldown += Time.deltaTime;

            if (enemySpawnCooldown >= spawnRate)
            {
                enemySpawnCooldown = 0f;
                Instantiate(enemyList[0], enemySpawnPoint.position, Quaternion.identity);
            }

            currentTime -= Time.deltaTime;
            TimerEvent?.Invoke(this, currentTime);

            if (currentTime < 0)
            {
                PlayerWon();
            }
        }
        else
        {
            // detectar inicio juego
        }
    }

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
            currentActivatedSwitches++;
            upperText.text = $"{currentActivatedSwitches}/{totalSwitches} interruptores";
            upperText.GetComponent<Animator>().SetTrigger("ShowText");
            if (currentActivatedSwitches == totalSwitches)
            {
                PlayerWon();
            }
        }
        else
        {
            currentActivatedSwitches--;
        }
    }

    private void PlayerWon()
    {
        inGame = false;
        winLoseText.text = "You won!";
        winLoseText.gameObject.SetActive(true);
    }

    private void PlayerLost()
    {
        inGame = false;
        winLoseText.text = "You lost...";
        winLoseText.gameObject.SetActive(true);
    }

    private void StartCatEvent()
    {

    }

    private void EndCatEvent()
    {

    }
}
