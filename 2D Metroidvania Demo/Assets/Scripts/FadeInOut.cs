using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeInOut : MonoBehaviour
{
    public FadeInOut instance;
    private CanvasGroup canvasGroup;
    private Image canvasImage;

    bool fadeIn;
    bool fadeOut;

    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        instance = this;
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }
    private void Start()
    {
        // Always fade out at the start
        StartFadeOut();
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (!canvasGroup.gameObject.activeInHierarchy)  
                canvasGroup.gameObject.SetActive(true);

            if (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime / fadeDuration;
            }
            else
            {
                canvasGroup.alpha = 1f;
                fadeIn = false;
            }
        }
        else if (fadeOut)
        {
            if (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            }
            else
            {
                canvasGroup.alpha = 0f;
                fadeOut = false;
                canvasGroup.gameObject.SetActive(false);
            }
        }
    }

    public void StartFadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }
    public void StartFadeOut()
    {
        fadeOut = true;
        fadeIn = false;
    }

    public float GetFadeDuration()
    {
        return fadeDuration;
    }
}
