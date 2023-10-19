using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropButton : MonoBehaviour
{
    private PlayerController player;
    private GameObject dropButton;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.OnInteractableChanged += ShowButton;
        dropButton = transform.GetChild(0).gameObject;
    }

    private void ShowButton(object sender, bool interactable)
    {
        if (interactable)
        {
            dropButton.SetActive(true);
        }
        else
        {
            dropButton.SetActive(false);
        }
    }
}
