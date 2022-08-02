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
        [Description("Male")]
        MALE = 0,
        [Description("Female")]
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
        SIGN = 4,
        WARNING_SIGN = 5
    }

    public enum InventoryType
    {
        ARMOR = 0,
        WEAPON = 1,
        POTION = 2,
        POUCH = 3
    }

    public enum InventorySlotType
    {
        ARMOR = 0,
        WEAPON = 1,
        POTION = 2,
        POUCH = 3
    }

    public enum DisplaySlotType
    {
        HEAD_ARMOR = 0,
        CHEST_ARMOR = 1,
        BOOTS_ARMOR = 2,
        WEAPON = 3
    }

    public enum EquipmentType
    {
        [Description("Head Armor")]
        HEAD_ARMOR = 0,
        [Description("Chest Armor")]
        CHEST_ARMOR = 1,
        [Description("Boots Armor")]
        BOOTS_ARMOR = 2,
        [Description("Melee Weapon")]
        MELEE_WEAPON = 3,
        [Description("Ranged Weapon")]
        RANGED_WEAPON = 4,
        [Description("Potion")]
        POTION = 5
    }

    public enum ArmorBuff
    {
        [Description("Bleeding Resistance")]
        BLEEDING_RESISTANCE = 0,
        [Description("Knonckback Resistance")]
        KNOCKBACK_RESISTANCE = 1,
        [Description("Element Resistance")]
        ELEMENT_RESISTANCE = 2,
        [Description("None")]
        NONE = 4 
    }

    public enum PotionBuff
    {
        [Description("Bleeding Resistance")]
        BLEEDING_RESISTANCE = 0,
        [Description("Knonckback Resistance")]
        KNOCKBACK_RESISTANCE = 1,
        [Description("Element Resistance")]
        ELEMENT_RESISTANCE = 2,
        [Description("Strength")]
        STRENGTH = 3,
        [Description("Speed")]
        SPEED = 4,
        [Description("Healing")]
        HEALING = 5
    }

    public enum Debuff
    {
        [Description("Bleeding")]
        BLEEDING = 0,
        [Description("Knonckback")]
        KNOCKBACK = 1,
        [Description("Element")]
        ELEMENT = 2,
        [Description("None")]
        NONE = 3
    }

    public enum ChestType
    {
        STARTER_CHEST = 0,
        GOLD_CHEST_SILVER = 1,
        GOLD_CHEST_GOLDEN = 2,
        ARMOR_CHEST = 3,
        WEAPON_CHEST = 4,
        POTION_CHEST = 5
    }
   
    public static readonly Color UnlockedSlotColor = new Color(Color.white.r, Color.white.g, Color.white.b, 100.0f/255.0f);
    public static readonly Color LockedSlotColor = new Color(Color.black.r, Color.black.g, Color.black.b, 100.0f/255.0f);
    public static readonly Color OccupiedSlotImageBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 1.0f);
    public static readonly Color UnoccupiedSlotImageBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.0f);
    public static readonly Color EquippedSlotBackgroundColor = new Color(211.0f/255.0f, 191.0f/255.0f, 169.0f/255.0f, 1.0f);
    public static readonly Color UnequippedSlotBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 100.0f/255.0f);

    public static string GetEnumDescription(Enum value)
    {
        return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    }
}
