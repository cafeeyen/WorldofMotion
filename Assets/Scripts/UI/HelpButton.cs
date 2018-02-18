using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, dotLV1, dotLV2, dotLV3, table, nextBtt, prevBtt , helpSection;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible

    // CS Mode
    public GameObject maewnam, cannon, cannonball, sparkle;
    // ED Mode
    public GameObject lessonBtt;

    private int page=0, oldPage=0, lesson = -1;

    private void OnEnable()
    {
        // Check if this open for first time
        if (PlayerPrefs.GetInt("CannonShooterMode") > 0 &&
            PlayerPrefs.GetInt("CannonShooter" + PlayerPrefs.GetInt("CannonShooterMode") + "FirstTime") == 0)
        {
            /* Cannon Shooter Tutorial */
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
        }

        if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
            setElement(false);
    }

    public void openEditorHelp()
    {
        helpSection.SetActive(true);
    }

    public void openLesson(int num) //openHelp
    {
        lesson = num;
        overlay.SetActive(true);
        nextBtt.SetActive(true);
        prevBtt.SetActive(true);
        dot[page].sprite = blackDot;
        switch (lesson)
        {
            case 0: //force
                page = 0;
                oldPage = 0;
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                break;
            case 1: //friction
                page = 1;
                oldPage = 1;
                dotLV1.SetActive(true);
                break;
            case 2: //gravity
                page = 4;
                oldPage = 4;
                dotLV1.SetActive(true);
                break;
            case 3: //momentum
                page = 7;
                oldPage = 7;
                dotLV1.SetActive(true);
                break;
            case 4: //motion
                page = 10;
                oldPage = 10;
                dotLV2.SetActive(true);
                break;
        }
        displayImage.sprite = gallery[page];
        dot[page].sprite = colorDot;
        lessonBtt.SetActive(false);
    }

    public void closeEditorHelp() //closeHelp
    {
        switch (lesson)
        {
            case 0: //force
                break;
            case 1: //friction
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                dotLV1.SetActive(false);
                break;
            case 2: //gravity
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                dotLV1.SetActive(false);
                break;
            case 3: //momentum
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                dotLV1.SetActive(false);
                break;
            case 4: //motion
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                dotLV2.SetActive(false);
                break;
            default: // -1 | close help
                helpSection.SetActive(false);
                overlay.SetActive(false);
                break;
        }
        if (lesson != -1)
        {
            displayImage.sprite = gallery[14];
            lessonBtt.SetActive(true);
        }
        lesson = -1;
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
        }
        if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
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
        }
    }

    public void changePage(bool nextPage)
    {
        oldPage = page;

        // CS Help
        if (PlayerPrefs.GetInt("CannonShooterMode") == 1)
            page = nextPage ? (page + 1) % 12 : page - 1 < 0 ? 11 : page - 1;
        else if (PlayerPrefs.GetInt("CannonShooterMode") == 2)
            page = nextPage ? (page + 1 > 15 ? 12 : page + 1) : (page - 1 < 12 ? 15 : page - 1);
        else if (PlayerPrefs.GetInt("CannonShooterMode") == 3)
            page = nextPage ? (page + 1 > 20 ? 16 : page + 1) : (page - 1 < 16 ? 20 : page - 1);

        // ED Help
        else if (lesson == 1) //friction
            page = nextPage ? (page + 1 > 3 ? 1 : page + 1) : (page - 1 < 1 ? 3 : page - 1);
        else if (lesson == 2) //gravity
            page = nextPage ? (page + 1 > 6 ? 4 : page + 1) : (page - 1 < 4 ? 6 : page - 1);
        else if (lesson == 3) //momentum
            page = nextPage ? (page + 1 > 9 ? 7 : page + 1) : (page - 1 < 7 ? 9 : page - 1);
        else if (lesson == 4) //motion
            page = nextPage ? (page + 1 > 13 ? 10 : page + 1) : (page - 1 < 10 ? 13 : page - 1);

        // CS Tutorial
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
