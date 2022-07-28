using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private bool _isInitialized = false;
    private int numberOfTypeOfNPCs = 4;
    private List<string> _nPCNames = new List<string>();
    private Dictionary<int, List<string>> _nPCDialogs = new Dictionary<int, List<string>>();

    private List<string> InitializeNPCDialog(Common.NPCName nPCName)
    {
        List<string> dialogs = new List<string>();

        switch(nPCName)
        {
            case Common.NPCName.GUIDE_INTRODUCTORY:
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
            case Common.NPCName.ARMORER:
            case Common.NPCName.WEAPONSMITH:
            case Common.NPCName.MEDIC:
            {
                dialogs.Add("Welcome! What is your pick for today?");
                break;
            }
            case Common.NPCName.GUIDE_SIGN:
            {
                dialogs.Add("Move to the next room when you are ready!");
                break;
            }
            default: 
                break;
        }
        return dialogs;
    } 

    public void InitializeNPCInfos()
    {
        if(_isInitialized)
            return;

        for (int i = 0; i < numberOfTypeOfNPCs; i++)
        {
            _nPCNames.Add(((Common.NPCName)i).ToString());
            _nPCDialogs.Add(i, InitializeNPCDialog((Common.NPCName)i));
        }
        _isInitialized = true;
    }

    public string GetNPCName(Common.NPCName nPCName)
    {
        return _nPCNames[(int)nPCName];
    }

    public List<string> GetNPCDialogs(Common.NPCName nPCName)
    {
        return _nPCDialogs[(int)nPCName];
    }

    public void UpdateGuideName(Common.PlayerGender playerGENDER)
    {
        if(playerGENDER == Common.PlayerGender.MALE)
            _nPCNames[0] = "Marley";
        else
            _nPCNames[0] = "Brad";
    }

}
