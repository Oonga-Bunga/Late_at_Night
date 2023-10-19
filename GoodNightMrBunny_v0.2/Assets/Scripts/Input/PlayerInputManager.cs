using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    #region InputManager, PlayerController and Awake

    private static PlayerInputManager _instance;
    public static PlayerInputManager Instance => _instance;

    [Header("Player")]

    [SerializeField] private PlayerController _player;
    public PlayerController Player
    {
        get => _player;
        set
        {
            _player = value;
            SetPlayer(_player);
        }
    }

    [Header("Input Actions")]

    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _run;
    [SerializeField] private InputActionReference _jump;
    [SerializeField] private InputActionReference _use;
    [SerializeField] private InputActionReference _drop;
    [SerializeField] private InputActionReference _interact;
    private Dictionary<string, ICommand> _commands;

    /// <summary>
    /// No se esplicar esto sinceramente, pero algo hace
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (_player)
        {
            SetPlayer(_player);
        }
    }

    #endregion

    #region Input Actions

    /// <summary>
    /// Constantemente leemos el input WASD y hacemos que el jugador se mueva acorde a este 
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 value = _move.action.ReadValue<Vector2>();
        _commands["move"].Execute(value);
    }

    /// <summary>
    /// Aquí se comprueba si el jugador está manteniendo pulsado el botón de disparo
    /// </summary>
    private void Update()
    {
        if (_use.action.ReadValue<float>() == 1f)
        {
            _commands["use"].Execute(IPlayerReceiver.InputType.Hold);
        }

        if (_run.action.ReadValue<float>() == 1f)
        {
            _commands["run"].Execute(IPlayerReceiver.InputType.Hold);
        }
    }

    /// <summary>
    /// Lo que hacemos aquí es llenar el diccionario con parejas acción-comando, después para cada input action
    /// le asignamos la acción correspondiente a eventos de presionar y/o soltar el botón correspondiente a la
    /// acción 
    /// En el caso de _move no hace falta porque en el FixedUpdate se usa su valor para llamar al Execute
    /// de "move"
    /// </summary>
    /// <param name="player">Referencia al script PlayerController del jugador</param>
    public void SetPlayer(PlayerController player)
    {
        _commands = new Dictionary<string, ICommand> {
            { "move", new MoveCommand(player) },
            { "run", new RunCommand(player) },
            { "jump", new JumpCommand(player) },
            { "use", new UseHeldObjectCommand(player) },
            { "interact", new InteractCommand(player) },
            { "drop", new DropObjectCommand(player) }
        };

        _move.action.Enable();

        _run.action.started += context =>
        {
            _commands["run"].Execute(IPlayerReceiver.InputType.Down);
        };
        _run.action.canceled += context =>
        {
            _commands["run"].Execute(IPlayerReceiver.InputType.Up);
        };
        _run.action.Enable();

        _jump.action.started += context =>
        {
            _commands["jump"].Execute(IPlayerReceiver.InputType.Down);
        };
        _jump.action.canceled += context =>
        {
            _commands["jump"].Execute(IPlayerReceiver.InputType.Up);
        };
        _jump.action.Enable();

        _use.action.started += context =>
        {
            _commands["use"].Execute(IPlayerReceiver.InputType.Down);
        };
        _use.action.canceled += context =>
        {
            _commands["use"].Execute(IPlayerReceiver.InputType.Up);
        };
        _use.action.Enable();

        _drop.action.started += context =>
        {
            _commands["drop"].Execute();
        };
        _drop.action.Enable();

        _interact.action.started += context =>
        {
            _commands["interact"].Execute(IPlayerReceiver.InputType.Down);
        };
        _interact.action.canceled += context =>
        {
            _commands["interact"].Execute(IPlayerReceiver.InputType.Up);
        };
        _interact.action.Enable();
    }

    #endregion
}
