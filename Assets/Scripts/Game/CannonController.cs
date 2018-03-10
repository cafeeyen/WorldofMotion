using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class CannonController : MonoBehaviour
{
    public TapGesture angleUp, angleDown, cannon;
    public Text angleText, heightText, timeText, disText, countText;
    public InputField powerText;
    public GameObject cannonBall, target, shootedTarget, trail;
    public Camera sideCam, ballCam;
    public LineRenderer arcLine, groundLine;
    public CS_UIController csCon;

    private float maxHeight = 0, maxDist = 0, maxTime = 0;
    private float angle = 0, height = 0, power = 0;
    private float step;
    private int shootCnt = 0;
    private Vector3 cannonPos = new Vector3(0, -1.8f, 5.5f), ballCamOffset = new Vector3(0, 2.8f, -5.5f), canyonPos = new Vector3(0, 27, 5.5f);
    private bool shooted = false;
    private AudioClip ShootClk;
    private bool modeAR;

    /*
     * CannonShooterMode
     * 1 : Lv1
     * 2 : Lv2
     * 3 : Lv3
     * 4 : Unlimited // Cancle(Merge with 1-3)
     * 5 : AR
    */

    private void OnEnable()
    {        
        cannon.Tapped += ShootCannon;
    }

    private void OnDisable()
    {
        cannon.Tapped -= ShootCannon;
    }

    private void Awake()
    {
        step = Time.fixedDeltaTime * 1f;
        ShootClk = (AudioClip)Resources.Load("Audios/Shooting", typeof(AudioClip));
        modeAR = PlayerPrefs.GetInt("CannonShooterMode") == 5;

        if (PlayerPrefs.GetInt("CannonShooterMode") != 1 && !modeAR)
        {
            transform.position = canyonPos;
            cannonBall.transform.position = canyonPos;
        }
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
            heightText.text = string.Format("ความสูงจากจุดเริ่ม {0} m.", System.Math.Round(maxHeight, 2));
            disText.text = string.Format("ระยะทางในแนวราบ {0} m.", System.Math.Round(maxDist, 2));
            timeText.text = string.Format("เวลาที่ลอยกลางอากาศ {0} s.", System.Math.Round(maxTime, 2));
            if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
                countText.text = string.Format("= {0}", shootCnt);

            if (string.IsNullOrEmpty(powerText.text))
                power = 0;
            else
            {
                try
                {
                    // Power min = 0, max = 30 (m/s)
                    var clamp = Mathf.Clamp(int.Parse(powerText.text), 0, 30);
                    powerText.text = clamp.ToString();
                    power = int.Parse(powerText.text);
                }
                catch { Debug.Log("Power input error."); }
            }
        }
        else if(modeAR)
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
                ballCam.depth = -2;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = step;
                drawCurve();
            }
        }

        cannonBall.transform.LookAt(-cannonBall.GetComponent<Rigidbody>().velocity);
        if(cannonBall.transform.position.y < -10)
            // We don't want ball position to limitbreak~
            resetBall();
    }

    private void ShootCannon(object sender, System.EventArgs e)
    {
        if(power > 0 && !shooted)
        {
            AudioSource.PlayClipAtPoint(ShootClk, Camera.main.transform.position);
            shootCnt++;
            resetBall();
            cannonBall.GetComponent<Rigidbody>().isKinematic = false;
            cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);
            shooted = true;
            if(!modeAR)
                trail.SetActive(true);

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
             * Max height = h + (vy0 * Rise) - (0.5 * g * Rise^2)
             * Time of flight = Rise(vy0 / g) + Fall(sqrt( 2Max height / g))
             * Distance = vx0 * time
             * 
             * vx0 = V0 * Cos(angle) <- ucos
             * vy0 = V0 * Sin(angle) <- usin
             * h = height difference between cannon and ground/target
            */
            var rise = power * Mathf.Sin(-angle * Mathf.Deg2Rad) / 9.81f;
            maxHeight = height + (power * Mathf.Sin(-angle * Mathf.Deg2Rad) * rise) - (0.5f * 9.81f * Mathf.Pow(rise, 2));
            var fall = Mathf.Sqrt(2 * maxHeight / 9.81f);
            maxTime = rise + fall;
            maxDist = power * Mathf.Cos(-angle * Mathf.Deg2Rad) * maxTime;
            if (modeAR)
            {
                // Reset ball camera depth(cutsceneCam)
                ballCam.depth = -2;

                if (target.GetComponent<TargetDetail>().Detected)
                {
                    //Swap camera
                    ballCam.depth = 2;
                    ballCam.transform.position = ballCamOffset + cannonPos;
                    csCon.viewToCutCam();

                    // Show shooted target in side camera
                    shootedTarget.transform.position = target.transform.position;
                }
                else
                {
                    shootedTarget.transform.position = new Vector3(0, 0, -100);
                    drawCurve();
                }
            }
            else
                drawCurve();
        }
    }

    private void resetBall()
    {
        // Reset current force and position(reuse ball)
        cannonBall.GetComponent<Rigidbody>().isKinematic = true;
        cannonBall.transform.localEulerAngles = Vector3.zero;
        cannonBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (PlayerPrefs.GetInt("CannonShooterMode") == 2 || PlayerPrefs.GetInt("CannonShooterMode") == 3)
            cannonBall.transform.position = canyonPos;
        else
            cannonBall.transform.position = cannonPos;
        shooted = false;

        if (!modeAR)
            trail.SetActive(false);
    }

    private void drawCurve()
    {
        // Y-1.8f and Z+5.5f from cannon offset
        if(modeAR)
        {
            if(!csCon.miniRawImage.enabled)
                csCon.miniRawImage.enabled = true;

            csCon.viewToSideCam();
            groundLine.positionCount = 2;
            groundLine.SetPosition(0, new Vector3(0, -1.8f, 0));
            groundLine.SetPosition(1, new Vector3(0, -1.8f, 100));
        }

        if (!arcLine.enabled)
            arcLine.enabled = true;

        int maxIndex = Mathf.RoundToInt(maxTime / step);
        arcLine.positionCount = maxIndex;

        Vector3 curPos;
        if (PlayerPrefs.GetInt("CannonShooterMode") == 2 || PlayerPrefs.GetInt("CannonShooterMode") == 3)
            curPos = canyonPos;
        else
            curPos = cannonPos;

        Vector3 curVel = transform.forward * power;

        for (int i = 0; i < maxIndex; i++)
        {
            arcLine.SetPosition(i, curPos);

            curVel += Physics.gravity * step;
            curPos += curVel * step;
        }
    }

    public void setHeight(float h){ height = h; }

    public int getShootCnt() { return shootCnt; }
}
