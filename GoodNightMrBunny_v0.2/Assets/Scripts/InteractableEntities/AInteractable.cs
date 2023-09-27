using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class AInteractable : MonoBehaviour, IInteractable
{
    #region Attributes

    [SerializeField] protected IInteractable.InteractType interactType = IInteractable.InteractType.Press; // Forma en la que se puede interactuar con este objeto, con algunos basta con presionar la tecla de interactuar, con otros hay que mantenerla pulsada y algunos tienen dos acciones distintas dependiendo de si solo se ha pulsado o si se ha mantenido

    // El siguiente grupo de atributos solo sirve para AInteractable de tipo Hold o PressAndHold
    protected float currentHoldTime = 0; // Tiempo que lleva el jugador presionando el bot�n de interactuar con este objeto
    [SerializeField] protected float holdDuration = 3f; // Tiempo que debe presionarse el bot�n de interactuar para llevar a cabo la acci�n de este objeto
    [SerializeField] protected float pressBuffer = 0.2f; // Tiempo de pulsado a partir del cual se considera como hold en vez de press
    protected bool isBeingInteracted = false; // Si el objeto est� siendo interactuado por el jugador
    
    protected bool canBeInteracted = true; // Si el objeto puede ser interactuado
    protected PlayerController player; // Referencia al jugador
    protected Outline outline; // Referencia al outline

    #endregion

    #region Initialization

    public virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    #endregion

    #region Updates

    /// <summary>
    /// Si el objeto est� siendo interactuado se incrementa el temporizador currentHoldTime, y si supera cierto tiempo 
    /// se ejecuta InteractedHoldAction
    /// </summary>
    protected virtual void Update()
    {
        if (isBeingInteracted)
        {
            currentHoldTime += Time.deltaTime;

            if (currentHoldTime >= holdDuration)
            {
                currentHoldTime = 0;
                InteractedHoldAction();
            }
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
            InteractedDown();
        }
        else
        {
            InteractedUp();
        }
    }

    /// <summary>
    /// Si el tipo de interacci�n es press ejecuta InteractedPressAction inmediatamente, si es cualquier otro
    /// isBeingInteracted pasa a ser true para registrar que el jugador est� interactuando con el objeto
    /// </summary>
    public virtual void InteractedDown()
    {
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

    /// <summary>
    /// El objeto deja de estar siendo interactuado y se resetea el temporizador currentHoldTime.
    /// Adem�s, si el tipo de interacci�n es press and hold y se ha levantado el bot�n antes de que pase cierto
    /// tiempo se ejecuta InteractedPressAction
    /// </summary>
    public virtual void InteractedUp()
    {
        isBeingInteracted = false;
        currentHoldTime = 0;

        if (interactType == IInteractable.InteractType.PressAndHold && currentHoldTime < pressBuffer)
        {
            InteractedPressAction();
        }
    }

    /// <summary>
    /// Lo llama el jugador cuando sale fuera del rango de interactuaci�n con este objeto o se ha acercado m�s a otro
    /// </summary>
    public virtual void PlayerExitedRange()
    {
        isBeingInteracted = false;
        currentHoldTime = 0;
    }

    /// <summary>
    /// Acci�n que se ejecuta tras un press del jugador
    /// </summary>
    public virtual void InteractedPressAction()
    {
        
    }

    /// <summary>
    /// Acci�n que se ejecuta tras un hold del jugador durante cierto tiempo
    /// </summary>
    public virtual void InteractedHoldAction()
    {

    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Activa el outline
    /// </summary>
    public virtual void EnableOutline()
    {
        outline.enabled = true;
    }

    /// <summary>
    /// Desactiva el outline
    /// </summary>
    public virtual void DisableOutline()
    {
        outline.enabled = false;
    }

    #endregion
}
