using System;
using System.Collections;
using UnityEngine;

public class FadeBetweenDay : MonoBehaviour
{
    public static FadeBetweenDay Instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private GameObject FadeObj;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasGroup.alpha = 0f;
    }

    public IEnumerator FadeTransition(System.Action onBlack = null)
    {
        Debug.Log("Fade started");
        yield return StartCoroutine(Fade(0f, 1f));

        Debug.Log("Screen should be black now");
        onBlack?.Invoke();

        yield return StartCoroutine(Fade(1f, 0f));
        Debug.Log("Fade done");
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    public void SpawnFadeObj()
    {
        Instantiate(FadeObj, this.transform);
    }

}
