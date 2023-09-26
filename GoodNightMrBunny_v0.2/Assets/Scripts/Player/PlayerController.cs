using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour, IPlayerReceiver
{
    #region Attributes

    #region Movement

    [Header("Movement")]

    private float maxCurrentSpeed = 7.0f;
    [SerializeField] private float maxWalkingSpeed = 7.0f; // Velocidad m�xima al caminar
    [SerializeField] private float maxRunningSpeed = 14.0f; // Velocidad m�xima al correr

    [SerializeField] private float maxStamina = 100.0f; // Máxima energia del jugador
    private float currentStamina = 100.0f; // Energia del jugador que consume para correr
    [SerializeField] private float staminaRecoveryRate = 10.0f; // Ratio de recuperación de energia
    [SerializeField] private float staminaComsumptionRate = 20.0f; // Ratio de recuperación de energia
    [SerializeField] [Range(0.0f, 1.0f)] private float minimumStaminaForRunning = 0.2f; // Porcentaje de energía mínimo para empezar a correr
    private bool isRunning = false; // Si el jugador está corriendo o no
    public EventHandler<float> staminaChanged; //Se invoca si cmabia el valor de la energía del jugador

    [SerializeField] private float acceleration = 8f; // Fuerza de Aceleraci�n al recibir inputs de movimiento WASD en el suelo
    [SerializeField] private float decceleration = 24f; // Fuerza de deceleraci�n al no recibir inputs de movimiento WASD
    [SerializeField] [Range(0.0f, 1.0f)] private float airAccelerationModifier = 0.25f; // Modificación de la aceleración y deceleración cuando el jugador está en el aire
    [SerializeField] private float velPower = 0.97f; // Ni idea de para que sirve, pero aqu� est�, no quitar

    [SerializeField] private float groundFrictionAmount = 0.44f; // Fricci�n en el suelo
    [SerializeField] private float airFrictionAmount = 0.22f; // Fricci�n en el aire

    #endregion

    #region Jump

    [Header("Jump")]

    [SerializeField] private float jumpForce = 1000.0f; // Fuerza de salto
    [SerializeField] [Range(0.0f, 1.0f)] private float jumpCutMultiplier = 0.4f; // Fuerza que acorta el salto cuando se deja de presionar la tecla de saltar

    // Este temporizador junto a su valor por defecto definen el tiempo de coyote, que permite al jugador saltar mientras no
    // haya pasado m�s de x tiempo (x = jumpCoyoteTime), es decir, mientras el valor de lastGroundedTime sea mayor que 0
    private float lastGroundedTime;
    [SerializeField] private float jumpCoyoteTime = 0.15f;

    // Este temporizador junto a su valor por defecto definen el buffer de salto, que permite al jugador saltar si se ha
    // presionado la tecla correspondiente en los �ltimos x segundos (x = jumpBufferTime), es decir, mientras el valor de
    // lastJumpTime sea mayor que 0
    private float lastJumpTime;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private float localGravityScale; // Factor por el que la gravedad afecta al objeto en cada momento, empieza como 1
    [SerializeField] private float fallGravityMultiplier = 1.0f; // Factor por el que aumenta la gravedad al caer

    #endregion

    #region Checks

    [Header("Checks")]

    [SerializeField] private BoxCollider groundCheck; // BoxCollider que se usa para realizar un BoxCast en el m�todo IsPlayerGrounded
    [SerializeField] private float maxGroundCheckDistance = 0.1f; // Distancia m�xima para el BoxCast / Distancia a la que el jugador detecta el suelo
    [SerializeField] private LayerMask groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    private AInteractable closestInteractable; // Almacena el objeto interactuable m�s cercano al jugador en todo momento
    [SerializeField] private float interactionDistance = 3f; // Distancia m�xima para interactuar con un objeto
    [SerializeField] private LayerMask interactableLayer; // Capa en la cual se encuentran todos los gameObjects con los que el jugador puede interactuar

    #endregion

    #region Other

    private Rigidbody rb;
    private Transform cameraTransform;
    private AWeapon currentWeapon;
    [SerializeField] private List<AWeapon> weaponList;
    private PauseManager pauseManager;

    #endregion

    #endregion

    #region Initialization

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        cameraTransform = Camera.main.transform;

        maxCurrentSpeed = maxWalkingSpeed;

        currentStamina = maxStamina;

        lastGroundedTime = 0;
        lastJumpTime = 0;
        localGravityScale = 1f;

        currentWeapon = GetComponentInChildren<EmptyWeapon>();
        pauseManager = FindObjectOfType<PauseManager>();
    }

    #endregion

    #region Updates

    /// <summary>
    /// Provoca el efecto de la gravedad en el jugador manualmente
    /// </summary>
    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * localGravityScale);
    }

    /// <summary>
    /// Temporizadores, salto, gravedad e interactuables
    /// </summary>
    void Update()
    {
        // Actualizar los temporizadores

        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;

        if (IsPlayerGrounded())
        {
            lastGroundedTime = jumpCoyoteTime;
        }

        // Saltar si se cumplen las condiciones

        if (lastGroundedTime > 0 && lastJumpTime > 0)
        {
            JumpAction();
        }

        // Aumentar la gravedad si el jugador est� cayendo

        if (rb.velocity.y < 0)
        {
            localGravityScale = fallGravityMultiplier;
        }
        else
        {
            localGravityScale = 1f;
        }

        // Modificamos la energia dependiendo de si el jugador está corriendo o no
        if (isRunning)
        {
            if (currentStamina == 0)
            {
                maxCurrentSpeed = maxWalkingSpeed;
                isRunning = false;
            }
            else
            {
                currentStamina = Mathf.Max(currentStamina - staminaComsumptionRate * Time.deltaTime, 0);
                staminaChanged.Invoke(this, currentStamina);
            }
        }
        else
        {
            currentStamina = Mathf.Min(currentStamina + staminaRecoveryRate * Time.deltaTime, maxStamina);
            staminaChanged.Invoke(this, currentStamina);
        }

        // Actualizamos el valor de closestInteractable para que sea el del IInteractable m�s cercano

        closestInteractable = GetClosestInteractable();
    }

    #endregion

    #region PlayerActions

    /// <summary>
    /// Mueve al jugador horizontalmente en la direcci�n recibida, limita la velocidad y a�ade fricci�n
    /// </summary>
    /// <param name="direction">Direcci�n de movimiento recibida a partir del input WASD</param>
    public void Move(Vector2 direction)
    {
        if (pauseManager.isPaused) return;
        // C�lculo de la direcci�n de movimiento con respecto a la c�mara

        Vector3 move = new Vector3(direction.x, 0, direction.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        move = move.normalized;

        // C�lculo de la velocidad necesaria

        Vector2 horizontalSpeed = new Vector2(rb.velocity.x, rb.velocity.z);

        Vector2 targetSpeed = new Vector2(move.x, move.z) * maxCurrentSpeed;

        Vector2 speedDif = targetSpeed - horizontalSpeed;

        float accelRate = (targetSpeed.magnitude > 0.01f) ? acceleration : decceleration;

        if (!IsPlayerGrounded())
        {
            accelRate *= airAccelerationModifier;
        }

        float movement = Mathf.Pow(speedDif.magnitude * accelRate, velPower);

        // Aplicaci�n del movimiento mediante una fuerza

        rb.AddForce(move * movement);

        // Limitaci�n de velocidad

        if (horizontalSpeed.magnitude > maxCurrentSpeed)
        {
            float adjustment = maxCurrentSpeed / horizontalSpeed.magnitude;

            rb.velocity = new Vector3(rb.velocity.x * adjustment, rb.velocity.y, rb.velocity.z * adjustment);
        }

        // Fricci�n en el suelo y el aire cuando el jugador no introduce input de movimiento WASD

        if (move.magnitude < 0.01f)
        {
            if (IsPlayerGrounded())
            {
                float amount = Mathf.Min(horizontalSpeed.magnitude, groundFrictionAmount);

                rb.AddForce(new Vector3(horizontalSpeed.normalized.x, 0, horizontalSpeed.normalized.y) * -amount, ForceMode.Impulse);
            }
            else
            {
                float amount = Mathf.Min(horizontalSpeed.magnitude, airFrictionAmount);

                rb.AddForce(new Vector3(horizontalSpeed.normalized.x, 0, horizontalSpeed.normalized.y) * -amount, ForceMode.Impulse);
            }
        }
    }

    public void Run(IPlayerReceiver.InputType jumpInput)
    {
        if (pauseManager.isPaused) return;
        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            if ((currentStamina / maxStamina) > minimumStaminaForRunning)
            {
                RunDown();
            }
        }
        else
        {
            RunUp();
        }
    }

    private void RunDown()
    {
        maxCurrentSpeed = maxRunningSpeed;
        isRunning = true;
    }

    private void RunUp()
    {
        maxCurrentSpeed = maxWalkingSpeed;
        isRunning = false;
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona o suelta la tecla de salto, y llama al m�todo correspondiente
    /// </summary>
    /// <param name="jumpInput">Tipo de input, Down o Up</param>
    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (pauseManager.isPaused) return;
        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            JumpDown();
        }
        else
        {
            JumpUp();
        }
    }

    /// <summary>
    /// Registramos el momento en el que el jugador ha querido saltar
    /// </summary>
    private void JumpDown()
    {
        lastJumpTime = jumpBufferTime;
    }

    /// <summary>
    /// Si el jugador est� saltando y deja de pulsar la tecla de salto este se acorta
    /// </summary>
    private void JumpUp()
    {
        if (rb.velocity.y > 0 && !IsPlayerGrounded())
        {
            rb.AddForce(Vector3.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode.Impulse);
        }

        lastJumpTime = 0;
    }

    /// <summary>
    /// Aplicaci�n del salto mediante una fuerza instantanea (Impulse), se resetean los temporizadores
    /// </summary>
    private void JumpAction()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        lastGroundedTime = 0;
        lastJumpTime = 0;
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de ataque, llama al m�todo Attack del arma que posee el
    /// jugador en ese momento
    /// </summary>
    /// <param name="attackInput">Tipo de input, Down, Hold o Up</param>
    public void Attack(IPlayerReceiver.InputType attackInput)
    {
        currentWeapon.Attack(attackInput);
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de interactuar, y ejecuta el m�todo interacted de
    /// closestInteractable
    /// </summary>
    public void Interact()
    {
        if (closestInteractable != null)
        {
            closestInteractable.Interacted(this);
        }
    }

    /// <summary>
    /// El jugador suelta en el suelo el arma que tiene equipada, y pasa a tener equipada el arma que recive como
    /// par�metro
    /// El jugador posee todas las armas, pero solo tiene una activa en cada momento
    /// </summary>
    /// <param name="weaponType">Tipo de arma a la que cambia el jugador</param>
    public void ChangeWeapon(IPlayerReceiver.WeaponType weaponType)
    {
        currentWeapon.Drop();

        foreach (AWeapon weapon in weaponList)
        {
            if (weaponType == weapon.weaponType)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon;
                return;
            }
        }
    }

    /// <summary>
    /// Es similar al m�todo ChangeWeapon, la �nica diferencia es que el arma a la que cambia el jugador es la vac�a
    /// </summary>
    public void DropWeapon()
    {
        currentWeapon.Drop();
        weaponList[0].gameObject.SetActive(true);
        currentWeapon = weaponList[0];
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Devuelve true si el jugador est� tocando el suelo, usando un BoxCast
    /// </summary>
    /// <returns>true o false</returns>
    private bool IsPlayerGrounded()
    {
        return Physics.BoxCast(groundCheck.gameObject.transform.position, groundCheck.size / 2, Vector3.down, transform.rotation, maxGroundCheckDistance, groundLayer);
    }

    /// <summary>
    /// Devuelve el objeto interactuable m�s cercano al jugador
    /// </summary>
    /// <returns></returns>
    private AInteractable GetClosestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance, interactableLayer);

        AInteractable bestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            AInteractable interactable = collider.GetComponent<AInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(interactable.transform.position, transform.position);

                if (distance < closestDistance)
                {
                    bestInteractable = interactable;
                    closestDistance = distance;
                }
            }
        }

        if (closestInteractable != null)
        {
            closestInteractable.DisableOutline();
        }
        if (bestInteractable != null)
        {
            bestInteractable.EnableOutline();
        }

        return bestInteractable;
    }

    /// <summary>
    /// Dibuja el area final del BoxCast anterior
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheck.gameObject.transform.position - transform.up * maxGroundCheckDistance, groundCheck.size);
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    #endregion
}
