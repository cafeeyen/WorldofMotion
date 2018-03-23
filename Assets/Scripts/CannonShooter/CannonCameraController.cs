using UnityEngine;
using Vuforia;

public class CannonCameraController : MonoBehaviour
{
    /* Game Object */
    public GameObject ball, shootedTarget;
    public Camera sideCam, ballCam;

    /* Parameter */
    private Vector3 ballCamOffset = new Vector3(0, 2.8f, -8f);

    /* Unity Function */
    private void OnEnable()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void FixedUpdate ()
    {
        ballCam.transform.position = ball.transform.position + ballCamOffset;

        var xDif = Mathf.Abs(shootedTarget.transform.position.x - ball.transform.position.x);
        var zDif = Mathf.Abs(shootedTarget.transform.position.z - ball.transform.position.z);

        if (Vector3.Distance(ball.transform.position, shootedTarget.transform.position) < 2 )
        {
            //Slow down if close to the target and after pass target to normal speed
            Time.timeScale = 0.08f;
            Time.fixedDeltaTime = Time.timeScale * .02f; // for smooth
            ballCam.transform.position = ball.transform.position + ballCamOffset + new Vector3(0, -1, 2 - zDif);
        }
        else if ((Time.timeScale == 0.08f || xDif > 5) || shootedTarget.transform.position.z - ball.transform.position.z < -2 || ball.transform.position.y < -1.8)
        {
            ballCam.depth = -2;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.Start();
    }
}
