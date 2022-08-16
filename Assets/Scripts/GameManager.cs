using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    #region Scene Management
    private GameScene _currentGameScene;
    #endregion

    #region References
    public FloatingTextManager floatingTextManager;
    public DialogManager dialogManager;
    public ConfirmationManager confirmationManager;
    public NotificationManager notificationManager;
    public LoadingScreenManager loadingScreenManager;
    public ExperienceManager experienceManager;
    public EquipmentManager equipmentManager;
    public ShopManager shopManager;
    public NPCManager nPCManager;
    public HUD hUD;
    public PlayerMenu playerMenu;
    public MainMenu mainMenu;
    public DeathMenu deathMenu;
    #endregion

    #region Reset Game
    public Button resetButton;
    private bool _isTryResetGame;
    public bool IsTryResetGame
    {
        get { return _isTryResetGame; }
        set { _isTryResetGame = value; }
    }
    #endregion

    #region Game
    private bool _isTryLoadMainMenu;
    public bool IsTryLoadMainMenu
    {
        get { return _isTryLoadMainMenu; }
        set { _isTryLoadMainMenu = value; }
    }
    private bool _isBlockGameActions;
    public bool IsBlockGameActions
    {
        get { return _isBlockGameActions; }
        set { _isBlockGameActions = value; }
    }
    public Player player;
    public Sprite[] playerSprites;
    private string _lastSavedTimeText;
    public string LastSavedTimeText
    {
        get { return _lastSavedTimeText; }
        set { _lastSavedTimeText = value; }
    }
    private bool _isMainMenuShown;
    public bool IsMainMenuShown
    {
        get { return _isMainMenuShown; }
        set { _isMainMenuShown = value; }
    }
    private bool _isHUDShown;
    public bool IsHUDShown
    {
        get { return _isHUDShown; }
        set { _isHUDShown = value; }
    }
    private bool _isDialogShown;
    public bool IsDialogShown
    {
        get { return _isDialogShown; }
        set { _isDialogShown = value; }
    }
    private bool _isDeathMenuShown;
    public bool IsDeathMenuShown
    {
        get { return _isDeathMenuShown; }
        set { _isDeathMenuShown = value; }
    }
    #endregion

    #region Custom Cursor
    public Texture2D defaultCursorTexture;
    public Texture2D hoverCursorTexture;
    #endregion

    private void Awake() 
    {
        if(GameManager.Instance != null)
        {
            Destroy(gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(dialogManager.gameObject);
            Destroy(confirmationManager.gameObject);
            Destroy(loadingScreenManager.gameObject);
            Destroy(experienceManager.gameObject);
            Destroy(equipmentManager.gameObject);
            Destroy(shopManager.gameObject);
            Destroy(nPCManager.gameObject);
            Destroy(notificationManager.gameObject);
            Destroy(hUD.gameObject);
            Destroy(playerMenu.gameObject);
            Destroy(mainMenu.gameObject);
            Destroy(player.gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() 
    {
        GameScene mainScene = new GameScene();
        mainScene.SceneName = Common.SceneName.MAIN_SCENE;
        mainScene.SceneDisplayName = "Main Menu";
        _currentGameScene = mainScene;
        _isBlockGameActions = true;
        SetResetButtonVisibility();
        ShowMainMenuDisplay();
        ChangeCursor(false);
    }

    #region Floating Text Manager
    public void ShowFloatingText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float showDuration)
    {
        floatingTextManager.Show(message, fontSize, color, position, motion, showDuration);
    }
    #endregion

    #region Dialog Manager
    public void ShowFullDialog(string nPCID)
    {
        if(!_isDialogShown)
        {
            _isBlockGameActions = true;
            dialogManager.ShowFullDialog(nPCID);
        }
    }

    public void ShowRunningDialog(string nPCID, bool isReset)
    {
        if(!_isDialogShown)
        {
            _isBlockGameActions = true;
            dialogManager.ShowRunningDialog(nPCID, isReset);
        }
    }
    #endregion

    #region Confirmation Manager
    public void ShowConfirmation(string text)
    {
        _isBlockGameActions = true;
        confirmationManager.Show(text);
    }

    public bool GetConfirmationResult()
    {
        bool result = confirmationManager.IsClickedYes;
        confirmationManager.IsClickedYes = false;
        return result;
    }
    #endregion

    #region Notification Manager
    public void ShowNotification(string text, Color color, bool isContinueBlockGameAction = true)
    {
        _isBlockGameActions = true;
        notificationManager.Show(text, isContinueBlockGameAction, color);
    }
    #endregion

    #region Loading Screen Manager
    public void LoadScene(GameScene scene, float delay = 0.0f)
    {
        _currentGameScene = scene;
        loadingScreenManager.LoadScene(scene, delay);
    }
    #endregion

    #region Experience Manager
    public int GetPlayerLevel()
    {
        return experienceManager.CalculateLevelFromExperience();
    }

    public int GetAccumulatedExperienceOfLevel(int level)
    {
        return experienceManager.GetAccumulatedExperienceOfLevel(level);
    }

    public void AddExperienceToPlayer(int experience)
    {
        experienceManager.AddExperience(experience);
        UpdateHUDExperience();
        UpdatePlayerMenuExperience();
    }
    #endregion

    #region Equipment Manager
    public List<Equipment> GetStarterEquipments()
    {
        return equipmentManager.GetStarterEquipments();
    }
    public Equipment GenerateRandomArmor()
    {
        return equipmentManager.GetRandomArmor();
    }

    public Equipment GenerateRandomWeapon()
    {
        return equipmentManager.GetRandomWeapon();
    }

    public Equipment GenerateRandomPotion()
    {
        return equipmentManager.GetRandomPotion();
    }
    #endregion

    #region Shop Manager
    public void InitializeShops()
    {
        shopManager.InitializeShops();
    }

    public void ShowShop(Common.NPCType shopOwner)
    {
        _isBlockGameActions = true;
        shopManager.ShowShop(shopOwner);
    }

    public void HideShop()
    {
        _isBlockGameActions = false;
        shopManager.HideShop();
        ResetActiveNPC();
    }

    public void ResetSelectedEquipmentToBuyDisplayInfo()
    {
        shopManager.ResetBuyEquipmentInfo();
    }

    public void UpdateSelectedEquipmentToBuyDisplayInfo(Equipment equipment)
    {
        shopManager.DeselectAnyActiveBuySlot();
        shopManager.SetSelectedBuyEquipmentInfo(equipment);
    }

    public void ResetSelectedEquipmentForSaleDisplayInfo()
    {
        shopManager.ResetSellEquipmentInfo();
    }

    public void UpdateSelectedEquipmentForSaleDisplayInfo(Equipment equipment, int amount, string inventorySlotID)
    {
        shopManager.DeselectAnyActiveSellSlot();
        shopManager.SetSelectedSellEquipmentInfo(equipment, amount, inventorySlotID);
    }

    public void UpdateShopSellSection()
    {
        shopManager.UpdateSellSlots();
    }

    public void AddToShopSellSection(Equipment equipment, string inventorySlotID, int? amount = null)
    {
        shopManager.AddToSellSlots(equipment, inventorySlotID, amount);
    }
    #endregion

    #region NPC Manager
    public void UpdateNPCManager(NPC nPC)
    {
        nPCManager.AddNPC(nPC);
    }

    public string GetNPCID(Common.NPCType nPCType)
    {
        return nPCManager.GetNPCID(nPCType);
    }

    public string GetNPCName(string nPCID)
    {
        return nPCManager.GetNPCName(nPCID);
    }

    public string[] GetNPCDialogs(string nPCID)
    {
        return nPCManager.GetNPCDialogs(nPCID);
    }

    public void UpdateGuideName(Common.PlayerGender playerGender)
    {
        nPCManager.UpdateGuide(playerGender);
    }

    public void SetActiveNPC(NPC nPC)
    {
        nPCManager.ActiveNPC = nPC;
    }

    public void ResetActiveNPC()
    {
        nPCManager.ResetActiveNPC();
    }

    public void ResetNPCManager()
    {
        nPCManager.ResetNPCList();
    }
    #endregion

    #region HUD
    public void InitializeHUD()
    {
        hUD.InitializeHUD();
    }

    public void ResetHUD()
    {
        hUD.ResetHUD();
    }
    
    public void ShowHUD()
    {
        if(!_isHUDShown) 
            hUD.Show();
    }

    public void HideHUD()
    {
        if(_isHUDShown) 
        {
            hUD.ResetHUD();
            hUD.Hide();
        }
    }

    public void UpdateHUDHealthPoints()
    {
        hUD.UpdateHealthPoints();
    }   

    public void UpdateHUDExperience()
    {
        hUD.UpdateExperience();
    } 

    public void UpdateHUDStatusInfo()
    {
        hUD.UpdateStatusText();
    }

    public void EquipToPouch(Potion potion, int amount)
    {
        hUD.AddToPouchSlot(potion, amount);
    }

    public void UnequipFromPouch(string equipmentID)
    {
        hUD.RemoveFromPouchSlot(equipmentID);
    }

    public void UpdatePouchSlot(string equipmentID, int amount)
    {
        hUD.UpdatePouchSlot(equipmentID, amount);
    }

    public void UnlockPouchSlot()
    {
        hUD.UnlockPouchSlot();
    }

    public void UpdateBagSprite(int playerLevel)
    {
        hUD.UpdateBagSprite(playerLevel);
    }
    #endregion

    #region Player Menu
    public void InitializePlayerMenu()
    {
        playerMenu.InitializePlayerMenu();
    }

    public void ShowPlayerMenu()
    {
        _isBlockGameActions = true;
        playerMenu.UpdateOnExpandPlayerMenu();
    }

    public void HidePlayerMenu()
    {
        _isBlockGameActions = false;
        playerMenu.Hide();
    }

    public void UpdatePlayerMenuHealthPoints()
    {
        playerMenu.UpdateHealthPoints();
    }

    public void UpdatePlayerMenuGold()
    {
        playerMenu.UpdateGold();
        playerMenu.UpdateInventoryUpgradeStatus();
    }

    public void UpdatePlayerMenuExperience()
    {
        playerMenu.UpdateExperience();
        playerMenu.UpdateInventoryUpgradeStatus();
    }

    public void UpdatePlayerMenuEquipmentInfo()
    {
        playerMenu.UpdateWeaponStatInfoText();
        playerMenu.UpdateArmorStatInfoText();
    }

    public void AddToDisplaySlot(Equipment equipment)
    {
        playerMenu.AddToDisplaySlot(equipment);
    }

    public void RemoveFromDisplaySlot(Equipment equipment)
    {
        playerMenu.RemoveFromDisplaySlot(equipment);
    }

    public void ShowEquipmentPopUp(Equipment equipment, int amount, Vector3 position)
    {
        if(playerMenu.IsPopUpShowing)
            return;
            
        playerMenu.UpdatePopUpInfo(equipment, amount, position);
        playerMenu.popUpAnimator.SetTrigger("Show");
        playerMenu.IsPopUpShowing = true;
    }

    public bool CheckIfPopUpShown()
    {
        return playerMenu.IsPopUpShowing;
    }

    public void HideEquipmentPopUp()
    {
        if(!playerMenu.IsPopUpShowing)
            return;

        playerMenu.popUpAnimator.SetTrigger("Hide");
        playerMenu.IsPopUpShowing = false;
    }
    #endregion

    #region Main Menu 
    public void ShowMainMenuDisplay()
    {
        if(!_isMainMenuShown) 
            mainMenu.ShowMenu();
    }

    public void HideMainMenuDisplay()
    {
        if(_isMainMenuShown) 
            mainMenu.HideMenu();
    }
    #endregion

    #region Death Menu
    public void ShowDeathMenu()
    {
        if(!_isDeathMenuShown) 
        {
            _isBlockGameActions = true;
            deathMenu.ShowDeathMenu();
        }
    }

    public void Respawn()
    {
        if(_isDeathMenuShown) 
        {
            deathMenu.HideDeathMenu();
            LoadScene(_currentGameScene);
            _isBlockGameActions = false;
            player.Respawn();
        }
    }
    #endregion

    #region Save, Load, Reset Game
    private void SpawnPlayer()
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        player.healthPoints = player.maxHealthPoints;
        if(spawnPoint != null)
        {
            player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        }
        else
        {
            player.transform.position = new Vector3(-500.0f, -500.0f, 0.0f);
        }
    }

    public void SaveGame()
    {
        if(_currentGameScene.SceneName == Common.SceneName.INTRODUCTORY)
        {
            ShowNotification("Please complete the tutorial before saving!", Color.red);
        }
        else if(_currentGameScene.SceneName == Common.SceneName.DUNGEON_ADVENTURE_MAP || _currentGameScene.SceneName == Common.SceneName.ENCHANTED_FOREST_ADVENTURE_MAP ||_currentGameScene.SceneName == Common.SceneName.FANTASY_ADVENTURE_MAP)
        {
            ShowNotification("You can only save game in central hubs!", Color.red);
        }
        else
        {
            /*
            Save with '|' as delimeter 
            Order of saving
            1. Player Name
            2. Player Gender
            3. Experience
            4. Gold
            5. ArmorInventory ('&' as delimeter)
                a. Inventory Level
                b. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. EquipmentID
                    iii. Amount
            6. WeaponInventory ('&' as delimeter)
                a. Inventory Level
                b. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. EquipmentID
                    iii. Amount
            7. PotionInventory ('&' as delimeter)
                a. Inventory Level
                b. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. EquipmentID
                    iii. Amount
            8. PouchInventory Inventory Level
            9. Last Save Location (Scene Name int)
            10. Last Save Time
            11. Equipped Head Armor Slot ID
            12. Equipped Chest Armor Slot ID
            13. Equipped Boots Armor Slot ID
            14. Equipped Weapon Slot ID
            15. Equipped Potions
                a. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. Amount
            */
            DateTime dateTime = DateTime.Now;
            string saveData = "";
            //Name
            saveData += player.Name + "|"; 
            //Gender
            saveData += ((int)player.Gender).ToString() + "|"; 
            //Experience
            saveData += player.Experience.ToString() + "|"; 
            //Gold
            saveData += player.Gold.ToString() + "|"; 
            //Armor Inventory
            Inventory armorInventory = player.GetInventory(Common.InventoryType.ARMOR);
            saveData += armorInventory.InventoryLevel.ToString() + "&";
            string armorSlotsData = "";
            for (int i = 0; i < armorInventory.UnlockedInventorySlots; i++)
            {
                if(armorInventory.slots[i].IsOccupied)
                {
                    armorSlotsData += armorInventory.slots[i].inventorySlotID + ":";
                    armorSlotsData += armorInventory.slots[i].Equipment.equipmentID + ":";
                    armorSlotsData += armorInventory.slots[i].Amount.ToString() + ",";
                }
            }
            armorSlotsData = armorSlotsData.TrimEnd(',');
            saveData += armorSlotsData + "|";
            //Weapon Inventory
            Inventory weaponInventory = player.GetInventory(Common.InventoryType.WEAPON);
            saveData += weaponInventory.InventoryLevel.ToString() + "&";
            string weaponSlotsData = "";
            for (int i = 0; i < weaponInventory.UnlockedInventorySlots; i++)
            {
                if(weaponInventory.slots[i].IsOccupied)
                {
                    weaponSlotsData += weaponInventory.slots[i].inventorySlotID + ":";
                    weaponSlotsData += weaponInventory.slots[i].Equipment.equipmentID + ":";
                    weaponSlotsData += weaponInventory.slots[i].Amount.ToString() + ",";
                }
            }
            weaponSlotsData = weaponSlotsData.TrimEnd(',');
            saveData += weaponSlotsData + "|";
            //Potion Inventory
            Inventory potionInventory = player.GetInventory(Common.InventoryType.POTION);
            saveData += potionInventory.InventoryLevel.ToString() + "&";
            string potionSlotsData = "";
            for (int i = 0; i < potionInventory.UnlockedInventorySlots; i++)
            {
                if(potionInventory.slots[i].IsOccupied)
                {
                    potionSlotsData += potionInventory.slots[i].inventorySlotID + ":";
                    potionSlotsData += potionInventory.slots[i].Equipment.equipmentID + ":";
                    potionSlotsData += potionInventory.slots[i].Amount.ToString() + ",";
                }
            }
            potionSlotsData = potionSlotsData.TrimEnd(',');
            saveData += potionSlotsData + "|";
            //Pouch Inventory
            Inventory pouchInventory = player.GetInventory(Common.InventoryType.POUCH);
            saveData += pouchInventory.InventoryLevel.ToString() + "|";
            //Last Save Location
            saveData += ((int)_currentGameScene.SceneName).ToString() + "|"; 
            //Last Save Time
            saveData += dateTime.ToString("MM/dd/yyyy h:mm tt") + "|"; 
            _lastSavedTimeText = dateTime.ToString("MM/dd/yyyy h:mm tt");
            //Equipped Head Armor
            InventorySlot headArmorSlot = armorInventory.slots.FirstOrDefault(x => x.IsOccupied && x.Equipment.equipmentType == Common.EquipmentType.HEAD_ARMOR && x.IsEquipped);
            if(headArmorSlot != null)
                saveData += headArmorSlot.inventorySlotID + "|"; 
            else
                saveData += "NotEquipped|";
            //Equipped Chest Armor
            InventorySlot chestArmorSlot = armorInventory.slots.FirstOrDefault(x => x.IsOccupied && x.Equipment.equipmentType == Common.EquipmentType.CHEST_ARMOR && x.IsEquipped);
            if(chestArmorSlot != null)
                saveData += chestArmorSlot.inventorySlotID + "|"; 
            else
                saveData += "NotEquipped|";
            //Equipped Boots Armor
            InventorySlot bootsArmorSlot = armorInventory.slots.FirstOrDefault(x => x.IsOccupied && x.Equipment.equipmentType == Common.EquipmentType.BOOTS_ARMOR && x.IsEquipped);
            if(bootsArmorSlot != null)
                saveData += bootsArmorSlot.inventorySlotID + "|"; 
            else
                saveData += "NotEquipped|";
            //Equipped Weapon
            InventorySlot weaponSlot = weaponInventory.slots.FirstOrDefault(x => x.IsOccupied && x.IsEquipped);
            if(weaponSlot != null)
                saveData += weaponSlot.inventorySlotID + "|"; 
            else
                saveData += "NotEquipped|";
            //Equipped Potions
            List<InventorySlot> potionSlots = potionInventory.slots.Where(x => x.IsOccupied && x.IsEquipped).ToList();
            if(potionSlots.Count == 0)
            {
                saveData += "NotEquipped";
            }
            else
            {
                string equippedPotionSlotsData = "";
                for (int i = 0; i < potionSlots.Count; i++)
                {
                    equippedPotionSlotsData += potionSlots[i].inventorySlotID + ":";
                    InventorySlot pouchSlotWithPotion = pouchInventory.slots.First(x => x.Equipment.equipmentID == potionSlots[i].Equipment.equipmentID);
                    equippedPotionSlotsData += pouchSlotWithPotion.Amount.ToString() + ",";
                }
                equippedPotionSlotsData = equippedPotionSlotsData.TrimEnd(',');
                saveData += equippedPotionSlotsData;
            }
            PlayerPrefs.SetString("P01SaveData", saveData);
            playerMenu.UpdateLastSaveTime();
            ShowNotification("Game saved successfully!", Color.black);
        }
    }

    public void TryReturnToMainMenu()
    {
        _isTryLoadMainMenu = true;
        ShowConfirmation("Return to main menu? All unsaved changes will be discarded");
    }

    public void ReturnToMainMenu()
    {
        HidePlayerMenu();
        GameScene mainScene = new GameScene();
        mainScene.SceneName = Common.SceneName.MAIN_SCENE;
        mainScene.SceneDisplayName = "Main Menu";
        LoadScene(mainScene, 0.5f); 
    }

    public void SetResetButtonVisibility()
    {
        if(!PlayerPrefs.HasKey("P01SaveData"))
            resetButton.gameObject.SetActive(false);
        else
            resetButton.gameObject.SetActive(true);
    }

    public void TryResetSave()
    {
        if(PlayerPrefs.HasKey("P01SaveData"))
        {
            _isTryResetGame = true;
            ShowConfirmation("Are you sure you want to reset your progress?");
        }
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteKey("P01SaveData");
        GameScene mainScene = new GameScene();
        mainScene.SceneName = Common.SceneName.MAIN_SCENE;
        mainScene.SceneDisplayName = "Main Menu";
        LoadScene(mainScene, 0.5f);
    }

    public void LoadGame()
    {
        if(!PlayerPrefs.HasKey("P01SaveData"))
        {
            _lastSavedTimeText = "-";
            GameScene introductoryScene = new GameScene();
            introductoryScene.SceneName = Common.SceneName.INTRODUCTORY;
            introductoryScene.SceneDisplayName = "";
            LoadScene(introductoryScene);
        }
        else
        {
            /*
            Save with '|' as delimeter 
            Order of saving
            1. Player Name
            2. Player Gender
            3. Experience
            4. Gold
            5. ArmorInventory ('&' as delimeter)
                a. Inventory Level
                b. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. EquipmentID
                    iii. Amount
            6. WeaponInventory ('&' as delimeter)
                a. Inventory Level
                b. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. EquipmentID
                    iii. Amount
            7. PotionInventory ('&' as delimeter)
                a. Inventory Level
                b. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. EquipmentID
                    iii. Amount
            8. PouchInventory Inventory Level
            9. Last Save Location (Scene Name int)
            10. Last Save Time
            11. Equipped Head Armor Slot ID
            12. Equipped Chest Armor Slot ID
            13. Equipped Boots Armor Slot ID
            14. Equipped Weapon Slot ID
            15. Equipped Potions
                a. Slots (',' as delimeter between slots, ':' as delimeter between members )
                    i. SlotID
                    ii. Amount
            */
            string[] saveData = PlayerPrefs.GetString("P01SaveData").Split('|');
            //Name
            player.Name = saveData[0];
            //Gender
            player.Gender = (Common.PlayerGender)(int.Parse(saveData[1]));
            player.SetPlayerSprite();
            UpdateGuideName(player.Gender);
            //Experience
            player.Experience = int.Parse(saveData[2]);
            player.InitializeLevelFromLoadGame();
            //Gold
            player.Gold = int.Parse(saveData[3]);
            InitializePlayerMenu();
            //Armor Inventory
            //0 => inventory level
            //1 => armor slots data
            string[] armorInventoryData = saveData[4].Split('&');
            player.InitializeInventory(Common.InventoryType.ARMOR, int.Parse(armorInventoryData[0]));
            Inventory armorInventory = player.GetInventory(Common.InventoryType.ARMOR);
            //i index => slot that has equipment 
            string[] armorSlotsData = armorInventoryData[1].Split(',');
            if(armorSlotsData.Length != 0 && armorSlotsData[0] != "")
            {
                for (int i = 0; i < armorSlotsData.Length; i++)
                {
                    //0 => slot ID
                    //1 => equipment ID
                    //2 => amount
                    string[] slotInfo = armorSlotsData[i].Split(':'); 
                    Equipment equipment = equipmentManager.GetEquipmentByID(slotInfo[1]); 
                    armorInventory.InitalizeEquipmentAtInventorySlot(slotInfo[0], equipment, int.Parse(slotInfo[2]));
                }
            }
            //Weapon Inventory
            //0 => inventory level
            //1 => weapon slots data
            string[] weaponInventoryData = saveData[5].Split('&');
            player.InitializeInventory(Common.InventoryType.WEAPON, int.Parse(weaponInventoryData[0]));
            Inventory weaponInventory = player.GetInventory(Common.InventoryType.WEAPON);
            //i index => slot that has equipment 
            string[] weaponSlotsData = weaponInventoryData[1].Split(',');
            if(weaponSlotsData.Length != 0 && weaponSlotsData[0] != "")
            {
                for (int i = 0; i < weaponSlotsData.Length; i++)
                {
                    //0 => slot ID
                    //1 => equipment ID
                    //2 => amount
                    string[] slotInfo = weaponSlotsData[i].Split(':'); 
                    Equipment equipment = equipmentManager.GetEquipmentByID(slotInfo[1]); 
                    weaponInventory.InitalizeEquipmentAtInventorySlot(slotInfo[0], equipment, int.Parse(slotInfo[2]));
                }
            }
            //Potion Inventory
            //0 => inventory level
            //1 => weapon slots data
            string[] potionInventoryData = saveData[6].Split('&');
            player.InitializeInventory(Common.InventoryType.POTION, int.Parse(potionInventoryData[0]));
            Inventory potionInventory = player.GetInventory(Common.InventoryType.POTION);
            //i index => slot that has equipment 
            string[] potionSlotsData = potionInventoryData[1].Split(',');
            if(potionSlotsData.Length != 0 && potionSlotsData[0] != "")
            {
                for (int i = 0; i < potionSlotsData.Length; i++)
                {
                    //0 => slot ID
                    //1 => equipment ID
                    //2 => amount
                    string[] slotInfo = potionSlotsData[i].Split(':'); 
                    Equipment equipment = equipmentManager.GetEquipmentByID(slotInfo[1]); 
                    potionInventory.InitalizeEquipmentAtInventorySlot(slotInfo[0], equipment, int.Parse(slotInfo[2]));
                }
            }
            player.InitializeInventory(Common.InventoryType.POUCH, int.Parse(potionInventoryData[0]));
            //Pouch Inventory
            Inventory pouchInventory = player.GetInventory(Common.InventoryType.POUCH);
            player.InitializeInventory(Common.InventoryType.POUCH, int.Parse(saveData[7]));
            InitializeHUD();
            //Last Save Location
            GameScene lastSavedScene = new GameScene();
            lastSavedScene.SceneName = (Common.SceneName)(int.Parse(saveData[8]));
            switch(lastSavedScene.SceneName)
            {
                case Common.SceneName.DUNGEON_CENTRAL_HUB:
                {
                    lastSavedScene.SceneDisplayName = "Dungeon Central Hub";
                    break;
                }
                case Common.SceneName.ENCHANTED_FOREST_CENTRAL_HUB:
                {
                    lastSavedScene.SceneDisplayName = "Enchanted Forest Central Hub";
                    break;
                }
                case Common.SceneName.FANTASY_CENTRAL_HUB:
                {
                    lastSavedScene.SceneDisplayName = "Fantasy Central Hub";
                    break;
                }
                default:
                {
                    lastSavedScene.SceneDisplayName = "";
                    break;
                }
            }
            //Last Save Time
            _lastSavedTimeText = saveData[9];
            //Equipped Head Armor
            if(saveData[10] != "NotEquipped")
            {
                armorInventory.slots.First(x => x.inventorySlotID == saveData[10]).TryInteract();
            }
            //Equipped Chest Armor
            if(saveData[11] != "NotEquipped")
            {
                armorInventory.slots.First(x => x.inventorySlotID == saveData[11]).TryInteract();
            }
            //Equipped Boots Armor
            if(saveData[12] != "NotEquipped")
            {
                armorInventory.slots.First(x => x.inventorySlotID == saveData[12]).TryInteract();
            }
            //Equipped Weapon Armor
            if(saveData[13] != "NotEquipped")
            {
                weaponInventory.slots.First(x => x.inventorySlotID == saveData[13]).TryInteract();
            }
            //Equipped Potion
            string[] equippedPotionSlotsData = saveData[14].Split(',');
            if(equippedPotionSlotsData.Length != 0 && equippedPotionSlotsData[0] != "NotEquipped")
            {
                for (int i = 0; i < equippedPotionSlotsData.Length; i++)
                {
                    //0 => slot ID
                    //1 => amount
                    string[] info = equippedPotionSlotsData[i].Split(':'); 
                    InventorySlot slot = potionInventory.slots.First(x => x.inventorySlotID == info[0]);
                    slot.TryInteract();
                    Potion potion = slot.Equipment as Potion;
                    int availableInPotionInventory = slot.Amount;
                    int equippedAmount = int.Parse(info[1]);
                    int maxAllowed = potion.maxNumberInPouch;
                    int amountToReduce = 0;
                    if(equippedAmount < availableInPotionInventory)
                    {
                        if(maxAllowed >= availableInPotionInventory)
                            amountToReduce = availableInPotionInventory - equippedAmount;
                        else
                            amountToReduce = maxAllowed - equippedAmount;
 
                    }
                    InventorySlot pouchInventorySlot = pouchInventory.slots.First(x => x.Equipment.equipmentID == potion.equipmentID);
                    pouchInventorySlot.UpdatePotionsAmount(amountToReduce);
                }
            }
            InitializeShops();
            LoadScene(lastSavedScene);
        }
    }

    //For first start, on scene loaded will be loaded before start
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isBlockGameActions = false;
        ResetNPCManager();
        SpawnPlayer();
        player.healthPoints = player.maxHealthPoints;

        if(_currentGameScene == null)
            return;

        //If it is not main menu scene, hide the main menu display
        if(_currentGameScene.SceneName != Common.SceneName.MAIN_SCENE)
        {
            HideMainMenuDisplay();
            //If it is also not introductory (means all other scenes, reset and show the hud)
            if(_currentGameScene.SceneName != Common.SceneName.INTRODUCTORY)
            {
                UpdateHUDHealthPoints();
                UpdateHUDExperience();
                UpdateHUDStatusInfo();
                ShowHUD();
            }
        }
        //If it is main menu scene, hide the hud and show menu display
        else
        {
            SetResetButtonVisibility();
            ShowMainMenuDisplay();
            HideHUD();
        }

        if(PlayerPrefs.HasKey("P01SaveData"))
            return;

        //if it is the first time entering central hub after completing tutorial, save the game
        if(_currentGameScene.SceneName == Common.SceneName.DUNGEON_CENTRAL_HUB)
        {
            SaveGame();
            Invoke("UnblockGame", 2.0f);
        }
    }

    private void UnblockGame()
    {
        _isBlockGameActions = false;
    }

    public bool CheckIsAtCentralHub()
    {
        if(_currentGameScene.SceneName == Common.SceneName.DUNGEON_CENTRAL_HUB || _currentGameScene.SceneName == Common.SceneName.ENCHANTED_FOREST_CENTRAL_HUB ||_currentGameScene.SceneName == Common.SceneName.FANTASY_CENTRAL_HUB)
            return true;
        
        return false;
    }
    #endregion

    #region Custom Cursor
    public void ChangeCursor(bool isHover)
    {
        if(!isHover)
            Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(hoverCursorTexture, Vector2.zero, CursorMode.Auto);
    }
    #endregion
}
