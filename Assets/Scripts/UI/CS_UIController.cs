using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_UIController : MonoBehaviour
{
    public GameObject bigImage, overlay;
    public RenderTexture sideCamRT, cutCamRT;
    public RawImage rawImage, miniRawImage;
    public Animator calTab;
    public SceneLoader sceneLoader;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("CannonShooterMode") == 4)
        {
            sideCamRT.width = (int)rawImage.rectTransform.rect.width;
            sideCamRT.height = (int)rawImage.rectTransform.rect.height;
            cutCamRT.width = sideCamRT.width;
            cutCamRT.height = sideCamRT.height;
        }
    }

    public void clickMini()
    {
        if (miniRawImage.enabled)
        {
            bigImage.SetActive(true);
            overlay.SetActive(true);
        }
    }

    public void clickImg()
    {
        bigImage.SetActive(false);
        overlay.SetActive(false);
    }

    public void backToMainMenu()
    {
        PlayerPrefs.SetInt("CannonShooterMode", 0);
        sceneLoader.loadNewScene(0);
    }

    public void displayCalTab()
    {
        calTab.SetBool("IsDisplayed", !calTab.GetBool("IsDisplayed"));
    }

    public void viewToCutCam()
    {
        miniRawImage.texture = cutCamRT;
        rawImage.texture = cutCamRT;
    }

    public void viewToSideCam()
    {
        miniRawImage.texture = sideCamRT;
        rawImage.texture = sideCamRT;
    }
}
