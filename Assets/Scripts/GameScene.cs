using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene
{
    #region class members
    private Enums.SceneName _sceneName;
    private string _sceneDisplayName;
    #endregion
    
    #region accessors
    public Enums.SceneName SceneName
    {
        get { return _sceneName; }
        set { _sceneName = value; }
    }

    public string SceneDisplayName
    {
        get { return _sceneDisplayName; }
        set { _sceneDisplayName = value; }
    }
    #endregion
}