using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Protection
{
    public int armorPoints;
    public Common.ArmorBuff[] armorBuffs;
    public int[] armorBuffsLevels;
    public Common.PotionBuff[] potionBuffs;
    public int[] potionBuffsLevels;

    public void InitializeBuffsInfo()
    {
        armorBuffs = new Common.ArmorBuff[3];
        armorBuffs[0] = Common.ArmorBuff.BLEEDING_RESISTANCE;
        armorBuffs[1] = Common.ArmorBuff.KNOCKBACK_RESISTANCE;
        armorBuffs[2] = Common.ArmorBuff.ELEMENT_RESISTANCE;

        armorBuffsLevels = new int[3];
        armorBuffsLevels[0] = 0;
        armorBuffsLevels[1] = 0;
        armorBuffsLevels[2] = 0;

        potionBuffs = new Common.PotionBuff[5];
        potionBuffs[0] = Common.PotionBuff.BLEEDING_RESISTANCE;
        potionBuffs[1] = Common.PotionBuff.KNOCKBACK_RESISTANCE;
        potionBuffs[2] = Common.PotionBuff.ELEMENT_RESISTANCE;
        potionBuffs[3] = Common.PotionBuff.STRENGTH;
        potionBuffs[4] = Common.PotionBuff.SPEED;

        potionBuffsLevels = new int[5];
        potionBuffsLevels[0] = 0;
        potionBuffsLevels[1] = 0;
        potionBuffsLevels[2] = 0;
        potionBuffsLevels[3] = 0;
        potionBuffsLevels[4] = 0;
    }

    public void SetArmorBuffLevel(Common.ArmorBuff armorBuff, int buffLevel)
    {
        if(armorBuff == Common.ArmorBuff.NONE)
            return;
            
        armorBuffsLevels[(int)armorBuff] += buffLevel;
        if(armorBuffsLevels[(int)armorBuff] > 3)
            armorBuffsLevels[(int)armorBuff] = 3;
    }

    public void SetPotionBuffLevel(Common.PotionBuff potionBuff, int buffLevel)
    {
        potionBuffsLevels[(int)potionBuff] += buffLevel;
        if(potionBuffsLevels[(int)potionBuff] > 3)
            potionBuffsLevels[(int)potionBuff] = 3;
    }
}
