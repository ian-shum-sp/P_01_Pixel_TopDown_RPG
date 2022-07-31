using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    private List<GameScene> _scenes = new List<GameScene>();
    private List<AsyncOperation> _sceneLoading = new List<AsyncOperation>();
    private float _totalProgress;
    private bool _isDoneStimulate;
    public Animator loadingScreenAnimator;
    public Image loadingScreenProgressBarMask;
    public TextMeshProUGUI loadingScreenInfoText;

    private IEnumerator GetSceneLoadProgress(string sceneDisplayName)
    {
        for (int i = 0; i < _sceneLoading.Count; i++)
        {
            while(!_sceneLoading[i].isDone || !_isDoneStimulate)
            {
                float ratio = (float)_totalProgress / (float)100.0f;
                loadingScreenProgressBarMask.fillAmount = ratio;
                loadingScreenInfoText.text = string.Format("Loading {0} ({1}%)", sceneDisplayName, ratio * 100.0f);

                yield return null;
            }
        }

        loadingScreenAnimator.SetTrigger("Hide");
        _totalProgress = 0.0f;
        _isDoneStimulate = false;
    }

    private IEnumerator StimulateLoad()
    {
        while(_totalProgress < 100.0f)
        {
            yield return new WaitForEndOfFrame();
            _totalProgress++;
        }
        yield return new WaitForSeconds(0.5f);
        _isDoneStimulate = true;
    }

    private IEnumerator LoadSceneDelay(GameScene scene, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameScene sceneToBeLoaded = _scenes.FirstOrDefault(x => x.SceneName == scene.SceneName);

        if(sceneToBeLoaded == null)
        {
            sceneToBeLoaded = scene;
            _scenes.Add(sceneToBeLoaded);
        }

        loadingScreenAnimator.SetTrigger("Show");
        _sceneLoading.Add(SceneManager.LoadSceneAsync(Common.GetEnumDescription(sceneToBeLoaded.SceneName)));
        _totalProgress = 0.0f;
        _isDoneStimulate = false;
        StartCoroutine(StimulateLoad());
        StartCoroutine(GetSceneLoadProgress(sceneToBeLoaded.SceneDisplayName));
    }

    public void LoadScene(GameScene scene, float delay = 0.0f)
    {
        StartCoroutine(LoadSceneDelay(scene, delay));
    }
}
