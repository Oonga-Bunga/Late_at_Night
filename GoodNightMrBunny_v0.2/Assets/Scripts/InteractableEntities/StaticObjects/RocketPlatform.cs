using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class RocketPlatform : AInteractable, IPlayerReceiver
{
    #region Attributes

    public enum RocketPlatformState
    {
        Ready,
        Mounted,
        RotatingDown,
        MovingDown,
        MovingUp,
        RotatingUp
    }

    private RocketPlatformState _state;

    [Header("Rocket Platform Settings")]

    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private float _rotationSpeed = 30f;

    [Header("Rocket Platform References")]

    [SerializeField] private GameObject _rotationPoint = null ;
    [SerializeField] private GameObject _lowerArm = null;
    private Animator _lowerArmAnimator;
    [SerializeField] private GameObject _mountingPoint = null;
    [SerializeField] private GameObject _rocketPlatformModel = null;
    [SerializeField] private GameObject _rocketPrefab = null;
    [SerializeField] private LayerMask _groundLayer;

    private LineRenderer _lineRenderer;

    #endregion

    #region Initialization

    private void Start()
    {
        _state = RocketPlatformState.Ready;
        _lowerArmAnimator = _lowerArm.GetComponent<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    #endregion

    #region Update

    private void Update()
    {
        switch (_state)
        {
            case RocketPlatformState.Mounted:
                ShowLaser();
                break;
            case RocketPlatformState.RotatingDown:
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

                // Calcula el ángulo entre los ángulos de inicio y destino
                float angleDifference = Quaternion.Angle(_rotationPoint.transform.rotation, targetRotation);

                // Calcula una velocidad basada en la diferencia angular
                float adjustedSpeed = 100f / angleDifference;

                // Aplica el Slerp con la velocidad ajustada
                _rotationPoint.transform.rotation = Quaternion.Slerp(_rotationPoint.transform.rotation, targetRotation, Time.deltaTime * adjustedSpeed);
                
                if (_rotationPoint.transform.rotation.eulerAngles.x == 0f && _rotationPoint.transform.rotation.eulerAngles.y == 180f) // Si la diferencia es pequeña, consideramos que la interpolación ha terminado
                {
                    _state = RocketPlatformState.MovingDown; // Cambiar el estado cuando la interpolación termine
                    _lowerArmAnimator.SetTrigger("MoveDown");
                }
                break;
            case RocketPlatformState.MovingDown:
                if (HasAnimationFinished("MoveDown"))
                {
                    _lowerArm.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    _rotationPoint.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    _state = RocketPlatformState.MovingUp;
                    Invoke("RechargeRocket", _cooldown);
                }
                break;
            case RocketPlatformState.MovingUp:
                if (HasAnimationFinished("MoveUp"))
                {
                    _state = RocketPlatformState.RotatingUp;
                    _lowerArmAnimator.SetTrigger("Static");
                }
                break;
            case RocketPlatformState.RotatingUp:
                float finalSpeed2 = ((60f - _rotationPoint.transform.rotation.eulerAngles.x) / 100f);
                float newXRotationFoward = Mathf.Lerp(_rotationPoint.transform.rotation.eulerAngles.x, 60f, 0.5f * (Time.deltaTime / finalSpeed2));
                _rotationPoint.transform.rotation = Quaternion.Euler(newXRotationFoward, _rotationPoint.transform.rotation.eulerAngles.y, _rotationPoint.transform.rotation.eulerAngles.z);

                if (Mathf.Abs(newXRotationFoward - 60f) < 0.01f) // Si la diferencia es pequeña, consideramos que la interpolación ha terminado
                {
                    _state = RocketPlatformState.Ready; // Cambiar el estado cuando la interpolación termine
                    _canBeInteracted = true;
                }
                break;
        }
    }

    #endregion

    #region Rocket Methods

    private void RechargeRocket()
    {
        _rocketPlatformModel.SetActive(true);
        _lowerArmAnimator.SetTrigger("MoveUp");
    }

    private bool HasAnimationFinished(string animationName)
    {
        // Obtiene el estado actual de la animación en la capa 0
        AnimatorStateInfo stateInfo = _lowerArmAnimator.GetCurrentAnimatorStateInfo(0);

        // Comprueba si el nombre de la animación actual coincide con el nombre de la animación que estamos buscando y si esta ha terminado
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }

    private void ShowLaser()
    {
        RaycastHit hit;
        Vector3 impactPoint;

        if (Physics.Raycast(_rocketPlatformModel.transform.position, _rocketPlatformModel.transform.up, out hit, 100000f))
        {
            impactPoint = hit.point;
        }
        else
        {
            impactPoint = _rocketPlatformModel.transform.position + _rocketPlatformModel.transform.forward * 100000f;
        }

        _lineRenderer.startWidth = 0.5f;
        _lineRenderer.endWidth = 0.5f;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _rocketPlatformModel.transform.position);
        _lineRenderer.SetPosition(1, impactPoint);
    }

    private void LaunchRocket()
    {
        if (_state != RocketPlatformState.Ready) { return; }

        _canBeInteracted = false;
        _state = RocketPlatformState.RotatingDown;
        Instantiate(_rocketPrefab, _rocketPlatformModel.transform.position, _rocketPlatformModel.transform.rotation);
        _rocketPlatformModel.SetActive(false);
    }

    protected override void InteractedPressAction()
    {
        _player.AssignMount(this, _mountingPoint);
        _state = RocketPlatformState.Mounted;
        _canBeInteracted = false;
        DisableOutlineAndCanvas();
        _lineRenderer.enabled = true;
    }

    #endregion

    #region Player Actions

    public void Move(Vector2 direction)
    {
        // Obt�n la rotaci�n actual del punto de rotaci�n como cuaterni�n
        Quaternion currentRotationX = _rotationPoint.transform.rotation;
        Quaternion currentRotationY = _lowerArm.transform.rotation;

        // Calcula la rotaci�n adicional en el eje X
        Quaternion xRotation = Quaternion.AngleAxis(-direction.y * Time.deltaTime * _rotationSpeed, Vector3.right);
        // Calcula la rotaci�n adicional en el eje Y
        Quaternion yRotation = Quaternion.AngleAxis(direction.x * Time.deltaTime * _rotationSpeed, Vector3.up);

        Quaternion newRotationX = currentRotationX * xRotation;
        Quaternion newRotationY = currentRotationY * yRotation;

        // Asigna la nueva rotaci�n al punto de rotaci�n
        _rotationPoint.transform.rotation = newRotationX;
        _lowerArm.transform.rotation = newRotationY;

        Vector3 suputamadre = _rotationPoint.transform.rotation.eulerAngles;

        if (suputamadre.z == 180f && suputamadre.x < 60)
        {
            suputamadre.x = 60;
        }
        else if (suputamadre.y == 180f && suputamadre.x >= 180)
        {
            suputamadre.x = 0;
        }

        _rotationPoint.transform.rotation = Quaternion.Euler(suputamadre);
    }

    public void Run(IPlayerReceiver.InputType runInput)
    {

    }

    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            _player.DisMount();
            _state = RocketPlatformState.Ready;
            _canBeInteracted = true;
            _lineRenderer.enabled = false;
            LaunchRocket();
        }
    }

    public void UseHeldObject(IPlayerReceiver.InputType useImput)
    {

    }

    public void Interact(IPlayerReceiver.InputType interactInput)
    {

    }

    public void ChangeHeldObject(IPlayerReceiver.HoldableObjectType objectType, bool dropPrefab, float initializationValue)
    {

    }

    public void DropHeldObject()
    {

    }

    #endregion
}