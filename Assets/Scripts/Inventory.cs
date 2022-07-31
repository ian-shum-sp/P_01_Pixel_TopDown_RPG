using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region class members
    public Common.InventoryType inventoryType;
    //stores the plain Level (not index)
    private int _inventoryLevel;
    private int _inventoryBaseNumberOfSlots;
    private int _unlockedInventorySlots;
    private int _maxNumberOfInventorySlots;
    public int maxLevel;
    public int[] upgradePrices;
    public Slot[] slots;
    #endregion
    
    #region accessors
    public int InventoryLevel
    {
        get { return _inventoryLevel; }
        set { _inventoryLevel = value; }
    }
    #endregion

    public void InitializeInventory(int inventoryLevel)
    {
        _inventoryLevel = inventoryLevel;
        if(_inventoryLevel > maxLevel)
            _inventoryLevel = maxLevel;

        _maxNumberOfInventorySlots = slots.Length;
        _inventoryBaseNumberOfSlots = Mathf.CeilToInt((float)_maxNumberOfInventorySlots / (float)maxLevel);
        _unlockedInventorySlots = _inventoryLevel * _inventoryBaseNumberOfSlots;
        if(_unlockedInventorySlots > _maxNumberOfInventorySlots)
            _unlockedInventorySlots = _maxNumberOfInventorySlots;

        for (int i = 0; i < _maxNumberOfInventorySlots; i++)
        {
            if(i < _unlockedInventorySlots)
                slots[i].UnlockSlot();
            else
                slots[i].LockSlot();
        }
    }

    public void UpgradeInventory()
    {
        
    }
}
