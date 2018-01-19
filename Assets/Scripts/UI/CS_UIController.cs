﻿using UnityEngine;
using UnityEngine.UI;

public class CS_UIController : MonoBehaviour
{
    public GameObject bigImage, overlay, ruleOverlay, canyon, ground, rule, cannon, maewnam, cannonball, sparkle;
    public RenderTexture sideCamRT, cutCamRT;
    public RawImage rawImage, miniRawImage;
    public Animator calTab;
    public SceneLoader sceneLoader;

    private void OnEnable()
    {
        switch(PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                canyon.SetActive(false);
                ground.SetActive(true);

                if(!overlay.activeSelf && PlayerPrefs.GetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star") < 3)
                    openRule();
                break;                
            case 5:
                sideCamRT.width = (int)rawImage.rectTransform.rect.width;
                sideCamRT.height = (int)rawImage.rectTransform.rect.height;
                cutCamRT.width = sideCamRT.width;
                cutCamRT.height = sideCamRT.height;
                break;
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

    public void openRule()
    {
        rule.SetActive(true);
        ruleOverlay.SetActive(true);

        cannon.SetActive(false);
        maewnam.SetActive(false);
        cannonball.SetActive(false);
        sparkle.SetActive(false);
    }

    public void closeRule()
    {
        rule.SetActive(false);
        ruleOverlay.SetActive(false);

        if (!overlay.activeSelf)
        {
            cannon.SetActive(true);
            maewnam.SetActive(true);
            cannonball.SetActive(true);
            sparkle.SetActive(true);
        }
    }

    public void backToSelectLv()
    {
        PlayerPrefs.SetInt("CannonShooterMode", 0);
        PlayerPrefs.SetInt("CSLvSelect", 1);
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
