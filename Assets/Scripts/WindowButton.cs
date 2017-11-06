using UnityEngine;
using TouchScript.Gestures;

public class WindowButton : MonoBehaviour
{
    public Animator anim;
    public UIController uiController;
    private TapGesture gesture;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        if (anim.GetBool("IsDisplayed") == true)
            anim.SetBool("IsDisplayed", false);
        else
        {
            anim.SetBool("IsDisplayed", true);
            uiController.changeState(anim);
        }
    }
}
