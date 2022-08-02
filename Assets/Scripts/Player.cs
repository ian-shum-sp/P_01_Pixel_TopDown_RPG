using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region class members
    private Vector3 _originalSize;
    private SpriteRenderer _spriteRenderer;
    private string _name;
    private Common.PlayerGender _gender;
    private int _gold;
    private int _experience;
    private Vector3 _moveDelta;
    private BoxCollider2D _boxCollider;
    private RaycastHit2D _hit;
    public Inventory[] _inventories;
    public float healthPoints;
    public float maxHealthPoints;
    public float xSpeed;
    public float ySpeed;
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

    private void Start()
    {
        _originalSize = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(!GameManager.Instance.IsBlockGameActions)
            UpdateMotor(new Vector3(x, y, 0.0f));
    }

    private void UpdateMotor(Vector3 input)
    {
        _moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0.0f);

        //swap sprite direction (right to left)
        if(_moveDelta.x > 0.0f)
        {
            transform.localScale = _originalSize;
        }
        else if(_moveDelta.x < 0.0f)
        {
            transform.localScale = new Vector3(_originalSize.x * -1.0f, _originalSize.y, _originalSize.z);
        }
        
        //check if player collides with Blocking and Actor layer by casting a box in the expected position first, if colliders is null, means no collision (for x)
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0.0f, new Vector2(_moveDelta.x, 0.0f), Mathf.Abs(_moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking","Character"));
        if(_hit.collider == null)
        {
            //move
            transform.Translate(_moveDelta.x * Time.deltaTime, 0.0f, 0.0f);
        }

        //check if player collides with Blocking and Actor layer by casting a box in the expected position first, if colliders is null, means no collision (for y)
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0.0f, new Vector2(0.0f, _moveDelta.y), Mathf.Abs(_moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking","Character"));
        if(_hit.collider == null)
        {
            //move
            transform.Translate(0.0f, _moveDelta.y * Time.deltaTime, 0.0f);
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

    public void AddEquipmentToInventory(Equipment equipment, int? amount = null)
    {
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                _inventories[(int)Common.InventoryType.ARMOR].AddEquipmentToInventory(equipment, amount);
                break;
            }  
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                _inventories[(int)Common.InventoryType.WEAPON].AddEquipmentToInventory(equipment, amount);
                break;
            }   
            case Common.EquipmentType.POTION:
            {
                _inventories[(int)Common.InventoryType.POTION].AddEquipmentToInventory(equipment, amount);
                break;
            } 
            default: 
                break;
        }
    }

    public Protection GetArmorInfo()
    {
        Protection protection = new Protection();
        protection.InitializeBuffsInfo();

        List<Equipment> armorEquipments = _inventories[(int)Common.InventoryType.ARMOR].GetEquippedEquipments();

        foreach(Equipment equipment in armorEquipments)
        {
            Armor armor = equipment as Armor;
            protection.armorPoints += armor.armorPoints;
            protection.SetArmorBuffLevel(armor.armorBuff, armor.buffLevel);
        }
        
        return protection;
    }

    public Damage GetWeaponInfo()
    {
        Damage damage = new Damage();
        damage.InitializeDebuffsInfo();

        List<Equipment> weaponEquipments = _inventories[(int)Common.InventoryType.WEAPON].GetEquippedEquipments();
        
        foreach(Equipment equipment in weaponEquipments)
        {
            Weapon weapon = equipment as Weapon;
            damage.isMelee = weapon.equipmentType == Common.EquipmentType.MELEE_WEAPON ? true : false;
            damage.origin = Vector3.zero;
            damage.damagePoints += weapon.damagePoints;
            damage.pushForce = weapon.pushForce;
            damage.attackRange = weapon.attackRange;
            damage.attackSpeed = Mathf.FloorToInt((2.0f - weapon.cooldown)*20);
            damage.SetWeaponBuffLevel(weapon.weaponDebuff, weapon.debuffLevel);
        }

        return damage;
    }

}
