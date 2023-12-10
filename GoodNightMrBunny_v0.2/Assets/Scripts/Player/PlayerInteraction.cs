using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private AInteractable _closestInteractable = null; // Almacena el objeto interactuable m�s cercano al jugador en todo momento
    [SerializeField] private float _interactionDistance = 3f; // Distancia m�xima para interactuar con un objeto
    [SerializeField] private LayerMask _interactableLayer; // Capa en la cual se encuentran todos los gameObjects con los que el jugador puede interactuar
    public event Action<bool> OnInteractableChanged; // Se invoca cuando el objeto interactivo más cercano cambia

    [SerializeField] private Transform _cameraHolder; // Referencia a la cámara principal
    private PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego

    private void Start()
    {
        _pauseManager = PauseManager.Instance;
    }

    private void Update()
    {
        _closestInteractable = GetClosestInteractable();
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de interactuar, y ejecuta el m�todo interacted de
    /// _closestInteractable
    /// </summary>
    /// <param name="interactInput">Tipo de input, Down, Hold o Up</param>
    public void Interact(IPlayerReceiver.InputType interactInput)
    {
        if (_pauseManager.IsPaused || !GameManager.Instance.IsInGame) return;

        if (_closestInteractable != null)
        {
            _closestInteractable.Interacted(interactInput);
        }
    }

    /// <summary>
    /// Devuelve el objeto interactuable m�s cercano al jugador, y si hay varios el que está más cerca de su punto de mira
    /// </summary>
    /// <returns></returns>
    private AInteractable GetClosestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _interactionDistance, _interactableLayer);
        AInteractable bestInteractable = null;

        if (hitColliders.Length > 1)
        {
            AInteractable interactable = GetClosestInteractableToLineOfSight(hitColliders);

            if (interactable != null)
            {
                bestInteractable = interactable;
                OnInteractableChanged?.Invoke(true);

                if (bestInteractable.CanBeInteracted && bestInteractable != _closestInteractable)
                {
                    bestInteractable.EnableOutlineAndCanvas();
                }
            }
        }
        else if (hitColliders.Length == 1)
        {
            AInteractable interactable = hitColliders[0].GetComponent<InteractableOutlineHolder>().Interactable;

            if (interactable != null)
            {
                bestInteractable = interactable;
                OnInteractableChanged?.Invoke(true);

                if (bestInteractable.CanBeInteracted)
                {
                    bestInteractable.EnableOutlineAndCanvas();
                }
                else
                {
                    bestInteractable.DisableOutlineAndCanvas();
                }
            }
        }

        // Si el objeto anterior no es nulo se desactiva su canvas y _outline, y si es distinto al nuevo se le informa
        // que ya no está en el rango del jugador

        if (_closestInteractable != null && _closestInteractable != bestInteractable)
        {
            _closestInteractable.DisableOutlineAndCanvas();
            _closestInteractable.PlayerExitedRange();
        }

        if (bestInteractable == null)
        {
            OnInteractableChanged?.Invoke(false);
        }

        return bestInteractable;
    }

    /// <summary>
    /// Devuelve el AInteractable del objeto interactivo más cercano a la linea de visión del jugador
    /// </summary>
    /// <param name="hitColliders">Array con los colliders de todos los objetos interactivos cercanos al jugador</param>
    /// <returns></returns>
    private AInteractable GetClosestInteractableToLineOfSight(Collider[] hitColliders)
    {
        AInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        Vector3 playerPosition = _cameraHolder.position;
        Vector3 lineOfSightDirection = _cameraHolder.forward;

        foreach (Collider obj in hitColliders)
        {
            Vector3 objectPosition = obj.gameObject.transform.position;
            Vector3 toObject = GetClosestPointOnLine(playerPosition, playerPosition + (lineOfSightDirection * _interactionDistance), objectPosition) - objectPosition;
            float distanceToLineOfSight = toObject.magnitude;

            if (distanceToLineOfSight < closestDistance)
            {
                AInteractable interactable = obj.GetComponent<InteractableOutlineHolder>().Interactable;

                // Si el objeto está más cerca de la línea de visión que el objeto más cercano actual
                if (interactable != null)
                {
                    closestDistance = distanceToLineOfSight;
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

    /// <summary>
    /// Devuelve el punto en una recta más cercano a otro punto, se usa en GetClosestInteractableToLineOfSight
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private Vector3 GetClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float t = Vector3.Dot(point - lineStart, lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        t = Mathf.Clamp01(t);
        Vector3 closestPoint = lineStart + t * lineDirection;
        return closestPoint;
    }
}
