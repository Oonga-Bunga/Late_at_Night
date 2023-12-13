using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        [Range(0.1f, 9f)][SerializeField] float _sensitivityX = 2f; // Sensibilidad en el eje X
        [Range(0.1f, 9f)][SerializeField] float _sensitivityY = 1f; // Sensibilidad en el eje Y
        [Range(0f, 90f)][SerializeField] float _yRotationLimit = 88f; // Limite de la rotaci�n en el eje Y para que la c�mara no haga flip
        [SerializeField] private InputActionReference _joystickValueAction;
        [SerializeField] private InputActionReference _mouseDeltaAction;

        private List<int> _bannedTouches = new List<int>(); // Lista de toques en la pantalla tactil que no se usar�n para mover la c�mara
        Vector2 rotation = Vector2.zero; // Rotaci�n de la c�mara

        public delegate void LookCallback(); // Delegado para la funci�n de comportamiento de la c�mara
        public LookCallback _lookFunction; // Funci�n de comportamiento de la c�mara, depende sel dispositivo

        private PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego
        private bool _isLookEnabled = true; // Si la c�mara del jugador sigue el rat�n/toque o no

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
            _mouseDeltaAction.action.Enable();
            _joystickValueAction.action.Enable();

            _pauseManager = FindAnyObjectByType<PauseManager>();
        }

        /// <summary>
        /// Si el juego no est� pausado y isLookEnabled es true entonces se ejecuta lookFunction
        /// </summary>
        private void Update()
        {
            if (_pauseManager.IsPaused || !_isLookEnabled) return;

            if (!GameManager.Instance.IsInGame) return;

            _lookFunction();
        }

        /// <summary>
        /// Impide que el jugador pueda mover la c�mara y resetea la posici�n de esta
        /// </summary>
        public void PlayerMounting()
        {
            _isLookEnabled = false;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Permite al jugador volver a mover la c�mara, y la rota para que est� mirando al frente
        /// </summary>
        public void PlayerDismounting()
        {
            rotation = new Vector2(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x);
            _isLookEnabled = true;
        }
    }
}