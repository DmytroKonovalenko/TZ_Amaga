using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Loading UI")]
    [SerializeField] private CanvasGroup loadingPanel;
    [SerializeField] private Image loadingBar;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        Instance = this; 

        if (loadingPanel != null)
            loadingPanel.alpha = 0;
        if (loadingBar != null)
            loadingBar.fillAmount = 0;
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.gameObject.SetActive(true);
        yield return loadingPanel.DOFade(1f, fadeDuration).WaitForCompletion();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progress;

            if (progress >= 1f)
                operation.allowSceneActivation = true;

            yield return null;
        }
    }
}
