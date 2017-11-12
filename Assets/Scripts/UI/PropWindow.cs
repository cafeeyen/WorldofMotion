using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class PropWindow : MonoBehaviour
{
    public Text posX, posY, posZ, scaleX, scaleY, scaleZ;
    public Slider sliderX, sliderY, sliderZ;
    public Toggle deg30, deg45;
    /* In development
     public Button type; 
     For type properties(mass, friction, flexibility)
     */
    public Toggle gravity, gyro, breakable, player;

    private GameObject selectedItemObject;
    private TapGesture gesture;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;

        sliderX.onValueChanged.AddListener(changeSlideValue);
        sliderY.onValueChanged.AddListener(changeSlideValue);
        sliderZ.onValueChanged.AddListener(changeSlideValue);

        gravity.onValueChanged.AddListener(toggleGravity);
        gyro.onValueChanged.AddListener(toggleGyro);
        breakable.onValueChanged.AddListener(toggleBreakable);
        player.onValueChanged.AddListener(togglePlayer);
    }

    private void OnDisable()
    {
        sliderX.onValueChanged.RemoveAllListeners();
        sliderY.onValueChanged.RemoveAllListeners();
        sliderZ.onValueChanged.RemoveAllListeners();

        gravity.onValueChanged.RemoveAllListeners();
        gyro.onValueChanged.RemoveAllListeners();
        breakable.onValueChanged.RemoveAllListeners();
        player.onValueChanged.RemoveAllListeners();
    }

    void Update ()
    {
        if(selectedItemObject != null)
        {
            // Change position text
            posX.text = "X : " + selectedItemObject.transform.position.x;
            posY.text = "Y : " + selectedItemObject.transform.position.y;
            posZ.text = "Z : " + selectedItemObject.transform.position.z;
        }
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        switch(gesture.GetScreenPositionHitData().Target.name)
        {
            case "RotateLeft": rotate(1); break;
            case "RotateRight": rotate(-1); break;
        }
    }

    public void setSelectedItemObject(GameObject itemObject)
    {
        selectedItemObject = itemObject;
        sliderX.value = itemObject.transform.localScale.x / 10;
        sliderY.value = itemObject.transform.localScale.y / 10;
        sliderZ.value = itemObject.transform.localScale.z / 10;

        gravity.isOn = itemObject.GetComponent<ItemObject>().IsGravity;
        gyro.isOn = itemObject.GetComponent<ItemObject>().IsGyro;
        breakable.isOn = itemObject.GetComponent<ItemObject>().IsBreakable;
        player.isOn = itemObject.GetComponent<ItemObject>().IsPlayer;
    }

    private void changeSlideValue(float value)
    {
        selectedItemObject.transform.localScale = new Vector3(sliderX.value, sliderY.value, sliderZ.value) * 10;
        // Change scale text;
        scaleX.text = sliderX.value.ToString();
        scaleY.text = sliderY.value.ToString();
        scaleZ.text = sliderZ.value.ToString();
    }

    // Tapped send state before trigger
    private void toggleGravity(bool state) { selectedItemObject.GetComponent<ItemObject>().IsGravity = state; }
    private void toggleGyro(bool state) { selectedItemObject.GetComponent<ItemObject>().IsGyro = state; }
    private void toggleBreakable(bool state) { selectedItemObject.GetComponent<ItemObject>().IsBreakable = state; }
    private void togglePlayer(bool state) { selectedItemObject.GetComponent<ItemObject>().IsPlayer = state; }

    // Call from OnClick() in Unity inspector
    public void rotate(int dir)
    {
        // 1 is Left, -1 is Right
        if(deg30.isOn)
        {
            selectedItemObject.transform.Rotate(Vector3.up, 30 * dir);
        }
        else // deg45.isOn
        {
            selectedItemObject.transform.Rotate(Vector3.up, 45 * dir);
        }
    }

}
