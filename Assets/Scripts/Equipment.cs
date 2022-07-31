using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    #region class members
    private bool _isEquipped = false;
    public string equipmentID;
    public Common.EquipmentType equipmentType;
    public int purchasePrice;
    public int levelRequirement;
    public Sprite equipmentSprite;
    #endregion
    
    #region accessors
    public bool IsEquipped
    
    {
        get { return _isEquipped; }
        set { _isEquipped = value; }
    }
    #endregion
}
