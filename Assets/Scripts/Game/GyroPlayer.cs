using UnityEngine;
using UnityEngine.UI;

public class GyroPlayer : MonoBehaviour
{
    public Text speedText;

    private float speed;
    private Rigidbody rb;

    private void Start()
    {
        speed = 0f;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        rb.AddForce(new Vector3(Input.acceleration.x * 1.5f, 0, 0));
        speedText.text = rb.velocity.magnitude.ToString("F2");
    }
}
