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
    private int i = 0;
    private int j = 0;//check if it is nextpage or backpage

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

    public void nextImage()
    {
        if (i + 1 < gallery.Length)
        {
            i++;
            j = 0;
        }
    }

    public void PrevImage()
    {
        if (i - 1 >= 0)
        {
            i--;
            j = 1;
        }
    }

    void Update()
    {
        displayImage.sprite = gallery[i];
        if (i  >= 1 && j==0)
        {
            dot[i].sprite = colorDot ;
            dot[i - 1].sprite = blackDot;
        }
        else if (i >= 0 && j==1)
        {
            dot[i].sprite = colorDot;
            dot[i + 1].sprite = blackDot;
        }
        else
        {
            dot[i].sprite = colorDot;
        }
    }
}
