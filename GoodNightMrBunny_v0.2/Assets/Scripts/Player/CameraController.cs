using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [Range(0.1f, 9f)][SerializeField] float _sensitivityX = 2f; // Sensibilidad en el eje X
    [Range(0.1f, 9f)][SerializeField] float _sensitivityY = 1f; // Sensibilidad en el eje Y
    [Range(0f, 90f)][SerializeField] float _yRotationLimit = 88f; // Limite de la rotación en el eje Y para que la cámara no haga flip
    [SerializeField] private InputActionReference _joystickValueAction;
    [SerializeField] private InputActionReference _mouseDeltaAction;

    private List<int> _bannedTouches = new List<int>(); // Lista de toques en la pantalla tactil que no se usarán para mover la cámara
    Vector2 rotation = Vector2.zero; // Rotación de la cámara

    public delegate void LookCallback(); // Delegado para la función de comportamiento de la cámara
    public LookCallback _lookFunction; // Función de comportamiento de la cámara, depende sel dispositivo

    private PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego
    private bool _isLookEnabled = true; // Si la cámara del jugador sigue el ratón/toque o no

    public float SensitivityX
    {
        get { return _sensitivityX; }
        set { _sensitivityX = value; }
    }

    public float SensitivityY
    {
        get { return _sensitivityY; }
        set { _sensitivityY = value; }
    }

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            _lookFunction = HandleMobileInput;
        }
        else
        {
            _lookFunction = HandlePCInput;
        }
        
        _mouseDeltaAction.action.Enable();
        _joystickValueAction.action.Enable();

        _pauseManager = FindAnyObjectByType<PauseManager>();
    }

    /// <summary>
    /// Si el juego no está pausado y isLookEnabled es true entonces se ejecuta lookFunction
    /// </summary>
    private void Update()
    {
        if (_pauseManager.IsPaused || !_isLookEnabled) return;

        _lookFunction();
    }

    /// <summary>
    /// Maneja el movimiento de la cámara usando el delta del ratón
    /// </summary>
    private void HandlePCInput()
    {
        Vector2 delta = _mouseDeltaAction.action.ReadValue<Vector2>();
        rotation.x += delta.x * _sensitivityX;
        rotation.y += delta.y * _sensitivityY;
        rotation.y = Mathf.Clamp(rotation.y, -_yRotationLimit, _yRotationLimit);

        var targetRotation = Quaternion.Euler(-rotation.y, rotation.x, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 30f);
    }

    /// <summary>
    /// Maneja el movimiento de la cámara usando el delta del toque que no esté tocando un elemento de la interfaz
    /// </summary>
    private void HandleMobileInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (!_bannedTouches.Contains(touch.fingerId))
            {
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Vector2 delta = touch.deltaPosition;
                    rotation.x += delta.x * _sensitivityX;
                    rotation.y += delta.y * _sensitivityY;
                    rotation.y = Mathf.Clamp(rotation.y, -_yRotationLimit, _yRotationLimit);

                    var targetRotation = Quaternion.Euler(-rotation.y, rotation.x, 0f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
                }
                else
                {
                    _bannedTouches.Add(touch.fingerId);
                }
            }
            else
            {
                if (touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    _bannedTouches.Remove(touch.fingerId);
                }
            }
        }

        Vector2 delta2 = _joystickValueAction.action.ReadValue<Vector2>();
        rotation.x += delta2.x * _sensitivityX * 2f;
        rotation.y += delta2.y * _sensitivityY * 2f;
        rotation.y = Mathf.Clamp(rotation.y, -_yRotationLimit, _yRotationLimit);

        var targetRotation2 = Quaternion.Euler(-rotation.y, rotation.x, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, Time.deltaTime * 30f);
    }

    /// <summary>
    /// Impide que el jugador pueda mover la cámara y resetea la posición de esta
    /// </summary>
    public void PlayerMounting()
    {
        _isLookEnabled = false;
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Permite al jugador volver a mover la cámara, y la rota para que esté mirando al frente
    /// </summary>
    public void PlayerDismounting()
    {
        rotation = new Vector2(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x);
        _isLookEnabled = true;
    }
}