using System.Collections;
using UnityEngine;

public class CameraForPlayer : MonoBehaviour
{
    public Transform player;

    private Vector3 offset = new Vector3(0, 2, -3);

    void Update ()
    {
        transform.position = player.transform.position + offset;
	}
}
