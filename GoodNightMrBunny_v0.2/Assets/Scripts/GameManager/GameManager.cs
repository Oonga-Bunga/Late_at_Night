using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI_Scripts.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Attributes

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private TextMeshProUGUI _loadingText;

    private Transform _playerSpawnPoint;

    public Transform PlayerSpawnPoint
    {
        get { return _playerSpawnPoint; }
        set { _playerSpawnPoint = value; }
    }

    private static List<Switch> _switchListInstance = new List<Switch>();
    public static List<Switch> SwitchListInstance => _switchListInstance;

    private int _totalSwitches = 0;

    private static List<FlashlightRechargeStation> _rechargeStationListInstance = new List<FlashlightRechargeStation>();
    public static List<FlashlightRechargeStation> RechargeStationListInstance => _rechargeStationListInstance;

    [SerializeField] private GameObject _playerInstance;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Camera _loadingScreenCamera;

    [SerializeField] private GameObject _mobileControls;
    [SerializeField] private float _maxTime = 60f;
    private float _currentTime;
    [SerializeField] private GameObject _gameUI;

    private PauseManager _pauseManager;
    private bool _isInGame = false;
    public EventHandler<bool> OnGameStarted;
    private int _currentActivatedSwitches = 0;
    [SerializeField] private TextMeshProUGUI _upperText;
    public EventHandler<float> OnTimeChanged;

    public bool IsInGame => _isInGame;
    
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

        SceneGenerator sceneGenerator = SceneGenerator.Instance;

        if (sceneGenerator != null)
        {
            sceneGenerator.OnSceneLoaded += (object sender, bool value) => StartCoroutine(GenerateLevel());
        }
    }

    private void Start()
    {
        _pauseManager = PauseManager.Instance;
    }

    private IEnumerator GenerateLevel()
    {
        #region Search key objects

        Debug.Log("Searching for switches");

        List<Switch> tempSwitchList = new List<Switch>();

        foreach (Switch switchComponent in SceneGenerator.Instance.transform.GetComponentsInChildren<Switch>())
        {
            switchComponent.OnTurnedOnOrOff += SwitchChangedState;
            tempSwitchList.Add(switchComponent);
            _totalSwitches++;
        }

        _switchListInstance = tempSwitchList;

        Debug.Log("Searching for flashlight recharge stations");

        List<FlashlightRechargeStation> tempRechargeStationList = new List<FlashlightRechargeStation>();

        foreach (FlashlightRechargeStation rechargeStation in FindObjectsOfType<FlashlightRechargeStation>())
        {
            tempRechargeStationList.Add(rechargeStation);
        }

        _rechargeStationListInstance = tempRechargeStationList;

        Debug.Log("Searching baby");

        Baby baby = FindObjectOfType<Baby>();

        if (baby != null)
        {
            baby.Died += BabyDied;
        }
        else
        {
            Debug.Log("No baby found");
        }

        #endregion

        #region Detect platform

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

        #endregion

        #region Instantiate player

        Debug.Log("Positioning player");

        if (_playerInstance == null)
        {
            _playerInstance = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        }
        _playerInstance.transform.SetParent(SceneGenerator.Instance.SceneHolder.transform);
        _playerInstance.transform.localPosition = _playerSpawnPoint.localPosition;
        _playerInstance.transform.localRotation = _playerSpawnPoint.localRotation;

        #endregion

        #region Disable loading screen and switch camera

        _loadingText.text = "Loading finished!";
        Debug.Log("Loading finished!");

        yield return new WaitForSeconds(2);

        _currentTime = _maxTime;
        _loadingScreen.SetActive(false);
        _loadingScreenCamera.enabled = false;
        //Mostrar instrucciones de nivel (Comentar en caso de quere quitar)
        try
        {
            FindObjectOfType<LevelMenuManager>().OpenInstructionsPanel();
        }
        catch {}

        #endregion

        #region Start game

        _gameUI.SetActive(true);
        //_pauseManager.PauseGame();
        _isInGame = true;
        OnGameStarted?.Invoke(this, true);

        #endregion

        yield return null;
    }

    #endregion

    #region Update

    void Update()
    {
        if (!_isInGame) return;

        if (_pauseManager.IsPaused) return;

        _currentTime -= Time.deltaTime;
        OnTimeChanged?.Invoke(this, _currentTime);

        if (_currentTime < 0)
        {
            PlayerWon();
        }
    }

    #endregion

    #region Methods

    private void BabyDied(object sender, bool value)
    {
        PlayerLost();
    }

    private void SwitchChangedState(object sender, bool isOn)
    {
        if (isOn)
        {
            _currentActivatedSwitches++;
            _upperText.text = $"{_currentActivatedSwitches}/{_totalSwitches} interruptores";
            Invoke("ResetText", 3);

            if (_currentActivatedSwitches == _totalSwitches)
            {
                PlayerWon();
            }
        }
        else
        {
            _currentActivatedSwitches--;
            _upperText.text = $"{_currentActivatedSwitches}/{_totalSwitches} interruptores";
            Invoke("ResetText", 3);
        }
    }
    
    private void ResetText()
    {
        _upperText.text = "";
    }

    private void PlayerWon()
    {
        StopAllCoroutines();
        _isInGame = false;
        SceneManager.LoadScene("WinScene");
    }

    private void PlayerLost()
    {
        StopAllCoroutines();
        _isInGame = false;
        SceneManager.LoadScene("LoseScene");
    }

    #endregion
}
