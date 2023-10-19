using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;
using System.Drawing;

public class PlayerController : MonoBehaviour, IPlayerReceiver
{
    #region Attributes

    #region Movement

    [Header("Speed")]

    [SerializeField] private float maxWalkingSpeed; // Velocidad m�xima al caminar
    [SerializeField] private float maxRunningSpeed; // Velocidad m�xima al correr
    private float maxCurrentSpeed; // Velocidad máxima del jugador en cada momento

    [Header("Stamina")]

    [SerializeField] private float maxStamina; // Máxima energia del jugador
    private float currentStamina; // Energia del jugador que consume para correr
    [SerializeField] private float staminaRecoveryRate; // Ratio de recuperación de energia
    [SerializeField] private float staminaComsumptionRate; // Ratio de recuperación de energia
    [SerializeField][Range(0.0f, 1.0f)] private float minimumStaminaForRunning; // Porcentaje de energía mínimo para empezar a correr
    private bool isPressingRunButton; // Si el jugador está corriendo o no
    private bool isRunning; // Si el jugador está corriendo o no
    public EventHandler<float> StaminaChanged; //Se invoca si cmabia el valor de la energía del jugador

    [Header("Acceleration")]

    [SerializeField] private float acceleration; // Fuerza de Aceleraci�n al recibir inputs de movimiento WASD en el suelo
    [SerializeField] private float decceleration; // Fuerza de deceleraci�n al no recibir inputs de movimiento WASD
    [SerializeField][Range(0.0f, 1.0f)] private float airAccelerationModifier; // Modificación de la aceleración y deceleración cuando el jugador está en el aire
    [SerializeField] private float velPower; // Ni idea de para que sirve, pero aqu� est�, no quitar

    [Header("Friction")]

    [SerializeField] private float groundFrictionAmount; // Fricci�n en el suelo
    [SerializeField] private float airFrictionAmount; // Fricci�n en el aire

    #endregion

    #region Jump

    [Header("Jump")]

    [SerializeField] private float jumpForce; // Fuerza de salto
    [SerializeField][Range(0.0f, 1.0f)] private float jumpCutMultiplier; // Fuerza que acorta el salto cuando se deja de presionar la tecla de saltar
    private bool isPlayerGrounded; // Si el jugador está en el suelo o no

    // Este temporizador junto a su valor por defecto definen el tiempo de coyote, que permite al jugador saltar mientras no
    // haya pasado m�s de x tiempo (x = jumpCoyoteTime), es decir, mientras el valor de lastGroundedTime sea mayor que 0
    private float lastGroundedTime = 0f;
    [SerializeField] private float jumpCoyoteTime;

    // Este temporizador junto a su valor por defecto definen el buffer de salto, que permite al jugador saltar si se ha
    // presionado la tecla correspondiente en los �ltimos x segundos (x = jumpBufferTime), es decir, mientras el valor de
    // lastJumpTime sea mayor que 0
    private float lastJumpTime = 0f;
    [SerializeField] private float jumpBufferTime;

    private float localGravityScale = 1f; // Factor por el que la gravedad afecta al objeto en cada momento, empieza como 1
    [SerializeField] private float fallGravityMultiplier; // Factor por el que aumenta la gravedad al caer

    #endregion

    #region Checks

    [Header("Checks")]

    [SerializeField] private CapsuleCollider groundCheck; // BoxCollider que se usa para realizar un BoxCast en el m�todo IsPlayerGrounded
    [SerializeField] private float maxGroundCheckDistance; // Distancia m�xima para el BoxCast / Distancia a la que el jugador detecta el suelo
    [SerializeField] private LayerMask groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    private AInteractable closestInteractable; // Almacena el objeto interactuable m�s cercano al jugador en todo momento
    [SerializeField] private float interactionDistance; // Distancia m�xima para interactuar con un objeto
    [SerializeField] private LayerMask interactableLayer; // Capa en la cual se encuentran todos los gameObjects con los que el jugador puede interactuar
    public EventHandler<bool> InteractableChanged; // Se invoca cuando el objeto interactivo más cercano cambia

    #endregion

    #region Dropping

    [Header("Dropping")]

    [SerializeField] private float dropDistance = 3f;
    [SerializeField] private float sphereRaycastRadius = 0.5f;
    [SerializeField] private float minimumDistanceFromCollision = 0.5f;

    #endregion

    #region Other

    [Header("Other")]

    [SerializeField] private List<AHoldableObject> holdableObjectList; // Lista con los objetos que puede llevar encima el jugador
    private AHoldableObject currentHeldObject; // Object que lleva encima en cada momento

    private Rigidbody rb; // Referencia al rigidbody del jugador
    private Transform cameraTransform; // Referencia a la cámara principal

    private PauseManager pauseManager;

    private IPlayerReceiver mount;

    #endregion

    #endregion

    #region Getters and Setters

    public AHoldableObject CurrentHeldObject
    {
        get { return currentHeldObject; }
    }

    public float MaxStamina
    {
        get { return maxStamina; }
    }

    #endregion

    #region Initialization

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        cameraTransform = Camera.main.transform;

        maxCurrentSpeed = maxWalkingSpeed;

        currentStamina = maxStamina;

        currentHeldObject = GetComponentInChildren<EmptyWeapon>();
        pauseManager = FindObjectOfType<PauseManager>();
    }

    #endregion

    #region Updates

    /// <summary>
    /// Provoca el efecto de la gravedad en el jugador manualmente
    /// </summary>
    private void FixedUpdate()
    {
        if (pauseManager.isPaused) return;

        rb.AddForce(Physics.gravity * localGravityScale);
    }

    /// <summary>
    /// Temporizadores, salto, gravedad, correr e interactuables
    /// </summary>
    private void Update()
    {
        if (closestInteractable != null)
        {
            Debug.Log(closestInteractable.gameObject.name);
        }

        if (pauseManager.isPaused) return;

        // Comprobar si el jugador está en el suelo

        isPlayerGrounded = IsPlayerGrounded();

        // Actualizar los temporizadores

        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;

        if (isPlayerGrounded)
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

        // Cambiar el modo de movimiento a correr si se cumplen las condiciones

        if (isPressingRunButton && !isRunning && isPlayerGrounded)
        {
            if ((currentStamina / maxStamina) > minimumStaminaForRunning)
            {
                isRunning = true;
            }
        }
        else if (!isPressingRunButton)
        {
            isRunning = false;
        }

        // Modificar la energia dependiendo de si el jugador está corriendo o no, y si se acaba parar de correr

        if (isRunning)
        {
            maxCurrentSpeed = maxRunningSpeed;

            if (currentStamina == 0)
            {
                isRunning = false;
            }
            else
            {
                currentStamina = Mathf.Max(currentStamina - staminaComsumptionRate * Time.deltaTime, 0);
                StaminaChanged?.Invoke(this, currentStamina);
            }
        }
        else
        {
            maxCurrentSpeed = maxWalkingSpeed;
            currentStamina = Mathf.Min(currentStamina + staminaRecoveryRate * Time.deltaTime, maxStamina);
            StaminaChanged?.Invoke(this, currentStamina);
        }

        // Actualizar el valor de closestInteractable para que sea el del IInteractable m�s cercano

        closestInteractable = GetClosestInteractable();
    }

    #endregion

    #region Player Actions

    /// <summary>
    /// Mueve al jugador horizontalmente en la direcci�n recibida, limita la velocidad y a�ade fricci�n
    /// </summary>
    /// <param name="direction">Direcci�n de movimiento recibida a partir del input del jugador</param>
    public void Move(Vector2 direction)
    {
        if (pauseManager.isPaused) return;

        if (mount != null) mount.Move(direction);

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

        if (!isPlayerGrounded)
        {
            accelRate *= airAccelerationModifier;
        }

        float movement = Mathf.Pow(speedDif.magnitude * accelRate, velPower);

        // Aplicaci�n del movimiento mediante una fuerza

        rb.AddForce(move * movement);

        // Limitaci�n de velocidad

        if (horizontalSpeed.magnitude > maxCurrentSpeed)
        {
            if (!isPlayerGrounded && horizontalSpeed.magnitude >= maxRunningSpeed)
            {
                float adjustment = maxRunningSpeed / horizontalSpeed.magnitude;

                rb.velocity = new Vector3(rb.velocity.x * adjustment, rb.velocity.y, rb.velocity.z * adjustment);
            }
            else
            {
                float adjustment = maxCurrentSpeed / horizontalSpeed.magnitude;

                rb.velocity = new Vector3(rb.velocity.x * adjustment, rb.velocity.y, rb.velocity.z * adjustment);
            }
        }

        // Fricci�n en el suelo y el aire cuando el jugador no introduce input de movimiento WASD

        if (move.magnitude < 0.01f)
        {
            if (isPlayerGrounded)
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

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de correr, y llama a un método u otro dependiendo del input
    /// </summary>
    /// <param name="runInput">Tipo de input, Down o Up</param>
    public void Run(IPlayerReceiver.InputType runInput)
    {
        if (pauseManager.isPaused) return;

        if (mount != null) mount.Run(runInput);

        if (runInput != IPlayerReceiver.InputType.Up)
        {
            isPressingRunButton = true;
        }
        else
        {
            isPressingRunButton = false;
        }
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona o suelta la tecla de salto, y llama al m�todo correspondiente
    /// </summary>
    /// <param name="jumpInput">Tipo de input, Down o Up</param>
    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (pauseManager.isPaused) return;

        if (mount != null) mount.Jump(jumpInput);

        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            lastJumpTime = jumpBufferTime;
        }
        else
        {
            if (rb.velocity.y > 0 && !isPlayerGrounded)
            {
                rb.AddForce(Vector3.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode.Impulse);
            }

            lastJumpTime = 0;
        }
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
    /// Se ejecuta cuando el jugador presiona la tecla de ataque, llama al m�todo UseHeldObject del objeto que posee el
    /// jugador en ese momento
    /// </summary>
    /// <param name="useInput">Tipo de input, Down, Hold o Up</param>
    public void UseHeldObject(IPlayerReceiver.InputType useInput)
    {
        if (pauseManager.isPaused) return;

        if (mount != null) mount.UseHeldObject(useInput);

        currentHeldObject.Use(useInput);
    }

    /// <summary>
    /// Se ejecuta cuando el jugador presiona la tecla de interactuar, y ejecuta el m�todo interacted de
    /// closestInteractable
    /// </summary>
    public void Interact(IPlayerReceiver.InputType interactInput)
    {
        if (pauseManager.isPaused) return;

        if (mount != null) mount.Interact(interactInput);

        if (closestInteractable != null)
        {
            closestInteractable.Interacted(interactInput);
        }
    }

    /// <summary>
    /// El jugador suelta en el suelo el objeto que llevaba encima, y pasa a llevar el objeto que recive como
    /// par�metro
    /// El jugador posee todos los objetos, pero solo tiene uno activo en cada momento
    /// </summary>
    /// <param name="objectType">Tipo de objeto al que cambia el jugador</param>
    /// <param name="dropPrefab">Si al cambiar de objeto se instancia el prefab del pickup del anterior</param>
    /// <param name="initializationValue">Valor con el que se inicia el arma, como la carga de la linterna</param>
    public void ChangeHeldObject(IPlayerReceiver.HoldableObjectType objectType, bool dropPrefab, float initializationValue = -1)
    {
        currentHeldObject.Drop(dropPrefab, dropDistance, sphereRaycastRadius, minimumDistanceFromCollision, groundLayer);

        foreach (AHoldableObject holdableObject in holdableObjectList)
        {
            if (objectType == holdableObject.holdableObjectType)
            {
                holdableObject.gameObject.SetActive(true);
                currentHeldObject = holdableObject;

                if (!(initializationValue < 0))
                {
                    currentHeldObject.Initialize(initializationValue);
                }

                return;
            }
        }
    }

    /// <summary>
    /// Es similar al m�todo ChangeHeldObject, la �nica diferencia es que el objeto al que cambia el jugador es el vac�o
    /// </summary>
    public void DropHeldObject()
    {
        if (pauseManager.isPaused) return;

        if (mount != null) mount.DropHeldObject();

        currentHeldObject.Drop(true, dropDistance, sphereRaycastRadius, minimumDistanceFromCollision, groundLayer);
        holdableObjectList[0].gameObject.SetActive(true);
        currentHeldObject = holdableObjectList[0];
    }

    /// <summary>
    /// 
    /// </summary>
    public void AssignMount(IPlayerReceiver mount, GameObject mountingPoint)
    {
        this.mount = mount;
        cameraTransform.gameObject.GetComponent<CameraController>().DisableLook(); //Impide mover la cámara con el ratón
        transform.parent = mountingPoint.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        cameraTransform.localRotation = Quaternion.identity; //Quiero que la cámara siempre mire al frente
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// 
    /// </summary>
    public void DisMount()
    {
        this.mount = null;
        transform.rotation = Quaternion.identity;
        cameraTransform.rotation = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
        transform.parent = null;
        cameraTransform.gameObject.GetComponent<CameraController>().EnableLook();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        JumpAction();
    }


    #endregion

    #region Utility Methods

    /// <summary>
    /// Devuelve true si el jugador est� tocando el suelo, usando un BoxCast
    /// </summary>
    /// <returns>true o false</returns>
    private bool IsPlayerGrounded()
    {
        return Physics.CapsuleCast(groundCheck.transform.position, groundCheck.transform.position, groundCheck.radius, Vector3.down, maxGroundCheckDistance, groundLayer);
    }

    /// <summary>
    /// Devuelve el objeto interactuable m�s cercano al jugador
    /// </summary>
    /// <returns></returns>
    private AInteractable GetClosestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance, interactableLayer);
        AInteractable bestInteractable = null;

        if (hitColliders.Length > 1)
        {
            AInteractable interactable = GetClosestInteractableToLineOfSight(hitColliders);

            if (interactable != null)
            {
                bestInteractable = interactable;
            }
        }
        else if (hitColliders.Length == 1)
        {
            AInteractable interactable = FindParentWithComponent<AInteractable>(hitColliders[0].gameObject.transform);

            if (interactable != null)
            {
                bestInteractable = interactable;
            }
        }

        // Si el objeto anterior no es nulo se desactiva su canvas y outline, y si es distinto al nuevo se le informa
        // que ya no está en el rango del jugador

        if (closestInteractable != null && closestInteractable != bestInteractable)
        {
            closestInteractable.DisableOutlineAndCanvas();
            closestInteractable.PlayerExitedRange();
        }

        // Si el nuevo objeto no es nulo se activa su canvas y outline

        if (bestInteractable != null && bestInteractable != closestInteractable)
        {
            if (bestInteractable.CanBeInteracted)
            {
                bestInteractable.EnableOutlineAndCanvas();
            }
            
            InteractableChanged?.Invoke(this, true);
        }
        else
        {
            InteractableChanged?.Invoke(this, false);
        }

        return bestInteractable;
    }

    public Transform player; // El transform del jugador
    public List<GameObject> objectsToCheck; // Lista de GameObjects a comprobar

    AInteractable GetClosestInteractableToLineOfSight(Collider[] hitColliders)
    {
        AInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        Vector3 playerPosition = cameraTransform.position;
        Vector3 lineOfSightDirection = cameraTransform.forward;

        foreach (Collider obj in hitColliders)
        {
            Vector3 objectPosition = obj.gameObject.transform.position;
            Vector3 toObject = GetClosestPointOnLine(playerPosition, playerPosition + (lineOfSightDirection * interactionDistance), objectPosition) - objectPosition;
            float distanceToLineOfSight = toObject.magnitude;

            if (distanceToLineOfSight < closestDistance)
            {
                AInteractable interactable = FindParentWithComponent<AInteractable>(obj.gameObject.transform);

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

    private Vector3 GetClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float t = Vector3.Dot(point - lineStart, lineDirection) / Vector3.Dot(lineDirection, lineDirection);
        t = Mathf.Clamp01(t);
        Vector3 closestPoint = lineStart + t * lineDirection;
        return closestPoint;
    }

    /// <summary>
    /// Dado un objeto, devuelve el componente del primer padre de la jerarquía que posea dicho componente
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="childTransform"></param>
    /// <returns></returns>
    public T FindParentWithComponent<T>(Transform childTransform) where T : Component
    {
        Transform parent = childTransform.parent;

        // Mientras haya un padre
        while (parent != null)
        {
            // Si el padre tiene el componente que estamos buscando, devuélvelo
            T component = parent.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            // Si no tiene el componente, sigue buscando en el padre del padre
            parent = parent.parent;
        }

        // Si no se encontró el componente en ningún padre, devuelve null
        return null;
    }

    #endregion
}
