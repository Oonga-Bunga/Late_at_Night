using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MyVector3
{
    public float X;
    public float Y;
    public float Z;
}

[System.Serializable]
public class SceneObject
{
    public string ID;
    public MyVector3 Position;
    public MyVector3 Rotation;
    public MyVector3 Scale;

    /// <summary>
    /// Devuelve un array con 3 vectores con la posici�n, rotaci�n y escala respectivamente
    /// </summary>
    /// <returns></returns>
    public Vector3[] GetInfoAsVector3List()
    {
        Vector3[] vectors = new Vector3[3];
        vectors[0] = new Vector3(Position.X, Position.Y, Position.Z);
        vectors[1] = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        vectors[2] = new Vector3(Scale.X, Scale.Y, Scale.Z);
        return vectors;
    }
}

[System.Serializable]
public class SceneData
{
    public List<SceneObject> SceneObjects;
    public List<MyVector3> SwitchNodes;
    public List<MyVector3> GroundEnemyNodes;
    public List<MyVector3> FlyingEnemyNodes;
    public List<MyVector3> CeilingEnemyNodes;
    public SceneObject PlayerSpawnPoint;
    public int TotalSwitches;
}

public class JsonGenerator : MonoBehaviour
{
    private static JsonGenerator _instance;
    public static JsonGenerator Instance => _instance;

    [SerializeField] private bool _generateJson = false; // Si debe generar el Json o no
    public bool GenerateJson => _generateJson;
    [SerializeField] private GameObject _sceneHolder; // Objeto que contiene todos los elementos del nivel que se van a guardar en el json
    [SerializeField] private int _totalSwitches = 3; // N�mero total de interruptores que va a tener el nivel


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
        if (_sceneHolder != null && _generateJson)
        {
            StartCoroutine(CreateJson());
        }
    }

    private IEnumerator CreateJson()
    {
        // Crear las listas donde se va a guardar la informaci�n del nivel

        List<SceneObject> sceneObjectList = new List<SceneObject>();
        List<MyVector3> switchNodeList = new List<MyVector3>();
        List<MyVector3> groundEnemyNodeList = new List<MyVector3>();
        List<MyVector3> flyingEnemyNodeList = new List<MyVector3>();
        List<MyVector3> ceilingEnemyNodeList = new List<MyVector3>();
        SceneObject playerSpawnPointData;

        // Buscar dentro del _sceneHolder los objetos que tienen ciertas tags

        GameObject[] sceneObjects = FindChildObjectsWithTag(_sceneHolder, "SceneObject");
        GameObject[] switchNodes = FindChildObjectsWithTag(_sceneHolder, "SwitchNode");
        GameObject[] groundEnemyNodes = FindChildObjectsWithTag(_sceneHolder, "GroundEnemyNode");
        GameObject[] flyingEnemyNodes = FindChildObjectsWithTag(_sceneHolder, "FlyingEnemyNode");
        GameObject[] ceilingEnemyNodes = FindChildObjectsWithTag(_sceneHolder, "CeilingEnemyNode");
        GameObject playerSpawnPoint = FindChildObjectsWithTag(_sceneHolder, "PlayerSpawnPoint")[0];

        // Guardar la informaci�n de los objetos de la escena

        foreach (GameObject sceneObject in sceneObjects)
        {
            Vector3 position = sceneObject.transform.localPosition;
            Vector3 rotation = sceneObject.transform.localRotation.eulerAngles;
            Vector3 scale = sceneObject.transform.localScale;
            SceneObject data = new SceneObject
            {
                ID = sceneObject.name,
                Position = new MyVector3 { X = position.x, Y = position.y, Z = position.z },
                Rotation = new MyVector3 { X = rotation.x, Y = rotation.y, Z = rotation.z },
                Scale = new MyVector3 { X = scale.x, Y = scale.y, Z = scale.z }
            };
            sceneObjectList.Add(data);
        }

        // Guardar la informaci�n de los puntos de spawn de los interruptores

        foreach (GameObject switchNode in switchNodes)
        {
            Vector3 position = switchNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            switchNodeList.Add(data);
        }

        // Guardar la informaci�n de los puntos de spawn de los enemigos terrestres

        foreach (GameObject groundEnemyNode in groundEnemyNodes)
        {
            Vector3 position = groundEnemyNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            groundEnemyNodeList.Add(data);
        }

        // Guardar la informaci�n de los puntos de spawn de los enemigos voladores

        foreach (GameObject flyingEnemyNode in flyingEnemyNodes)
        {
            Vector3 position = flyingEnemyNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            flyingEnemyNodeList.Add(data);
        }

        // Guardar la informaci�n de los puntos de spawn de los enemigos que reptan por el techo

        foreach (GameObject ceilingEnemyNode in ceilingEnemyNodes)
        {
            Vector3 position = ceilingEnemyNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            ceilingEnemyNodeList.Add(data);
        }

        // Guardar la informaci�n de los puntos de spawn del jugador

        Vector3 playerPosition = playerSpawnPoint.transform.localPosition;
        Vector3 playerRotation = playerSpawnPoint.transform.localRotation.eulerAngles;
        Vector3 playerScale = playerSpawnPoint.transform.localScale;
        SceneObject playerData = new SceneObject
        {
            ID = playerSpawnPoint.name,
            Position = new MyVector3 { X = playerPosition.x, Y = playerPosition.y, Z = playerPosition.z },
            Rotation = new MyVector3 { X = playerRotation.x, Y = playerRotation.y, Z = playerRotation.z },
            Scale = new MyVector3 { X = playerScale.x, Y = playerScale.y, Z = playerScale.z }
        };
        playerSpawnPointData = playerData;

        // Crear un objeto Json que contiene las listas con la informaci�n del nivel

        var sceneData = new
        {
            SceneObjects = sceneObjectList,
            SwitchNodes = switchNodeList,
            GroundEnemyNodes = groundEnemyNodeList,
            FlyingEnemyNodes = flyingEnemyNodeList,
            CeilingEnemyNodes = ceilingEnemyNodeList,
            PlayerSpawnPoint = playerSpawnPointData,
            TotalSwitches = _totalSwitches
        };

        // Convertir el objeto Json a una cadena Json
        string jsonData = JsonConvert.SerializeObject(sceneData, Newtonsoft.Json.Formatting.Indented);

        // Guardar la cadena Json en un archivo
        File.WriteAllText(Application.dataPath + "/Scripts/GameManager/LevelJsons/newSceneData.json", jsonData);

        Debug.Log("Datos de la escena guardados como Json.");

        yield return null;
    }

    /// <summary>
    /// Devuelve un array con todos los hijos de parent que tengan cierta tag
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    private GameObject[] FindChildObjectsWithTag(GameObject parent, string tag)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true); // Incluye componentes inactivos

        // Filtra los hijos por la etiqueta
        GameObject[] childrenWithTag = System.Array.FindAll(children, child => child != parent && child.CompareTag(tag))
            .Select(child => child.gameObject)
            .ToArray();

        return childrenWithTag;
    }
}
