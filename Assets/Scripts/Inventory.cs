using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int[] upgradeLevelRequirements;
    public int[] upgradePrices;
    public List<InventorySlot> slots;
    #endregion
    
    #region accessors
    public int InventoryLevel
    {
        get { return _inventoryLevel; }
        set { _inventoryLevel = value; }
    }

    public int UnlockedInventorySlots
    {
        get { return _unlockedInventorySlots; }
        set { _unlockedInventorySlots = value; }
    }

    public int MaxNumberOfInventorySlots
    {
        get { return _maxNumberOfInventorySlots; }
        set { _maxNumberOfInventorySlots = value; }
    }
    #endregion

    public void InitializeInventory(int inventoryLevel)
    {
        _inventoryLevel = inventoryLevel;
        if(_inventoryLevel > maxLevel)
            _inventoryLevel = maxLevel;

        _maxNumberOfInventorySlots = slots.Count;
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

    public string AddEquipmentToInventory(Equipment equipment, int? amount = null)
    {
        string inventorySlotID = null;
        if(equipment.equipmentType != Common.EquipmentType.POTION)
        {
            InventorySlot slot = slots.First(x => !x.IsOccupied);
            inventorySlotID = slot.AddEquipmentToSlot(equipment, amount);
        }
        else
        {
            InventorySlot slot = slots.FirstOrDefault(x => x.Equipment != null && x.Equipment.equipmentID == equipment.equipmentID);
            if(slot == null)
                slot = slots.First(x => !x.IsOccupied);
            
            inventorySlotID = slot.AddEquipmentToSlot(equipment, amount);
        }
        return inventorySlotID;
    }

    public void RemoveEquipmentFromInventory(string inventorySlotID)
    {
        InventorySlot slot = slots.First(x => x.inventorySlotID == inventorySlotID);
        slot.RemoveFromSlot();
    }

    public void ReduceEquipmentAmount(Equipment equipment, int amount)
    {
        slots.First(x => x.Equipment.equipmentID == equipment.equipmentID).ReduceAmount(amount);
    }
   
    // public List<Equipment> GetEquippedEquipments()
    // {
    //     List<Equipment> equippedEquipments = new List<Equipment>();
    //     List<InventorySlot> equippedSlots = slots.Where(x => x.IsEquipped).ToList();
    //     foreach(InventorySlot slot in equippedSlots)
    //     {
    //         equippedEquipments.Add(slot.Equipment);
    //     }
    //     return equippedEquipments;
    // }

    public bool CheckIsInventoryFull()
    {
        InventorySlot unoccupiedSlot = slots.FirstOrDefault(x => x.IsUnlocked && !x.IsOccupied);

        if(unoccupiedSlot == null)
            return true;

        return false;
    }

    public void TryUpgradeInventory()
    {
        if(_inventoryLevel == maxLevel)
            return;

        int playerLevel = GameManager.Instance.GetPlayerLevel();

        if(playerLevel >= upgradeLevelRequirements[_inventoryLevel])
        {
            if(GameManager.Instance.player.Gold >= upgradePrices[_inventoryLevel])
            {
                GameManager.Instance.player.Gold -= upgradePrices[_inventoryLevel];
                _inventoryLevel++;
                int oldUnlockedInventorySlot = _unlockedInventorySlots;
                _unlockedInventorySlots = _inventoryLevel * _inventoryBaseNumberOfSlots;
                for (int i = oldUnlockedInventorySlot; i < _unlockedInventorySlots; i++)
                {
                    slots[i].UnlockSlot();
                }
                GameManager.Instance.UpdatePlayerMenuGold();
                GameManager.Instance.UpdateShopSellSection();
            }
            else
            {
                GameManager.Instance.ShowWarning("Not enough gold!");
            }
        }
        else
        {
            GameManager.Instance.ShowWarning("Not enough level!");
        }
    }
}
