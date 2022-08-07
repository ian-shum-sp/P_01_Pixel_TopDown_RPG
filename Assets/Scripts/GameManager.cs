using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    #region Scene Management
    private GameScene _currentGameScene;
    public GameScene CurrentGameScene
    {
        get { return _currentGameScene; }
        set { _currentGameScene = value; }
    }
    #endregion

    #region Main Menu 
    public Animator mainMenuAnimator;
    #endregion

    #region References
    public FloatingTextManager floatingTextManager;
    public DialogManager dialogManager;
    public ConfirmationManager confirmationManager;
    public WarningManager warningManager;
    public LoadingScreenManager loadingScreenManager;
    public ExperienceManager experienceManager;
    public EquipmentManager equipmentManager;
    public NPCManager nPCManager;
    public HUD hUD;
    public PlayerMenu playerMenu;
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
            Destroy(nPCManager.gameObject);
            Destroy(hUD.gameObject);
            Destroy(playerMenu.gameObject);
            Destroy(player.gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() 
    {
        ShowMainMenu();
        ChangeCursor(false);
    }

    #region Floating Text Manager
    public void ShowFloatingText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float showDuration)
    {
        floatingTextManager.Show(message, fontSize, color, position, motion, showDuration);
    }
    #endregion

    #region Dialog Manager
    public void ShowFullDialog(Common.NPCType nPCName, Color? color = null)
    {
        _isBlockGameActions = true;
        dialogManager.ShowFullDialog(nPCName, color);
    }

    public void ShowRunningDialog(Common.NPCType nPCName, Color? color = null)
    {
        _isBlockGameActions = true;
        dialogManager.ShowRunningDialog(nPCName, color);
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

    #region Warning Manager
    public void ShowWarning(string text, bool isContinueBlockGameAction = true)
    {
        _isBlockGameActions = true;
        warningManager.Show(text, isContinueBlockGameAction);
    }
    #endregion

    #region Loading Screen Manager
    public void LoadScene(GameScene scene, float delay = 0.0f)
    {
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

    #region NPC Manager
    public void UpdateNPCManager(NPC nPC)
    {
        nPCManager.AddNPC(nPC);
    }

    public string GetNPCName(Common.NPCType nPCType)
    {
        return nPCManager.GetNPCName(nPCType);
    }

    public string[] GetNPCDialogs(Common.NPCType nPCType)
    {
        return nPCManager.GetNPCDialogs(nPCType);
    }

    public void UpdateGuideName(Common.PlayerGender playerGender)
    {
        nPCManager.UpdateGuide(playerGender);
    }
    #endregion

    #region HUD
    public void ShowHUD()
    {
        hUD.InitializeHUD();
        hUD.Show();
    }

    public void UpdateHUDHealthPoints()
    {
        hUD.UpdateHealthPoints();
    }   

    public void UpdateHUDExperience()
    {
        hUD.UpdateExperience();
    } 

    public void UpdateStatusInfo()
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
        playerMenu.popUpAnimator.SetTrigger("Hide");
        playerMenu.IsPopUpShowing = false;
    }
    #endregion


    #region Save, Load, Reset Game
    private void SpawnPlayer()
    {
        GameObject spawnPoint = GameObject.Find("SpawnPoint");

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
            ShowWarning("Please complete the tutorial before saving!");
        }
        else
        {
            /*
            Save with '|' as delimeter 
            Order of saving
            1. Player Name
            2. Experience
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
    }

    public void ShowMainMenu()
    {
        GameScene mainScene = new GameScene();
        mainScene.SceneName = Common.SceneName.MAIN_SCENE;
        mainScene.SceneDisplayName = "";
        _currentGameScene = mainScene;
        _isBlockGameActions = true;
        mainMenuAnimator.SetTrigger("Show");
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
        mainScene.SceneDisplayName = "";
        LoadScene(mainScene, 0.5f);
        Invoke("ShowMainMenu", 0.5f); 
    }

    public void LoadGame()
    {
        _isBlockGameActions = false;
        if(!PlayerPrefs.HasKey("P01SaveData"))
        {
            _lastSavedTimeText = "-";
            GameScene introductoryScene = new GameScene();
            introductoryScene.SceneName = Common.SceneName.INTRODUCTORY;
            introductoryScene.SceneDisplayName = "";
            _currentGameScene = introductoryScene;
            LoadScene(introductoryScene);
        }
        else
        {
            string[] saveData = PlayerPrefs.GetString("P01SaveData").Split('|');
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayer();
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
