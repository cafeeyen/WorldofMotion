using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, helpSection, maewnam, cannon, cannonball, sparkle, dotLV1, dotLV2, dotLV3, table;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible
    private int page, oldPage;

    private void OnEnable()
    {
        // Check if this open for first time
        if (PlayerPrefs.GetInt("CannonShooter" + PlayerPrefs.GetInt("CannonShooterMode") + "FirstTime") == 0)
        {
            /* Tutorial */
            if (PlayerPrefs.GetInt("CannonShooterMode") == 1)
            {
                page = 0;
                oldPage = 0;
                displayImage.sprite = gallery[page];
            }
            else if (PlayerPrefs.GetInt("CannonShooterMode") == 2)
            {
                page = 12;
                oldPage = 12;
                displayImage.sprite = gallery[page];
            }
            else if (PlayerPrefs.GetInt("CannonShooterMode") == 3)
            {
                page = 16;
                oldPage = 16;
                displayImage.sprite = gallery[page];
            }

            openCSHelp();
            PlayerPrefs.SetInt("CannonShooter" + PlayerPrefs.GetInt("CannonShooterMode") + "FirstTime", 1);
        }
    }

    public void openCSHelp()
    {
        overlay.SetActive(true);
        helpSection.SetActive(true);
        switch (PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                dotLV1.SetActive(true);
                break;
            case 2:
                dotLV2.SetActive(true);
                break;
            case 3:
                dotLV3.SetActive(true);
                break;
            case 4:
                break;
        }

        if (PlayerPrefs.GetInt("CannonShooterMode") != 4)
            setElement(false);
    }

    public void closeCSHelp()
    {
        overlay.SetActive(false);
        helpSection.SetActive(false);
        switch (PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                dotLV1.SetActive(false);
                break;
            case 2:
                dotLV2.SetActive(false);
                break;
            case 3:
                dotLV3.SetActive(false);
                break;
            case 4:
                break;
        }

        if (PlayerPrefs.GetInt("CannonShooterMode") != 4)
            setElement(true);
    }

    public void openTable()
    {
        table.SetActive(true);
        switch (PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                dotLV1.SetActive(false);
                break;
            case 2:
                dotLV2.SetActive(false);
                break;
            case 3:
                dotLV3.SetActive(false);
                break;
            case 4:
                break;
        }
    }

    public void exitTable()
    {
        table.SetActive(false);
        switch (PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                dotLV1.SetActive(true);
                break;
            case 2:
                dotLV2.SetActive(true);
                break;
            case 3:
                dotLV3.SetActive(true);
                break;
            case 4:
                break;
        }
    }

    public void changePage(bool nextPage)
    {
        oldPage = page;

        if (PlayerPrefs.GetInt("CannonShooterMode") == 1)
            page = nextPage ? (page + 1) % 12 : page - 1 < 0 ? 11 : page - 1;
        else if (PlayerPrefs.GetInt("CannonShooterMode") == 2)
            page = nextPage ? (page + 1 > 15 ? 12 : page + 1) : (page - 1 < 12 ? 15 : page - 1);
        else if (PlayerPrefs.GetInt("CannonShooterMode") == 3)
            page = nextPage ? (page + 1 > 20 ? 16 : page + 1) : (page - 1 < 16 ? 20 : page - 1);
        else
            page = nextPage ? (page + 1) % 8 : page - 1 < 0 ? 7 : page - 1;
        changeImage();
    }
    
    private void changeImage()
    {
        displayImage.sprite = gallery[page];
        dot[oldPage].sprite = blackDot;
        dot[page].sprite = colorDot;  
    }

    private void setElement(bool active)
    {
        cannon.SetActive(active);
        maewnam.SetActive(active);
        cannonball.SetActive(active);
        sparkle.SetActive(active);
    }
}
