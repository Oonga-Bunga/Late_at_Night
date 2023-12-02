using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBehaviour : MonoBehaviour
{
    #region Attributes
    [SerializeField] private List<Image> _switchesIcons;
    [SerializeField] private Sprite _switchOnImage;
    [SerializeField] private Sprite _switchOffImage;

    private List<Switch> _switches;
    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        _switches = GameManager.SwitchListInstance;
        Debug.Log("SWITCHES COUNT = "+_switches.Count);
        for(int i = 0; i < _switches.Count; i++)
        {
            _switches[i].OnTurnedOnOrOff += SwitchOnOrOff;
            _switches[i].OnAttacked += SwitchAttacked;
            _switches[i].OnPlayerRange += SwitchLookAt;
            _switchesIcons[i].enabled = true;
        }
    }

    private void SwitchOnOrOff(object light, bool value)
    {
        int lightIdx = _switches.IndexOf((Switch)light);
        if (value)
        {
            _switchesIcons[lightIdx].sprite = _switchOnImage;
        }
        else
        {
            _switchesIcons[lightIdx].sprite = _switchOffImage;
        }
    }

    private void SwitchAttacked(object light, bool value)
    {
        int lightIdx = _switches.IndexOf((Switch)light);
        _switchesIcons[lightIdx].GetComponent<Animator>().SetTrigger("SwitchState");
    }

    private void SwitchLookAt(object light, bool lookingAt)
    {
        int lightIdx = _switches.IndexOf((Switch)light);
        if (lookingAt)
        {
            _switchesIcons[lightIdx].GetComponent<UnityEngine.UI.Outline>().enabled = true;
        }
        else
        {
            _switchesIcons[lightIdx].GetComponent<UnityEngine.UI.Outline>().enabled = false;
        }
    }
}
