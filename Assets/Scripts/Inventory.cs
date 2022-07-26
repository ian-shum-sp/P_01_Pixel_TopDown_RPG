using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region class members
    public Common.InventoryType inventoryType;
    //stores the plain Level (not index)
    private int _inventoryLevel;
    private int _unlockedInventorySlots;
    public int maxLevel;
    public int maxNumberOfInventorySlots;
    #endregion
    
    #region accessors
    public int InventoryLevel
    {
        get { return _inventoryLevel; }
        set { _inventoryLevel = value; }
    }
    #endregion

    public void UpdateInventory(int inventoryLevel)
    {
        _inventoryLevel = inventoryLevel;
        Common.UpdateInventoryBaseNumberOfSlots(inventoryType, maxNumberOfInventorySlots, maxLevel);
        _unlockedInventorySlots = _inventoryLevel * Common.GetInventoryBaseNumberOfSlots(inventoryType);
        if(_unlockedInventorySlots > maxNumberOfInventorySlots)
        {
            _unlockedInventorySlots = maxNumberOfInventorySlots;
        }
    }
}
