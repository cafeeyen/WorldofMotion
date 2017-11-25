using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;
using System.Linq;

public class PropWindow : MonoBehaviour
{
    public Text posX, posY, posZ, scaleX, scaleY, scaleZ, mass, staticfic, dynamicfic;
    public Slider sliderX, sliderY, sliderZ;

    private GameObject itemObject;
    private TapGesture gesture;
    private Toggle r_deg30, r_deg45;
    private ToggleGroup type;
    private string selectedType, currentType;
    private Toggle st_wood, st_ice, st_metal, st_rubber;
    private Toggle e_gravity, e_gyro, e_breakable, e_player;
    private bool changeState = false;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        sliderX.onValueChanged.AddListener(changeSlideValue);
        sliderY.onValueChanged.AddListener(changeSlideValue);
        sliderZ.onValueChanged.AddListener(changeSlideValue);

        // Set rotate toggle
        r_deg30 = GameObject.Find("30deg").GetComponent<Toggle>();
        r_deg45 = GameObject.Find("45deg").GetComponent<Toggle>();

        // Set type toggle
        type = GameObject.Find("Type").GetComponent<ToggleGroup>();
        st_wood = GameObject.Find("Wood").GetComponent<Toggle>();
        st_ice = GameObject.Find("Ice").GetComponent<Toggle>();
        st_metal = GameObject.Find("Metal").GetComponent<Toggle>();
        st_rubber = GameObject.Find("Rubber").GetComponent<Toggle>();

        // Set effect toggle
        e_gravity = GameObject.Find("IsGravity").GetComponent<Toggle>();
        e_gyro = GameObject.Find("IsGyro").GetComponent<Toggle>();
        e_breakable = GameObject.Find("IsBreakable").GetComponent<Toggle>();
        e_player = GameObject.Find("IsPlayer").GetComponent<Toggle>();

        e_gravity.onValueChanged.AddListener(toggleGravity);
        e_gyro.onValueChanged.AddListener(toggleGyro);
        e_breakable.onValueChanged.AddListener(toggleBreakable);
        e_player.onValueChanged.AddListener(togglePlayer);
    }

    private void OnDisable()
    {
        sliderX.onValueChanged.RemoveAllListeners();
        sliderY.onValueChanged.RemoveAllListeners();
        sliderZ.onValueChanged.RemoveAllListeners();

        e_gravity.onValueChanged.RemoveAllListeners();
        e_gyro.onValueChanged.RemoveAllListeners();
        e_breakable.onValueChanged.RemoveAllListeners();
        e_player.onValueChanged.RemoveAllListeners();
    }

    void Update ()
    {
        if(itemObject != null)
        {
            // Change position text
            posX.text = "X : " + itemObject.transform.position.x;
            posY.text = "Y : " + itemObject.transform.position.y;
            posZ.text = "Z : " + itemObject.transform.position.z;

            // Check type toggle
            currentType = type.ActiveToggles().FirstOrDefault<Toggle>().name.ToString();
            if (selectedType != currentType && !changeState)
            {
                itemObject.GetComponent<ItemObject>().setSurType(currentType);
                changeFriction(itemObject.GetComponent<ItemObject>().getSurType());
                selectedType = currentType;
            }
        }
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        if(gesture.GetScreenPositionHitData().Target != null)
        {
            switch (gesture.GetScreenPositionHitData().Target.name)
            {
                case "RotateLeft": rotate(1); break;
                case "RotateRight": rotate(-1); break;
            }
        }
    }

    public void setPropValue(GameObject selectedItemObject)
    {
        itemObject = selectedItemObject;

        // Start change state so slider won't change itemObject scale while set value from new itemObject
        changeState = true;
        sliderX.value = itemObject.transform.localScale.x / 10;
        sliderY.value = itemObject.transform.localScale.y / 10;
        sliderZ.value = itemObject.transform.localScale.z / 10;

        ItemObject itemObjectSc = itemObject.GetComponent<ItemObject>();
        selectedType = itemObjectSc.getSurType().getName();
        switch(selectedType)
        {
            case "Wood": st_wood.isOn = true; break;
            case "Metal": st_metal.isOn = true; break;
            case "Ice": st_ice.isOn = true; break;
            case "Rubber": st_rubber.isOn = true; break;
        }

        mass.text = itemObjectSc.Mass.ToString();
        changeFriction(itemObjectSc.getSurType());

        e_gravity.isOn = itemObject.GetComponent<ItemObject>().IsGravity;
        e_gyro.isOn = itemObject.GetComponent<ItemObject>().IsGyro;
        e_breakable.isOn = itemObject.GetComponent<ItemObject>().IsBreakable;
        e_player.isOn = itemObject.GetComponent<ItemObject>().IsPlayer;
        // Finish change, cancel change state
        changeState = false;
    }

    private void changeFriction(SurfaceType surType)
    {
        staticfic.text = surType.getStaticFiction().ToString();
        dynamicfic.text = surType.getDynamicFiction().ToString();
    }

    private void changeSlideValue(float value)
    {
        if(!changeState)
            itemObject.transform.localScale = new Vector3(sliderX.value, sliderY.value, sliderZ.value) * 10;
        // Change scale text;
        scaleX.text = sliderX.value.ToString();
        scaleY.text = sliderY.value.ToString();
        scaleZ.text = sliderZ.value.ToString();
    }

    // Tapped send state before trigger
    private void toggleGravity(bool state) { itemObject.GetComponent<ItemObject>().IsGravity = state; }
    private void toggleGyro(bool state) { itemObject.GetComponent<ItemObject>().IsGyro = state; }
    private void toggleBreakable(bool state) { itemObject.GetComponent<ItemObject>().IsBreakable = state; }
    private void togglePlayer(bool state) { itemObject.GetComponent<ItemObject>().IsPlayer = state; }

    // Call from OnClick() in Unity inspector
    public void rotate(int dir)
    {
        // 1 is Left, -1 is Right
        if(r_deg30.isOn)
        {
            itemObject.transform.Rotate(Vector3.up, 30 * dir);
        }
        else if(r_deg45.isOn)
        {
            itemObject.transform.Rotate(Vector3.up, 45 * dir);
        }
    }
}
