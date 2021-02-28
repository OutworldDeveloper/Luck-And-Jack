using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class MapLoader : MonoBehaviour
{

    public static MapLoader Instance { get; private set; }

    private const float delay = 0.2f;

    [SerializeField] private float fadeInSpeed = 3f;
    [SerializeField] private float fadeOutSpeed = 3f;

    private CanvasGroup group;
    private bool isLoading;

    public void LoadMenu() => LoadScene(0);
    public void LoadTutorial() => LoadScene(1);
    public void LoadSurvival() => LoadScene(2);
    public void ReloadCurrentScene() => LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        group = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (isLoading)
        {
            if (group.alpha < 1f)
                group.alpha += Time.unscaledDeltaTime * fadeInSpeed;
            else
                group.alpha = 1f;
        }
        else
        {
            if (group.alpha > 0f)
                group.alpha -= Time.unscaledDeltaTime * fadeOutSpeed;
            else
                group.alpha = 0f;
        }
    }

    private void LoadScene(int sceneIndex)
    {
        if (isLoading)
            return;
        StartCoroutine(SceneTransition(sceneIndex));
    }

    private IEnumerator SceneTransition(int sceneIndex)
    {
        isLoading = true;
        group.blocksRaycasts = true;

        yield return new WaitForSecondsRealtime(1f / fadeInSpeed);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return new WaitUntil(() => operation.isDone);

        isLoading = false;

        yield return new WaitForSecondsRealtime(1f / fadeOutSpeed);

        group.blocksRaycasts = false;
    }

}