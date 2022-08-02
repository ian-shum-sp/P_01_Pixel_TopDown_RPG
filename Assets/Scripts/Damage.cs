using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage
{
    public bool isMelee;
    public Vector3 origin;
    public int damagePoints;
    public float pushForce;
    public int attackRange;
    public int attackSpeed;
    public Common.Debuff[] weaponDebuffs;
    public int[] weaponDebuffsLevels;

    public void InitializeDebuffsInfo()
    {
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
