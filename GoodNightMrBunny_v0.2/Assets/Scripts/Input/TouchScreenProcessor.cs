using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class MyValueShiftProcessor : InputProcessor<Vector2>
{
    private List<int> _bannedTouches = new List<int>(); // Lista de toques en la pantalla tactil que no se usar�n para mover la c�mara
    Vector2 rotation = Vector2.zero; // Rotaci�n de la c�mara

#if UNITY_EDITOR
    static MyValueShiftProcessor()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<MyValueShiftProcessor>();
    }

    [Tooltip("Number to add to incoming values.")]
    [SerializeField] private float valueShift = 0;

    public override Vector2 Process(Vector2 value, InputControl control)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (!_bannedTouches.Contains(touch.fingerId))
            {
                if (!IsPointerOverUIObject(touch))
                {
                    Debug.Log("bad");
                    return -touch.deltaPosition;
                }
                else
                {
                    Debug.Log("hey");
                    _bannedTouches.Add(touch.fingerId);
                }
            }
            else
            {
                if (touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    _bannedTouches.Remove(touch.fingerId);
                }
            }
        }

        return Vector2.zero;
    }

    private bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("TouchIgnore"))
            {
                return true;
            }
        }
        
        return false;
    }
}