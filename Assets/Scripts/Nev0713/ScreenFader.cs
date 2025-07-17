using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Unity.VisualScripting;


public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private static ScreenFader m_instance;
    private Coroutine m_currentRoutine;

    public static bool IsFading { get; private set; } = false;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void FadeIn(Action pOnComplete = null)
    {
        if (m_instance != null)
            m_instance.StartFade(0f, 1f, pOnComplete);
    }

    public static void FadeOut(Action pOnComplete = null)
    {
        if (m_instance != null)
            m_instance.StartFade(1f, 0f, pOnComplete);
    }

    private void StartFade(float pFrom, float pTo, Action pOnComplete)
    {
        if (m_currentRoutine != null)
            StopCoroutine(m_currentRoutine);

        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        m_currentRoutine = StartCoroutine(FadeCoroutine(pFrom, pTo, pOnComplete));
    }

    private IEnumerator FadeCoroutine(float pFrom, float pTo, Action pOnComplete)
    {
        IsFading = true;
        float timer = 0f;
        Color color = fadeImage.color;
        color.a = pFrom;
        fadeImage.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            color.a = Mathf.Lerp(pFrom, pTo, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = pTo;
        fadeImage.color = color;

        IsFading = false;
        pOnComplete?.Invoke();
    }
}
