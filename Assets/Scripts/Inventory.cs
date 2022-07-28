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
    public int maxLevel;
    public int maxNumberOfInventorySlots;
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
        if(maxNumberOfInventorySlots > slots.Length)
        {
            Slot[] newSlots = new Slot[maxNumberOfInventorySlots];
            for (int i = 0; i < maxNumberOfInventorySlots; i++)
            {
                Slot newSlot = slots[i];
                newSlots[i] = newSlot;
            }
        }
        else if(maxNumberOfInventorySlots < slots.Length)
        {
            maxNumberOfInventorySlots = slots.Length;
        }

        _inventoryLevel = inventoryLevel;
        if(_inventoryLevel > maxLevel)
            _inventoryLevel = maxLevel;

        _inventoryBaseNumberOfSlots = maxNumberOfInventorySlots / maxLevel;
        _unlockedInventorySlots = _inventoryLevel * _inventoryBaseNumberOfSlots;

        for (int i = 0; i < maxNumberOfInventorySlots; i++)
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
