using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class CannonController : MonoBehaviour
{
    public TapGesture angleUp, angleDown, cannon;
    public Text angleText, heightText, timeText, disText;
    public InputField powerText;
    public GameObject cannonBall, bigImage, overlay, target, shootedTarget;
    public Camera sideCam, ballCam;
    public LineRenderer arcLine, groundLine;
    public RenderTexture sideCamRT, cutCamRT;
    public RawImage rawImage, miniRawImage;
    public SceneLoader sceneLoader;


    private float maxHeight = 0, maxDist = 0, maxTime = 0;
    private float angle = 0, height = 0, power = 0;
    private float step;
    private Vector3 cannonPos = new Vector3(0, -1.8f, 5.5f), ballCamOffset = new Vector3(0, 2.8f, -5.5f);
    private bool shooted = false;
    private AudioClip ShootClk;

    private void OnEnable()
    {
        step = Time.fixedDeltaTime * 1f;
        sideCamRT.width = (int)rawImage.rectTransform.rect.width;
        sideCamRT.height = (int)rawImage.rectTransform.rect.height;
        cutCamRT.width = sideCamRT.width;
        cutCamRT.height = sideCamRT.height;
        cannon.Tapped += ShootCannon;
        ShootClk = (AudioClip)Resources.Load("Audios/Shooting", typeof(AudioClip));
    }

    private void OnDisable()
    {
        cannon.Tapped -= ShootCannon;
    }

    void FixedUpdate()
    {
        if(!shooted)
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
            heightText.text = string.Format("Max height : {0} m.", System.Math.Round(maxHeight, 2));
            timeText.text = string.Format("Time of flight : {0} s.", System.Math.Round(maxTime, 2));
            disText.text = string.Format("Max distance : {0} m.", System.Math.Round(maxDist, 2));

            if (string.IsNullOrEmpty(powerText.text))
            {
                power = 0;
            }
            else
            {
                try
                {
                    // Power min = 0, max = 30 (m/s)
                    var clamp = Mathf.Clamp(int.Parse(powerText.text), 0, 30);
                    power = int.Parse(powerText.text);
                    powerText.text = clamp.ToString();
                }
                catch { }
            }
        }
        else
        {
            ballCam.transform.position = cannonBall.transform.position + ballCamOffset;

            var xDif = Mathf.Abs(shootedTarget.transform.position.x - cannonBall.transform.position.x);
            var zDif = Mathf.Abs(shootedTarget.transform.position.z - cannonBall.transform.position.z);

            if (Vector3.Distance(cannonBall.transform.position, shootedTarget.transform.position) < 2)
            {
                //Slow down if close to the target and after pass target to normal speed
                Time.timeScale = 0.08f;
                Time.fixedDeltaTime = Time.timeScale * .02f; // for smooth
                ballCam.transform.position = cannonBall.transform.position + ballCamOffset + new Vector3(0, -1, 2 - zDif);
            }
            else if ((Time.timeScale == 0.08f || xDif > 5) || shootedTarget.transform.position.z - cannonBall.transform.position.z < -2 || cannonBall.transform.position.y < -1.8)
            {
                shooted = false;
                ballCam.depth = -2;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = step;
                drawCurve();
            }
        }

        cannonBall.transform.LookAt(-cannonBall.GetComponent<Rigidbody>().velocity);

        if(cannonBall.transform.position.y <= -50)
            // We don't want ball position to limitbreak~
            resetBall();
    }

    private void ShootCannon(object sender, System.EventArgs e)
    {
        if (!miniRawImage.enabled)
            miniRawImage.enabled = true;

        AudioSource.PlayClipAtPoint(ShootClk, this.transform.position);

        // Reset ball camera depth(cutsceneCam)
        ballCam.depth = -2;
        
        resetBall();
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

        // Y-1.8f and Z+5.5f from cannon offset
        if (-angle == 0)
        {
            sideCam.transform.position = new Vector3(-10, -1.8f, 1 + 5.5f);
            sideCam.orthographicSize = 1;
        }
        else if(maxHeight <= maxDist / 2)
        {
            sideCam.transform.position = new Vector3(-10, maxHeight / 2 - 1.8f, maxDist / 2 + 5.5f);
            sideCam.orthographicSize = Mathf.Max(1,  maxDist / 3);
        }
        else
        {
            sideCam.transform.position = new Vector3(-10, maxHeight / 2 - 1.8f, maxDist / 2 + 5.5f);
            sideCam.orthographicSize = Mathf.Max(1, maxHeight * 0.9f);
        }

        if(target.GetComponent<TargetDetail>().Detected)
        {
            //Swap camera
            ballCam.depth = 2;
            ballCam.transform.position = ballCamOffset + cannonPos;
            //ballCam.transform.LookAt(target.transform);

            // Show shooted target in side camera
            shootedTarget.transform.position = target.transform.position;

            miniRawImage.texture = cutCamRT;
            rawImage.texture = cutCamRT;

            shooted = true;
        }
        else
            drawCurve();
    }

    private void resetBall()
    {
        // Reset current force and position(reuse ball)
        cannonBall.GetComponent<Rigidbody>().isKinematic = true;
        cannonBall.transform.position = cannonPos;
        cannonBall.transform.localEulerAngles = Vector3.zero;
        cannonBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void drawCurve()
    {
        // Y-1.8f and Z+5.5f from cannon offset
        miniRawImage.texture = sideCamRT;
        rawImage.texture = sideCamRT;

        int maxIndex = Mathf.RoundToInt(maxTime / step);
        arcLine.positionCount = maxIndex;
        groundLine.positionCount = 2;

        Vector3 curPos = cannonPos;
        Vector3 curVel = transform.forward * power;

        groundLine.SetPosition(0, cannonPos);
        groundLine.SetPosition(1, new Vector3(0, -1.8f, maxDist + 5.5f));

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
