using UnityEngine;
using TouchScript.Gestures;

public class UIController : MonoBehaviour
{
    public Animator menu, item, prop;
    private TapGesture gesture;
    private GameObject itemObject;
    private ItemObjectController itemCon;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        itemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void Update() {}

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
        else if(anim.name != "PropBar" || itemObject != null)
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

    public void setItemObject(GameObject selectedItemObject)
    {
        itemObject = selectedItemObject;
    }

    public void createItemObject(GameObject prefeb)
    {
        // Can't create new one while current one is overlapping
        if(itemObject == null || !itemObject.GetComponent<ItemObject>().IsOverlap)
        {
            // Spawn at center of screen,  distance 80
            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + 80));
            Vector3 refinePosition = new Vector3(Mathf.Round(screenPosition.x / 10) * 10, Mathf.Round(screenPosition.y / 10) * 10, Mathf.Round(screenPosition.z / 10) * 10);
            GameObject newItemObject = (GameObject)Instantiate(prefeb, refinePosition, Quaternion.identity);

           itemCon.setItemObject(newItemObject);
        }
    }
}
