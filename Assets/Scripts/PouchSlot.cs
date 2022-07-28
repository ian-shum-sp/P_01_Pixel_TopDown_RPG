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

    protected override void EmptySlot()
    {
        base.EmptySlot();
        potionImage.sprite = null;
        potionImage.color = Common.UnoccupiedSlotImageBackgroundColor;
        potionAmountText.text = null;
    }

    public override void LockSlot()
    {
        base.LockSlot();
        pouchSlotImage.color = Common.LockedSlotColor;
        EmptySlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        pouchSlotImage.color = Common.UnlockedSlotColor;
        EmptySlot();
    }

    public override void ActivateSlot()
    {
        throw new System.NotImplementedException();
    }
}
