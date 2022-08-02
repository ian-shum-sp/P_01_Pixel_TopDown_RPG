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
    public float pushForce;
    public float cooldown;
    #endregion
    
    #region accessors
    #endregion
}
