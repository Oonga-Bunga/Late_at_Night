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

    public InputAction Move;
    public InputAction Run;
    public InputAction Jump;
    public InputAction Attack;
    public InputAction Interact;
    public InputAction DropWeapon;

    private Dictionary<string, ICommand> commands;

    /// <summary>
    /// Constantemente leemos el input WASD y hacemos que el jugador se mueva acorde a este 
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 value = Move.ReadValue<Vector2>();
        commands["move"].Execute(value);
    }

    /// <summary>
    /// Aquí se comprueba si el jugador está manteniendo pulsado el botón de disparo
    /// </summary>
    private void Update()
    {
        if (Attack.ReadValue<float>() == 1f)
        {
            commands["attack"].Execute(IPlayerReceiver.InputType.Hold);
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
                { "attack", new AttackCommand(player) },
                { "interact", new InteractCommand(player) },
                { "dropWeapon", new DropWeaponCommand(player) }
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

        Attack.started += context =>
        {
            commands["attack"].Execute(IPlayerReceiver.InputType.Down);
        };
        Attack.canceled += context =>
        {
            commands["attack"].Execute(IPlayerReceiver.InputType.Up);
        };
        Attack.Enable();

        Interact.started += context =>
        {
            commands["interact"].Execute();
        };
        Interact.Enable();

        DropWeapon.started += context =>
        {
            commands["dropWeapon"].Execute();
        };
        DropWeapon.Enable();
    }

    #endregion
}
