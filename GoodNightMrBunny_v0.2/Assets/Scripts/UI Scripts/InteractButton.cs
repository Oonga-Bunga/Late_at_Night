using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    private PlayerController player;
    private Button interactButton;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.interactableChanged += ShowButton;
    }

    private void ShowButton(object sender, bool interactable)
    {
        if (interactable)
        {
            interactButton.gameObject.SetActive(true);
        }
        else
        {
            interactButton.gameObject.SetActive(false);
        }
    }
}
