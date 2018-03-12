using UnityEngine;
using UnityEngine.UI;

public class CannonUIController : MonoBehaviour
{
    /* Input Components */
    public InputField powField;
    public Slider powSlider;

    /* Output Components */
    public Text angleText, tarDist, tatHeight,
                calDist, calHeight, calTime;

    /* Parameters */
    private int angle, power;

    /* Scripts */
    // public CannonController canCon;

    /* BGs */
    public GameObject canyon, ground;

    /* Unity Functions */
    private void OnEnable()
    {
        if(PlayerPrefs.GetInt("CannonShooterMode") == 1)
        {
            canyon.SetActive(false);
            ground.SetActive(true);
        }
    }

    private void Update()
    {
        
    }

    /* My Functions */
    public void changePowSlider() { powField.text = powSlider.value.ToString(); }

    public void changePowText()
    {
        if (string.IsNullOrEmpty(powField.text))
            power = 0;
        else
        {
            try
            {
                power = Mathf.Clamp(int.Parse(powField.text), 0, 30);
                powField.text = power.ToString();
                powSlider.value = power;
            }
            catch { Debug.Log("Power input error."); }
        }
    }

    public void shoot()
    {
        //canCon.shoot(angle, power);
    }

    public void changeAngle(string action)
    {
        if(action == "Increase")
            angle = angle + 1 > 90 ? 90 : angle + 1;
        else if(action == "Decrese")
            angle = angle - 1 < 0 ? 0 : angle - 1;
        //canCon.rotateAngle(angle);
    }
}
