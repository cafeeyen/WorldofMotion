using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class CannonController : MonoBehaviour
{
    public TapGesture angleUp, angleDown, cannon;
    public Text angleText;
    public GameObject cannonBall;

    private float rotX, height = 0, maxHeight = 0, maxDist = 0, maxTime = 0, curDis, power = 20f;
    private bool floating = false;

    private void OnEnable()
    {
        cannon.Tapped += ShootCannon;
    }

    private void OnDisable()
    {
        cannon.Tapped -= ShootCannon;
    }

    void Update ()
    {
        rotX = transform.eulerAngles.x;
        rotX = rotX > 180 ? rotX - 360 : rotX;

        if (angleUp.State == Gesture.GestureState.Possible && rotX > -90)
            rotX -= 1;
        else if (angleDown.State == Gesture.GestureState.Possible && rotX < 0)
            rotX += 1;

        rotX = Mathf.Clamp(rotX, -90, 0);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        angleText.text = string.Format("{0} \u00B0", Mathf.Round(-rotX));

        // Calculate ball position
        if(floating)
        {
            if (curDis == maxDist)
                floating = false;
        }
    }

    private void ShootCannon(object sender, System.EventArgs e)
    {
        // Reset current force and position(reuse ball)
        cannonBall.transform.position = Vector3.zero;
        floating = true;

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
        var rise = power * Mathf.Sin(-rotX * Mathf.Deg2Rad) / 9.81f;
        maxHeight = height + (power * Mathf.Sin(-rotX * Mathf.Deg2Rad) * rise) - (0.5f * 9.81f * Mathf.Pow(rise, 2));
        var fall = Mathf.Sqrt(2 * maxHeight / 9.81f);
        maxTime = rise + fall;
        maxDist = power * Mathf.Cos(-rotX * Mathf.Deg2Rad) * maxTime;
    }
}
