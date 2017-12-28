using UnityEngine;

public class TargetController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall")
        {

        }
    }
}
