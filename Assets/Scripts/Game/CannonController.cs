using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class CannonController : MonoBehaviour
{
    public TapGesture angleUp, angleDown, cannon;
    public Text angleText, heightText, timeText, disText;
    public InputField powerText;
    public GameObject cannonBall, bigImage, overlay;
    public Camera sideCam;
    public LineRenderer arcLine, groundLine;
    public RenderTexture sideCamRT;
    public RawImage rawImage, miniRawImage;
    public SceneLoader sceneLoader;

    private float maxHeight = 0, maxDist = 0, maxTime = 0;
    private float angle = 0, height = 0, power = 0;
    private float step;

    private void OnEnable()
    {
        step = Time.fixedDeltaTime * 1f;
        sideCamRT.width = (int)rawImage.rectTransform.rect.width;
        sideCamRT.height = (int)rawImage.rectTransform.rect.height;
        cannon.Tapped += ShootCannon;
    }

    private void OnDisable()
    {
        cannon.Tapped -= ShootCannon;
    }

    void FixedUpdate()
    {
        angle = transform.eulerAngles.x;
        angle = angle > 180 ? angle - 360 : angle;

        if (angleUp.State == Gesture.GestureState.Possible && angle > -90)
            angle -= 1;
        else if (angleDown.State == Gesture.GestureState.Possible && angle < 0)
            angle += 1;

        angle = Mathf.Clamp(angle, -90, 0);

        transform.localRotation = Quaternion.Euler(angle, 0, 0);
        angleText.text = string.Format("{0} \u00B0", Mathf.Round(-angle));
        heightText.text = string.Format("Max height : {0} m.", System.Math.Round(maxHeight,2));
        timeText.text = string.Format("Time of flight : {0} s.", System.Math.Round(maxTime,2));
        disText.text = string.Format("Max distance : {0} m.", System.Math.Round(maxDist, 2));

        if (string.IsNullOrEmpty(powerText.text))
        {
            power = 0;
        }
        else
        {
            try
            {
                var clamp = Mathf.Clamp(int.Parse(powerText.text), 0, 50);
                power = int.Parse(powerText.text);
                powerText.text = clamp.ToString();
            }
            catch { }
        }    
    }

    private void ShootCannon(object sender, System.EventArgs e)
    {
        // Reset current force and position(reuse ball)
        cannonBall.GetComponent<Rigidbody>().isKinematic = true;
        cannonBall.transform.position = Vector3.zero;
        cannonBall.transform.localEulerAngles = Vector3.zero;
        cannonBall.GetComponent<Rigidbody>().velocity = Vector3.zero;

        cannonBall.GetComponent<Rigidbody>().isKinematic = false;
        cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);

        /* Don't care about air resistance
         * Use G = 9.81 m/s^2
         * Start point height is cannon
         * End point height is target
         * Lowest between two will be height for ground
         * 
         * -------------------------------------------------------------
         * 
         * *** Start point and end point have SAME height ***
         * 
         * Max height = (initial velocity^2 * sin^2(angle)) / 2g 
         * Time of flight = 2initial velocity * sin(angle) / g
         * Distance = (initial velocity^2 * sin2(angle)) / g
         * 
         * -------------------------------------------------------------
         * 
         * *** Start point and end point can be DIFFERENCE height *** <<< Use this one
         * 
         * Max height = y0 + (vy0 * Rise) - (0.5 * g * Rise^2)
         * Time of flight = Rise(vy0 / g) + Fall(sqrt( 2Max height / g))
         * Distance = vx0 * time
         * 
         * vx0 = V0 * Cos(angle)
         * vy0 = V0 * Sin(angle)
         * y0 = height difference between cannon and ground
        */
        var rise = power * Mathf.Sin(-angle * Mathf.Deg2Rad) / 9.81f;
        maxHeight = height + (power * Mathf.Sin(-angle * Mathf.Deg2Rad) * rise) - (0.5f * 9.81f * Mathf.Pow(rise, 2));
        var fall = Mathf.Sqrt(2 * maxHeight / 9.81f);
        maxTime = rise + fall;
        maxDist = power * Mathf.Cos(-angle * Mathf.Deg2Rad) * maxTime;

        if (-angle == 0)
        {
            sideCam.transform.position = new Vector3(-10, 0, 1);
            sideCam.orthographicSize = 1;
        }
        else if (angle <= 65)
        {
            sideCam.transform.position = new Vector3(-10, maxHeight / 2, maxDist / 2);
            sideCam.orthographicSize = maxDist / 3;
        }
        else // 66 - 90
        {
            sideCam.transform.position = new Vector3(-10, 0, maxDist / 2);
            sideCam.orthographicSize = maxHeight;
        }

        drawCurve();
    }

    private void drawCurve()
    {
        if (!miniRawImage.enabled)
            miniRawImage.enabled = true;

        int maxIndex = Mathf.RoundToInt(maxTime / step);
        arcLine.positionCount = maxIndex;
        groundLine.positionCount = 2;

        Vector3 curPos = Vector3.zero;
        Vector3 curVel = transform.forward * power;

        groundLine.SetPosition(0, Vector3.zero);
        groundLine.SetPosition(1, new Vector3(0, 0, maxDist));

        for (int i = 0; i < maxIndex; i++)
        {
            arcLine.SetPosition(i, curPos);

            curVel += Physics.gravity * step;
            curPos += curVel * step;
        }
    }

    public void clickMini()
    {
        if(miniRawImage.enabled)
        {
            bigImage.SetActive(true);
            overlay.SetActive(true);
        }
    }

    public void clickImg()
    {
        bigImage.SetActive(false);
        overlay.SetActive(false);
    }

    public void backToMainMenu()
    {
        sceneLoader.loadNewScene(0);
    }
}
