using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, dotLV1, dotLV2, dotLV3, table, nextBtt, prevBtt, helpSection, choice, powerbar;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible

    // CS Mode
    public GameObject maewnam, cannon, cannonball, sparkle;
    // ED Mode
    public GameObject lessonBtt;
    public Animator lessonAni;

    private int page = 0, oldPage = 0, lesson = 0;

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

    /************* Cannon Shooter Mode *************/

    public void openChoice()
    {
        choice.SetActive(true);
        setElement(false);
    }

    public void closeChoice()
    {
        choice.SetActive(false);
        setElement(true);
    }

    public void openCSHelp()
    {
        dot[page].sprite = blackDot;
        choice.SetActive(false);
        overlay.SetActive(true);
        helpSection.SetActive(true);
        switch (PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
                page = 0;
                dotLV1.SetActive(true);
                break;
            case 2:
                page = 12;
                dotLV2.SetActive(true);
                break;
            case 3:
                page = 16;
                dotLV3.SetActive(true);
                break;
        }
        changeImage();

        if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
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
        }
        dot[page].sprite = blackDot;

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

    private void setElement(bool active)
    {
        if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
        {
            cannon.SetActive(active);
            maewnam.SetActive(active);
            cannonball.SetActive(active);
            sparkle.SetActive(active);
            powerbar.SetActive(active);
        }
    }

    /************* Editor Mode *************/
    public void openEditorHelp()
    {
        helpSection.SetActive(true);
        if (PlayerPrefs.GetInt("Lesson") == 1)
            openLesson(5);
    }

    public void openLesson(int num) //openHelp
    {
        Debug.Log(num);
        lesson = num;
        overlay.SetActive(true);
        nextBtt.SetActive(true);
        prevBtt.SetActive(true);

        page = 1;
        oldPage = 1;
        lessonAni.SetInteger("Subject", num);
        lessonAni.SetInteger("Page", 1);
        lessonBtt.SetActive(false);

        switch (lesson)
        {
            case 1: //force
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                break;
            case 2: //friction
            case 3: //gravity
            case 4: //momentum
                dot[0].sprite = colorDot;
                dotLV1.SetActive(true);
                break;
            case 5: //motion
                dot[3].sprite = colorDot;
                dotLV2.SetActive(true);
                break;
        }
    }

    public void closeEditorHelp() //closeHelp
    {
        switch (lesson)
        {
            case 1: //force
                break;
            case 2: //friction
            case 3: //gravity
            case 4: //momentum
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                dot[page - 1].sprite = blackDot;
                dotLV1.SetActive(false);
                break;
            case 5: //motion
                nextBtt.SetActive(false);
                prevBtt.SetActive(false);
                dot[page + 2].sprite = blackDot;
                dotLV2.SetActive(false);
                break;
            default: // 0 | close help
                helpSection.SetActive(false);
                overlay.SetActive(false);
                break;
        }

        if (lesson != 0)
        {
            lessonAni.SetInteger("Subject", 0);
            lessonAni.SetInteger("Page", 0);
            lessonBtt.SetActive(true);
            lesson = 0;
        }

        // Lesson Tutorial
        if (PlayerPrefs.GetInt("Lesson") == 1)
        {
            helpSection.SetActive(false);
            overlay.SetActive(false);
        }
    }

    /************* All Mode *************/
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
        else if (lesson == 2 || lesson == 3 || lesson == 4) //friction | gravity | momentum
            page = nextPage ? (page + 1 > 3 ? 1 : page + 1) : (page - 1 < 1 ? 3 : page - 1);
        else if (lesson == 5) //motion
            page = nextPage ? (page + 1 > 4 ? 1 : page + 1) : (page - 1 < 1 ? 4 : page - 1);

        // CS Tutorial
        else
            page = nextPage ? (page + 1) % 8 : page - 1 < 0 ? 7 : page - 1;

        changeImage();
    }

    private void changeImage()
    {
        if (PlayerPrefs.GetInt("CannonShooterMode") == 0)
        {
            lessonAni.SetInteger("Page", page);
            if (lesson == 5)
            {
                // +3 -1 = +2
                dot[oldPage + 2].sprite = blackDot;
                dot[page + 2].sprite = colorDot;
            }
            else if (lesson != 0)
            {
                dot[oldPage - 1].sprite = blackDot;
                dot[page - 1].sprite = colorDot;
            }
        }
        else
        {
            dot[oldPage].sprite = blackDot;
            dot[page].sprite = colorDot;
            displayImage.sprite = gallery[page];
        }
    }
}
