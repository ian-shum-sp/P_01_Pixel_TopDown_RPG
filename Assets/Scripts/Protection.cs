using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Protection
{
    public int movementSpeed;
    public int armorPoints;
    public Common.ArmorBuff[] armorBuffs;
    public int[] armorBuffsLevels;
    public Common.PotionBuff[] potionBuffs;
    public int[] potionBuffsLevels;
    public Common.Debuff[] appliedDebuffs;
    public int[] appliedDebuffsLevels;
    public float[] appliedDebuffsDuration;

    public void Initialize()
    {
        movementSpeed = 0;
        armorPoints = 0;

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

        appliedDebuffs = new Common.Debuff[3];
        appliedDebuffs[0] = Common.Debuff.BLEEDING;
        appliedDebuffs[1] = Common.Debuff.KNOCKBACK;
        appliedDebuffs[2] = Common.Debuff.ELEMENT;

        appliedDebuffsLevels = new int[3];
        appliedDebuffsLevels[0] = 0;
        appliedDebuffsLevels[1] = 0;
        appliedDebuffsLevels[2] = 0;

        appliedDebuffsDuration = new float[3];
        appliedDebuffsDuration[0] = 0.0f;
        appliedDebuffsDuration[1] = 0.0f;
        appliedDebuffsDuration[2] = 0.0f;
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
        if(potionBuff == Common.PotionBuff.HEALING)
            return;

        potionBuffsLevels[(int)potionBuff] += buffLevel;
        if(potionBuffsLevels[(int)potionBuff] > 3)
            potionBuffsLevels[(int)potionBuff] = 3;
    }

    public bool ApplyDebuffLevel(Common.Debuff debuff, int debuffLevel)
    {
        bool isRefreshDuration = false;

        if(debuff == Common.Debuff.NONE)
            return isRefreshDuration;

        if(debuffLevel >= appliedDebuffsLevels[(int)debuff])  
        {
            appliedDebuffsLevels[(int)debuff] = debuffLevel;
            float duration = debuffLevel == 1 ? 6.0f : (debuffLevel == 2 ? 10.0f : 20.0f); 
            appliedDebuffsDuration[(int)debuff] = duration;
            isRefreshDuration = true;
            return isRefreshDuration;
        }

        return isRefreshDuration;
    }

    public void RemoveArmorBuffLevel(Common.ArmorBuff armorBuff, int buffLevel)
    {
        if(armorBuff == Common.ArmorBuff.NONE)
            return;

        armorBuffsLevels[(int)armorBuff] -= buffLevel;
        if(armorBuffsLevels[(int)armorBuff] < 0)
            armorBuffsLevels[(int)armorBuff] = 0;
    }

    public void RemovePotionBuffLevel(Common.PotionBuff potionBuff, int buffLevel)
    {
        if(potionBuff == Common.PotionBuff.HEALING)
            return;

        potionBuffsLevels[(int)potionBuff] -= buffLevel;
        if(potionBuffsLevels[(int)potionBuff] < 0)
            potionBuffsLevels[(int)potionBuff] = 0;
    }

    public void RemoveAppliedDebuff(Common.Debuff debuff)
    {
        if(debuff == Common.Debuff.NONE)
            return;

        appliedDebuffsLevels[(int)debuff] = 0;
        appliedDebuffsDuration[(int)debuff] = 0.0f;
    }

    public float GetDebuffDuration(Common.Debuff debuff)
    {
        return appliedDebuffsDuration[(int)debuff];
    }

    public int GetTotalBleedingResistanceLevel()
    {
        int totalBleedingResistanceLevel = armorBuffsLevels[(int)Common.ArmorBuff.BLEEDING_RESISTANCE] + potionBuffsLevels[(int)Common.PotionBuff.BLEEDING_RESISTANCE] - appliedDebuffsLevels[(int)Common.Debuff.BLEEDING];
        if(totalBleedingResistanceLevel > 3)
            totalBleedingResistanceLevel = 3;
        
        return totalBleedingResistanceLevel;
    }

    public int GetTotalKnockbackResistanceLevel()
    {
        int totalKnockbackResistanceLevel = armorBuffsLevels[(int)Common.ArmorBuff.KNOCKBACK_RESISTANCE] + potionBuffsLevels[(int)Common.PotionBuff.KNOCKBACK_RESISTANCE] - appliedDebuffsLevels[(int)Common.Debuff.KNOCKBACK];
        if(totalKnockbackResistanceLevel > 3)
            totalKnockbackResistanceLevel = 3;
        return totalKnockbackResistanceLevel;
    }

    public int GetTotalElementResistanceLevel()
    {
        int totalElementResistanceLevel = armorBuffsLevels[(int)Common.ArmorBuff.ELEMENT_RESISTANCE] + potionBuffsLevels[(int)Common.PotionBuff.ELEMENT_RESISTANCE] - appliedDebuffsLevels[(int)Common.Debuff.ELEMENT];
        if(totalElementResistanceLevel > 3)
            totalElementResistanceLevel = 3;
        return totalElementResistanceLevel;
    }

    public int GetTotalStrengthLevel()
    {
        return potionBuffsLevels[(int)Common.PotionBuff.STRENGTH];
    }

    public int GetTotalSpeedLevel()
    {
        return potionBuffsLevels[(int)Common.PotionBuff.SPEED];
    }
}
