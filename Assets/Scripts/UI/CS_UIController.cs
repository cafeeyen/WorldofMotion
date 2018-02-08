using UnityEngine;
using UnityEngine.UI;

public class CS_UIController : MonoBehaviour
{
    public GameObject bigImage, overlay, tutorialAskPanel, canyon, ground, rule, cannon, maewnam, cannonball, sparkle, helpSection;
    public RenderTexture sideCamRT, cutCamRT;
    public GameObject[] RuleImage;
    public Button Rule;
    public RawImage rawImage, miniRawImage;
    public Animator calTab;
    public SceneLoader sceneLoader;
    public HelpButton helpBtt;

    private int countRule = 0;

    private bool helpChk = false;

    private void OnEnable()
    {
        switch(PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                canyon.SetActive(false);
                ground.SetActive(true);

                if (!overlay.activeSelf && PlayerPrefs.GetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star") < 1)
                {
                    if (PlayerPrefs.GetInt("CannonShooter1FirstTime") == 0)
                        helpChk = true;
                    helpSection.SetActive(false);
                    tutorialAskPanel.SetActive(true);
                }
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

    public void tutorialAsk(bool tuAns)
    {
        if(tuAns)
            openRule(false || helpChk);
        tutorialAskPanel.SetActive(false);
    }

    public void openRule(bool helpClk)
    {
        helpChk = helpClk;
        overlay.SetActive(true);
        rule.SetActive(true);
        RuleImage[countRule].SetActive(true);
        if(helpChk)
            helpSection.SetActive(false);
        setElement(false);
    }

    public void NextRule()
    {
        countRule++;
        if(countRule <= 7)
        {
            RuleImage[countRule].SetActive(true);
            RuleImage[countRule - 1].SetActive(false);
        }
        else if(countRule > 7)
        {
            RuleImage[countRule-1].SetActive(false);
            rule.SetActive(false);
            closeRule();
        }    
    }

    public void closeRule()
    {
        overlay.SetActive(false);
        countRule = 0;
        if (!overlay.activeSelf)
            setElement(true);
        if (helpChk)
            helpBtt.openCSHelp();
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

    private void setElement(bool active)
    {
        cannon.SetActive(active);
        maewnam.SetActive(active);
        cannonball.SetActive(active);
        sparkle.SetActive(active);
    }
}
