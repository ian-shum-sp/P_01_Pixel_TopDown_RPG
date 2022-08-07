using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage
{
    public bool isHaveWeapon;
    public bool isMelee;
    public Vector3 origin;
    public float damagePoints;
    public float knockbackForce;
    public int attackRange;
    public float cooldown;
    public int attackSpeed;
    public Common.Debuff[] weaponDebuffs;
    public int[] weaponDebuffsLevels;

    public void Initialize()
    {
        isHaveWeapon = false;
        isMelee = false;
        origin = Vector3.zero;
        damagePoints = 0;
        knockbackForce = 0.0f;
        attackRange = 0;
        cooldown = 0.0f;
        attackSpeed = 0;

        weaponDebuffs = new Common.Debuff[3];
        weaponDebuffs[0] = Common.Debuff.BLEEDING;
        weaponDebuffs[1] = Common.Debuff.KNOCKBACK;
        weaponDebuffs[2] = Common.Debuff.ELEMENT;

        weaponDebuffsLevels = new int[3];
        weaponDebuffsLevels[0] = 0;
        weaponDebuffsLevels[1] = 0;
        weaponDebuffsLevels[2] = 0;
    }

    public void SetWeaponBuffLevel(Common.Debuff debuff, int buffLevel)
    {
        if(debuff == Common.Debuff.NONE)
            return;
            
        weaponDebuffsLevels[(int)debuff] += buffLevel;
        if(weaponDebuffsLevels[(int)debuff] > 3)
            weaponDebuffsLevels[(int)debuff] = 3;
    }
}
