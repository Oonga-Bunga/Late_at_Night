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

    private RocketPlatformState state;

    [Header("Rocket Platform Settings")]

    [SerializeField] private float cooldown;
    [SerializeField] private float rotationSpeed;

    [Header("Rocket Platform References")]

    [SerializeField] private GameObject rotationPoint;
    [SerializeField] private GameObject lowerArm;
    private Animator lowerArmAnimator;
    [SerializeField] private GameObject mountingPoint;
    [SerializeField] private GameObject rocketPlatformModel;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private LayerMask groundLayer;

    private LineRenderer lineRenderer;

    #endregion

    #region Initialization

    private void Start()
    {
        state = RocketPlatformState.Ready;
        lowerArmAnimator = lowerArm.GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    #endregion

    #region Update

    private void Update()
    {
        switch (state)
        {
            case RocketPlatformState.Mounted:
                RaycastHit hit;
                Vector3 impactPoint;

                if (Physics.Raycast(rocketPlatformModel.transform.position, rocketPlatformModel.transform.up, out hit, 100000f))
                {
                    impactPoint = hit.point;
                    ShowLaser(rocketPlatformModel.transform.position, impactPoint);
                }
                else
                {
                    impactPoint = rocketPlatformModel.transform.position + rocketPlatformModel.transform.forward * 100000f;
                    ShowLaser(rocketPlatformModel.transform.position, impactPoint);
                }
                break;
            case RocketPlatformState.RotatingDown:
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

                // Calcula el ángulo entre los ángulos de inicio y destino
                float angleDifference = Quaternion.Angle(rotationPoint.transform.rotation, targetRotation);

                // Calcula una velocidad basada en la diferencia angular
                float adjustedSpeed = 100f / angleDifference;

                // Aplica el Slerp con la velocidad ajustada
                rotationPoint.transform.rotation = Quaternion.Slerp(rotationPoint.transform.rotation, targetRotation, Time.deltaTime * adjustedSpeed);
                
                if (rotationPoint.transform.rotation.eulerAngles.x == 0f && rotationPoint.transform.rotation.eulerAngles.y == 180f) // Si la diferencia es pequeña, consideramos que la interpolación ha terminado
                {
                    state = RocketPlatformState.MovingDown; // Cambiar el estado cuando la interpolación termine
                    lowerArmAnimator.SetTrigger("MoveDown");
                }
                break;
            case RocketPlatformState.MovingDown:
                if (HasAnimationFinished("MoveDown"))
                {
                    lowerArm.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    rotationPoint.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    state = RocketPlatformState.MovingUp;
                    Invoke("RechargeRocket", cooldown);
                }
                break;
            case RocketPlatformState.MovingUp:
                if (HasAnimationFinished("MoveUp"))
                {
                    state = RocketPlatformState.RotatingUp;
                    lowerArmAnimator.SetTrigger("Static");
                }
                break;
            case RocketPlatformState.RotatingUp:
                float finalSpeed2 = ((60f - rotationPoint.transform.rotation.eulerAngles.x) / 100f);
                float newXRotationFoward = Mathf.Lerp(rotationPoint.transform.rotation.eulerAngles.x, 60f, 0.5f * (Time.deltaTime / finalSpeed2));
                rotationPoint.transform.rotation = Quaternion.Euler(newXRotationFoward, rotationPoint.transform.rotation.eulerAngles.y, rotationPoint.transform.rotation.eulerAngles.z);

                if (Mathf.Abs(newXRotationFoward - 60f) < 0.01f) // Si la diferencia es pequeña, consideramos que la interpolación ha terminado
                {
                    state = RocketPlatformState.Ready; // Cambiar el estado cuando la interpolación termine
                    canBeInteracted = true;
                }
                break;
        }
    }

    #endregion

    #region Rocket Methods

    private void RechargeRocket()
    {
        rocketPlatformModel.SetActive(true);
        lowerArmAnimator.SetTrigger("MoveUp");
    }

    private bool HasAnimationFinished(string animationName)
    {
        // Obtiene el estado actual de la animación en la capa 0
        AnimatorStateInfo stateInfo = lowerArmAnimator.GetCurrentAnimatorStateInfo(0);

        // Comprueba si el nombre de la animación actual coincide con el nombre de la animación que estamos buscando y si esta ha terminado
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }

    private void ShowLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void LaunchRocket()
    {
        if (state != RocketPlatformState.Ready) { return; }

        canBeInteracted = false;
        state = RocketPlatformState.RotatingDown;
        Instantiate(rocketPrefab, rocketPlatformModel.transform.position, rocketPlatformModel.transform.rotation);
        rocketPlatformModel.SetActive(false);
    }

    protected override void InteractedPressAction()
    {
        player.AssignMount(this, mountingPoint);
        state = RocketPlatformState.Mounted;
        canBeInteracted = false;
        DisableOutlineAndCanvas();
        lineRenderer.enabled = true;
    }

    #endregion

    #region Player Actions

    public void Move(Vector2 direction)
    {
        // Obt�n la rotaci�n actual del punto de rotaci�n como cuaterni�n
        Quaternion currentRotationX = rotationPoint.transform.rotation;
        Quaternion currentRotationY = lowerArm.transform.rotation;

        // Calcula la rotaci�n adicional en el eje X
        Quaternion xRotation = Quaternion.AngleAxis(-direction.y * Time.deltaTime * rotationSpeed, Vector3.right);
        // Calcula la rotaci�n adicional en el eje Y
        Quaternion yRotation = Quaternion.AngleAxis(direction.x * Time.deltaTime * rotationSpeed, Vector3.up);

        Quaternion newRotationX = currentRotationX * xRotation;
        Quaternion newRotationY = currentRotationY * yRotation;

        // Asigna la nueva rotaci�n al punto de rotaci�n
        rotationPoint.transform.rotation = newRotationX;
        lowerArm.transform.rotation = newRotationY;

        Vector3 suputamadre = rotationPoint.transform.rotation.eulerAngles;

        if (suputamadre.z == 180f && suputamadre.x < 60)
        {
            suputamadre.x = 60;
        }
        else if (suputamadre.y == 180f && suputamadre.x >= 180)
        {
            suputamadre.x = 0;
        }

        rotationPoint.transform.rotation = Quaternion.Euler(suputamadre);
    }

    public void Run(IPlayerReceiver.InputType runInput)
    {

    }

    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            player.DisMount();
            state = RocketPlatformState.Ready;
            canBeInteracted = true;
            lineRenderer.enabled = false;
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