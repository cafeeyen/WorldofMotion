using UnityEngine;
using TouchScript.Gestures;
using System;

public class UIController : Observer
{
    public Animator menu, item, prop;

    private GameObject ItemObject;
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
        string tapped = gesture.GetScreenPositionHitData().Target.name;
        if(tapped == "MenuButton" || tapped == "MenuArrow") displayWindows(menu);
        else if(tapped == "ItemButton" || tapped == "ItemArrow") displayWindows(item);
        else if(tapped == "PropButton" || tapped == "PropArrow") displayWindows(prop);
    }

    public void displayWindows(Animator anim)
    {
        if (anim.GetBool("IsDisplayed") == true)
            anim.SetBool("IsDisplayed", false);
        // PropBar can open when some ItemObject has selected
        else if(anim.name != "PropBar" || ItemObject != null)
        {
            anim.SetBool("IsDisplayed", true);
            changeState(anim);
        }
    }
	private void changeState(Animator anim)
    {
        // Can open one window at the same time
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

    public override void selectedItemObject(GameObject itemObject)
    {
        ItemObject = itemObject;
    }
}
