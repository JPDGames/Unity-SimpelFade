/* =====================================================================>
 * 
 * Author:          Jan-Philip Duering
 * Version:         1.2
 * 
 * 
 * Description:
 * =====================================================================>
 * The FadeSystem generates a canvas with an image that can fade in
 * or fade out. The image can take any color or custom sprites.
 * if a scene was specified at fade out, the scene will load after that.
 * =====================================================================>
*/

// Includes
// =====================================================================>
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour
{
    public bool fadeInAtStart = true;

    [Header("Background")]
    public BackgroundType backgroundType;
    public Sprite backgroundImage;
    public Color backgroundColor;

    const int uiElementLayer = 5;
    Image image;

    void Start()
    {
        InitializeFade();

        if (fadeInAtStart)
        {
            FadeIn();
        }
    }

    void InitializeFade()
    {
        var emptyObject = gameObject;
        emptyObject.layer = uiElementLayer;

        var canvas = emptyObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var fadeSystem = gameObject;
        image = fadeSystem.AddComponent<Image>();

        if (backgroundType == BackgroundType.defaultBackground)
        {
            image.color = new Color(0, 0, 0, 0);
        }
        else if (backgroundType == BackgroundType.color)
        {
            image.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
        }
        else if (backgroundType == BackgroundType.image)
        {
            image.color = new Color(255, 255, 255, 0);
            image.sprite = backgroundImage;
        }
    }

    bool isFadeing = false;
    bool isFadeIn = false;
    float fadeValue = 0;
    float fadeTimer = 0.0f;
    string loadScene;

    public void FadeIn()
    {
        isFadeing = true;
        isFadeIn = true;
        fadeValue = 1;
        fadeTimer = 1;
    }

    public void FadeOut(string uLoadScene = "")
    {
        isFadeing = true;
        isFadeIn = false;
        fadeValue = 0;
        fadeTimer = 1;
        loadScene = uLoadScene;
    }

    private void Update()
    {
        if (isFadeing)
        {
            if (fadeTimer < 0)
            {
                fadeTimer -= Time.deltaTime;
            }
            else
            {
                if (isFadeIn)
                {
                    fadeValue -= 0.01f;
                }
                else
                {
                    fadeValue += 0.01f;
                }
            }

            if (backgroundType == BackgroundType.defaultBackground)
            {
                image.color = new Color(0, 0, 0, fadeValue);
            }
            else if (backgroundType == BackgroundType.color)
            {
                image.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, fadeValue);
            }
            else if (backgroundType == BackgroundType.image)
            {
                image.color = new Color(255, 255, 255, fadeValue);
            }

            if (fadeValue >= 1 && isFadeIn == false)
            {
                if (loadScene != "")
                {
                    SceneManager.LoadScene(loadScene);
                }
            }
        }
    }

    public enum BackgroundType
    {
        defaultBackground = 0,
        image = 1,
        color = 2
    }
}
