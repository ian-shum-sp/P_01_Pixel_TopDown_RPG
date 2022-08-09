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
    public bool IsEquipped
    {
        get { return _isEquipped; }
        set { _isEquipped = value; }
    }

    public int Amount
    {
        get { return _amount; }
        set { _amount = value; }
    }
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
            switch(_equipment.equipmentType)
            {
                //if it is an armor, unequip from the player gameobject first
                case Common.EquipmentType.CHEST_ARMOR:
                case Common.EquipmentType.HEAD_ARMOR:
                case Common.EquipmentType.BOOTS_ARMOR:
                {
                    //unequip old armor from player object and equip current armor
                    GameManager.Instance.player.UnequipArmor(_inventory.slots.First(x => x._isEquipped).Equipment as Armor);
                    Armor armor = _equipment as Armor;
                    GameManager.Instance.player.EquipArmor(armor);
                    break;
                }
                //if it is a weapon, unequip from the player gameobject first
                case Common.EquipmentType.MELEE_WEAPON:
                case Common.EquipmentType.RANGED_WEAPON:
                {
                    //unequip old weapon from player object and equip current weapon
                    GameManager.Instance.player.UnequipWeapon(_inventory.slots.First(x => x._isEquipped).Equipment.equipmentID);
                    Weapon weapon = _equipment as Weapon;
                    GameManager.Instance.player.EquipWeapon(weapon);
                    break;
                }
                //if it is a potion, unequip from the pouch
                case Common.EquipmentType.POTION:
                {
                    //unequip old potion from the pouch and equip current potion to the pouch
                    _inventory.slots.First(x => x._isEquipped).UnequipPotions();
                    Potion potion = _equipment as Potion;
                    _inventory.slots.First(x => !x._isEquipped).EquipPotions(potion, _amount);
                    _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
                    break;
                }
                default: 
                    break;
            }

            _inventory.slots.First(x => x._isEquipped).UnequipEquipments();
            EquipEquipments();
        }
    }

    private void TryEquip()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        if(playerLevel < _equipment.levelRequirement)
        {
            GameManager.Instance.ShowWarning("Not enough level!");
            return;
        }

        _isTryEquip = true;
        switch(_equipment.equipmentType)
        {
            case Common.EquipmentType.CHEST_ARMOR:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                if(_inventory.slots.Count(x => x._isEquipped == true && (x._equipment != null && x._equipment.equipmentType == Common.EquipmentType.CHEST_ARMOR)) > 0)
                {
                    GameManager.Instance.ShowConfirmation("Do you want to change your chest armor?");
                }
                else
                {
                    //equip to the player game object
                    Armor armor = _equipment as Armor;
                    GameManager.Instance.player.EquipArmor(armor);
                    EquipEquipments();
                }
                break;
            }
            case Common.EquipmentType.HEAD_ARMOR:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                if(_inventory.slots.Count(x => x._isEquipped && (x._equipment != null && x._equipment.equipmentType == Common.EquipmentType.HEAD_ARMOR)) > 0)
                {
                    GameManager.Instance.ShowConfirmation("Do you want to change your head armor?");
                }
                else
                {
                    //equip to the player game object
                    Armor armor = _equipment as Armor;
                    GameManager.Instance.player.EquipArmor(armor);
                    EquipEquipments();
                }
                break;
            }
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                if(_inventory.slots.Count(x => x._isEquipped && (x._equipment != null && x._equipment.equipmentType == Common.EquipmentType.BOOTS_ARMOR)) > 0)
                {
                    GameManager.Instance.ShowConfirmation("Do you want to change your boots armor?");
                }   
                else
                {
                     //equip to the player game object
                    Armor armor = _equipment as Armor;
                    GameManager.Instance.player.EquipArmor(armor);
                    EquipEquipments();
                }
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
                if(_inventory.slots.Count(x => x._isEquipped && (x._equipment != null && (x._equipment.equipmentType == Common.EquipmentType.MELEE_WEAPON || x._equipment.equipmentType == Common.EquipmentType.RANGED_WEAPON))) > 0)
                {
                    GameManager.Instance.ShowConfirmation("Do you want to change your weapon?");
                }
                else
                {
                    //equip to the player game object
                    Weapon weapon = _equipment as Weapon;
                    GameManager.Instance.player.EquipWeapon(weapon);
                    EquipEquipments();
                }
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
                    Potion potion = _equipment as Potion;
                    //equip potion to the pouch
                    _inventory.slots.First(x => !x._isOccupied).EquipPotions(potion, _amount);
                    //equip in the potion inventory
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
        GameManager.Instance.EquipToPouch(potion, _amount);
    }

    private void UnequipPotions()
    {
        GameManager.Instance.UnequipFromPouch(_equipment.equipmentID);
        base.RemoveFromSlot();
        _amount = 0;
        _isEquipped = false;
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equipmentText.text = "Empty";
    }

    private void UpdatePotionsAmount(int amount)
    {
        _amount -= amount;
        GameManager.Instance.UpdatePouchSlot(_equipment.equipmentID, _amount);
        if(_amount > 0)
            equipmentText.text = "x" + _amount;
        else
        {
            //Unequip from the potion inventory if the usable reach 0
            Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
            potionInventory.slots.First(x => x.Equipment.equipmentID == _equipment.equipmentID).UnequipEquipments();
            _amount = 0;
            ResetSlot();
        }
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
        _isEquipped = false;
        _amount = 0;
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

    public string AddEquipmentToSlot(Equipment equipment, int? amount = null)
    {
        AddToSlot(equipment);
        _amount += amount != null ? (int)amount : 1;
        return inventorySlotID;
    }

    public void OnCursorEnter()
    {
        if(_isUnlocked && _isOccupied && GameManager.Instance.CheckIfPopUpShown() == false)
        {
            GameManager.Instance.ChangeCursor(true);
            GameManager.Instance.ShowEquipmentPopUp(_equipment, _amount, transform.position + new Vector3(-280.0f, 160.0f, 0.0f));
        }
    }

    public void OnCursorExit()
    {
        GameManager.Instance.ChangeCursor(false);
        if(GameManager.Instance.CheckIfPopUpShown() == true)
            GameManager.Instance.HideEquipmentPopUp();
    }
    
    public void TryInteract()
    {
        if(!_isOccupied)
            return;

        if(!_isEquipped)
            TryEquip();
        else
        {
            switch(_equipment.equipmentType)
            {
                //if it is an armor, unequip from the player gameobject first
                case Common.EquipmentType.CHEST_ARMOR:
                case Common.EquipmentType.HEAD_ARMOR:
                case Common.EquipmentType.BOOTS_ARMOR:
                {
                    GameManager.Instance.player.UnequipArmor(_equipment as Armor);
                    break;
                }
                //if it is a weapon, unequip from the player gameobject first
                case Common.EquipmentType.MELEE_WEAPON:
                case Common.EquipmentType.RANGED_WEAPON:
                {
                    GameManager.Instance.player.UnequipWeapon(_equipment.equipmentID);
                    break;
                }
                //if it is a potion, unequip from the pouch
                case Common.EquipmentType.POTION:
                {
                    _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
                    _inventory.slots.First(x => x._equipment.equipmentID == _equipment.equipmentID).UnequipPotions();
                    break;
                }
                default: 
                    break;
            }

            //if it is a weapon, unequip from the player gameobject first
            if(_equipment.equipmentType == Common.EquipmentType.MELEE_WEAPON || _equipment.equipmentType == Common.EquipmentType.RANGED_WEAPON)
                GameManager.Instance.player.UnequipWeapon(_equipment.equipmentID);

            UnequipEquipments();
        }
    }

    public void ReduceAmount(int amount)
    {
        _amount -= amount;

        if(_equipment.equipmentType == Common.EquipmentType.POTION)
        {
            Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
            pouchInventory.slots.First(x => x.Equipment.equipmentID == _equipment.equipmentID).UpdatePotionsAmount(amount);
        }
        
        if(_amount <= 0)
            RemoveFromSlot();
    }
}
