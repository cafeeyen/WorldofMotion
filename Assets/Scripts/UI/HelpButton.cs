using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public GameObject overlay, helpSection;

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
}
