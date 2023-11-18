using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJsons : MonoBehaviour
{
    [SerializeField] private TextAsset _sceneJsonFile;
    [SerializeField] private TextAsset _enemyWavesJsonFile;

    #region Methods
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
