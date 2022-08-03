using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    #region class members
    public int damagePoints;
    public Common.Debuff weaponDebuff;
    public int debuffLevel;
    public int attackRange;
    public float baseKnockbackForce;
    public float cooldown;
    #endregion
    
    #region accessors
    #endregion
}
