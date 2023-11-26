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
    /// Devuelve un array con 3 vectores con la posición, rotación y escala respectivamente
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
    public List<MyVector3> ZanybellNodes;
    public List<MyVector3> EvilBunnyNodes;
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
    [SerializeField] private int _totalSwitches = 3; // Número total de interruptores que va a tener el nivel


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
        // Crear las listas donde se va a guardar la información del nivel

        List<SceneObject> sceneObjectList = new List<SceneObject>();
        List<MyVector3> switchNodeList = new List<MyVector3>();
        List<MyVector3> zanybellNodeList = new List<MyVector3>();
        List<MyVector3> evilBunnyNodeList = new List<MyVector3>();
        SceneObject playerSpawnPointData;

        // Buscar dentro del _sceneHolder los objetos que tienen ciertas tags

        GameObject[] sceneObjects = FindChildObjectsWithTag(_sceneHolder, "SceneObject");
        GameObject[] switchNodes = FindChildObjectsWithTag(_sceneHolder, "SwitchNode");
        GameObject[] zanybellNodes = FindChildObjectsWithTag(_sceneHolder, "ZanybellNode");
        GameObject[] evilBunnyNodes = FindChildObjectsWithTag(_sceneHolder, "EvilBunnyNode");
        GameObject playerSpawnPoint = FindChildObjectsWithTag(_sceneHolder, "PlayerSpawnPoint")[0];

        // Guardar la información de los objetos de la escena

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

        // Guardar la información de los puntos de spawn de los interruptores

        foreach (GameObject switchNode in switchNodes)
        {
            Vector3 position = switchNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            switchNodeList.Add(data);
        }

        // Guardar la información de los puntos de spawn de los zanybell

        foreach (GameObject zanybellNode in zanybellNodes)
        {
            Vector3 position = zanybellNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            zanybellNodeList.Add(data);
        }

        // Guardar la información de los puntos de spawn de los evil bunny

        foreach (GameObject evilBunnyNode in evilBunnyNodes)
        {
            Vector3 position = evilBunnyNode.transform.localPosition;
            MyVector3 data = new MyVector3 { X = position.x, Y = position.y, Z = position.z };
            evilBunnyNodeList.Add(data);
        }

        // Guardar la información de los puntos de spawn del jugador

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

        // Crear un objeto Json que contiene las listas con la información del nivel

        var sceneData = new
        {
            SceneObjects = sceneObjectList,
            SwitchNodes = switchNodeList,
            ZanybellNodes = zanybellNodeList,
            EvilBunnyNodes = evilBunnyNodeList,
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
