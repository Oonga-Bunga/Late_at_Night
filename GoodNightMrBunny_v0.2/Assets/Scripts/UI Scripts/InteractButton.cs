using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    private PlayerController player;
    private GameObject interactButton;

    private void Start()
    {
        player = PlayerController.Instance;
        player.PlayerInteraction.OnInteractableChanged += ShowButton;
        interactButton = transform.GetChild(0).gameObject;
    }

    private void ShowButton(bool isInteractableInRange)
    {
        interactButton.SetActive(isInteractableInRange);
    }
}
