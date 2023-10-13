using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class ButtonSpriteMapping
{
    public string keyName;
    public Sprite sprite;
}

public class ButtonPrompt : MonoBehaviour
{
    public Sprite defaultSprite;

    [SerializeField]
    private InputActionReference actionReference;

    [SerializeField]
    private List<ButtonSpriteMapping> spriteMappings;

    private Image buttonImage;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();

        string buttonName;

        if (Application.isMobilePlatform)
        {
            buttonName = actionReference.action.bindings[1].ToDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice);
        }
        else
        {
            buttonName = actionReference.action.bindings[0].ToDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice);
        }

        ButtonSpriteMapping spriteMapping = spriteMappings.FirstOrDefault(mapping => mapping.keyName == buttonName);

        buttonImage.sprite = spriteMapping != null ? spriteMapping.sprite : defaultSprite;
    }
}
