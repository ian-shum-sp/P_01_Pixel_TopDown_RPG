using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : Slot
{
    protected Image _inventorySlotImage;
    public Image equipmentSprite;
    public Button inventorySlotButton;

    protected virtual void Start()
    {
        _inventorySlotImage = inventorySlotButton.GetComponent<Image>();
    }

    public override void AddToSlot(Equipment equipment)
    {
        base.AddToSlot(equipment);
        equipmentSprite.sprite = _equipment.equipmentSprite;
        equipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
    }

    public virtual void TryInteract()
    {
        
    }

    public override void DeselectSlot()
    {
        base.DeselectSlot();
        _inventorySlotImage.color = Common.NotSelectedSlotBackgroundColor;
    }

    public void OnCursorEnter()
    {
        if(_isUnlocked && _isOccupied)
            GameManager.Instance.ChangeCursor(true);
    }

    public void OnCursorExit()
    {
        GameManager.Instance.ChangeCursor(false);
    }
}
