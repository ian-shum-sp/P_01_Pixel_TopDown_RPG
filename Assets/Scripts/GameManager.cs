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
    private List<GameScene> _scenes = new List<GameScene>();
    #endregion

    #region Main Menu 
    public Animator mainMenuAnimator;
    #endregion

    #region Loading Screen
    private List<AsyncOperation> _sceneLoading = new List<AsyncOperation>();
    private float _totalProgress;
    private bool _isDoneStimulate;
    public Animator loadingScreenAnimator;
    public Image loadingScreenProgressBarMask;
    public TextMeshProUGUI loadingScreenInfoText;
    #endregion 

    #region References
    public FloatingTextManager floatingTextManager;
    public DialogManager dialogManager;
    public ConfirmationManager confirmationManager;
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

    #region Character Creation

    #endregion

    #region Game
    public Player player;
    public List<Sprite> playerSprites = new List<Sprite>();
    #endregion

    private void Awake() 
    {
        if(GameManager.Instance != null)
        {
            Destroy(gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(dialogManager.gameObject);
            Destroy(confirmationManager.gameObject);
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
    }

    #region Loading Screen
    public void LoadScene(GameScene scene, float delay = 0.0f)
    {
        StartCoroutine(LoadSceneDelay(scene, delay));
    }

    public IEnumerator LoadSceneDelay(GameScene scene, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameScene sceneToBeLoaded = _scenes.Find(x => x.SceneName == scene.SceneName);

        if(sceneToBeLoaded == null)
        {
            sceneToBeLoaded = scene;
            _scenes.Add(sceneToBeLoaded);
        }

        loadingScreenAnimator.SetTrigger("Show");
        _sceneLoading.Add(SceneManager.LoadSceneAsync(Enums.GetEnumDescription(sceneToBeLoaded.SceneName)));
        _totalProgress = 0;
        _isDoneStimulate = false;
        StartCoroutine(StimulateLoad());
        StartCoroutine(GetSceneLoadProgress(sceneToBeLoaded.SceneDisplayName));
    }

    private IEnumerator GetSceneLoadProgress(string sceneDisplayName)
    {
        for (int i = 0; i < _sceneLoading.Count; i++)
        {
            while(!_sceneLoading[i].isDone || !_isDoneStimulate)
            {
                float ratio = (float) _totalProgress / (float) 100;
                loadingScreenProgressBarMask.fillAmount = ratio;
                loadingScreenInfoText.text = string.Format("Loading {0} ({1}%)", sceneDisplayName, ratio * 100.0f);

                yield return null;
            }
        }

        loadingScreenAnimator.SetTrigger("Hide");
        _totalProgress = 0;
        _isDoneStimulate = false;
    }

    private IEnumerator StimulateLoad()
    {
        while(_totalProgress < 100)
        {
            yield return new WaitForEndOfFrame();
            _totalProgress++;
        }
        yield return new WaitForSeconds(0.5f);
        _isDoneStimulate = true;
    }

    #endregion

    #region Floating Text Manager
    public void ShowFloatingText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float showDuration)
    {
        floatingTextManager.Show(message, fontSize, color, position, motion, showDuration);
    }
    #endregion

    #region Dialog Manager
    public void ShowDialog(Enums.NPCName nPCName, Color? color = null)
    {
        player.IsActive = false;
        dialogManager.Show(nPCName, color);
    }
    #endregion

    #region Confirmation Manager
    public void ShowConfirmation(string text)
    {
        player.IsActive = false;
        confirmationManager.Show(text);
    }
    #endregion

    #region Save, Load, Reset Game
    private void SpawnPlayer()
    {
        GameObject _spawnPoint = GameObject.Find("SpawnPoint");

        if(_spawnPoint != null)
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

    public void ShowMainMenu()
    {
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
        GameScene _mainScene = new GameScene();
        _mainScene.SceneName = Enums.SceneName.MAIN_SCENE;
        _mainScene.SceneDisplayName = "";
        LoadScene(_mainScene, 0.5f);
    }

    public void LoadGame()
    {
        if(!PlayerPrefs.HasKey("P01SaveData"))
        {
            GameScene _introductoryScene = new GameScene();
            _introductoryScene.SceneName = Enums.SceneName.INTRODUCTORY;
            _introductoryScene.SceneDisplayName = "";
            LoadScene(_introductoryScene);
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
}
