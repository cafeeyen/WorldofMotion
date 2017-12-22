using UnityEngine;
using UnityEngine.UI;

public class ShowBigImage : MonoBehaviour
{
    public GameObject rawImage, overlay;

    public void clickMini()
    {
        if(GetComponent<RawImage>().enabled)
        {
            rawImage.SetActive(true);
            overlay.SetActive(true);
        }
    }

    public void clickImg()
    {
        rawImage.SetActive(false);
        overlay.SetActive(false);
    }
}
