using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region InputManager, PlayerController and Awake

    private static InputManager instance;
    public static InputManager Instance => instance;

    [SerializeField] private PlayerController player;
    public PlayerController Player
    {
        get => player;
        set
        {
            player = value;
            SetPlayer(player);
        }
    }

    [SerializeField] private InputAction Move;
    [SerializeField] private InputAction Run;
    [SerializeField] private InputAction Jump;
    [SerializeField] private InputAction Use;
    [SerializeField] private InputAction Interact;
    [SerializeField] private InputAction DropWeapon;

    [SerializeField] private Joystick moveJoystick;

    private Dictionary<string, ICommand> commands;

    /// <summary>
    /// No se esplicar esto sinceramente, pero algo hace
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (player)
        {
            SetPlayer(player);
        }
    }

    #endregion

    #region Input Actions

    /// <summary>
    /// Constantemente leemos el input WASD y hacemos que el jugador se mueva acorde a este 
    /// </summary>
    private void FixedUpdate()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            //M�vil
            float x = moveJoystick.Horizontal;
            float z = moveJoystick.Vertical;
            Vector2 joy = new Vector2(x, z);
            commands["move"].Execute(joy);
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //PC
            Vector2 value = Move.ReadValue<Vector2>();
            commands["move"].Execute(value);
        }
    }

    /// <summary>
    /// Aqu� se comprueba si el jugador est� manteniendo pulsado el bot�n de disparo
    /// </summary>
    private void Update()
    {
        if (Use.ReadValue<float>() == 1f)
        {
            commands["use"].Execute(IPlayerReceiver.InputType.Hold);
        }

        if (Run.ReadValue<float>() == 1f)
        {
            commands["run"].Execute(IPlayerReceiver.InputType.Hold);
        }
    }

    /// <summary>
    /// Lo que hacemos aqu� es llenar el diccionario con parejas acci�n-comando, despu�s para cada input action
    /// le asignamos la acci�n correspondiente a eventos de presionar y/o soltar el bot�n correspondiente a la
    /// acci�n 
    /// En el caso de Move no hace falta porque en el FixedUpdate se usa su valor para llamar al Execute
    /// de "move"
    /// </summary>
    /// <param name="player">Referencia al script PlayerController del jugador</param>
    public void SetPlayer(PlayerController player)
    {
        commands = new Dictionary<string, ICommand> {
                { "move", new MoveCommand(player) },
                { "run", new RunCommand(player) },
                { "jump", new JumpCommand(player) },
                { "use", new UseHeldObjectCommand(player) },
                { "interact", new InteractCommand(player) },
                { "drop", new DropObjectCommand(player) }
            };

        Move.Enable();

        Run.started += context =>
        {
            commands["run"].Execute(IPlayerReceiver.InputType.Down);
        };
        Run.canceled += context =>
        {
            commands["run"].Execute(IPlayerReceiver.InputType.Up);
        };
        Run.Enable();

        Jump.started += context =>
        {
            commands["jump"].Execute(IPlayerReceiver.InputType.Down);
        };
        Jump.canceled += context =>
        {
            commands["jump"].Execute(IPlayerReceiver.InputType.Up);
        };
        Jump.Enable();

        Use.started += context =>
        {
            commands["use"].Execute(IPlayerReceiver.InputType.Down);
        };
        Use.canceled += context =>
        {
            commands["use"].Execute(IPlayerReceiver.InputType.Up);
        };
        Use.Enable();

        Interact.started += context =>
        {
            commands["interact"].Execute(IPlayerReceiver.InputType.Down);
        };
        Interact.canceled += context =>
        {
            commands["interact"].Execute(IPlayerReceiver.InputType.Up);
        };
        Interact.Enable();

        DropWeapon.started += context =>
        {
            commands["drop"].Execute();
        };
        DropWeapon.Enable();
    }

    #endregion
}