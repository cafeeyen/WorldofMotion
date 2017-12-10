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
    private AudioSource audioSource;
    private AudioClip bttClk, bttDeny;
    private SceneLoader sl;

    private void OnEnable()
    {
        Time.timeScale = 0;
        state = mode.Edit;

        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        itemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        WorldObject = GameObject.Find("WorldObject");
        audioSource = GetComponent<AudioSource>();
        bttClk = (AudioClip)Resources.Load("Audios/ButtonClick", typeof(AudioClip));
        bttDeny = (AudioClip)Resources.Load("Audios/ButtonClickDeny", typeof(AudioClip));
        sl = GetComponent<SceneLoader>();
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        string tapped = gesture.GetScreenPositionHitData().Target.name;
        if (tapped == "MenuButton" || tapped == "MenuArrow") displayWindows(menu);
        else if (tapped == "ItemButton" || tapped == "ItemArrow")
        {
            if (state == mode.Edit)
                displayWindows(item);
            else
                playSound("deny");
        }
        else if (tapped == "PropButton" || tapped == "PropArrow") displayWindows(prop);
    }

    public void displayWindows(Animator anim, bool cancleSelect = false)
    {
        // If opened, close it
        if (anim.GetBool("IsDisplayed") == true)
        {
            anim.SetBool("IsDisplayed", false);
            playSound("clk");
        }
        // PropBar can open when some ItemObject has selected
        else if (anim.name != "PropBar" || itemObject != null)
        {
            anim.SetBool("IsDisplayed", true);
            changeState(anim);
        }
        // Block deny sound play when unselect item while prop bar didn't open
        else if(!cancleSelect)
        {
            playSound("deny");
        }
    }

    private void changeState(Animator anim)
    {
        // Can open one window at the same time
        if (anim == menu)
        {
            item.SetBool("IsDisplayed", false);
            prop.SetBool("IsDisplayed", false);
        }
        else if (anim == item)
        {
            menu.SetBool("IsDisplayed", false);
            prop.SetBool("IsDisplayed", false);
        }
        else // anim == prop
        {
            menu.SetBool("IsDisplayed", false);
            item.SetBool("IsDisplayed", false);
        }
        playSound("clk");
    }

    public void setItemObject(GameObject selectedItemObject)
    {
        itemObject = selectedItemObject;
    }

    public void createItemObject(GameObject prefeb)
    {
        // Can't create new one while current one is overlapping
        if (itemObject == null || !itemObject.GetComponent<ItemObject>().IsOverlap)
        {
            // Spawn at center of screen,  distance 10
            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + 10));
            Vector3 refinePosition = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), Mathf.Round(screenPosition.z));
            GameObject newItemObject = (GameObject)Instantiate(prefeb, refinePosition, Quaternion.identity);
            newItemObject.transform.parent = WorldObject.transform;

            itemCon.setItemObject(newItemObject);
            playSound("clk");
        }
        else
            playSound("deny");
    }

    // Menu bar buttons
    public void startButton()
    {
        if (itemObject == null || !itemObject.GetComponent<ItemObject>().IsOverlap)
        {
            // Cancle select item
            if (itemObject != null)
                itemCon.setItemObject(itemObject);

            if (state == mode.Edit)
            {
                worldSc.saveState();
                deleteBtt.SetActive(false);
                Time.timeScale = 1;
                state = mode.Play;
                propWindow.setToggleLock();
            }
            else
            {
                Time.timeScale = 0;
                state = mode.Edit;
                deleteBtt.SetActive(true);
                worldSc.loadState();
                propWindow.setToggleLock();
            }
            playSound("clk");
        }
        else
            playSound("deny");
    }

    public void pauseButton()
    {
        if (state == mode.Play)
        {
            Time.timeScale = 0;
            state = mode.Pause;
        }
        else if (state == mode.Pause)
        {
            Time.timeScale = 1;
            state = mode.Play;
        }
        playSound("clk");
    }

    public void MainMenuButton()
    {
        sl.loadNewScene(0);
    }

    public void setWorld(WorldObject worldScript)
    {
        worldSc = worldScript;
    }

    public void playSound(string sound)
    {
        if (sound == "clk")
            audioSource.PlayOneShot(bttClk, 2f);
        else if (sound == "deny")
            audioSource.PlayOneShot(bttDeny, 0.4f);
    }
}
