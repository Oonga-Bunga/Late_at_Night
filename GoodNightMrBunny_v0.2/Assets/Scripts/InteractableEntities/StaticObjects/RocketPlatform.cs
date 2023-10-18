using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class RocketPlatform : AInteractable, IPlayerReceiver
{
    public enum RocketState
    {
        Ready,
        Mounted,
        RotatingBack,
        MovingBack,
        MovingFoward,
        RotatingFoward
    }

    private float rocketCooldown;
    private LineRenderer lineRenderer;
    private bool hasFinishedRotating;
    private bool hasFinishedMoving;
    private RocketState state;
    [SerializeField] private Animator armAnimator;
    [SerializeField] private float rotationSpeed;
    [SerializeField] GameObject rotationPointX;
    [SerializeField] GameObject rotationPointY;
    [SerializeField] GameObject rocket;
    [SerializeField] GameObject mountingPoint;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] private LayerMask groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    private float initialRotation;

    private void Start()
    {
        state = RocketState.Ready;
        canBeInteracted = true;
        lineRenderer = GetComponent<LineRenderer>();
        initialRotation = 0f;
    }

    private void Update()
    {
        switch (state)
        {
            case RocketState.Mounted:
                RaycastHit hit;
                Vector3 impactPoint;

                if (Physics.Raycast(rocket.transform.position, rocket.transform.up, out hit, 100000f))
                {
                    impactPoint = hit.point;
                    ShowLaser(rocket.transform.position, impactPoint);
                }
                else
                {
                    impactPoint = rocket.transform.position + rocket.transform.forward * 100000f;
                    ShowLaser(rocket.transform.position, impactPoint);
                }
                break;
            case RocketState.RotatingBack:
                Vector3 targetEulerAngles = new Vector3(0f, 180f, 0f);
                Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

                // Calcula el ángulo entre los ángulos de inicio y destino
                float angleDifference = Quaternion.Angle(rotationPointX.transform.rotation, targetRotation);

                // Calcula una velocidad basada en la diferencia angular
                float adjustedSpeed = 100f / angleDifference;

                // Aplica el Slerp con la velocidad ajustada
                rotationPointX.transform.rotation = Quaternion.Slerp(rotationPointX.transform.rotation, targetRotation, Time.deltaTime * adjustedSpeed);
                
                if (rotationPointX.transform.rotation.eulerAngles.x == 0f && rotationPointX.transform.rotation.eulerAngles.y == 180f) // Si la diferencia es pequeña, consideramos que la interpolación ha terminado
                {
                    state = RocketState.MovingBack; // Cambiar el estado cuando la interpolación termine
                    armAnimator.SetTrigger("MoveDown");
                }
                break;
            case RocketState.MovingBack:
                if (HasAnimationFinished("MovementAnimation"))
                {
                    rotationPointY.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    rotationPointX.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    rocket.SetActive(true);
                    state = RocketState.MovingFoward;
                    armAnimator.SetTrigger("MoveUp");
                }
                break;
            case RocketState.MovingFoward:
                if (HasAnimationFinished("MovementAnimation 0"))
                {
                    state = RocketState.RotatingFoward;
                    armAnimator.SetTrigger("Static");
                }
                break;
            case RocketState.RotatingFoward:
                float finalSpeed2 = ((60f - rotationPointX.transform.rotation.eulerAngles.x) / 100f);
                float newXRotationFoward = Mathf.Lerp(rotationPointX.transform.rotation.eulerAngles.x, 60f, 0.5f * (Time.deltaTime / finalSpeed2));
                rotationPointX.transform.rotation = Quaternion.Euler(newXRotationFoward, rotationPointX.transform.rotation.eulerAngles.y, rotationPointX.transform.rotation.eulerAngles.z);

                if (Mathf.Abs(newXRotationFoward - 60f) < 0.01f) // Si la diferencia es pequeña, consideramos que la interpolación ha terminado
                {
                    state = RocketState.Ready; // Cambiar el estado cuando la interpolación termine
                    canBeInteracted = true;
                }
                break;
        }
    }

    bool HasAnimationFinished(string animationName)
    {
        // Obtiene el estado actual de la animación en la capa 0
        AnimatorStateInfo stateInfo = armAnimator.GetCurrentAnimatorStateInfo(0);

        // Comprueba si el nombre de la animación actual coincide con el nombre de la animación que estamos buscando
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
        if (state != RocketState.Ready) { return; }

        initialRotation = rotationPointX.transform.rotation.eulerAngles.x;
        canBeInteracted = false;
        state = RocketState.RotatingBack;
        Instantiate(rocketPrefab, rocket.transform.position, rocket.transform.rotation);
        rocket.SetActive(false);



        //activar el cohete cuando el brazo se haya replegado
        //rocket.SetActive(true);

        //salir de la base con el cohete y ponerse en posición
        //permitir que el jugador interactue
    }

    protected override void InteractedPressAction()
    {
        player.AssignMount(this, mountingPoint);
        state = RocketState.Mounted;
        canBeInteracted = false;
        DisableOutlineAndCanvas();
        lineRenderer.enabled = true;
    }

    public void Move(Vector2 direction)
    {
        // Obt�n la rotaci�n actual del punto de rotaci�n como cuaterni�n
        Quaternion currentRotationX = rotationPointX.transform.rotation;
        Quaternion currentRotationY = rotationPointY.transform.rotation;

        // Calcula la rotaci�n adicional en el eje X
        Quaternion xRotation = Quaternion.AngleAxis(-direction.y * Time.deltaTime * rotationSpeed, Vector3.right);
        // Calcula la rotaci�n adicional en el eje Y
        Quaternion yRotation = Quaternion.AngleAxis(direction.x * Time.deltaTime * rotationSpeed, Vector3.up);

        Quaternion newRotationX = currentRotationX * xRotation;
        Quaternion newRotationY = currentRotationY * yRotation;

        // Asigna la nueva rotaci�n al punto de rotaci�n
        rotationPointX.transform.rotation = newRotationX;
        rotationPointY.transform.rotation = newRotationY;

        Vector3 suputamadre = rotationPointX.transform.rotation.eulerAngles;

        if (suputamadre.z == 180f && suputamadre.x < 60)
        {
            suputamadre.x = 60;
        }
        else if (suputamadre.y == 180f && suputamadre.x >= 180)
        {
            suputamadre.x = 0;
        }

        rotationPointX.transform.rotation = Quaternion.Euler(suputamadre);
    }

    public void Run(IPlayerReceiver.InputType runInput)
    {

    }

    public void Jump(IPlayerReceiver.InputType jumpInput)
    {
        if (jumpInput == IPlayerReceiver.InputType.Down)
        {
            player.DisMount();
            state = RocketState.Ready;
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
}