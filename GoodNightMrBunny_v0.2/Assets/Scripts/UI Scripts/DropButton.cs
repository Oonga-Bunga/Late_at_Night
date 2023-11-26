using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropButton : MonoBehaviour
{
    private PlayerController _player;
    private GameObject _dropButton;

    private void Start()
    {
        _player = PlayerController.Instance;
        _player.PlayerInteraction.OnInteractableChanged += ShowButton;
        _dropButton = transform.GetChild(0).gameObject;
    }

    private void ShowButton(bool interactable)
    {
        if (interactable)
        {
            _dropButton.SetActive(true);
        }
        else
        {
            _dropButton.SetActive(false);
        }
    }
}
