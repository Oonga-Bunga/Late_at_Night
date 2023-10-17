using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class RocketPlatform : AInteractable, IPlayerReceiver
{
    private bool isRocketReady;
    private bool isPlayerMounted;
    private float rocketCooldown;
    private LineRenderer lineRenderer;
    [SerializeField] private float rotationSpeed;
    [SerializeField] GameObject rotationPointX;
    [SerializeField] GameObject rotationPointY;
    [SerializeField] GameObject rocket;
    [SerializeField] GameObject mountingPoint;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] private LayerMask groundLayer; // Capa en la cual se encuentran todos los gameObjects que sirven como suelo al jugador

    private void Start()
    {
        isPlayerMounted = false;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isPlayerMounted)
        {
            RaycastHit hit;
            Vector3 impactPoint;

            if (Physics.Raycast(rocket.transform.position, rocket.transform.up, out hit, 100000f))
            {
                // Si hay una colisión, muestra el láser y el efecto visual de impacto.
                impactPoint = hit.point;
                MostrarLaser(rocket.transform.position, impactPoint);
                MostrarImpacto(impactPoint);
            }
            else
            {
                // Si no hay colisión, muestra el láser hasta el punto máximo y el efecto visual de impacto en ese punto.
                impactPoint = rocket.transform.position + rocket.transform.forward * 100000f;
                MostrarLaser(rocket.transform.position, impactPoint);
                MostrarImpacto(impactPoint);
            }
        }
    }

    private void MostrarImpacto(Vector3 position)
    {
        //ParticleSystem impacto = Instantiate(impactoPrefab, position, Quaternion.identity);
        //Destroy(impacto.gameObject, impacto.main.duration); // Destruye el objeto después de que termine la animación.
    }

    private void MostrarLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    protected override void InteractedPressAction()
    {
        player.AssignMount(this, mountingPoint);
        isPlayerMounted = true;
        DisableOutlineAndCanvas();
    }

    public void Move(Vector2 direction)
    {
        // Obt�n la rotaci�n actual del punto de rotaci�n como cuaterni�n
        Quaternion currentRotationX = rotationPointX.transform.rotation;
        Quaternion currentRotationY = rotationPointY.transform.rotation;

        // Calcula la rotaci�n adicional en el eje X
        Quaternion xRotation = Quaternion.AngleAxis(direction.y * Time.deltaTime * rotationSpeed, Vector3.right);
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
            isPlayerMounted = false;
            EnableOutlineAndCanvas();
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