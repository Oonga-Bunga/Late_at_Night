using eDmitriyAssets.NavmeshLinksGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    private static SceneGenerator _instance;
    public static SceneGenerator Instance => _instance;

    //in
    [SerializeField] private TextAsset _sceneJsonFile;
    public TextAsset SceneJsonFile
    {
        get { return _sceneJsonFile; }
        set { _sceneJsonFile = value; }
    }

    [SerializeField] private GameObject _sceneHolder;
    public GameObject SceneHolder => _sceneHolder;

    [SerializeField] private List<GameObject> _prefabsList;
    [SerializeField] private GameObject _switchPrefab;
    private Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();

    //out
    private List<Vector3> _switchSpawnLocations = new List<Vector3>();
    private List<Vector3> _flyingEnemySpawnLocations = new List<Vector3>();
    private List<Vector3> _groundedEnemySpawnLocations = new List<Vector3>();
    private Transform _playerSpawnPoint;

    public EventHandler<bool> OnSceneLoaded;

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
        if (JsonGenerator.Instance != null)
        {
            if (JsonGenerator.Instance.GenerateJson)
            {
                Debug.Log("Can't generate scene because a JsonGenerator is creating a new json from the scene");
                return;
            }
        }

        if (LevelJsons.Instance != null)
        {
            _sceneJsonFile = LevelJsons.Instance.SceneJsonFile;
        }

        StartCoroutine(LoadPrefabs());
    }

    private IEnumerator LoadPrefabs()
    {
        foreach (var propPrefab in _prefabsList)
        {
            _prefabDictionary[propPrefab.name] = propPrefab;
        }

        StartCoroutine(LoadSceneFromJson());

        yield return null;
    }

    private IEnumerator LoadSceneFromJson()
    {
        string jsonString = _sceneJsonFile.text;

        SceneData sceneData = JsonUtility.FromJson<SceneData>(jsonString);

        Debug.Log("Generating: ");

        // Ahora puedes acceder a los datos como objetos C#
        foreach (var sceneObject in sceneData.SceneObjects)
        {
            Vector3 position = new Vector3(sceneObject.Position.X, sceneObject.Position.Y, sceneObject.Position.Z);
            Vector3 rotation = new Vector3(sceneObject.Rotation.X, sceneObject.Rotation.Y, sceneObject.Rotation.Z);
            Vector3 scale = new Vector3(sceneObject.Scale.X, sceneObject.Scale.Y, sceneObject.Scale.Z);

            if (_prefabDictionary.ContainsKey(sceneObject.ID))
            {
                Debug.Log("Generating: " + sceneObject.ID);

                GameObject sceneObjectPrefab = _prefabDictionary[sceneObject.ID];
                GameObject sceneObjectInstance = Instantiate(sceneObjectPrefab, Vector3.zero, Quaternion.identity, _sceneHolder.transform);
                sceneObjectInstance.transform.localScale = scale;
                sceneObjectInstance.transform.localPosition = position;
                sceneObjectInstance.transform.localRotation = Quaternion.Euler(rotation);
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

        Debug.Log("Reading player spawn point");

        var playerSpawnPointData = sceneData.PlayerSpawnPoint;

        Vector3 playerPosition = new Vector3(playerSpawnPointData.Position.X, playerSpawnPointData.Position.Y, playerSpawnPointData.Position.Z);
        Vector3 playerRotation = new Vector3(playerSpawnPointData.Rotation.X, playerSpawnPointData.Rotation.Y, playerSpawnPointData.Rotation.Z);
        Vector3 playerScale = new Vector3(playerSpawnPointData.Scale.X, playerSpawnPointData.Scale.Y, playerSpawnPointData.Scale.Z);

        GameObject emptyGameObject = new GameObject("EmptyGameObject");
        emptyGameObject.transform.SetParent(_sceneHolder.transform);
        emptyGameObject.transform.localScale = playerScale;
        emptyGameObject.transform.localPosition = playerPosition;
        emptyGameObject.transform.localRotation = Quaternion.Euler(playerRotation);

        _playerSpawnPoint = emptyGameObject.transform;

        Debug.Log("Sending data to game manager and generating switches");

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
        }
        else
        {
            Debug.Log("There is no game manager and/or enemy waves manager in the scene");
        }

        int nSwitches = Mathf.Min(_switchSpawnLocations.Count, sceneData.TotalSwitches);
        int randomPos;

        for (int i = 0; i < nSwitches; i++)
        {
            randomPos = UnityEngine.Random.Range(0, _switchSpawnLocations.Count);
            GameObject switchInstance = Instantiate(_switchPrefab, Vector3.zero, Quaternion.identity);
            switchInstance.transform.SetParent(_sceneHolder.transform);
            switchInstance.transform.localPosition = _switchSpawnLocations[randomPos];

            _switchSpawnLocations.RemoveAt(randomPos);
        }

        Debug.Log("Baking the NavMesh");

        _sceneHolder.GetComponent<NavMeshSurface>().BuildNavMesh();
        _sceneHolder.GetComponent<NavMeshLinks_AutoPlacer>().Generate();

        yield return new WaitForSeconds(2f);
        OnSceneLoaded?.Invoke(this, true);

        yield return null;
    }
}
