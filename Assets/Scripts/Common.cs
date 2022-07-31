using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class Common 
{
    public enum PlayerGender
    {
        MALE = 0,
        FEMALE = 1
    }

    public enum SceneName
    {
        [Description("MainScene")]
        MAIN_SCENE = 0,
        [Description("Introductory")]
        INTRODUCTORY = 1,
        [Description("DungeonCentralHub")]
        DUNGEON_CENTRAL_HUB = 2,
        [Description("EnchantedForestCentralHub")]
        ENCHANTED_FOREST_CENTRAL_HUB = 3,
        [Description("FantasyCentralHub")]
        FANTASY_CENTRAL_HUB = 4
    }

    public enum NPCType
    {
        GUIDE = 0,
        ARMORER = 1,
        WEAPONSMITH  = 2,
        POTION_BREWER = 3,
        SIGN = 4
    }

    public enum InventoryType
    {
        ARMOR = 0,
        WEAPON = 1,
        POTION = 2,
        POUCH = 3
    }

    public enum DisplaySlotType
    {
        HEAD_ARMOR = 0,
        WEAPON = 1,
        CHEST_ARMOR = 2,
        BOOTS_ARMOR = 3,
        POUCH = 4
    }

    public enum EquipmentType
    {
        HEAD_ARMOR = 0,
        CHEST_ARMOR = 1,
        BOOTS_ARMOR = 2,
        MELEE_WEAPON = 3,
        RANGED_WEAPON = 4,
        POTION = 5
    }

    public enum ArmorBuff
    {
        BLEEDING_RESISTANCE = 0,
        KNOCKBACK_RESISTANCE = 1,
        ELEMENT_RESISTANCE = 2,
        NONE = 4 
    }

    public enum PotionBuff
    {
        BLEEDING_RESISTANCE = 0,
        KNOCKBACK_RESISTANCE = 1,
        ELEMENT_RESISTANCE = 2,
        STRENGTH = 3,
        SPEED = 4,
        HEALING = 5
    }

    public enum Debuff
    {
        BLEEDING = 0,
        KNOCKBACK = 1,
        ELEMENT = 2,
        NONE = 3
    }
   
    public static readonly Color UnlockedSlotColor = new Color(Color.white.r, Color.white.g, Color.white.b, 100.0f/255.0f);
    public static readonly Color LockedSlotColor = new Color(Color.black.r, Color.black.g, Color.black.b, 100.0f/255.0f);
    public static readonly Color OccupiedSlotImageBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 1.0f);
    public static readonly Color UnoccupiedSlotImageBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.0f);
    public static readonly Color EquippedSlotBackgroundColor = new Color(211.0f, 191.0f, 169.0f, 1.0f);
    public static readonly Color UnequippedSlotBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 100.0f/255.0f);

    public static string GetEnumDescription(Enum value)
    {
        return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    }
}
