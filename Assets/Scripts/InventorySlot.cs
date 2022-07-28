using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Slot
{
    #region class members
    private Image _inventorySlotButtonImage;
    public Button inventorySlotButton;
    public Image equipmentSprite;
    #endregion
    
    #region accessors
    #endregion

    private void Start() 
    {
        _inventorySlotButtonImage = inventorySlotButton.GetComponent<Image>();
    }

    protected override void EmptySlot()
    {
        base.EmptySlot();
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
    }

    public void OnCursorEnter()
    {
        if(_isUnlocked)
            GameManager.Instance.ChangeCursor(true);
    }

    public void OnCursorExit()
    {
        GameManager.Instance.ChangeCursor(false);
    }

    public override void LockSlot()
    {
        _isUnlocked = false;
        _inventorySlotButtonImage.color = Common.LockedSlotColor;
        EmptySlot();
    }

    public override void UnlockSlot()
    {
        _isUnlocked = true;
        _inventorySlotButtonImage.color = Common.UnlockedSlotColor;
        EmptySlot();
    }
    
    public void Equip()
    {
        _isEquipped = true;
        _inventorySlotButtonImage.color = Common.EquippedSlotBackgroundColor;
    }

    public void Unequip()
    {
        _isEquipped = false;
        _inventorySlotButtonImage.color = Common.UnequippedSlotBackgroundColor;
    }
}
