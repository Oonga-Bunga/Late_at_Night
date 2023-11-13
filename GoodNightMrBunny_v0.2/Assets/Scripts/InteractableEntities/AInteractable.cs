using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AInteractable : MonoBehaviour, IInteractable
{
    #region Attributes

    [Header("AInteractable Settings")]

    [SerializeField] protected IInteractable.InteractType _interactType = IInteractable.InteractType.Press; // Forma en la que se puede interactuar con este objeto, con algunos basta con presionar la tecla de interactuar, con otros hay que mantenerla pulsada y algunos tienen dos acciones distintas dependiendo de si solo se ha pulsado o si se ha mantenido

    // El siguiente grupo de atributos solo sirve para AInteractable de tipo Hold o PressAndHold
    protected float _currentHoldTime = 0f; // Tiempo que lleva el jugador presionando el bot�n de interactuar con este objeto
    [SerializeField] protected float _holdDuration; // Tiempo que debe presionarse el bot�n de interactuar para llevar a cabo la acci�n de este objeto
    [SerializeField] protected float _pressBuffer; // Tiempo de pulsado a partir del cual se considera como hold en vez de press
    protected bool _isBeingInteracted = false; // Si el objeto est� siendo interactuado por el jugador
    
    protected bool _canBeInteracted = true; // Si el objeto puede ser interactuado

    protected PlayerController _player; // Referencia al jugador
    [SerializeField] protected GameObject _outlineHolder = null; // Referencia al objeto que tiene el _outline y con el que debe interactuar el jugador
    [SerializeField] protected Outline _outline; // Referencia al _outline
    [SerializeField] protected Canvas _promptCanvas; // Referencia al canvas del button prompt
    [SerializeField] protected Image _radialBar; // Referencia a la barra radial del button prompt

    protected PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego

    public bool CanBeInteracted => _canBeInteracted;

    #endregion

    #region Initialization

    protected virtual void Start()
    {
        _player = PlayerController.Instance;
        _promptCanvas.enabled = false;
        _radialBar.fillAmount = 0f;
    }

    #endregion

    #region Updates

    /// <summary>
    /// Si el objeto est� siendo interactuado se incrementa el temporizador _currentHoldTime, y si supera cierto tiempo 
    /// se ejecuta InteractedHoldAction
    /// Tambi�n actualiza barra radial
    /// </summary>
    protected virtual void Update()
    {
        if (_isBeingInteracted)
        {
            _currentHoldTime += Time.deltaTime;
            
            if (_currentHoldTime >= _holdDuration)
            {
                _isBeingInteracted = false;
                _currentHoldTime = 0;
                InteractedHoldAction();
            }
        }

        if (_currentHoldTime < _pressBuffer)
        {
            _radialBar.fillAmount = 0;
        }
        else
        {
            _radialBar.fillAmount = _currentHoldTime / _holdDuration;
        }
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Llama a un m�todo u otro dependiendo del input del jugador
    /// </summary>
    /// <param name="interactInput"></param>
    public virtual void Interacted(IPlayerReceiver.InputType interactInput)
    {
        if (interactInput == IPlayerReceiver.InputType.Down)
        {
            // Si el objeto es de tipo Press ejecuta InteractedPressAction, sino _isBeingInteracted pasa a ser true

            if (_canBeInteracted)
            {
                if (!(_interactType == IInteractable.InteractType.Press))
                {
                    _isBeingInteracted = true;
                }
                else
                {
                    InteractedPressAction();
                }
            }
        }
        else
        {
            // Si el objeto es de tipo PressAndHold y la duraci�n de la pulsaci�n ha sido muy corta se ejecuta InteractedPressAction

            if (_interactType == IInteractable.InteractType.PressAndHold && _currentHoldTime < _pressBuffer && _isBeingInteracted)
            {
                InteractedPressAction();
            }

            _isBeingInteracted = false;
            _currentHoldTime = 0;
        }
    }

    /// <summary>
    /// Lo llama el jugador cuando sale fuera del rango de interactuaci�n con este objeto o se ha acercado m�s a otro
    /// </summary>
    public virtual void PlayerExitedRange()
    {
        _isBeingInteracted = false;
        _currentHoldTime = 0;
    }

    /// <summary>
    /// Acci�n que se ejecuta tras un press del jugador
    /// </summary>
    protected virtual void InteractedPressAction()
    {
        
    }

    /// <summary>
    /// Acci�n que se ejecuta tras un hold del jugador durante cierto tiempo
    /// </summary>
    protected virtual void InteractedHoldAction()
    {

    }

    #endregion

    #region Other Methods

    /// <summary>
    /// Activa el ouline y el canvas
    /// </summary>
    public virtual void EnableOutlineAndCanvas()
    {
        if (!_outline.enabled)
        {
            _outline.enabled = true;
        }
        if (!_promptCanvas.enabled && _canBeInteracted)
        {
            _promptCanvas.enabled = true;
        }
    }

    /// <summary>
    /// Desactiva _outline y el canvas
    /// </summary>
    public virtual void DisableOutlineAndCanvas()
    {
        if (_outline.enabled)
        {
            _outline.enabled = false;
        }
        if (_promptCanvas.enabled)
        {
            _promptCanvas.enabled = false;
        }
    }

    public virtual void Initialize(float value)
    {

    }

    #endregion
}
