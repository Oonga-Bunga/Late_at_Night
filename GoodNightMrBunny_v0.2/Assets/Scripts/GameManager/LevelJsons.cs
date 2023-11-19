using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJsons : MonoBehaviour
{
    private TextAsset _sceneJsonFile;
    private TextAsset _enemyWavesJsonFile;

    #region Methods

    private void Awake()
    {
        
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public TextAsset SceneJsonFile
    {
        get { return _sceneJsonFile; }
        set { _sceneJsonFile = value; }
    }

    public TextAsset EnemyWavesJsonFile
    {
        get { return _enemyWavesJsonFile; }
        set { _enemyWavesJsonFile = value; }
    }
    #endregion
}
