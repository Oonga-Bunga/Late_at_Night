using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walking Speed")]
    [SerializeField] private float _maxWalkingSpeed = 7.0f; // Velocidad m�xima al caminar
    [SerializeField] private float _maxRunningSpeed = 14.0f; // Velocidad m�xima al correr
    private float _maxCurrentSpeed; // Velocidad máxima del jugador en cada momento

    [Header("Stamina")]
    [SerializeField] private float _maxStamina = 100.0f; // Máxima energia del jugador
    private float _currentStamina; // Energia del jugador que consume para correr
    [SerializeField] private float _staminaRecoveryRate = 10.0f; // Ratio de recuperación de energia
    [SerializeField] private float _staminaComsumptionRate = 20.0f; // Ratio de recuperación de energia
    [SerializeField, Range(0.0f, 1.0f)] private float _minimumStaminaForRunning = 0.2f; // Porcentaje de energía mínimo para empezar a correr
    private bool _isRunning = false; // Si el jugador está corriendo o no
    
    [Header("Acceleration")]
    [SerializeField] private float _acceleration = 13.0f; // Fuerza de Aceleraci�n al recibir inputs de movimiento WASD en el suelo
    [SerializeField] private float _decceleration = 16.0f; // Fuerza de deceleraci�n al no recibir inputs de movimiento WASD
    [SerializeField, Range(0.0f, 1.0f)] private float _airAccelerationModifier = 0.5f; // Modificación de la aceleración y deceleración cuando el jugador está en el aire
    [SerializeField] private float _velPower = 0.96f; // Ni idea de para que sirve, pero aqu� est�, no quitar

    [Header("Friction")]
    [SerializeField] private float _groundFriction = 1.0f; // Fricci�n en el suelo
    [SerializeField] private float _airFriction = 0.25f; // Fricci�n en el aire

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 20f; // Fuerza de salto
    [SerializeField, Range(0.0f, 1.0f)] private float _jumpCutMultiplier = 0.4f; // Fuerza que acorta el salto cuando se deja de presionar la tecla de saltar
    [SerializeField] private float _jumpCoyoteTime = 0.15f; // Tiempo tras dejar de tocar el suelo durante el cual se puede saltar
    [SerializeField] private float _jumpBufferTime = 0.1f; // Buffer de salto, para que tras presionar la tecla de salto haya cierto tiempo curante el cual se comprueba si el juagdor salta, no solo en el frame que se presiona la tecla
    private float _lastJumpTime = 0f; // Temporizador del buffer de salto
    private float _lastGroundedTime = 0f; // Temporizador del coyote time
    private bool _isPlayerGrounded = false; // Si el jugador está en el suelo o no

    [Header("Falling")]
    [SerializeField] private float _fallGravityMultiplier = 1.2f; // Factor por el que aumenta la gravedad al caer
    private float _localGravityScale = 1f; // Factor por el que la gravedad afecta al objeto en cada momento, empieza como 1

    [Header("Ground Check")]
    [SerializeField] private CapsuleCollider _groundCheck; // BoxCollider que se usa para realizar un BoxCast en el m�todo CheckPlayerGrounded
    [SerializeField] private float _maxGroundCheckDistance = 0.3f; // Distancia m�xima para el BoxCast / Distancia a la que el jugador detecta el suelo
    [SerializeField] private LayerMask _groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    [Header("References")]
    [SerializeField] private Transform _cameraHolder; // Referencia a la cámara principal
    [SerializeField] private Rigidbody _rb; // Referencia al rigidbody del jugador
    private PauseManager _pauseManager; // Referencia al PauseManager que se encarga de manejar la pausa del juego

    // Eventos
    public event Action<float> OnStaminaChanged; // Invocado cuando cambia el valor de la energía del jugador
    public event Action OnJumped; // Invocado cuando el jugador salta
    public event Action OnLanded; // Invocado cuando el jugador aterriza

    public float MaxCurrentSpeed => _maxCurrentSpeed;
    public float MaxStamina => _maxStamina;
    public bool IsRunning => _isRunning;
    public bool IsPlayerGrounded => _isPlayerGrounded;

    public float GetCurrentVelocity()
    {
        return _rb.velocity.magnitude;
    }

    private void Awake()
    {
        _rb.useGravity = false;

        _maxCurrentSpeed = _maxWalkingSpeed;
        _currentStamina = _maxStamina;
    }

    private void Start()
    {
        _pauseManager = PauseManager.Instance;
    }

    /// <summary>
    /// Provoca el efecto de la gravedad en el jugador manualmente
    /// </summary>
    private void FixedUpdate()
    {
        if (_pauseManager.IsPaused) return;

        _rb.AddForce(Physics.gravity * _localGravityScale);
    }

    void Update()
    {
        if (_pauseManager.IsPaused) return;

        // Comprobar si el jugador está en el suelo

        bool tempGrounded = _isPlayerGrounded;
        _isPlayerGrounded = CheckPlayerGrounded();
        if (tempGrounded == false && _isPlayerGrounded) 
        {
            OnLanded?.Invoke();
            Debug.Log("hey");
        }

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
                OnStaminaChanged?.Invoke(_currentStamina);
            }
        }
        else
        {
            if (_isPlayerGrounded)
            {
                _maxCurrentSpeed = _maxWalkingSpeed;
            }

            _currentStamina = Mathf.Min(_currentStamina + _staminaRecoveryRate * Time.deltaTime, _maxStamina);
            OnStaminaChanged?.Invoke(_currentStamina);
        }
    }

    /// <summary>
    /// Mueve al jugador horizontalmente en la direcci�n recibida, limita la velocidad y a�ade fricci�n
    /// </summary>
    /// <param name="direction">Direcci�n de movimiento recibida a partir del input del jugador</param>
    public void Move(Vector2 direction)
    {
        if (_pauseManager.IsPaused) return;

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
    /// <param name="runInput">Tipo de input, Down, Hold o Up</param>
    public void Run(IPlayerReceiver.InputType runInput)
    {
        if (_pauseManager.IsPaused) return;

        if (runInput != IPlayerReceiver.InputType.Up)
        {
            if (!_isRunning && _isPlayerGrounded)
            {
                if ((_currentStamina / _maxStamina) > _minimumStaminaForRunning)
                {
                    _isRunning = true;
                }
            }
        }
        else
        {
            _isRunning = false;
        }
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona o suelta la tecla de salto, en el primer caso actualiza el
    /// buffer de salto, y en el segundo se acorta el salto
    /// </summary>
    /// <param name="jumpInput">Tipo de input, Down o Up</param>
    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (_pauseManager.IsPaused) return;

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
    public void JumpAction()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _lastGroundedTime = 0;
        _lastJumpTime = 0;
        OnJumped?.Invoke();
    }

    private bool CheckPlayerGrounded()
    {
        return Physics.CapsuleCast(_groundCheck.transform.position, _groundCheck.transform.position, _groundCheck.radius, Vector3.down, _maxGroundCheckDistance, _groundLayer);
    }
}
