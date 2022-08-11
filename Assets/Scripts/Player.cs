using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Movable
{
    #region class members
    private SpriteRenderer _spriteRenderer;
    private string _name;
    private Common.PlayerGender _gender;
    private int _gold;
    private int _experience;
    private Weapon _currentEquippedWeapon;
    private PlayerWeapon _currentEquippedPlayerWeapon;
    public Inventory[] _inventories;
    public GameObject[] allWeapons;
    #endregion
    
    #region accessors
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public Common.PlayerGender Gender
    {
        get { return _gender; }
        set { _gender = value; }
    }
    public int Experience
    {
        get { return _experience; }
        set { _experience = value; }
    }
    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    #endregion

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //if there is any active dialogs or player menu shown
        if(!GameManager.Instance.IsBlockGameActions)
        {
            //if the player does not have a weapon, can move freely
            if(_currentEquippedPlayerWeapon == null)
            {
                UpdateMotor(new Vector3(x, y, 0.0f));
                return;
            }

            //if melee weapon, can move when attacking
            if(_currentDamage.isMelee)
            {
                UpdateMotor(new Vector3(x, y, 0.0f));
                return;
            } 
            else
            {
                //player cannot move when he is attacking (in range weapon), if he is attacking and at the same time being attacked, knockback is applied
                if(!_currentEquippedPlayerWeapon._isAttacking)
                {
                    UpdateMotor(new Vector3(x, y, 0.0f));
                    return;
                }

                if(_isAttacked)
                {
                    UpdateMotor(new Vector3(0.0f, 0.0f, 0.0f));
                    return;
                }
            }   
        }
    }

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if(_currentEquippedWeapon == null || _isAttacked)
            return;
            
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _currentEquippedPlayerWeapon.TryAttack(_currentDamage);
        }
    }

    protected override void ReceiveDamage(Damage damage)
    {
        base.ReceiveDamage(damage);
        GameManager.Instance.UpdateHUDHealthPoints();
        GameManager.Instance.UpdatePlayerMenuHealthPoints();
    }


    protected override void UpdateDamage()
    {
        if(_currentProtection.GetTotalStrengthLevel() == 1 || _currentProtection.GetTotalStrengthLevel() == 2)
            _currentDamage.damagePoints = _currentEquippedWeapon != null ? (float)_currentEquippedWeapon.damagePoints + 5 : 5;
        else if(_currentProtection.GetTotalStrengthLevel() == 3)
            _currentDamage.damagePoints = _currentEquippedWeapon != null ? (float)_currentEquippedWeapon.damagePoints + 10 : 10;
        else
            _currentDamage.damagePoints = _currentEquippedWeapon != null ? (float)_currentEquippedWeapon.damagePoints : 0;

        if(_isElementBlighted)
        {
            _currentElementResistanceLevel = _currentProtection.GetTotalElementResistanceLevel();
            int elementLevel = Mathf.Abs(_currentElementResistanceLevel);
            if(elementLevel == 1 || elementLevel == 2)
            {
                _currentDamage.damagePoints *= 0.9f;
                _currentDamage.damagePoints *= 0.9f;
            }
            else
            {
                _currentDamage.damagePoints *= 0.8f;
                _currentDamage.damagePoints *= 0.8f;
            }
        }
    }

    public void SetPlayerSprite()
    {
        _spriteRenderer.sprite = GameManager.Instance.playerSprites[(int)_gender];
    }

    public void InitializeInventory(Common.InventoryType inventoryType, int inventoryLevel)
    {
        _inventories[(int)inventoryType].InitializeInventory(inventoryLevel);
    }

    public Inventory GetInventory(Common.InventoryType inventoryType)
    {
        return _inventories[(int)inventoryType];
    }

    public AddEquipmentActionResult AddEquipmentToInventory(Equipment equipment, int? amount = null, bool isContinueBlockGameAction = true)
    {
        bool isAdded = false;
        string inventorySlotID = null;
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                if(_inventories[(int)Common.InventoryType.ARMOR].CheckIsInventoryFull() == true)
                    GameManager.Instance.ShowWarning("Inventory is full!", isContinueBlockGameAction);
                else
                {
                    inventorySlotID = _inventories[(int)Common.InventoryType.ARMOR].AddEquipmentToInventory(equipment, amount);
                    isAdded = true;
                }
                break;
            }  
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                if(_inventories[(int)Common.InventoryType.WEAPON].CheckIsInventoryFull() == true)
                    GameManager.Instance.ShowWarning("Inventory is full!", isContinueBlockGameAction);
                else
                {
                    inventorySlotID = _inventories[(int)Common.InventoryType.WEAPON].AddEquipmentToInventory(equipment, amount);
                    isAdded = true;
                }
                break;
            }   
            case Common.EquipmentType.POTION:
            {
                if(_inventories[(int)Common.InventoryType.POTION].CheckIsInventoryFull() == true)
                    GameManager.Instance.ShowWarning("Inventory is full!", isContinueBlockGameAction);
                else
                {
                    inventorySlotID = _inventories[(int)Common.InventoryType.POTION].AddEquipmentToInventory(equipment, amount);
                    isAdded = true;
                }
                break;
            } 
            default: 
                break;
        }

        AddEquipmentActionResult result = new AddEquipmentActionResult();
        result.isAdded = isAdded;
        result.inventorySlotID = inventorySlotID;

        if(isAdded)
            GameManager.Instance.AddToShopSellSection(equipment, inventorySlotID, amount);

        return result;
    }

    public void RemoveEquipmentFromInventory(Equipment equipment, string inventorySlotID)
    {
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                _inventories[(int)Common.InventoryType.ARMOR].RemoveEquipmentFromInventory(inventorySlotID);
                break;
            }  
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                _inventories[(int)Common.InventoryType.WEAPON].RemoveEquipmentFromInventory(inventorySlotID);
                break;
            }   
            case Common.EquipmentType.POTION:
            {
                _inventories[(int)Common.InventoryType.POTION].RemoveEquipmentFromInventory(inventorySlotID);
                break;
            } 
            default: 
                break;
        }
    }

    public void EquipArmor(Armor armor)
    {
        _currentProtection.armorPoints += armor.armorPoints;
        _currentProtection.SetArmorBuffLevel(armor.armorBuff, armor.buffLevel);
        if(armor.armorBuff == Common.ArmorBuff.KNOCKBACK_RESISTANCE)
            UpdateKnockbackRecoverySpeed();
    }

    public void UnequipArmor(Armor armor)
    {
        _currentProtection.armorPoints -= armor.armorPoints;
        _currentProtection.RemoveArmorBuffLevel(armor.armorBuff, armor.buffLevel);
    }

    public void EquipWeapon(Weapon weapon)
    {
        GameObject weaponToEquip = allWeapons.First(x => x.GetComponent<Weapon>().equipmentID == weapon.equipmentID);
        weaponToEquip.SetActive(true);
        _currentEquippedWeapon = weaponToEquip.GetComponent<Weapon>();

        if(_currentEquippedWeapon.equipmentType == Common.EquipmentType.MELEE_WEAPON)
            _currentEquippedPlayerWeapon = weaponToEquip.GetComponent<PlayerWeapon>();
        else
            _currentEquippedPlayerWeapon = weaponToEquip.GetComponentInChildren<PlayerWeapon>();

        _currentDamage.isHaveWeapon = true;
        _currentDamage.isMelee = _currentEquippedWeapon.equipmentType == Common.EquipmentType.MELEE_WEAPON ? true : false;
        _currentDamage.origin = _currentEquippedWeapon.transform.position;
        _currentDamage.damagePoints += (float)_currentEquippedWeapon.damagePoints;
        _currentDamage.knockbackForce = _currentEquippedWeapon.baseKnockbackForce;
        _currentDamage.cooldown = _currentEquippedWeapon.cooldown;
        _currentDamage.attackRange = _currentEquippedWeapon.attackRange;
        _currentDamage.attackSpeed = Mathf.FloorToInt((2.0f - _currentEquippedWeapon.cooldown)*20);
        _currentDamage.SetWeaponBuffLevel(_currentEquippedWeapon.weaponDebuff, _currentEquippedWeapon.debuffLevel);
        UpdateDamage();
    }

    public void UnequipWeapon(string equipmentID)
    {
        GameObject weaponToUnequip = allWeapons.First(x => x.GetComponent<Weapon>().equipmentID == equipmentID);
        weaponToUnequip.SetActive(false);
        _currentEquippedWeapon = null;
        _currentEquippedPlayerWeapon = null;
        _currentDamage = new Damage();
        _currentDamage.Initialize();
    }

    public void AddPotionEffect(Potion potion)
    {
        _currentProtection.SetPotionBuffLevel(potion.potionBuff, potion.buffLevel);
        switch(potion.potionBuff)
        {
            case Common.PotionBuff.KNOCKBACK_RESISTANCE:
            {
                UpdateKnockbackRecoverySpeed();
                break;
            }
            case Common.PotionBuff.STRENGTH:
            {
                UpdateDamage();
                break;
            }
            case Common.PotionBuff.SPEED:
            {
                UpdateSpeed();
                break;
            }
            case Common.PotionBuff.HEALING:
            {
                if(potion.buffLevel == 1)
                    Heal(30.0f);
                else if(potion.buffLevel == 2)
                    Heal(90.0f);
                else
                    Heal(200.0f);
                break;
            }
            default:
                break;
        }
        _inventories[(int)Common.InventoryType.POTION].ReduceEquipmentAmount(potion, 1);
        GameManager.Instance.UpdateStatusInfo();
        GameManager.Instance.UpdatePlayerMenuEquipmentInfo();
    }

    public void RemovePotionEffect(Potion potion)
    {
        _currentProtection.RemovePotionBuffLevel(potion.potionBuff, potion.buffLevel);
        switch(potion.potionBuff)
        {
            case Common.PotionBuff.KNOCKBACK_RESISTANCE:
            {
                UpdateKnockbackRecoverySpeed();
                break;
            }
            case Common.PotionBuff.STRENGTH:
            {
                UpdateDamage();
                break;
            }
            case Common.PotionBuff.SPEED:
            {
                UpdateSpeed();
                break;
            }
            default:
                break;
        }
        GameManager.Instance.UpdateStatusInfo();
        GameManager.Instance.UpdatePlayerMenuEquipmentInfo();
    }

    public Protection GetArmorInfo()
    {
        return _currentProtection;
    }

    public Damage GetWeaponInfo()
    {
        return _currentDamage;
    }

    public void Heal(float healingAmount)
    {
        if(healthPoints == maxHealthPoints)
            return;

        healthPoints += healingAmount;

        if(healthPoints > maxHealthPoints)
            healthPoints = maxHealthPoints;

        GameManager.Instance.ShowFloatingText("+" + healingAmount, 25, Color.green, transform.position, Vector3.up * 30, 2.0f);
    }

    public void LevelUp()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        if(playerLevel <= 9)
            maxHealthPoints += 5;
        else if(playerLevel >= 10 && playerLevel <= 20)
            maxHealthPoints += 10;
        else if(playerLevel >= 21 && playerLevel <= 24)
            maxHealthPoints += 15;
        else if(playerLevel == 25)
            maxHealthPoints += 40;
        
        healthPoints += healthPoints * 0.1f;
        GameManager.Instance.UpdateHUDHealthPoints();
        GameManager.Instance.UpdatePlayerMenuHealthPoints();
    }

    public void InitializeLevelFromLoadGame()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        if(playerLevel != 1)
        {
            for (int i = 0; i < playerLevel; i++)
            {
                LevelUp();
            }
        }
    }
}
