using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellSlot : ShopSlot
{
    private int _amount = 0;
    private string _inventorySlotID;
    public string InventorySlotID
    {
        get { return _inventorySlotID; }
        set { _inventorySlotID = value; }
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
        _amount = 0;
        _inventorySlotID = null;
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
    }

    public override void LockSlot()
    {
        base.LockSlot();
        _inventorySlotImage.color = Common.LockedSlotColor;
        ResetSlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        _inventorySlotImage.color = Common.UnlockedSlotColor;
        ResetSlot();
    }

    public override void RemoveFromSlot()
    {
        ResetSlot();
    }

    public override void TryInteract()
    {
        if(!_isOccupied)
            return;

        if(_isSelected)
        {
            GameManager.Instance.DeselectAnyActiveBuySlot();
            DeselectSlot();
        }
        else
        {
            GameManager.Instance.UpdateSelectedEquipmentForSaleDisplayInfo(_equipment, _amount, _inventorySlotID);
            SelectSlot();
        }
    }

    public override void SelectSlot()
    {
        base.SelectSlot();
        _inventorySlotImage.color = Common.SellSlotSelectedSlotBackgroundColor;
    }

    public void AddEquipmentToSlot(Equipment equipment, string inventorySlotID, int? amount = null)
    {
        AddToSlot(equipment);
        _inventorySlotID = inventorySlotID;
        _amount += amount != null ? (int)amount : 1;
    }
}
