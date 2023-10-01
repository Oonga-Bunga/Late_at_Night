using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public float SensitivityX
    {
        get { return sensitivityX; }
        set { sensitivityX = value; }
    }

    public float SensitivityY
    {
        get { return sensitivityY; }
        set { sensitivityY = value; }
    }

    [Range(0.1f, 9f)][SerializeField] float sensitivityX = 8f;
    [Range(0.1f, 9f)][SerializeField] float sensitivityY = 4f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    [SerializeField] private InputAction mouseDeltaAction;
    private List<int> bannedTouches = new List<int>();

    Vector2 rotation = Vector2.zero;

    public delegate void Look(); //Delegate with the firing function of the gun that depends on the GunObject
    public Look lookFunction; //Firing function

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            lookFunction = HandleMobileInput;
        }
        else
        {
            lookFunction = HandleMobileInput; // Debería ser PC, está así provisionalmente para provarlo con unity remote
        }
    }

    private void Update()
    {
        lookFunction();
    }

    private void HandlePCInput()
    {
        Vector2 delta = mouseDeltaAction.ReadValue<Vector2>();
        rotation.x += delta.x * sensitivityX;
        rotation.y += delta.y * sensitivityY;   
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
    }

    private void HandleMobileInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            // Verifica si el objeto golpeado tiene la etiqueta deseada
            // O si está en la capa deseada
            if (!bannedTouches.Contains(touch.fingerId))
            {
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Vector2 delta = touch.deltaPosition;
                    rotation.x += delta.x * sensitivityX * 0.1f;
                    rotation.y += delta.y * sensitivityY * 0.1f;
                    rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
                    var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
                    var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

                    transform.localRotation = xQuat * yQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
                }
                else
                {
                    bannedTouches.Add(touch.fingerId);
                }
            }
            else
            {
                if (touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    bannedTouches.Remove(touch.fingerId);
                }
            }
        }
    }
}