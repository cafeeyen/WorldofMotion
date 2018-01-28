using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;
using System.Linq;

public class PropWindow : MonoBehaviour
{
    public Text posX, posY, posZ, scaleX, scaleY, scaleZ, mass, staticfic, dynamicfic;
    public Slider sliderX, sliderY, sliderZ;
    public UIController UICon;
    public InputField veloX, veloY, veloZ;

    private GameObject itemObject;
    private TapGesture gesture;
    private Toggle r_deg30, r_deg45;
    private ToggleGroup type;
    private string selectedType, currentType;
    private Toggle st_wood, st_ice, st_metal, st_rubber, e_gravity;
    private bool changeState = false;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        sliderX.onValueChanged.AddListener(changeSlideValue);
        sliderY.onValueChanged.AddListener(changeSlideValue);
        sliderZ.onValueChanged.AddListener(changeSlideValue);

        veloX.onEndEdit.AddListener(changeVelocity);
        veloY.onEndEdit.AddListener(changeVelocity);
        veloZ.onEndEdit.AddListener(changeVelocity);

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
        e_gravity.onValueChanged.AddListener(toggleGravity);
    }

    private void OnDisable()
    {
        sliderX.onValueChanged.RemoveAllListeners();
        sliderY.onValueChanged.RemoveAllListeners();
        sliderZ.onValueChanged.RemoveAllListeners();
        e_gravity.onValueChanged.RemoveAllListeners();
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
        if (itemObject != null)
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

            mass.text = itemObjectSc.Mass.ToString("F3");
            changeFriction(itemObjectSc.getSurType());

            veloX.text = itemObjectSc.Velocity.x.ToString();
            veloY.text = itemObjectSc.Velocity.y.ToString();
            veloZ.text = itemObjectSc.Velocity.z.ToString();

            e_gravity.isOn = itemObject.GetComponent<ItemObject>().IsGravity;
            // Finish change, cancel change state
            changeState = false;
        }
    }

    private void changeFriction(SurfaceType surType)
    {
        staticfic.text = surType.getStaticFiction().ToString("F3");
        dynamicfic.text = surType.getDynamicFiction().ToString("F3");
    }

    private void changeSlideValue(float value)
    {
        if (UICon.state == UIController.mode.Edit)
        {
            if (!changeState)
            {
                itemObject.transform.localScale = new Vector3(sliderX.value, sliderY.value, sliderZ.value);
                itemObject.GetComponent<ItemObject>().checkCollider();
            }

            // Change scale text;
            scaleX.text = sliderX.value.ToString();
            scaleY.text = sliderY.value.ToString();
            scaleZ.text = sliderZ.value.ToString();
        }
    }

    private void changeVelocity(string value)
    {
        if (UICon.state == UIController.mode.Edit && !changeState)
        {
            if (string.IsNullOrEmpty(veloX.text))
                veloX.text = "0";
            if (string.IsNullOrEmpty(veloY.text))
                veloY.text = "0";
            if (string.IsNullOrEmpty(veloZ.text))
                veloZ.text = "0";

            var test = 0;
            try
            {
                var x = Mathf.Clamp(int.Parse(veloX.text), -30, 30);
                test++;
                var y = Mathf.Clamp(int.Parse(veloY.text), -30, 30);
                test++;
                var z = Mathf.Clamp(int.Parse(veloZ.text), -30, 30);

                itemObject.GetComponent<ItemObject>().Velocity = new Vector3(x, y, z);

                veloX.text = x.ToString();
                veloY.text = y.ToString();
                veloZ.text = z.ToString();
            }
            catch
            {
                Debug.Log("Power input error.");
                if(test == 0)
                    veloX.text = "0";
                else if(test == 1)
                    veloY.text = "0";
                else
                    veloZ.text = "0";
            }
        }
    }

    // Tapped send state before trigger
    private void toggleGravity(bool state)
    {
        UICon.playSound("clk");
        itemObject.GetComponent<ItemObject>().IsGravity = state;
    }
    // Call from OnClick() in Unity inspector
    public void rotate(int dir)
    {
        if (UICon.state == UIController.mode.Edit)
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
        if (UICon.state == UIController.mode.Edit)
        {

            r_deg30.enabled = true;
            r_deg45.enabled = true;
            st_wood.enabled = true;
            st_ice.enabled = true;
            st_metal.enabled = true;
            st_rubber.enabled = true;
            e_gravity.enabled = true;
        }
        else
        {
            r_deg30.enabled = false;
            r_deg45.enabled = false;
            st_wood.enabled = false;
            st_ice.enabled = false;
            st_metal.enabled = false;
            st_rubber.enabled = false;
            e_gravity.enabled = false;
        }
    }
}
