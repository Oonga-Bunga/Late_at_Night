using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class AInteractable : MonoBehaviour, IInteractable
{
    protected PlayerController player;
    protected Outline outline;
    protected float currentHoldTime = 0;
    [SerializeField] protected float pressBuffer = 0.2f;
    [SerializeField] protected float holdDuration = 3f;
    [SerializeField] public IInteractable.InteractType interactType = IInteractable.InteractType.Press;
    [SerializeField] protected bool isBeingInteracted = false;
    [SerializeField] protected bool canBeInteracted = true;

    public virtual void Awake()
    {
        outline = GetComponent<Outline>();
        player = FindAnyObjectByType<PlayerController>();
    }

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

    public virtual void Interacted(PlayerController player, IPlayerReceiver.InputType interactInput)
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

    public virtual void InteractedUp()
    {
        isBeingInteracted = false;
        currentHoldTime = 0;

        if (interactType == IInteractable.InteractType.PressAndHold && currentHoldTime < pressBuffer)
        {
            InteractedPressAction();
        }
    }

    public virtual void InteractedPressAction()
    {

    }

    public virtual void InteractedHoldAction()
    {

    }

    public virtual void EnableOutline()
    {
        outline.enabled = true;
    }

    public virtual void DisableOutline()
    {
        outline.enabled = false;
    }
}
