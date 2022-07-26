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

    public enum NPCName
    {
        GUIDE_INTRODUCTORY = 0,
        ARMORER = 1,
        WEAPONSMITH  = 2,
        MEDIC = 3,
        GUIDE_SIGN = 4
    }

    public enum InventoryType
    {
        ARMOR = 0,
        WEAPON = 1,
        POTION = 2,
        POUCH = 3
    }

    private static readonly int _numberOfTypeOfNPCs = 4;
    private static List<string> _nPCNames = new List<string>();
    private static Dictionary<int, List<string>> _nPCDialogs = new Dictionary<int, List<string>>();
    private static int[] _inventoryBaseNumberOfSlotsList = {0, 0, 0, 0};
    public static Color EnabledSlotColor = new Color(Color.white.r, Color.white.g, Color.white.b, 100.0f/255.0f);
    public static Color DisabledSlotColor = new Color(Color.black.r, Color.black.g, Color.black.b, 100.0f/255.0f);
    public static Color EquippedSlotImageBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 1.0f);
    public static Color UnequippedSlotImageBackgroundColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.0f);
    static Common()
    {
       InitializeNPCInfos();
    }

    private static void InitializeNPCInfos()
    {
        for (int i = 0; i < _numberOfTypeOfNPCs; i++)
        {
            _nPCNames.Add(((NPCName)i).ToString());
            _nPCDialogs.Add(i, InitializeNPCDialog((NPCName)i));
        }
    }

    private static List<string> InitializeNPCDialog(NPCName nPCName)
    {
        List<string> dialogs = new List<string>();

        switch(nPCName)
        {
            case NPCName.GUIDE_INTRODUCTORY:
            {
                dialogs.Add("Welcome to First RPG!\n" +
                            "I will guide you through this tutorial, so please follow closely!\n" + 
                            "First of all, the movement is controlled by the Arrow Buttons. You can also use W A S D buttons.\n" +
                            "Try to move around the room and go to the next room when you are ready!");
                dialogs.Add("Move closer to the signs, chests and NPCs and press button I when the \"Interact\" word shows up to interact with them!");
                dialogs.Add("At top left of your screen, you can see your current Level, Level Progress, Health and Status.\n" + 
                            "At bottom left of your screen, you can see your pouch with equipped potions.\n" + 
                            "Click on the bag icon at the bottom right of your screen or button M to open up the Player Menu.\n" + 
                            "In the Player Menu, you can view your stats and inventory, save the game or go back to the Main Menu screen.\n");
                dialogs.Add("You can equip/unequip armors, weapons and potions by pressing button E.\n" + 
                            "Inventory slots and pouch (for potions) are unlocked by levelling up and spending gold.");
                dialogs.Add("Press Space to attack!\n" +
                            "To use the equipped potions in the pouch, press button 1, 2, 3 and 4 for respective potion slots.\n" + 
                            "Note: You can only use equipped potions during adventure mode, and you cannot change equipped potion during adventure mode.");
                dialogs.Add("Congratulation! You have finished the tutorial part!\n" + 
                            "Once you go through the door, the game will be automatically saved for the first time and your adventure begins!\n" + 
                            "I will see you around! Good luck adventurer!");
                break;
            }
            case NPCName.ARMORER:
            case NPCName.WEAPONSMITH:
            case NPCName.MEDIC:
            {
                dialogs.Add("Welcome! What is your pick for today?");
                break;
            }
            case NPCName.GUIDE_SIGN:
            {
                dialogs.Add("Move to the next room when you are ready!");
                break;
            }
            default: 
                break;
        }
        return dialogs;
    } 

    public static void UpdateInventoryBaseNumberOfSlots(InventoryType inventoryType, int maxNumberOfInventorySlots, int maxLevel)
    {
        _inventoryBaseNumberOfSlotsList[(int)inventoryType] = maxNumberOfInventorySlots / maxLevel;
    }

    public static string GetEnumDescription(Enum value)
    {
        return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    }

    public static string GetNPCName(NPCName nPCName)
    {
        return _nPCNames[(int)nPCName];
    }

    public static List<string> GetNPCDialogs(NPCName nPCName)
    {
        return _nPCDialogs[(int)nPCName];
    }

    public static int GetInventoryBaseNumberOfSlots(InventoryType inventoryType)
    {
        return _inventoryBaseNumberOfSlotsList[(int)inventoryType];
    }

    public static void UpdateGuideName(PlayerGender playerGENDER)
    {
        if(playerGENDER == PlayerGender.MALE)
            _nPCNames[0] = "Marley";
        else
            _nPCNames[0] = "Brad";
    }

    //Calculate the Level from the player's current Experience, returned value is the plain Level (not index)
    public static int CalculateLevelFromExperience()
    {
        int currentLevel = 0;
        //this variable keeps track of the accumulated experience of current level
        int totalExperienceOfCurrentLevel = 0;

        //while the player has more than the accumulated experience of current level, means it has the level equal to the current level 
        while(GameManager.Instance.player.Experience >= totalExperienceOfCurrentLevel)
        {
            totalExperienceOfCurrentLevel += GameManager.Instance.experienceTable[currentLevel];
            currentLevel++;

            if(currentLevel == GameManager.Instance.experienceTable.Count)
                return currentLevel;
        }

        return currentLevel;
    }

    //Calculate the Accumulated Experience needed to reach the Level passed in as paramater, the Level paramter is the plain level (not index)
    public static int GetAccumulatedExperienceOfLevel(int level)
    {
        int runningLevel = 0;
        int totalExperience = 0;

        while(runningLevel < level)
        {
            totalExperience += GameManager.Instance.experienceTable[runningLevel];
            runningLevel++;
        }

        return totalExperience;
    }
}
