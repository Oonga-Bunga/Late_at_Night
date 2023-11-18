using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJsons : MonoBehaviour
{
    [SerializeField] private TextAsset _json01;
    [SerializeField] private TextAsset _json02;

    #region Methods
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetFirstJason(TextAsset json)
    {
        _json01 = json;
    }

    public void SetSecondJason(TextAsset json)
    {
        _json02 = json;
    }
    
    public TextAsset GetFirstJason()
    {
        return _json01;
    }

    public TextAsset GetSecondJason()
    {
        return _json02;
    }
    #endregion
}
