using UnityEngine;
using TouchScript.Gestures;
using UnityEngine.UI;

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
    public GameObject deleteBtt, saveAlert, saveDeny, ground, overlay, tip1;
    public RectTransform statWin, setWin;
    public Button saveBtt, arBtt;
    public Camera ARCamera;
    public TrackingObject tracker;
    public FingerController fingCon;
    public ProblemGenerator problemGenerator;

    private TapGesture gesture;
    private GameObject itemObject, WorldObject, ExperimentWorld;
    public Image[] rulePageInSet;
    private ItemObjectController itemCon;
    private WorldObject worldSc;
    private AudioSource audioSource;
    private AudioClip bttClk, bttDeny;
    private SceneLoader sl;
    private bool arMode = false, tip = true;
    private Vector3 camPos;
    private Quaternion camRot;

    private void OnEnable()
    {
        state = mode.Edit;

        sl = GetComponent<SceneLoader>();
        bttClk = (AudioClip)Resources.Load("Audios/ButtonClick", typeof(AudioClip));
        bttDeny = (AudioClip)Resources.Load("Audios/ButtonClickDeny", typeof(AudioClip));
        audioSource = GetComponent<AudioSource>();

        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        if (PlayerPrefs.GetInt("LessonTask") == 0)
        {
            itemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
            WorldObject = GameObject.Find("WorldObject");
            // Check if this open for first time
            if (PlayerPrefs.GetInt("EditorMode") == 0)
            {
                /* Tutorial */
                PlayerPrefs.SetInt("EditorMode", 1);
            }
        }
        else //question lesson.
        {
            problemGenerator.newProblem();
            Time.timeScale = 1;
        }
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        string tapped = gesture.GetScreenPositionHitData().Target.name;
        if (tapped == "MenuButton" || tapped == "MenuArrow") displayWindows(menu);
        else if (tapped == "ItemButton" || tapped == "ItemArrow") displayWindows(item);
        else if (tapped == "PropButton" || tapped == "PropArrow") displayWindows(prop);
    }

    private void Update()
    {
        if(arMode && state == mode.Play)
        {
            if (WorldObject.GetComponentInChildren<Collider>().enabled)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }
    }

    public void displayWindows(Animator anim)
    {
        anim.SetBool("IsDisplayed", !anim.GetBool("IsDisplayed"));
        playSound("clk");
    }

    public void setItemObject(GameObject selectedItemObject)
    {
        itemObject = selectedItemObject;
    }

    public void createItemObject(GameObject prefeb)
    {
        // Can't create new one while current one is overlapping
        if (state == mode.Edit && (itemObject == null || !itemObject.GetComponent<ItemObject>().IsOverlap))
        {
            // Spawn at center of screen,  distance 10
            Vector3 screenPosition = ARCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, ARCamera.nearClipPlane + 10));
            Vector3 refinePosition = new Vector3(Mathf.Round(screenPosition.x), Mathf.Round(screenPosition.y), Mathf.Round(screenPosition.z));
            GameObject newItemObject = (GameObject)Instantiate(prefeb, refinePosition, Quaternion.identity);
            newItemObject.transform.parent = WorldObject.transform;
            newItemObject.GetComponent<ItemObject>().ItemType = prefeb.name;

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
            if (state == mode.Edit)
            {
                if (itemObject != null)
                    itemCon.showAxis();

                state = mode.Play;
                saveBtt.interactable = false;
                arBtt.interactable = false;

                worldSc.saveState();
                propWindow.setToggleLock();
                deleteBtt.SetActive(false);

                if (arMode)
                {
                    fingCon.enabled = true;
                    ground.GetComponent<MeshRenderer>().enabled = false;
                    camPos = ARCamera.transform.localPosition;
                    camRot = ARCamera.transform.localRotation;
                    ARCamera.clearFlags = CameraClearFlags.SolidColor;
                    tracker.UseAR = true;
                    showItemInWorld(false);
                }
                else
                    Time.timeScale = 1;
            }
            else
            {
                if (arMode)
                {
                    fingCon.enabled = false;
                    ground.GetComponent<MeshRenderer>().enabled = true;
                    ARCamera.clearFlags = CameraClearFlags.Skybox;
                    tracker.UseAR = false;
                    ARCamera.transform.localPosition = camPos;
                    ARCamera.transform.localRotation = camRot;
                    showItemInWorld(true);
                }

                Time.timeScale = 0;
                state = mode.Edit;
                deleteBtt.SetActive(true);
                worldSc.loadState();
                propWindow.setToggleLock();

                if (itemObject != null)
                    itemCon.showAxis();

                saveBtt.interactable = true;
                arBtt.interactable = true;
            }
            playSound("clk");
        }
        else
            playSound("deny");
    }

    public void swapWindow()
    {
        var x = setWin.anchoredPosition.x;
        setWin.anchoredPosition = new Vector3(statWin.anchoredPosition.x, setWin.anchoredPosition.y, 0);
        statWin.anchoredPosition = new Vector3(x, 0, 0);
    }

    public void frictionTip()
    {
        if (tip)
        {
            tip1.SetActive(true);
            tip = false;
        }
        else
        {
            tip1.SetActive(false);
            tip = true;
        }
    }

    private void showItemInWorld(bool state)
    {
        //----------Copy from DefaultTrackableEventHandler (Vuforia)------------
        var rendererComponents = WorldObject.GetComponentsInChildren<Renderer>(state);
        var colliderComponents = WorldObject.GetComponentsInChildren<Collider>(state);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = state;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = state;
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
        PlayerPrefs.SetInt("Lesson", 0);
        sl.loadNewScene(0);
    }

    public void BackToSelectButton()
    {
        PlayerPrefs.SetInt("LessonTask", 0);
        PlayerPrefs.SetInt("Lesson", 0);
        PlayerPrefs.SetInt("LessonSelect", 1);
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

    public void setDeny(bool active)
    {
        saveDeny.SetActive(active);
    }

    public void setAlert(bool active)
    {
        saveAlert.SetActive(active);
    }

    public void setAR()
    {
        arMode = !arMode;
        // Color need to normalize to 0-1
        if (arMode)
            arBtt.image.color = new Color(0, 1, 0.13f, 0.78f);
        else
            arBtt.image.color = new Color(0.5f, 1 ,1, 0.68f);
    }
}
