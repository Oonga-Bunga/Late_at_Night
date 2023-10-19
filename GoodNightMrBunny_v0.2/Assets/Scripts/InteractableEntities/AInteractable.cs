using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class AInteractable : MonoBehaviour, IInteractable
{
    #region Attributes

    [Header("AInteractable Settings")]

    [SerializeField] protected IInteractable.InteractType interactType = IInteractable.InteractType.Press; // Forma en la que se puede interactuar con este objeto, con algunos basta con presionar la tecla de interactuar, con otros hay que mantenerla pulsada y algunos tienen dos acciones distintas dependiendo de si solo se ha pulsado o si se ha mantenido

    // El siguiente grupo de atributos solo sirve para AInteractable de tipo Hold o PressAndHold
    protected float currentHoldTime = 0f; // Tiempo que lleva el jugador presionando el botón de interactuar con este objeto
    [SerializeField] protected float holdDuration; // Tiempo que debe presionarse el botón de interactuar para llevar a cabo la acción de este objeto
    [SerializeField] protected float pressBuffer; // Tiempo de pulsado a partir del cual se considera como hold en vez de press
    protected bool isBeingInteracted = false; // Si el objeto está siendo interactuado por el jugador
    
    protected bool canBeInteracted = true; // Si el objeto puede ser interactuado

    protected PlayerController player; // Referencia al jugador
    protected Outline outline; // Referencia al outline
    protected Canvas promptCanvas; // Referencia al canvas del button prompt
    protected Image radialBar; // Referencia a la barra radial del button prompt

    protected PauseManager pauseManager;

    public bool CanBeInteracted
    {
        get { return canBeInteracted; }
    }

    #endregion

    #region Initialization

    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        outline = GetComponent<Outline>();
        promptCanvas = transform.GetChild(0).gameObject.GetComponentInChildren<Canvas>();
        radialBar = promptCanvas.gameObject.GetComponentInChildren<Image>();
        promptCanvas.enabled = false;
        radialBar.fillAmount = currentHoldTime / holdDuration;
    }

    #endregion

    #region Updates

    /// <summary>
    /// Si el objeto está siendo interactuado se incrementa el temporizador currentHoldTime, y si supera cierto tiempo 
    /// se ejecuta InteractedHoldAction
    /// También actualiza barra radial
    /// </summary>
    protected virtual void Update()
    {
        if (isBeingInteracted)
        {
            currentHoldTime += Time.deltaTime;
            
            if (currentHoldTime >= holdDuration)
            {
                isBeingInteracted = false;
                currentHoldTime = 0;
                InteractedHoldAction();
            }
        }

        if (currentHoldTime < pressBuffer)
        {
            radialBar.fillAmount = 0;
        }
        else
        {
            radialBar.fillAmount = currentHoldTime / holdDuration;
        }
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Llama a un método u otro dependiendo del input del jugador
    /// </summary>
    /// <param name="interactInput"></param>
    public virtual void Interacted(IPlayerReceiver.InputType interactInput)
    {
        if (interactInput == IPlayerReceiver.InputType.Down)
        {
            // Si el objeto es de tipo Press ejecuta InteractedPressAction, sino isBeingInteracted pasa a ser true

            if (canBeInteracted)
            {
                if (!(interactType == IInteractable.InteractType.Press))
                {
                    isBeingInteracted = true;
                }
                else
                {
                    InteractedPressAction();
                }
            }
        }
        else
        {
            // Si el objeto es de tipo PressAndHold y la duración de la pulsación ha sido muy corta se ejecuta InteractedPressAction

            if (interactType == IInteractable.InteractType.PressAndHold && currentHoldTime < pressBuffer && isBeingInteracted)
            {
                InteractedPressAction();
            }

            isBeingInteracted = false;
            currentHoldTime = 0;
        }
    }

    /// <summary>
    /// Lo llama el jugador cuando sale fuera del rango de interactuación con este objeto o se ha acercado más a otro
    /// </summary>
    public virtual void PlayerExitedRange()
    {
        isBeingInteracted = false;
        currentHoldTime = 0;
    }

    /// <summary>
    /// Acción que se ejecuta tras un press del jugador
    /// </summary>
    protected virtual void InteractedPressAction()
    {
        
    }

    /// <summary>
    /// Acción que se ejecuta tras un hold del jugador durante cierto tiempo
    /// </summary>
    protected virtual void InteractedHoldAction()
    {

    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Activa el ouline y el canvas
    /// </summary>
    public virtual void EnableOutlineAndCanvas()
    {
        if (!outline.enabled)
        {
            outline.enabled = true;
        }
        if (!promptCanvas.enabled && canBeInteracted)
        {
            promptCanvas.enabled = true;
        }
    }

    /// <summary>
    /// Desactiva outline y el canvas
    /// </summary>
    public virtual void DisableOutlineAndCanvas()
    {
        if (outline.enabled)
        {
            outline.enabled = false;
        }
        if (promptCanvas.enabled)
        {
            promptCanvas.enabled = false;
        }
    }

    #endregion
}
