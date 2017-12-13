using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class CannonController : MonoBehaviour
{
    public TapGesture angleUp, angleDown, cannon;
    public Text angleText;
    public GameObject cannonBall;

    private float rotX, power = 20f;

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
    }

    private void ShootCannon(object sender, System.EventArgs e)
    {
        // Reset current force and position(reuse ball)
        cannonBall.GetComponent<Rigidbody>().isKinematic = true;
        cannonBall.transform.position = Vector3.zero;
        cannonBall.transform.localEulerAngles = Vector3.zero;
        cannonBall.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Shoot!!
        cannonBall.GetComponent<Rigidbody>().isKinematic = false;
        cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);
    }
}
