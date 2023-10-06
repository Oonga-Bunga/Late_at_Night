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

    // Tiempo de supervivencia
    [SerializeField] private float maxTime;
    private float currentTime;
    public EventHandler<float> timer;

    // Spawn de enemigos
    private List<AMonster> enemyList;
    private float spawnRate;

    // Spawn de props y objetos
    public GameObject[] propPrefabs;
    public GameObject[] objectPrefabs;

    private Dictionary<string, GameObject> propDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

    [SerializeField] private TextAsset jsonFile;

    // Otros
    private bool inGame;

    void Start()
    {
        currentTime = maxTime;
        currentActivatedSwitches = 0;
        totalSwitches = 0;

        // Create switches

        foreach (Switch interruptor in FindObjectsOfType<Switch>())
        {
            interruptor.TurnedOnOrOff += SwitchChangedState;
            totalSwitches++;
        }

        // Create baby?

        FindObjectOfType<Baby>().HealthChanged += BabyDied;

        // Create objects

        LoadPrefabs();

        string jsonString = jsonFile.text;
        Data datos = JsonUtility.FromJson<Data>(jsonString);

        foreach (Object prop in datos.Props)
        {
            Vector3 position = new Vector3(prop.Position.X, prop.Position.Y, prop.Position.Z);
            Vector3 rotation = new Vector3(prop.Rotation.X, prop.Rotation.Y, prop.Rotation.Z);

            GameObject propPrefab = propDictionary[prop.ID];
            Instantiate(propPrefab, position, Quaternion.Euler(rotation));
        }

        foreach (Object obj in datos.Objects)
        {
            Vector3 position = new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z);
            Vector3 rotation = new Vector3(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z);

            GameObject objPrefab = propDictionary[obj.ID];
            Instantiate(objPrefab, position, Quaternion.Euler(rotation));
        }

        // Enemies?

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
        if (inGame)
        {
            currentTime -= Time.deltaTime;
            timer?.Invoke(this, currentTime);

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
        Debug.Log("win");
    }

    private void PlayerLost()
    {
        inGame = false;
        Debug.Log("lose");
    }

    private void StartCatEvent()
    {

    }

    private void EndCatEvent()
    {

    }
}
