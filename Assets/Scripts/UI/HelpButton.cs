using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, dotLV1, dotLV2, dotLV3, nextBtt, prevBtt, helpSection;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible

    public GameObject lessonBtt;
    public Animator lessonAni;

    private int page = 0, oldPage = 0, lesson = 0;

    public void openEditorHelp()
    {
        helpSection.SetActive(true);
        if (PlayerPrefs.GetInt("Lesson") == 1)
            openLesson(5);
    }

    public void openLesson(int num) //openHelp
    {
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

    public void changePage(bool nextPage)
    {
        oldPage = page;

        if (lesson == 2 || lesson == 3 || lesson == 4) //friction | gravity | momentum
            page = nextPage ? (page + 1 > 3 ? 1 : page + 1) : (page - 1 < 1 ? 3 : page - 1);
        else if (lesson == 5) //motion
            page = nextPage ? (page + 1 > 4 ? 1 : page + 1) : (page - 1 < 1 ? 4 : page - 1);

        changeImage();
    }

    private void changeImage()
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
}
