using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region class members
    private bool _isActive;
    public Text levelText;
    public Text playerNameText;
    public Text statusInfoText;
    public Image levelProgressBarMask;
    public Image healthBarMask;
    public GameObject[] potionSlots;
    #endregion
    
    #region accessors
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    #endregion

    


}
