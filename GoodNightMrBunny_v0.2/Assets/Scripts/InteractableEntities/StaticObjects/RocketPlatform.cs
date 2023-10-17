using UnityEngine;

public class RocketPlatform : AInteractable, IPlayerReceiver
{
    private bool isRocketReady;
    private bool isPlayerMounted;
    private float rocketCooldown;
    private float actualFuckingRotationXValueFuckYou;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxYAngle;
    [SerializeField] private float minYAngle;
    [SerializeField] GameObject rotationPointX;
    [SerializeField] GameObject rotationPointY;
    [SerializeField] GameObject rocket;
    [SerializeField] GameObject mountingPoint;
    private MeshRenderer rocketMeshRenderer;
    private Collider rocketCollider;

    private void Start()
    { 

    }

    protected override void InteractedPressAction()
    {
        player.AssignMount(this, mountingPoint);
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