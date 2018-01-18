using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, helpSection, maewnam, cannon,cannonball,sparkle;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible
    private int page = 0, oldPage = 0;

    private void OnEnable()
    {
        // Check if this open for first time
        if (PlayerPrefs.GetInt("CannonShooterARFirstTime") == 0)
        {
            /* Tutorial */
            openHelp();
            PlayerPrefs.SetInt("CannonShooterARFirstTime", 1);
        }
    }

    public void openHelp()
    {
        overlay.SetActive(true);
        helpSection.SetActive(true);
        maewnam.SetActive(false);
        cannon.SetActive(false);
        cannonball.SetActive(false);
        sparkle.SetActive(false);
    }

    public void closeHelp()
    {
        overlay.SetActive(false);
        helpSection.SetActive(false);
        maewnam.SetActive(true);
        cannon.SetActive(true);
        cannonball.SetActive(true);
        sparkle.SetActive(true);
    }

    public void changePage(bool nextPage)
    {
        oldPage = page;
        page = nextPage ? (page + 1) % gallery.Length : page - 1 < 0 ? gallery.Length - 1 : page - 1;
        changeImage();
    }
    
    private  void changeImage()
    {
        displayImage.sprite = gallery[page];
        dot[oldPage].sprite = blackDot;
        dot[page].sprite = colorDot;  
    }
}
