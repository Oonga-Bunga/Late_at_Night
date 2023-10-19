using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    #region InputManager, PlayerController and Awake

    private static PlayerInputManager instance;
    public static PlayerInputManager Instance => instance;

    [Header("Player")]

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

    [Header("Input Actions")]

    [SerializeField] private InputActionReference Move;
    [SerializeField] private InputActionReference Run;
    [SerializeField] private InputActionReference Jump;
    [SerializeField] private InputActionReference Use;
    [SerializeField] private InputActionReference Drop;
    [SerializeField] private InputActionReference Interact;
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
        Vector2 value = Move.action.ReadValue<Vector2>();
        commands["move"].Execute(value);
    }

    /// <summary>
    /// Aquí se comprueba si el jugador está manteniendo pulsado el botón de disparo
    /// </summary>
    private void Update()
    {
        if (Use.action.ReadValue<float>() == 1f)
        {
            commands["use"].Execute(IPlayerReceiver.InputType.Hold);
        }

        if (Run.action.ReadValue<float>() == 1f)
        {
            commands["run"].Execute(IPlayerReceiver.InputType.Hold);
        }
    }

    /// <summary>
    /// Lo que hacemos aquí es llenar el diccionario con parejas acción-comando, después para cada input action
    /// le asignamos la acción correspondiente a eventos de presionar y/o soltar el botón correspondiente a la
    /// acción 
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

        Move.action.Enable();

        Run.action.started += context =>
        {
            commands["run"].Execute(IPlayerReceiver.InputType.Down);
        };
        Run.action.canceled += context =>
        {
            commands["run"].Execute(IPlayerReceiver.InputType.Up);
        };
        Run.action.Enable();

        Jump.action.started += context =>
        {
            commands["jump"].Execute(IPlayerReceiver.InputType.Down);
        };
        Jump.action.canceled += context =>
        {
            commands["jump"].Execute(IPlayerReceiver.InputType.Up);
        };
        Jump.action.Enable();

        Use.action.started += context =>
        {
            commands["use"].Execute(IPlayerReceiver.InputType.Down);
        };
        Use.action.canceled += context =>
        {
            commands["use"].Execute(IPlayerReceiver.InputType.Up);
        };
        Use.action.Enable();

        Drop.action.started += context =>
        {
            commands["drop"].Execute();
        };
        Drop.action.Enable();

        Interact.action.started += context =>
        {
            commands["interact"].Execute(IPlayerReceiver.InputType.Down);
        };
        Interact.action.canceled += context =>
        {
            commands["interact"].Execute(IPlayerReceiver.InputType.Up);
        };
        Interact.action.Enable();
    }

    #endregion
}
