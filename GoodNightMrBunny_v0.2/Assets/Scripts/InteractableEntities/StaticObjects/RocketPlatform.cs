using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using DG.Tweening;

public class RocketPlatform : AInteractable, IPlayerReceiver
{
    #region Attributes

    [Header("Rocket Platform Settings")]

    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private float _rotationSpeed = 30f;

    [Header("Rocket Platform References")]

    [SerializeField] private GameObject _rotationPoint = null ;
    [SerializeField] private GameObject _lowerArm = null;
    [SerializeField] private GameObject _mountingPoint = null;
    [SerializeField] private GameObject _rocketPlatformModel = null;
    [SerializeField] private GameObject _rocketPrefab = null;

    [SerializeField] private MeshRenderer _laser;

    #endregion

    #region Initialization

    protected override void Awake()
    {
        base.Awake();

        _laser.enabled = false;
    }

    #endregion

    #region Rocket Methods

    private void RechargeRocket()
    {
        _rocketPlatformModel.SetActive(true);
    }

    private void RocketReady()
    {
        _canBeInteracted = true;
    }

    private void LaunchRocket()
    {
        _canBeInteracted = false;
        Instantiate(_rocketPrefab, _rocketPlatformModel.transform.position, _rocketPlatformModel.transform.rotation);
        _rocketPlatformModel.SetActive(false);

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(_rotationPoint.transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, 0f, 0f)), 3));
        mySequence.Append(_lowerArm.transform.DOLocalMoveY(-3, 3));
        mySequence.Insert(3, _lowerArm.transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(0f, 0f, 0f)), 3).OnComplete(RechargeRocket));
        mySequence.AppendInterval(_cooldown);
        mySequence.Append(_lowerArm.transform.DOLocalMoveY(0, 3));
        mySequence.Append(_rotationPoint.transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(60f, 0f, 0f)), 3).OnComplete(RocketReady));
    }

    protected override void InteractedPressAction()
    {
        _player.AssignMount(this, _mountingPoint);
        _canBeInteracted = false;
        _laser.enabled = true;
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
            _laser.enabled = false;
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