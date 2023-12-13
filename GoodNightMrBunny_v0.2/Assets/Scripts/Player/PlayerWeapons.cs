using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Holdable Objects")]
    [SerializeField] private List<AHoldableObject> _holdableObjectList; // Lista con los objetos que puede llevar encima el jugador
    private AHoldableObject _currentHeldObject; // SceneObject que lleva encima en cada momento
    private Transform _cameraHolder; // Referencia a la cámara principal

    [Header("Dropping")]
    [SerializeField] private float _dropDistance = 3.5f; // Distancia a la cual el jugador suelta el objeto
    [SerializeField] private float _sphereRaycastRadius = 0.3f; // Radio de la esfera que se usa para detectar si el lugar donde se suelta el objeto está obstruido
    [SerializeField] private float _minimumDistanceFromCollision = 0.3f; // Distancia desde la obstrucción donde se dropea el objeto
    [SerializeField] private LayerMask _groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    private PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego
    public event Action OnPick;
    public event Action OnDrop;

    public AHoldableObject CurrentHeldObject => _currentHeldObject;

    // Start is called before the first frame update
    private void Awake()
    {
        _currentHeldObject = _holdableObjectList[0];
    }

    private void Start()
    {
        _pauseManager = PauseManager.Instance;
        _cameraHolder = Camera.main.transform;
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de ataque, llama al m�todo UseHeldObject del objeto 
    /// que posee el jugador en ese momento
    /// </summary>
    /// <param name="useInput">Tipo de input, Down, Hold o Up</param>
    public void UseHeldObject(IPlayerReceiver.InputType useInput)
    {
        if (_pauseManager.IsPaused || !GameManager.Instance.IsInGame) return;

        _currentHeldObject.Use(useInput);
    }

    /// <summary>
    /// El jugador suelta en el suelo el objeto que llevaba encima, y pasa a llevar el objeto que recive como
    /// par�metro, que puede inicializarse con cierto valor
    /// </summary>
    /// <param name="objectType">Tipo de objeto al que cambia el jugador</param>
    /// <param name="dropPrefab">Si al cambiar de objeto se instancia el prefab del pickup del anterior</param>
    /// <param name="initializationValue">Valor con el que se inicia el arma, como la carga de la linterna</param>
    public void ChangeHeldObject(IPlayerReceiver.HoldableObjectType objectType, bool dropPrefab, float initializationValue = -1)
    {
        _currentHeldObject.Drop(dropPrefab, _dropDistance, _sphereRaycastRadius, _minimumDistanceFromCollision, _groundLayer);

        AHoldableObject newHeldObject = _holdableObjectList[0];

        switch (objectType)
        {
            case IPlayerReceiver.HoldableObjectType.Flashlight:
                newHeldObject = _holdableObjectList[1];
                break;
            case IPlayerReceiver.HoldableObjectType.ClayBalls:
                newHeldObject = _holdableObjectList[2];
                break;
            case IPlayerReceiver.HoldableObjectType.ClayBin:
                newHeldObject = _holdableObjectList[3];
                break;
        }
        newHeldObject.gameObject.SetActive(true);
        _currentHeldObject = newHeldObject;

        if (initializationValue != -1)
        {
            _currentHeldObject.Initialize(initializationValue);
        }

        OnPick?.Invoke();
    }

    /// <summary>
    /// Es similar al m�todo ChangeHeldObject, la �nica diferencia es que el objeto al que cambia el jugador es el vac�o
    /// </summary>
    /// <param name="force">Fuerza con la que se suelta el objeto</param>
    public void DropHeldObject()
    {
        if (_pauseManager.IsPaused || !GameManager.Instance.IsInGame || _currentHeldObject.HoldableObjectType == IPlayerReceiver.HoldableObjectType.None) return;

        _currentHeldObject.Drop(true, _dropDistance, _sphereRaycastRadius, _minimumDistanceFromCollision, _groundLayer);
        _holdableObjectList[0].gameObject.SetActive(true);
        _currentHeldObject = _holdableObjectList[0];
        OnDrop?.Invoke();
    }
}
