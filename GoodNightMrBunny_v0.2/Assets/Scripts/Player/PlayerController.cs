using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;
using System.Drawing;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerReceiver
{
    #region Attributes

    private static PlayerController _instance; // Instancia del jugador, singleton

    public static PlayerController Instance => _instance;

    #region Movement

    [Header("Speed")]

    [SerializeField] private float _maxWalkingSpeed = 7.0f; // Velocidad m�xima al caminar
    [SerializeField] private float _maxRunningSpeed = 14.0f; // Velocidad m�xima al correr
    private float _maxCurrentSpeed; // Velocidad máxima del jugador en cada momento

    [Header("Stamina")]

    [SerializeField] private float _maxStamina = 100.0f; // Máxima energia del jugador
    private float _currentStamina; // Energia del jugador que consume para correr
    [SerializeField] private float _staminaRecoveryRate = 10.0f; // Ratio de recuperación de energia
    [SerializeField] private float _staminaComsumptionRate = 20.0f; // Ratio de recuperación de energia
    [SerializeField, Range(0.0f, 1.0f)] private float _minimumStaminaForRunning = 0.2f; // Porcentaje de energía mínimo para empezar a correr
    private bool _isPressingRunButton = false; // Si el jugador está corriendo o no
    private bool _isRunning = false; // Si el jugador está corriendo o no
    public EventHandler<float> OnStaminaChanged; //Se invoca si cmabia el valor de la energía del jugador

    [Header("Acceleration")]

    [SerializeField] private float _acceleration = 13.0f; // Fuerza de Aceleraci�n al recibir inputs de movimiento WASD en el suelo
    [SerializeField] private float _decceleration = 16.0f; // Fuerza de deceleraci�n al no recibir inputs de movimiento WASD
    [SerializeField, Range(0.0f, 1.0f)] private float _airAccelerationModifier = 0.5f; // Modificación de la aceleración y deceleración cuando el jugador está en el aire
    [SerializeField] private float _velPower = 0.96f; // Ni idea de para que sirve, pero aqu� est�, no quitar

    [Header("Friction")]

    [SerializeField] private float _groundFriction = 1.0f; // Fricci�n en el suelo
    [SerializeField] private float _airFriction = 0.25f; // Fricci�n en el aire

    #endregion

    #region Jump

    [Header("Jump")]

    [SerializeField] private float _jumpForce = 20f; // Fuerza de salto
    [SerializeField, Range(0.0f, 1.0f)] private float _jumpCutMultiplier = 0.4f; // Fuerza que acorta el salto cuando se deja de presionar la tecla de saltar
    private bool _isPlayerGrounded = false; // Si el jugador está en el suelo o no

    // Este temporizador junto a su valor por defecto definen el tiempo de coyote, que permite al jugador saltar mientras no
    // haya pasado m�s de x tiempo (x = _jumpCoyoteTime), es decir, mientras el valor de _lastGroundedTime sea mayor que 0
    private float _lastGroundedTime = 0f;
    [SerializeField] private float _jumpCoyoteTime = 0.15f;

    // Este temporizador junto a su valor por defecto definen el buffer de salto, que permite al jugador saltar si se ha
    // presionado la tecla correspondiente en los �ltimos x segundos (x = _jumpBufferTime), es decir, mientras el valor de
    // _lastJumpTime sea mayor que 0
    private float _lastJumpTime = 0f;
    [SerializeField] private float _jumpBufferTime = 0.1f;

    private float _localGravityScale = 1f; // Factor por el que la gravedad afecta al objeto en cada momento, empieza como 1
    [SerializeField] private float _fallGravityMultiplier = 1.2f; // Factor por el que aumenta la gravedad al caer

    #endregion

    #region Checks

    [Header("Checks")]

    [SerializeField] private CapsuleCollider _groundCheck = null; // BoxCollider que se usa para realizar un BoxCast en el m�todo CheckPlayerGrounded
    [SerializeField] private float _maxGroundCheckDistance = 0.3f; // Distancia m�xima para el BoxCast / Distancia a la que el jugador detecta el suelo
    [SerializeField] private LayerMask _groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    private AInteractable _closestInteractable = null; // Almacena el objeto interactuable m�s cercano al jugador en todo momento
    [SerializeField] private float _interactionDistance = 3f; // Distancia m�xima para interactuar con un objeto
    [SerializeField] private LayerMask _interactableLayer; // Capa en la cual se encuentran todos los gameObjects con los que el jugador puede interactuar
    public EventHandler<bool> OnInteractableChanged; // Se invoca cuando el objeto interactivo más cercano cambia

    #endregion

    #region Dropping

    [Header("Dropping")]

    [SerializeField] private float _dropDistance = 3.5f; // Distancia a la cual el jugador suelta el objeto
    [SerializeField] private float _sphereRaycastRadius = 0.3f; // Radio de la esfera que se usa para detectar si el lugar donde se suelta el objeto está obstruido
    [SerializeField] private float _minimumDistanceFromCollision = 0.3f; // Distancia desde la obstrucción donde se dropea el objeto

    #endregion

    #region Other

    [Header("Other")]

    [SerializeField] private List<AHoldableObject> _holdableObjectList; // Lista con los objetos que puede llevar encima el jugador
    private AHoldableObject _currentHeldObject; // Object que lleva encima en cada momento

    private Rigidbody _rb; // Referencia al rigidbody del jugador
    private Transform _cameraHolder; // Referencia a la cámara principal

    private PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego

    private IPlayerReceiver _mount = null; // Objeto en el que está montado el jugador, puede ser nulo

    #endregion

    #endregion

    #region Getters and Setters

    public AHoldableObject CurrentHeldObject => _currentHeldObject;

    public float MaxStamina => _maxStamina;

    public bool IsPlayerGrounded => _isPlayerGrounded;

    public float MaxCurrentSpeed => _maxCurrentSpeed;

    #endregion

    #region Initialization

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _cameraHolder = Camera.main.transform.parent;
        _maxCurrentSpeed = _maxWalkingSpeed;
        _currentStamina = _maxStamina;
        _currentHeldObject = GetComponentInChildren<EmptyWeapon>();
        _pauseManager = PauseManager.Instance;
        PlayerInputManager.Instance.SetPlayer(this);

    }

    #endregion

    #region Updates

    /// <summary>
    /// Provoca el efecto de la gravedad en el jugador manualmente
    /// </summary>
    private void FixedUpdate()
    {
        if (_pauseManager.isPaused) return;

        _rb.AddForce(Physics.gravity * _localGravityScale);
    }

    /// <summary>
    /// Temporizadores, salto, gravedad, correr e interactuables
    /// </summary>
    private void Update()
    {
        if (_pauseManager.isPaused) return;

        // Comprobar si el jugador está en el suelo

        _isPlayerGrounded = CheckPlayerGrounded();

        // Actualizar los temporizadores

        _lastGroundedTime -= Time.deltaTime;
        _lastJumpTime -= Time.deltaTime;

        if (_isPlayerGrounded)
        {
            _lastGroundedTime = _jumpCoyoteTime;
        }

        // Saltar si se cumplen las condiciones

        if (_lastGroundedTime > 0 && _lastJumpTime > 0)
        {
            JumpAction();
        }

        // Aumentar la gravedad si el jugador est� cayendo

        if (_rb.velocity.y < 0)
        {
            _localGravityScale = _fallGravityMultiplier;
        }
        else
        {
            _localGravityScale = 1f;
        }

        // Cambiar el modo de movimiento a correr si se cumplen las condiciones

        if (_isPressingRunButton && !_isRunning && _isPlayerGrounded)
        {
            if ((_currentStamina / _maxStamina) > _minimumStaminaForRunning)
            {
                _isRunning = true;
            }
        }
        else if (!_isPressingRunButton)
        {
            _isRunning = false;
        }

        // Modificar la energia dependiendo de si el jugador está corriendo o no, y si se acaba parar de correr

        if (_isRunning)
        {
            _maxCurrentSpeed = _maxRunningSpeed;

            if (_currentStamina == 0)
            {
                _isRunning = false;
            }
            else
            {
                _currentStamina = Mathf.Max(_currentStamina - _staminaComsumptionRate * Time.deltaTime, 0);
                OnStaminaChanged?.Invoke(this, _currentStamina);
            }
        }
        else
        {
            _maxCurrentSpeed = _maxWalkingSpeed;
            _currentStamina = Mathf.Min(_currentStamina + _staminaRecoveryRate * Time.deltaTime, _maxStamina);
            OnStaminaChanged?.Invoke(this, _currentStamina);
        }

        // Actualizar el valor de _closestInteractable para que sea el del IInteractable m�s cercano

        _closestInteractable = GetClosestInteractable();
    }

    #endregion

    #region Player Actions

    /// <summary>
    /// Mueve al jugador horizontalmente en la direcci�n recibida, limita la velocidad y a�ade fricci�n
    /// </summary>
    /// <param name="direction">Direcci�n de movimiento recibida a partir del input del jugador</param>
    public void Move(Vector2 direction)
    {
        if (_pauseManager.isPaused) return;

        // C�lculo de la direcci�n de movimiento con respecto a la c�mara

        Vector3 move = new Vector3(direction.x, 0, direction.y);
        move = _cameraHolder.forward * move.z + _cameraHolder.right * move.x;
        move.y = 0f;
        move = move.normalized;

        // C�lculo de la velocidad necesaria

        Vector2 horizontalSpeed = new Vector2(_rb.velocity.x, _rb.velocity.z);
        Vector2 targetSpeed = new Vector2(move.x, move.z) * _maxCurrentSpeed;
        Vector2 speedDif = targetSpeed - horizontalSpeed;
        float accelRate = (targetSpeed.magnitude > 0.01f) ? _acceleration : _decceleration;

        if (!_isPlayerGrounded)
        {
            accelRate *= _airAccelerationModifier;
        }

        float movement = Mathf.Pow(speedDif.magnitude * accelRate, _velPower);

        // Aplicaci�n del movimiento mediante una fuerza

        _rb.AddForce(move * movement);

        // Limitaci�n de velocidad

        if (horizontalSpeed.magnitude > _maxCurrentSpeed)
        {
            if (!_isPlayerGrounded && horizontalSpeed.magnitude >= _maxRunningSpeed)
            {
                float adjustment = _maxRunningSpeed / horizontalSpeed.magnitude;

                _rb.velocity = new Vector3(_rb.velocity.x * adjustment, _rb.velocity.y, _rb.velocity.z * adjustment);
            }
            else
            {
                float adjustment = _maxCurrentSpeed / horizontalSpeed.magnitude;

                _rb.velocity = new Vector3(_rb.velocity.x * adjustment, _rb.velocity.y, _rb.velocity.z * adjustment);
            }
        }

        // Fricci�n en el suelo y el aire cuando el jugador no introduce input de movimiento WASD

        if (move.magnitude < 0.01f)
        {
            if (_isPlayerGrounded)
            {
                float amount = Mathf.Min(horizontalSpeed.magnitude, _groundFriction);

                _rb.AddForce(new Vector3(horizontalSpeed.normalized.x, 0, horizontalSpeed.normalized.y) * -amount, ForceMode.Impulse);
            }
            else
            {
                float amount = Mathf.Min(horizontalSpeed.magnitude, _airFriction);

                _rb.AddForce(new Vector3(horizontalSpeed.normalized.x, 0, horizontalSpeed.normalized.y) * -amount, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de correr, y guarda en una variable si la tecla está siendop pulsada o no
    /// </summary>
    /// <param name="runInput">Tipo de input, Down o Up</param>
    public void Run(IPlayerReceiver.InputType runInput)
    {
        if (_pauseManager.isPaused) return;

        if (runInput != IPlayerReceiver.InputType.Up)
        {
            _isPressingRunButton = true;
        }
        else
        {
            _isPressingRunButton = false;
        }
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona o suelta la tecla de salto, en el primer caso actualiza el
    /// buffer de salto, y en el segundo se acorta el salto
    /// </summary>
    /// <param name="jumpInput">Tipo de input, Down o Up</param>
    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (_pauseManager.isPaused) return;

        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            _lastJumpTime = _jumpBufferTime;
        }
        else
        {
            if (_rb.velocity.y > 0 && !_isPlayerGrounded)
            {
                _rb.AddForce(Vector3.down * _rb.velocity.y * (1 - _jumpCutMultiplier), ForceMode.Impulse);
            }

            _lastJumpTime = 0;
        }
    }

    /// <summary>
    /// Aplicaci�n del salto mediante una fuerza instantanea (Impulse), se resetean los temporizadores
    /// </summary>
    private void JumpAction()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _lastGroundedTime = 0;
        _lastJumpTime = 0;
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de ataque, llama al m�todo UseHeldObject del objeto 
    /// que posee el jugador en ese momento
    /// </summary>
    /// <param name="useInput">Tipo de input, Down, Hold o Up</param>
    public void UseHeldObject(IPlayerReceiver.InputType useInput)
    {
        if (_pauseManager.isPaused) return;

        _currentHeldObject.Use(useInput);
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de interactuar, y ejecuta el m�todo interacted de
    /// _closestInteractable
    /// </summary>
    /// <param name="interactInput">Tipo de input, Down, Hold o Up</param>
    public void Interact(IPlayerReceiver.InputType interactInput)
    {
        if (_pauseManager.isPaused) return;

        if (_closestInteractable != null)
        {
            _closestInteractable.Interacted(interactInput);
        }
    }

    /// <summary>
    /// El jugador suelta en el suelo el objeto que llevaba encima, y pasa a llevar el objeto que recive como
    /// par�metro, que puede inicializarse con cierto valor
    /// </summary>
    /// <param name="objectType">Tipo de objeto al que cambia el jugador</param>
    /// <param name="dropPrefab">Si al cambiar de objeto se instancia el prefab del pickup del anterior</param>
    /// <param name="initializationValue">Valor con el que se inicia el arma, como la carga de la linterna</param>
    public void ChangeHeldObject(IPlayerReceiver.HoldableObjectType objectType, bool dropPrefab, float initializationValue = -1)
    {
        _currentHeldObject.Drop(dropPrefab, _dropDistance, _sphereRaycastRadius, _minimumDistanceFromCollision, _groundLayer);

        AHoldableObject newHeldObject = _holdableObjectList[0];

        switch (objectType)
        {
            case IPlayerReceiver.HoldableObjectType.Flashlight:
                newHeldObject = _holdableObjectList[1];
                break;
            case IPlayerReceiver.HoldableObjectType.ClayBalls:
                newHeldObject = _holdableObjectList[2];
                break;
            case IPlayerReceiver.HoldableObjectType.ClayBin:
                newHeldObject = _holdableObjectList[3];
                break;
        }
        newHeldObject.gameObject.SetActive(true);
        _currentHeldObject = newHeldObject;

        if (!(initializationValue < 0))
        {
            _currentHeldObject.Initialize(initializationValue);
        }
    }

    /// <summary>
    /// Es similar al m�todo ChangeHeldObject, la �nica diferencia es que el objeto al que cambia el jugador es el vac�o
    /// </summary>
    /// <param name="force">Fuerza con la que se suelta el objeto</param>
    public void DropHeldObject(float force = 0)
    {
        if (_pauseManager.isPaused) return;

        _currentHeldObject.Drop(true, _dropDistance, _sphereRaycastRadius, _minimumDistanceFromCollision, _groundLayer);
        _holdableObjectList[0].gameObject.SetActive(true);
        _currentHeldObject = _holdableObjectList[0];
    }

    /// <summary>
    /// Asigna al jugador el objeto sobre el que está montado, y es llamado por dicho objeto
    /// Los controles del jugador pasarán a ser procesados por la montura, y su movimiento queda anclado a esta
    /// </summary>
    /// <param name="mount">Referencia al script del objeto que va a montar el jugador</param>
    /// <param name="mountingPoint">Punto en el que el jugador se situa respecto a la montura</param>
    public void AssignMount(IPlayerReceiver mount, GameObject mountingPoint)
    {
        this._mount = mount;
        PlayerInputManager.Instance.SetPlayer(mount);
        transform.parent = mountingPoint.transform;
        _cameraHolder.GetComponent<CameraController>().PlayerMounting();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// Llamado por la montura del jugador cuando este se desmonta, deshace los cambios hechos en AssignMount
    /// El jugador siempre ejecuta un salto al desmontarse
    /// </summary>
    public void DisMount()
    {
        this._mount = null;
        PlayerInputManager.Instance.SetPlayer(this);
        transform.parent = null;
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z));
        _cameraHolder.GetComponent<CameraController>().PlayerDismounting();

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        JumpAction();
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Devuelve true si el jugador est� tocando el suelo, usando un BoxCast
    /// </summary>
    /// <returns>true o false</returns>
    private bool CheckPlayerGrounded()
    {
        return Physics.CapsuleCast(_groundCheck.transform.position, _groundCheck.transform.position, _groundCheck.radius, Vector3.down, _maxGroundCheckDistance, _groundLayer);
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
                OnInteractableChanged?.Invoke(this, true);

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
                OnInteractableChanged?.Invoke(this, true);

                if (bestInteractable.CanBeInteracted && bestInteractable != _closestInteractable)
                {
                    bestInteractable.EnableOutlineAndCanvas();
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
            OnInteractableChanged?.Invoke(this, false);
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
  
    #endregion
}
