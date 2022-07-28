using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySlot : Slot
{
    private Image _displaySlotBackgroundImage;
    public Common.DisplaySlotType slotType;
    public Image equipmentSprite;
    public TextMeshProUGUI equipmentText;

    private void Start() 
    {
        _displaySlotBackgroundImage = GetComponent<Image>();
    }

    protected override void EmptySlot()
    {
        base.EmptySlot();
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        if(slotType == Common.DisplaySlotType.POUCH)
        {
            if(!_isUnlocked)
                equipmentText.text = "Locked";
            else
                equipmentText.text = "Empty";
        }
    }

    public override void LockSlot()
    {
        base.LockSlot();
        _displaySlotBackgroundImage.color = Common.LockedSlotColor;
        EmptySlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        _displaySlotBackgroundImage.color = Common.UnlockedSlotColor;
        EmptySlot();
    }
}
