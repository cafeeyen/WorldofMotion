using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;
using System.Linq;

public class PropWindow : MonoBehaviour
{
    public Text posX, posY, posZ, scaleX, scaleY, scaleZ, veloXT, veloYT, veloZT, mass, staticfic, dynamicfic;
    public Text acc, spd, move, dist, disp,sFriction,dFriction;
    public Slider sliderX, sliderY, sliderZ, veloX, veloY, veloZ;
    public UIController UICon;

    private GameObject itemObject;
    private TapGesture gesture;
    private Toggle r_deg30, r_deg45;
    private ToggleGroup type;
    private string selectedType, currentType;
    private Toggle st_wood, st_ice, st_metal, st_rubber, e_kinematic;
    private bool changeState = false;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        sliderX.onValueChanged.AddListener(changeScaleValue);
        sliderY.onValueChanged.AddListener(changeScaleValue);
        sliderZ.onValueChanged.AddListener(changeScaleValue);

        veloX.onValueChanged.AddListener(changeVeloValue);
        veloY.onValueChanged.AddListener(changeVeloValue);
        veloZ.onValueChanged.AddListener(changeVeloValue);

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
        e_kinematic = GameObject.Find("IsKinematic").GetComponent<Toggle>();
        e_kinematic.onValueChanged.AddListener(toggleKinematic);
    }

    private void OnDisable()
    {
        sliderX.onValueChanged.RemoveAllListeners();
        sliderY.onValueChanged.RemoveAllListeners();
        sliderZ.onValueChanged.RemoveAllListeners();
        e_kinematic.onValueChanged.RemoveAllListeners();
    }

    void Update()
    {
        if (itemObject != null)
        {
            // Change position text
            posX.text = "X : " + Mathf.Round(itemObject.transform.position.x * 100) / 100;
            posY.text = "Y : " + Mathf.Round(itemObject.transform.position.y * 100) / 100;
            posZ.text = "Z : " + Mathf.Round(itemObject.transform.position.z * 100) / 100;

            // Check type toggle
            if (UICon.state == UIController.mode.Edit)
            {
                currentType = type.ActiveToggles().FirstOrDefault<Toggle>().name.ToString();
                if (selectedType != currentType && !changeState)
                {
                    itemObject.GetComponent<ItemObject>().setSurType(currentType);
                    changeFriction(itemObject.GetComponent<ItemObject>().getSurType());
                    selectedType = currentType;
                    UICon.playSound("clk");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (UICon.state == UIController.mode.Play && itemObject != null)
        {
            acc.text = itemObject.GetComponent<ItemObject>().acc.ToString("F2") + " m/s^2";
            spd.text = itemObject.GetComponent<ItemObject>().spd.ToString("F2") + " m/s";
            move.text = itemObject.GetComponent<ItemObject>().movetime.ToString("F2") + " s";
            disp.text = itemObject.GetComponent<ItemObject>().disp.ToString("F2") + " m";
            dist.text = itemObject.GetComponent<ItemObject>().dist.ToString("F2") + " m";
            //calculate is in ItemObject.cs cal when touch another surface.
            sFriction.text = itemObject.GetComponent<ItemObject>().Fst.ToString("F2") + " N";
            dFriction.text = itemObject.GetComponent<ItemObject>().Fsl.ToString("F2") + " N";
        }
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        if (gesture.GetScreenPositionHitData().Target != null)
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
        if (selectedItemObject != null && itemObject != selectedItemObject)
        {
            // Start change state so slider won't change itemObject scale while set value from new itemObject
            changeState = true;
            sliderX.value = itemObject.transform.localScale.x;
            sliderY.value = itemObject.transform.localScale.y;
            sliderZ.value = itemObject.transform.localScale.z;

            ItemObject itemObjectSc = itemObject.GetComponent<ItemObject>();
            selectedType = itemObjectSc.getSurType().getName();
            st_wood.isOn = selectedType == "Wood";
            st_metal.isOn = selectedType == "Metal";
            st_ice.isOn = selectedType == "Ice";
            st_rubber.isOn = selectedType == "Rubber";

            mass.text = (itemObject.transform.localScale.x * itemObject.transform.localScale.y * itemObject.transform.localScale.z).ToString();
            changeFriction(itemObjectSc.getSurType());

            veloX.value = itemObjectSc.Velocity.x;
            veloY.value = itemObjectSc.Velocity.y;
            veloZ.value = itemObjectSc.Velocity.z;

            e_kinematic.isOn = !itemObject.GetComponent<ItemObject>().IsKinematic;
            // Finish change, cancel change state
            changeState = false;
        }
    }

    private void changeFriction(SurfaceType surType)
    {
        if (UICon.state == UIController.mode.Edit && itemObject != null)
        {
            staticfic.text = surType.getStaticFiction().ToString("F3");
            dynamicfic.text = surType.getDynamicFiction().ToString("F3");
        }
    }

    private void changeScaleValue(float value)
    {
        if(itemObject != null)
        {
            if (UICon.state == UIController.mode.Edit && !changeState)
            {

                itemObject.transform.localScale = new Vector3(sliderX.value, sliderY.value, sliderZ.value);
                itemObject.GetComponent<ItemObject>().checkCollider();
                itemObject.GetComponent<Rigidbody>().mass = sliderX.value * sliderY.value * sliderZ.value;

            }
            // Change scale text;
            scaleX.text = sliderX.value.ToString();
            scaleY.text = sliderY.value.ToString();
            scaleZ.text = sliderZ.value.ToString();
            mass.text = itemObject.GetComponent<Rigidbody>().mass.ToString();
        }
    }

    private void changeVeloValue(float value)
    {
        if (itemObject != null)
        {
            if (UICon.state == UIController.mode.Edit && !changeState)
                itemObject.GetComponent<ItemObject>().Velocity = new Vector3(veloX.value, veloY.value, veloZ.value);
            // Change velo text
            veloXT.text = veloX.value.ToString();
            veloYT.text = veloY.value.ToString();
            veloZT.text = veloZ.value.ToString();
        }
    }

    // Tapped send state before trigger
    private void toggleKinematic(bool state)
    {
        if (UICon.state == UIController.mode.Edit && itemObject != null)
        {
            UICon.playSound("clk");
            itemObject.GetComponent<ItemObject>().IsKinematic = !state;
        }

    }
    // Call from OnClick() in Unity inspector
    public void rotate(int dir)
    {
        if (UICon.state == UIController.mode.Edit && itemObject != null)
        {
            // 1 is Left, -1 is Right
            if (r_deg30.isOn)
                itemObject.transform.Rotate(Vector3.up, 30 * dir);

            else if (r_deg45.isOn)
                itemObject.transform.Rotate(Vector3.up, 45 * dir);

            itemObject.GetComponent<ItemObject>().checkCollider();
            UICon.playSound("clk");
        }
    }

    public void setToggleLock()
    {
        r_deg30.enabled = UICon.state == UIController.mode.Edit;
        r_deg45.enabled = UICon.state == UIController.mode.Edit;
        st_wood.enabled = UICon.state == UIController.mode.Edit;
        st_ice.enabled = UICon.state == UIController.mode.Edit;
        st_metal.enabled = UICon.state == UIController.mode.Edit;
        st_rubber.enabled = UICon.state == UIController.mode.Edit;
        e_kinematic.enabled = UICon.state == UIController.mode.Edit;
    }
}
