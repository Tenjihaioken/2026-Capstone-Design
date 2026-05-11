using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance;

    [Header("페이드 이미지")]
    public Image fadeImage;

    [Header("페이드 속도")]
    public float fadeDuration = 0.8f;

    private bool isFading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        if (isFading)
            return;

        StartCoroutine(FadeOutLoad(sceneName));
    }

    private IEnumerator FadeOutLoad(string sceneName)
    {
        isFading = true;

        yield return StartCoroutine(Fade(0f, 1f));

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);

        yield return null;

        yield return StartCoroutine(Fade(1f, 0f));

        isFading = false;
    }

    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;

        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        fadeImage.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        if (endAlpha <= 0f)
            fadeImage.gameObject.SetActive(false);
    }
}