using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class AInteractable : MonoBehaviour, IInteractable
{
    protected Outline outline;

    public virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public virtual void Interacted(object agent)
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
