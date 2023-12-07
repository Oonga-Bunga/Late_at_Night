using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerReceiver
{
    private static PlayerController _instance; // Instancia del jugador, singleton

    public static PlayerController Instance => _instance;

    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement; // Referencia al script que maneja el movimiento del jugador
    [SerializeField] private PlayerWeapons _playerWeapons; // Referencia al script que maneja los holdable objects del jugador
    [SerializeField] private PlayerInteraction _playerInteraction; // Referencia al script que maneja la interacción del jugador con objetos interactivos
    [SerializeField] private PlayerHealth _playerHealth; // Referencia al script que maneja la vida del jugador
    [SerializeField] private Transform _cameraHolder; // Referencia a la cámara principal

    public PlayerMovement PlayerMovement => _playerMovement;
    public PlayerWeapons PlayerWeapons => _playerWeapons;
    public PlayerInteraction PlayerInteraction => _playerInteraction;
    public PlayerHealth PlayerHealth => _playerHealth;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        PlayerInputManager.Instance.Player = this;
    }


    #region Player Actions

    public void Move(Vector2 direction)
    {
        _playerMovement.Move(direction);
    }

    public void Run(IPlayerReceiver.InputType runInput)
    {
        _playerMovement.Run(runInput);
    }

    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        _playerMovement.Jump(jumpInput);
    }

    public void UseHeldObject(IPlayerReceiver.InputType useInput)
    {
        _playerWeapons.UseHeldObject(useInput);
    }

    public void ChangeHeldObject(IPlayerReceiver.HoldableObjectType objectType, bool dropPrefab, float initializationValue = -1)
    {
        _playerWeapons.ChangeHeldObject(objectType, dropPrefab, initializationValue);
    }

    public void DropHeldObject()
    {
        _playerWeapons.DropHeldObject();
    }

    public void Interact(IPlayerReceiver.InputType interactInput)
    {
        _playerInteraction.Interact(interactInput);
    }

    #endregion

    #region Mounting Methods

    /// <summary>
    /// Asigna al jugador el objeto sobre el que está montado, y es llamado por dicho objeto
    /// Los controles del jugador pasarán a ser procesados por la montura, y su movimiento queda anclado a esta
    /// </summary>
    /// <param name="mount">Referencia al script del objeto que va a montar el jugador</param>
    /// <param name="mountingPoint">Punto en el que el jugador se situa respecto a la montura</param>
    public void Mount(IPlayerReceiver mount, GameObject mountingPoint)
    {
        PlayerInputManager.Instance.Player = mount;

        transform.parent = mountingPoint.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _cameraHolder.GetComponent<Player.CameraController>().PlayerMounting();

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// Llamado por la montura del jugador cuando este se desmonta, deshace los cambios hechos en Mount
    /// El jugador siempre ejecuta un salto al desmontarse
    /// </summary>
    public void Dismount()
    {
        PlayerInputManager.Instance.Player = this;

        transform.parent = null;
        transform.rotation = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z));

        _cameraHolder.GetComponent<Player.CameraController>().PlayerDismounting();

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        _playerMovement.JumpAction();
    }

    #endregion
}