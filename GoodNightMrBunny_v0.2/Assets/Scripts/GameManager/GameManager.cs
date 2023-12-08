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

    private Transform _playerSpawnPoint;

    public Transform PlayerSpawnPoint{ set => _playerSpawnPoint = value; }

    private static List<Switch> _switchListInstance = new List<Switch>();
    public static List<Switch> SwitchListInstance => _switchListInstance;

    private static List<FlashlightRechargeStation> _rechargeStationListInstance = new List<FlashlightRechargeStation>();
    public static List<FlashlightRechargeStation> RechargeStationListInstance => _rechargeStationListInstance;

    [SerializeField] private GameObject _playerInstance;
    [SerializeField] private GameObject _playerPrefab;

    //[SerializeField] private float _maxTime = 60f;
    //private float _currentTime;

    private PauseManager _pauseManager;
    [SerializeField] private LevelMenuManager _levelMenuManager;
    private bool _isInGame = false;
    private bool _isReady = false;
    public event Action OnGameStarted;
    private int _currentActivatedSwitches = 0;
    public event Action<float> OnTimeChanged;

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
    }

    private void Start()
    {
        LevelGenerator levelGenerator = LevelGenerator.Instance;
        EnemyWavesManager waveManager = EnemyWavesManager.Instance;

        if (levelGenerator != null)
        {
            levelGenerator.OnLevelLoaded += () => StartCoroutine(GenerateLevel());
        }

        if (waveManager != null)
        {
            waveManager.OnAllWavesDefeated += () => PlayerWon();
        }

        _pauseManager = PauseManager.Instance;
    }

    private IEnumerator GenerateLevel()
    {
        #region Search key objects

        Debug.Log("Searching for switches");

        List<Switch> tempSwitchList = new List<Switch>();

        foreach (Switch switchComponent in LevelGenerator.Instance.LevelHolder.transform.GetComponentsInChildren<Switch>())
        {
            switchComponent.OnTurnedOnOrOff += SwitchChangedState;
            tempSwitchList.Add(switchComponent);
            if (switchComponent.IsOn)
            {
                _currentActivatedSwitches++;
            }
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
            baby.OnDied += () => PlayerLost();
        }
        else
        {
            Debug.Log("No baby found");
        }

        #endregion

        #region Instantiate player

        Debug.Log("Positioning _player");

        if (_playerInstance == null)
        {
            _playerInstance = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        }
        _playerInstance.transform.SetParent(LevelGenerator.Instance.LevelHolder.transform);
        _playerInstance.transform.localPosition = _playerSpawnPoint.localPosition;
        _playerInstance.transform.localRotation = _playerSpawnPoint.localRotation;

        #endregion

        #region Wait a bit and start game

        Debug.Log("Loading finished!");

        yield return new WaitForSeconds(2);

        _isReady = true;
        // StartGame();

        //quitar estas 2 cosas después
        OnGameStarted?.Invoke();
        _isInGame = true;

        #endregion

        yield return null;
    }

    public void StartGame()
    {
        if (_isReady)
        {
            // if booleano del script del cuento es true
            OnGameStarted?.Invoke();
            _isInGame = true;
        }
    }

    #endregion

    #region Update

    void Update()
    {
        if (!_isInGame) return;

        if (_pauseManager.IsPaused) return;
        /*
        _currentTime -= Time.deltaTime;
        OnTimeChanged?.Invoke(_currentTime);

        if (_currentTime < 0)
        {
            PlayerWon();
        }
        */
    }

    #endregion

    #region Methods

    private void SwitchChangedState(object obj, bool isOn)
    {
        if (isOn)
        {
            _currentActivatedSwitches++;
            //_upperText.text = $"{_currentActivatedSwitches}/{_totalSwitches} interruptores";
            Invoke("ResetText", 3);
        }
        else
        {
            _currentActivatedSwitches--;
            //_upperText.text = $"{_currentActivatedSwitches}/{_totalSwitches} interruptores";
            Invoke("ResetText", 3);

            if (_currentActivatedSwitches == 0)
            {
                PlayerLost();
            }
        }
    }
    
    private void ResetText()
    {
        //_upperText.text = "";
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
