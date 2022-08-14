using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public int damagePoints;
    public Common.Debuff weaponDebuff;
    public int debuffLevel;
    public int attackRange;
    public float baseKnockbackForce;
    public float cooldown;
}
