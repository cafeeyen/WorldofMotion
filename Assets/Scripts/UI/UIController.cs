﻿using UnityEngine;
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
    public GameObject deleteBtt, saveAlert, saveDeny, ground;
    public Button saveBtt, exBtt, undoBtt;
    public Camera ARCamera, OutputCamera;
    public TrackingObject tracker;

    private TapGesture gesture;
    private GameObject itemObject, WorldObject, ExperimentWorld;
    private ItemObjectController itemCon;
    private WorldObject worldSc;
    private AudioSource audioSource;
    private AudioClip bttClk, bttDeny;
    private SceneLoader sl;
    private bool arMode = false;
    private Vector3 camPos;
    private Quaternion camRot;

    private void OnEnable()
    {
        Time.fixedDeltaTime = 0.05f; // 30 FPS
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
        // Check if this open for first time
        if (PlayerPrefs.GetInt("EditorMode") == 0)
        {
            /* Tutorial */
            PlayerPrefs.SetInt("EditorMode", 1);
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
        else if (tapped == "ItemButton" || tapped == "ItemArrow")
        {
            if (state == mode.Edit)
                displayWindows(item);
            else
                playSound("deny");
        }
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
            // Cancle select item
            if (itemObject != null)
                itemCon.setItemObject(itemObject);

            if (state == mode.Edit)
            {
                worldSc.saveState();
                propWindow.setToggleLock();
                deleteBtt.SetActive(false);

                if (arMode)
                {
                    WorldObject.transform.localScale = Vector3.one;
                    ground.GetComponent<MeshRenderer>().enabled = false;

                    camPos = ARCamera.transform.localPosition;
                    camRot = ARCamera.transform.localRotation;

                    //WorldObject.GetComponent<DefaultTrackableEventHandler>().enabled = true;
                    ARCamera.clearFlags = CameraClearFlags.SolidColor;
                    OutputCamera.depth = 2;
                    tracker.UseAR = true;

                    //----------Copy from DefaultTrackableEventHandler (Vuforia)------------
                    var rendererComponents = WorldObject.GetComponentsInChildren<Renderer>(true);
                    var colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);

                    // Enable rendering:
                    foreach (var component in rendererComponents)
                        component.enabled = false;

                    // Enable colliders:
                    foreach (var component in colliderComponents)
                        component.enabled = false;
                    //-----------------------------------------------------------------------
                }
                else
                    Time.timeScale = 1;

                state = mode.Play;
                saveBtt.interactable = false;
                exBtt.interactable = false;
                undoBtt.interactable = false;
            }
            else
            {
                if (arMode)
                {
                    WorldObject.transform.localScale = Vector3.one;
                    ground.GetComponent<MeshRenderer>().enabled = true;

                    //WorldObject.GetComponent<DefaultTrackableEventHandler>().enabled = false;
                    ARCamera.clearFlags = CameraClearFlags.Skybox;
                    OutputCamera.depth = -10;
                    tracker.UseAR = false;

                    //----------Copy from DefaultTrackableEventHandler (Vuforia)------------
                    var rendererComponents = WorldObject.GetComponentsInChildren<Renderer>(true);
                    var colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);

                    // Enable rendering:
                    foreach (var component in rendererComponents)
                        component.enabled = true;

                    // Enable colliders:
                    foreach (var component in colliderComponents)
                        component.enabled = true;
                    //-----------------------------------------------------------------------
                    ARCamera.transform.localPosition = camPos;
                    ARCamera.transform.localRotation = camRot;
                }

                Time.timeScale = 0;
                state = mode.Edit;
                deleteBtt.SetActive(true);
                worldSc.loadState();
                propWindow.setToggleLock();

                saveBtt.interactable = true;
                exBtt.interactable = true;
                undoBtt.interactable = true;
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
            undoBtt.image.color = new Color(0, 1, 0.13f, 0.78f);
        else
            undoBtt.image.color = new Color(0.5f, 1 ,1, 0.68f);
    }
}
