using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class Enums 
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
        GUIDE = 0,
        ARMORER = 1,
        WEAPONSMITH  = 2,
        MEDIC = 3,
        SIGN = 4
    }

    private static readonly int _numberOfTypeOfNPCs = 4;
    private static List<string> _nPCNames = new List<string>();
    private static Dictionary<int, List<string>> _nPCDialogs = new Dictionary<int, List<string>>();

    static Enums()
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
            case NPCName.GUIDE:
            {
                dialogs.Add("Welcome to FIrst RPG!\n" +
                            "I will guide you through this tutorial, so please follow closely!");
                dialogs.Add("First of all, the movement is controlled by the Arrow Buttons. You can also use W A S D buttons. Try to move around the room.");
                dialogs.Add("Press I button to interact with signs, chests and NPCs.");
                dialogs.Add("At top left of your screen, you can see your current Level, Level Progress, Health and Status.\n" + 
                            "Click on the bag icon on the bottom right of your screen or M button to open up the Player Menu.\n" + 
                            "In the Player Menu, you can view your stats and inventory, and also save the game or go back to the Main Menu screen.\n" + 
                            "You can equip/unequip armors, weapons and potions by pressing E button. Potion slots are unlocked by levelling up.");
                dialogs.Add("Press Space to attack!\n" +
                            "To use the equipped potions, press 1, 2, 3, 4 button for respective potion slots.\n" + 
                            "Note: You can only use equipped potions during adventure mode, and you cannot change equipped potion during adventure mode.");
                dialogs.Add("Congratulation! You have finished the tutorial part!\n" + 
                            "Once you go through the door, the game will be automatically saved for the first time and your adventure begins!\nGood luck adventurer!");
                break;
            }
            case NPCName.ARMORER:
            case NPCName.WEAPONSMITH:
            case NPCName.MEDIC:
            {
                dialogs.Add("Welcome! What is your pick for today?");
                break;
            }
            default: 
                break;
        }
        return dialogs;
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
}
