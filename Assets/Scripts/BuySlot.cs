using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySlot : ShopSlot
{
    public Equipment shopEquipment;

    protected override void Start()
    {
        base.Start();
        base.UnlockSlot();
        base.AddToSlot(shopEquipment);
    }

    public override void TryInteract()
    {
        if(!_isOccupied)
            return;

        if(_isSelected)
        {
            GameManager.Instance.ResetSelectedEquipmentToBuyDisplayInfo();
            DeselectSlot();
        }
        else
        {
            GameManager.Instance.UpdateSelectedEquipmentToBuyDisplayInfo(_equipment);
            SelectSlot();
        }
    }

    public override void SelectSlot()
    {
        base.SelectSlot();
        _inventorySlotImage.color = Common.BuySlotSelectedSlotBackgroundColor;
    }
}
