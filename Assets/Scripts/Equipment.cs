using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    private Common.EquipmentType _equipmentType;
    public Common.EquipmentType EquipmentType
    {
        get { return _equipmentType; }
        set { _equipmentType = value; }
    }

    private int _isEquipped;
    public int IsEquipped
    {
        get { return _isEquipped; }
        set { _isEquipped = value; }
    }

    private int _purchasePrice;
    public int PurchasePrice
    {
        get { return _purchasePrice; }
        set { _purchasePrice = value; }
    }
    
    private int _sellPrice;
    public int SellPrice
    {
        get { return _sellPrice; }
        set { _sellPrice = value; }
    }
    
    



}
