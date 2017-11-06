using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator menu, item, prop;
	
	public void changeState(Animator anim)
    {
        // Can open one window at the same time.
        if(anim == menu)
        {
            item.SetBool("IsDisplayed", false);
            prop.SetBool("IsDisplayed", false);
        }
        else if(anim == item)
        {
            menu.SetBool("IsDisplayed", false);
            prop.SetBool("IsDisplayed", false);
        }
        else // anim == prop
        {
            menu.SetBool("IsDisplayed", false);
            item.SetBool("IsDisplayed", false);
        }
    }
}
