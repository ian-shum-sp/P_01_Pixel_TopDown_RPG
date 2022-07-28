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
    #endregion

    #region Main Menu 
    public Animator mainMenuAnimator;
    #endregion

    #region References
    public FloatingTextManager floatingTextManager;
    public DialogManager dialogManager;
    public ConfirmationManager confirmationManager;
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
        nPCManager.InitializeNPCInfos();
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
    public void ShowFullDialog(Common.NPCName nPCName, Color? color = null)
    {
        _isBlockGameActions = true;
        dialogManager.ShowFullDialog(nPCName, color);
    }

    public void ShowRunningDialog(Common.NPCName nPCName, Color? color = null)
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
    #endregion

    #region Equipment Manager

    #endregion

    #region NPC Manager
    public string GetNPCName(Common.NPCName nPCName)
    {
        return nPCManager.GetNPCName(nPCName);
    }

    public List<string> GetNPCDialogs(Common.NPCName nPCName)
    {
        return nPCManager.GetNPCDialogs(nPCName);
    }

    public void UpdateGuideName(Common.PlayerGender playerGENDER)
    {
        nPCManager.UpdateGuideName(playerGENDER);
    }
    #endregion

    #region HUD
    public void ShowHUD()
    {
        hUD.InitializeHUD();
        hUD.Show();
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

    public void ShowMainMenu()
    {
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
    }

    public void LoadGame()
    {
        _isBlockGameActions = false;
        if(!PlayerPrefs.HasKey("P01SaveData"))
        {
            GameScene introductoryScene = new GameScene();
            introductoryScene.SceneName = Common.SceneName.INTRODUCTORY;
            introductoryScene.SceneDisplayName = "";
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
