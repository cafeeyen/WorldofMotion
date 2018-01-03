using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, helpSection;
    public Sprite[] gallery; //store all tutorial image
    public Image[] dot; //store dot that apear on screen
    public Sprite colorDot; //store color dot image
    public Sprite blackDot; //store black dot image
    public Image displayImage; //The current image thats visible
    private int page = 0, oldPage = 0;

    private void OnEnable()
    {
        // Check if this open for first time
        if (PlayerPrefs.GetInt("CannonShooterMode") == 0)
        {
            /* Tutorial */
            openHelp();
            PlayerPrefs.SetInt("CannonShooterMode", 1);
        }
    }

    public void openHelp()
    {
        overlay.SetActive(true);
        helpSection.SetActive(true);
    }

    public void closeHelp()
    {
        overlay.SetActive(false);
        helpSection.SetActive(false);
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
