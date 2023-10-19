using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractableOutlineHolder : MonoBehaviour
{
    [SerializeField] private AInteractable _interactable = null;

    public AInteractable Interactable
    {
        get { return _interactable; }
    }
}
