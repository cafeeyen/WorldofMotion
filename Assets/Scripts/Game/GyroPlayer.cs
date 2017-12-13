using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class GyroPlayer : MonoBehaviour
{
    public Text speedText;
    public TapGesture acc, brk;

    private float maxSpeed = 25f, holdTime;
    private Rigidbody rb;

    private void OnEnable()
    {
        holdTime = 0f;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        rb.AddForce(new Vector3(Input.acceleration.x * 5f, 0, 0));
        speedText.text = rb.velocity.magnitude.ToString("F2") + " m/s";

        holdTime += Time.deltaTime / 10;

        if (acc.State == Gesture.GestureState.Possible)
            rb.AddForce(new Vector3(0, 0, Mathf.Min(holdTime, 1) * 5f));
        else if (brk.State == Gesture.GestureState.Possible)
            rb.AddForce(new Vector3(0, 0, Mathf.Min(holdTime, 1) * - 5f));
        else
            holdTime = 0;

        Debug.Log(holdTime);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
