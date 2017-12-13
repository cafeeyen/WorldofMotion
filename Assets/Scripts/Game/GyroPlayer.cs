using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class GyroPlayer : MonoBehaviour
{
    public Text speedText;
    public TapGesture acc, brk;

    private float speed, maxSpeed=25f;
    private Rigidbody rb;
    private bool forward = false, backward = false;

    private void OnEnable()
    {
        speed = 0f;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        rb.AddForce(new Vector3(Input.acceleration.x * 1.5f, 0, 0));
        speedText.text = rb.velocity.magnitude.ToString("F2") + " m/s";

        if (acc.State == Gesture.GestureState.Possible)
            rb.AddForce(new Vector3(0, 0, 3f));

        if(brk.State == Gesture.GestureState.Possible)
            rb.AddForce(new Vector3(0, 0, -3f));

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
