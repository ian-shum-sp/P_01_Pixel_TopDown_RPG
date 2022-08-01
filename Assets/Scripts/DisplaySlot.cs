using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySlot : Slot
{
    private Image _displaySlotBackgroundImage;
    public Common.DisplaySlotType displaySlotType;
    public Image equipmentSprite;
    public TextMeshProUGUI equipmentText;

    private void Start() 
    {
        _displaySlotBackgroundImage = GetComponent<Image>();
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equipmentText.text = "Not Equipped";
    }

    public override void LockSlot()
    {
        base.LockSlot();
        _displaySlotBackgroundImage.color = Common.LockedSlotColor;
        ResetSlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        _displaySlotBackgroundImage.color = Common.UnlockedSlotColor;
        ResetSlot();
    }

    public override void AddToSlot(Equipment equipment)
    {
        base.AddToSlot(equipment);
        equipmentSprite.sprite = _equipment.equipmentSprite;
        equipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
        equipmentText.text = _equipment.equipmentName;
    }

    public override void RemoveFromSlot()
    {
        ResetSlot();
    }

    public void UpdateDisplaySlot()
    {
        if(_isOccupied)
        {
            equipmentSprite.sprite = _equipment.equipmentSprite;
            equipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
            equipmentText.text = _equipment.equipmentName;
        }
        else
        {
            equipmentSprite.sprite = null;
            equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
            equipmentText.text = "Not Equipped";
        }
    }
}
