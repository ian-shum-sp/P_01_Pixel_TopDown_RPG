using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PouchSlot : ActivatableSlot
{
    public Image pouchSlotImage;
    public Image potionImage;
    public TextMeshProUGUI potionAmountText;

    protected override void ResetSlot()
    {
        base.ResetSlot();
        potionImage.sprite = null;
        potionImage.color = Common.UnoccupiedSlotImageBackgroundColor;
        potionAmountText.text = null;
    }

    public override void LockSlot()
    {
        base.LockSlot();
        pouchSlotImage.color = Common.LockedSlotColor;
        ResetSlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        pouchSlotImage.color = Common.UnlockedSlotColor;
        ResetSlot();
    }

    public override void ActivateSlot()
    {
        throw new System.NotImplementedException();
    }

    public void AddToPouchSlot(Potion potion, int amount)
    {
        base.AddToSlot(potion);
        potionImage.sprite = potion.equipmentSprite;
        potionImage.color = Common.OccupiedSlotImageBackgroundColor;
        potionAmountText.text = amount.ToString();
    }

    public void RemoveFromPouchSlot()
    {
        ResetSlot();
    }
}
