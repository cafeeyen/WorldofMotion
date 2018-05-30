using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject nextBtt, prevBtt, helpSection;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible
    public Animator lessonAni;
    public Text pageNumber;

    private int page = 0, lesson = 0;

    private void OnEnable()
    {
        if(PlayerPrefs.GetInt("LessonTask") != 0)
            openLesson();
    }

    public void openEditorHelp()
    {
        helpSection.SetActive(true);
    }

    public void openLesson() //openHelp
    {
        helpSection.SetActive(true);
        lesson = PlayerPrefs.GetInt("LessonTask");
        nextBtt.SetActive(true);
        prevBtt.SetActive(true);

        page = 1;
        lessonAni.SetInteger("Subject", lesson);
        lessonAni.SetInteger("Page", 1);

        if(lesson == 1)
        {
            nextBtt.SetActive(false);
            prevBtt.SetActive(false);
        }
    }

    public void closeEditorHelp() //closeHelp
    {
        helpSection.SetActive(false);
    }

    public void changePage(bool nextPage)
    {
        if (lesson == 2 || lesson == 4) //friction | momentum
            page = nextPage ? (page + 1 > 3 ? 1 : page + 1) : (page - 1 < 1 ? 3 : page - 1);
        else if (lesson == 3) //motion
            page = nextPage ? (page + 1 > 7 ? 1 : page + 1) : (page - 1 < 1 ? 7 : page - 1);
        changeImage();
    }

    private void changeImage()
    {
        pageNumber.text = page.ToString();
        lessonAni.SetInteger("Page", page);
    }
}
