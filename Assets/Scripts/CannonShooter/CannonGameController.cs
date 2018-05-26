using UnityEngine;

public class CannonGameController : MonoBehaviour
{
    /* Game Objects */
    public GameObject cannon, ball, target, trail;
    public AudioClip shootSound;

    /* Game Parameters */
    private bool isShooting = false;
    private Vector3 cannonPos = new Vector3(0, -1.8f, 8), canyonPos = new Vector3(0, 27, 8);

    /* Unity Functions */
    private void OnEnable()
    {
        if ((PlayerPrefs.GetString("CannonShooterMode") == "Lv2" || PlayerPrefs.GetString("CannonShooterMode") == "Lv3") && !isShooting)
        {
            cannon.transform.position = canyonPos;
            ball.transform.position = canyonPos;
        }
    }
    private void FixedUpdate()
    {
        if (ball.transform.position.y < -10)
            resetBall();
        ball.transform.LookAt(-ball.GetComponent<Rigidbody>().velocity);
    }

    /* Cannon-Ball Functions */
    public void rotateAngle(int angle)
    {
        cannon.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
    public bool shoot(int angle, int power)
    {
        if (!isShooting)
        {
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
            resetBall();
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);
            if (PlayerPrefs.GetString("CannonShooterMode") != "AR")
                trail.SetActive(true);
            isShooting = true;
            return true;
        }
        else
            return false;
    }
    private void resetBall()
    {
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.transform.localEulerAngles = Vector3.zero;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (PlayerPrefs.GetString("CannonShooterMode") == "Lv2" || PlayerPrefs.GetString("CannonShooterMode") == "Lv3")
            ball.transform.position = canyonPos;
        else
            ball.transform.position = cannonPos;

        if (PlayerPrefs.GetString("CannonShooterMode") != "AR")
            trail.SetActive(false);
        isShooting = false;
    }

    /* Game-UI Functions */
    public void pauseGame(bool isPause)
    {
        cannon.SetActive(!isPause);
        ball.SetActive(!isPause);
        target.SetActive(!isPause);
    }
    public Vector3 getCannonForward() { return transform.forward; }
}
