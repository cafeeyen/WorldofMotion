using UnityEngine;
using TouchScript.Gestures;

public class UIController : MonoBehaviour
{
    public mode state;
    public enum mode
    {
        Play,
        Edit,
        Pause
    }

    public Animator menu, item, prop;
    public PropWindow propWindow;
    public GameObject deleteBtt;

    private TapGesture gesture;
    private GameObject itemObject, WorldObject, ExperimentWorld;
    private ItemObjectController itemCon;
    private WorldObject worldSc;

    private void OnEnable()
    {
        Time.timeScale = 0;
        state = mode.Edit;

        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        itemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        WorldObject = GameObject.Find("WorldObject");
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        string tapped = gesture.GetScreenPositionHitData().Target.name;
        if(tapped == "MenuButton" || tapped == "MenuArrow") displayWindows(menu);
        else if((tapped == "ItemButton" || tapped == "ItemArrow") && state == mode.Edit) displayWindows(item);
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
            newItemObject.transform.parent = WorldObject.transform;

            itemCon.setItemObject(newItemObject);
        }
    }

    // Menu bar buttons
    public void startButton()
    {
        // Cancle select item
        if (itemObject != null)
            itemCon.setItemObject(itemObject);

        if (state == mode.Edit)
        {
            // Don't do this ;_;
            //ExperimentWorld = Instantiate(WorldObject, Vector3.zero, Quaternion.identity);
            //WorldObject.SetActive(false);
            
            worldSc.saveState();
            deleteBtt.SetActive(false);
            Time.timeScale = 1;
            state = mode.Play;
            propWindow.setToggleLock();
        }
        else
        {
            // Noooooooooooooo
            //Destroy(ExperimentWorld);
            //WorldObject.SetActive(true);

            Time.timeScale = 0;
            state = mode.Edit;
            deleteBtt.SetActive(true);
            worldSc.loadState();
            propWindow.setToggleLock();
        }
    }

    public void pauseButton()
    {
        if (state == mode.Play)
        {
            Time.timeScale = 0;
            state = mode.Pause;
        }
        else
        {
            Time.timeScale = 1;
            state = mode.Play;
        }
    }

    public void setWorld(WorldObject worldScript)
    {
        worldSc = worldScript;
    }
}
