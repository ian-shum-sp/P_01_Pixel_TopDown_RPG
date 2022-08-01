using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    #region class members
    public string equipmentID;
    public string equipmentName;
    public Common.EquipmentType equipmentType;
    public int purchasePrice;
    public int levelRequirement;
    public Sprite equipmentSprite;
    #endregion
    
    #region accessors
    #endregion
}
