using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class InventorySlot : Slot
{
    #region class members
    private bool _isEquipped = false;
    private Image _inventorySlotImage;
    private bool _isTryEquip;
    private Inventory _inventory;
    private int _amount = 0;
    public string inventorySlotID;
    public Button inventorySlotButton;
    public Image equipmentSprite;
    public Common.InventorySlotType slotType;
    public TextMeshProUGUI equipmentText;

    #endregion
    
    #region accessors
    #endregion

    private void Start() 
    {
        if(slotType != Common.InventorySlotType.POUCH)
            _inventorySlotImage = inventorySlotButton.GetComponent<Image>();
        else
            _inventorySlotImage = GetComponent<Image>();
    }

    private void Update()
    {
        if(!_isTryEquip)
            return;

        if(GameManager.Instance.GetConfirmationResult() == true)
        {
            if(_equipment.equipmentType == Common.EquipmentType.POTION)
            {
                _inventory.slots.First(x => x._isEquipped).UnequipPotions();
                Potion potion = (Potion)_equipment;
                _inventory.slots.First(x => !x._isEquipped).EquipPotions(potion, _amount);
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
            }
            _inventory.slots.First(x => x._isEquipped).UnequipEquipments();
            EquipEquipments();
        }
    }

    private void TryEquip()
    {
        _isTryEquip = true;
        switch(_equipment.equipmentType)
        {
            case Common.EquipmentType.CHEST_ARMOR:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                if(_inventory.slots.Count(x => x._isEquipped == true && (x._equipment != null && x._equipment.equipmentType == Common.EquipmentType.CHEST_ARMOR)) > 0)
                    GameManager.Instance.ShowConfirmation("Do you want to change your chest armor?");
                else
                    EquipEquipments();
                break;
            }
            case Common.EquipmentType.HEAD_ARMOR:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                if(_inventory.slots.Count(x => x._isEquipped && (x._equipment != null && x._equipment.equipmentType == Common.EquipmentType.HEAD_ARMOR)) > 0)
                    GameManager.Instance.ShowConfirmation("Do you want to change your head armor?");
                else
                    EquipEquipments();
                break;
            }
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                if(_inventory.slots.Count(x => x._isEquipped && (x._equipment != null && x._equipment.equipmentType == Common.EquipmentType.BOOTS_ARMOR)) > 0)
                    GameManager.Instance.ShowConfirmation("Do you want to change your boots armor?");
                else
                    EquipEquipments();
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
                if(_inventory.slots.Count(x => x._isEquipped && (x._equipment != null && (x._equipment.equipmentType == Common.EquipmentType.MELEE_WEAPON || x._equipment.equipmentType == Common.EquipmentType.RANGED_WEAPON))) > 0)
                    GameManager.Instance.ShowConfirmation("Do you want to change your weapon?");
                else
                    EquipEquipments();
                break;
            }
            case Common.EquipmentType.POTION:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
                Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
                if(potionInventory.slots.Count(x => x._isEquipped) == _inventory.UnlockedInventorySlots)
                {
                    GameManager.Instance.ShowConfirmation("Do you want to change your first potion in place?");
                }
                else
                {
                    Potion potion = (Potion)_equipment;
                    _inventory.slots.First(x => !x._isOccupied).EquipPotions(potion, _amount);
                    EquipEquipments();
                }
                break;
            }
            default: 
                break;
        }
    }
    
    private void EquipEquipments()
    {
        _isEquipped = true;
        _inventorySlotImage.color = Common.EquippedSlotBackgroundColor;
        if(_equipment.equipmentType != Common.EquipmentType.POTION)
            GameManager.Instance.AddToDisplaySlot(_equipment);
        _isTryEquip = false;
        _inventory = null;
    }
   
    private void UnequipEquipments()
    {
        _isEquipped = false;
        _inventorySlotImage.color = Common.UnequippedSlotBackgroundColor;
        if(_equipment.equipmentType != Common.EquipmentType.POTION)
            GameManager.Instance.RemoveFromDisplaySlot(_equipment);
        _inventory = null;
    }

    private void EquipPotions(Potion potion, int amount)
    {
        base.AddToSlot(potion);
        int maxAllowed = potion.maxNumberInPouch;
        if(amount > maxAllowed)
            _amount = maxAllowed;
        else
            _amount = amount;
        _isEquipped = true;
        equipmentSprite.sprite = _equipment.equipmentSprite;
        equipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
        equipmentText.text = "x" + _amount;
    }


    private void UnequipPotions()
    {
        base.RemoveFromSlot();
        _amount = 0;
        _isEquipped = false;
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equipmentText.text = "Empty";
    }


    protected override void ResetSlot()
    {
        base.ResetSlot();
        _isEquipped = false;
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        if(slotType == Common.InventorySlotType.POUCH)
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
        _inventorySlotImage.color = Common.LockedSlotColor;
        ResetSlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        _inventorySlotImage.color = Common.UnlockedSlotColor;
        ResetSlot();
    }

    public override void AddToSlot(Equipment equipment)
    {
        base.AddToSlot(equipment);
        equipmentSprite.sprite = _equipment.equipmentSprite;
        equipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
    }

    public override void RemoveFromSlot()
    {
        ResetSlot();
    }

    public void AddEquipmentToSlot(Equipment equipment, int? amount = null)
    {
        AddToSlot(equipment);
        _amount += amount != null ? (int)amount : 1;
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
    
    public void TryInteract()
    {
        if(!_isOccupied)
            return;

        if(!_isEquipped)
            TryEquip();
        else
        {
            if(_equipment.equipmentType == Common.EquipmentType.POTION)
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
                _inventory.slots.First(x => x._equipment.equipmentID == _equipment.equipmentID).UnequipPotions();
            }
            UnequipEquipments();
        }
    }
}
