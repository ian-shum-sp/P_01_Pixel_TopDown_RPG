using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : Slot
{
    #region class members
    protected bool _isSelected = false;
    protected Image _inventorySlotImage;
    public Image equipmentSprite;
    public Button inventorySlotButton;
    #endregion
    
    #region accessors
    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }
    
    #endregion

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

    public virtual void SelectSlot()
    {
        _isSelected = true;
    }

    public void DeselectSlot()
    {
        _isSelected = false;
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
