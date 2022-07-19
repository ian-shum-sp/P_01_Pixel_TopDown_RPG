using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region References to Singleton
    public FloatingTextManager floatingTextManager;
    #endregion

    #region Floating Text Manager
    public void Show(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float showDuration)
    {
        floatingTextManager.Show(message, fontSize, color, position, motion, showDuration);
    }
    #endregion

    #region Save, Load, Reset Game
    public void SaveGame()
    {
        /*
        Save with '|' as delimeter 
        Order of saving
        1. Player Name
        2. Xp
        3. Gold
        4. Inventory ('&' as delimeter)
        5. Central Hub Location
        6. Equipped Head Armor
        7. Equipped Chest Armor
        8. Equipped Boot Armor
        9. Equipped Weapon
        10. Pouch Item ('&' as delimeter)
        */

        string saveData = "";

        PlayerPrefs.SetString("P01SaveData", saveData);

    }

    public void ResetSave()
    {
        if(PlayerPrefs.HasKey("P01SaveData"))
        {
            PlayerPrefs.DeleteKey("P01SaveData");
        }
    }

    public void LoadGame()
    {
        string[] saveData = PlayerPrefs.GetString("P01SaveData").Split('|');
    }
    #endregion
}
