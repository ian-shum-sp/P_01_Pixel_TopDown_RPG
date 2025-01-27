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

    private void FixedUpdate()
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
                    GameManager.Instance.player.UnequipArmor(_inventory.slots.First(x => x._isEquipped && x._equipment.equipmentType == _equipment.equipmentType).Equipment as Armor);
                    Armor armor = _equipment as Armor;
                    GameManager.Instance.player.EquipArmor(armor);
                    _inventory.slots.First(x => x._isEquipped && x._equipment.equipmentType == _equipment.equipmentType).UnequipEquipments();
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
                    _inventory.slots.First(x => x._isEquipped).UnequipEquipments();
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
                    _inventory.slots.First(x => x._isEquipped).UnequipEquipments();
                    break;
                }
                default: 
                    break;
            }
            EquipEquipments();
            GameManager.Instance.HideUnequippedEquipmentInfo();
        }

        if(GameManager.Instance.GetConfirmationResult() == false && !GameManager.Instance.IsBlockGameActions)
        {
            _isTryEquip = false;
            GameManager.Instance.IsBlockGameActions = true;
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

    public override void SelectSlot()
    {
        base.SelectSlot();
        _inventorySlotImage.color = Common.InventorySelectedSlotBackgroundColor;
    }

    public override void DeselectSlot()
    {
        base.DeselectSlot();
        if(_isEquipped)
            _inventorySlotImage.color = Common.EquippedSlotBackgroundColor;
        else 
            _inventorySlotImage.color = Common.UnequippedSlotBackgroundColor;
    }

    public void TryEquip()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        if(playerLevel < _equipment.levelRequirement)
        {
            GameManager.Instance.ShowNotification("Not enough level!", Color.red);
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
                    GameManager.Instance.HideUnequippedEquipmentInfo();
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
                    GameManager.Instance.HideUnequippedEquipmentInfo();
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
                    GameManager.Instance.HideUnequippedEquipmentInfo();
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
                    GameManager.Instance.HideUnequippedEquipmentInfo();
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
                    GameManager.Instance.HideUnequippedEquipmentInfo();
                }
                break;
            }
            default: 
                break;
        }
    }
    
    public void EquipEquipments()
    {
        _isEquipped = true;
        _inventorySlotImage.color = Common.EquippedSlotBackgroundColor;
        if(_equipment.equipmentType != Common.EquipmentType.POTION)
            GameManager.Instance.AddToDisplaySlot(_equipment);
        _isTryEquip = false;
        _inventory = null;
        SelectSlot();
        GameManager.Instance.ShowEquippedEquipmentInfo(false, _equipment, _amount, inventorySlotID);
    }

    public void EquipPotions(Potion potion, int amount)
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

    public void UnequipEquipments()
    {
        _isEquipped = false;
        _inventorySlotImage.color = Common.UnequippedSlotBackgroundColor;
        if(_equipment.equipmentType != Common.EquipmentType.POTION)
            GameManager.Instance.RemoveFromDisplaySlot(_equipment);
        _inventory = null;
        DeselectSlot();
        GameManager.Instance.HideEquippedEquipmentInfo();
    }

    public void UnequipPotions()
    {
        GameManager.Instance.UnequipFromPouch(_equipment.equipmentID);
        base.RemoveFromSlot();
        _amount = 0;
        _isEquipped = false;
        equipmentSprite.sprite = null;
        equipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equipmentText.text = "Empty";
    }


    public string AddEquipmentToSlot(Equipment equipment, int? amount = null)
    {
        AddToSlot(equipment);
        _amount += amount != null ? (int)amount : 1;
        return inventorySlotID;
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

    public void TryInteract()
    {
        if(!_isOccupied)
            return;

        if(_isSelected)
        {
            DeselectSlot();
            if(_isEquipped)
                GameManager.Instance.HideEquippedEquipmentInfo();
            else
                GameManager.Instance.HideUnequippedEquipmentInfo();
        }
        else
        {
            Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
            Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
            Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
            //deselect the other selected slot first (if any) before selecting
            switch(_equipment.equipmentType)
            {
                case Common.EquipmentType.CHEST_ARMOR:
                case Common.EquipmentType.HEAD_ARMOR:
                case Common.EquipmentType.BOOTS_ARMOR:
                {
                    weaponInventory.DeselectAnySelectedSlots();
                    potionInventory.DeselectAnySelectedSlots();
                    if(_isEquipped)
                    {
                        InventorySlot slot = armorInventory.slots.FirstOrDefault(x => x._isSelected && x._isEquipped);
                        if(slot != null)
                        {
                            slot.DeselectSlot();
                        }
                        SelectSlot();
                        GameManager.Instance.ShowEquippedEquipmentInfo(false, _equipment, _amount, inventorySlotID);
                    }
                    else
                    {
                        InventorySlot selectedEquippedSlot = armorInventory.slots.FirstOrDefault(x => x._isSelected && x._isEquipped);
                        if(selectedEquippedSlot != null)
                        {
                            selectedEquippedSlot.DeselectSlot();
                        }
                        InventorySlot seletedUnequippedSlot = armorInventory.slots.FirstOrDefault(x => x._isSelected && !x._isEquipped);
                        if(seletedUnequippedSlot != null)
                        {
                            seletedUnequippedSlot.DeselectSlot();
                        }
                        SelectSlot();
                        GameManager.Instance.ShowUnequippedEquipmentInfo(_equipment, _amount, inventorySlotID);
                        GameManager.Instance.ShowEquippedEquipmentInfo(true);
                    }
                    break;
                }
                case Common.EquipmentType.MELEE_WEAPON:
                case Common.EquipmentType.RANGED_WEAPON:
                {
                    armorInventory.DeselectAnySelectedSlots();
                    potionInventory.DeselectAnySelectedSlots();
                    if(_isEquipped)
                    {
                        InventorySlot slot = weaponInventory.slots.FirstOrDefault(x => x._isSelected && x._isEquipped);
                        if(slot != null)
                        {
                            slot.DeselectSlot();
                        }
                        SelectSlot();
                        GameManager.Instance.ShowEquippedEquipmentInfo(false, _equipment, _amount, inventorySlotID);
                    }
                    else
                    {
                        InventorySlot slot = weaponInventory.slots.FirstOrDefault(x => x._isSelected && !x._isEquipped);
                        if(slot != null)
                        {
                            slot.DeselectSlot();
                        }
                        SelectSlot();
                        GameManager.Instance.ShowUnequippedEquipmentInfo(_equipment, _amount, inventorySlotID);
                        GameManager.Instance.ShowEquippedEquipmentInfo(true);
                    }
                    break;
                }
                case Common.EquipmentType.POTION:
                {
                    armorInventory.DeselectAnySelectedSlots();
                    weaponInventory.DeselectAnySelectedSlots();
                    if(_isEquipped)
                    {
                        InventorySlot slot = potionInventory.slots.FirstOrDefault(x => x._isSelected && x._isEquipped);
                        if(slot != null)
                        {
                            slot.DeselectSlot();
                        }
                        SelectSlot();
                        GameManager.Instance.ShowEquippedEquipmentInfo(false, _equipment, _amount, inventorySlotID);
                    }
                    else
                    {
                        InventorySlot slot = potionInventory.slots.FirstOrDefault(x => x._isSelected && !x._isEquipped);
                        if(slot != null)
                        {
                            slot.DeselectSlot();
                        }
                        SelectSlot();
                        GameManager.Instance.ShowUnequippedEquipmentInfo(_equipment, _amount, inventorySlotID);
                    }
                    break;
                }
                default: 
                    break;
            }
        }
    }

    // public void TryInteract()
    // {
    //     if(!_isOccupied)
    //         return;

    //     if(!_isEquipped)
    //     {
    //         if(_equipment.equipmentType != Common.EquipmentType.POTION)
    //         {
    //             TryEquip();
    //         }
    //         else
    //         {
    //             if(GameManager.Instance.CheckIsAtCentralHub() == true)
    //                 TryEquip();
    //             else
    //                 GameManager.Instance.ShowNotification("You can only equip/unequip potion in central hubs!", Color.red);
    //         }
    //     }
    //     else
    //     {
    //         switch(_equipment.equipmentType)
    //         {
    //             //if it is an armor, unequip from the player gameobject first
    //             case Common.EquipmentType.CHEST_ARMOR:
    //             case Common.EquipmentType.HEAD_ARMOR:
    //             case Common.EquipmentType.BOOTS_ARMOR:
    //             {
    //                 GameManager.Instance.player.UnequipArmor(_equipment as Armor);
    //                 break;
    //             }
    //             //if it is a weapon, unequip from the player gameobject first
    //             case Common.EquipmentType.MELEE_WEAPON:
    //             case Common.EquipmentType.RANGED_WEAPON:
    //             {
    //                 GameManager.Instance.player.UnequipWeapon(_equipment.equipmentID);
    //                 break;
    //             }
    //             //if it is a potion, unequip from the pouch
    //             case Common.EquipmentType.POTION:
    //             {
    //                 if(GameManager.Instance.CheckIsAtCentralHub() == true)
    //                 {
    //                     _inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
    //                     _inventory.slots.First(x => x.IsOccupied && x.Equipment.equipmentID == _equipment.equipmentID).UnequipPotions();
    //                 }
    //                 else
    //                 {
    //                     GameManager.Instance.ShowNotification("You can only equip/unequip potion in central hubs!", Color.red);
    //                 }
    //                 break;
    //             }
    //             default: 
    //                 break;
    //         }
    //         UnequipEquipments();
    //     }
    // }

    public void ReduceAmount(int amount)
    {
        _amount -= amount;

        if(_equipment.equipmentType == Common.EquipmentType.POTION)
        {
            Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
            pouchInventory.slots.First(x => x.IsOccupied && x.Equipment.equipmentID == _equipment.equipmentID).UpdatePotionsAmount(amount);
        }
        
        if(_amount <= 0)
            RemoveFromSlot();

        GameManager.Instance.UpdateShopSellSection();
    }

    public void UpdatePotionsAmount(int amount)
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
}
