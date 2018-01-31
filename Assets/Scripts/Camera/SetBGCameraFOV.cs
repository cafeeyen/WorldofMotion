using UnityEngine;

public class SetBGCameraFOV : MonoBehaviour
{
    public Camera ARCamera;

    private Camera BGCamera;

    private void Start()
    {
        BGCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        BGCamera.fieldOfView = ARCamera.fieldOfView;
    }
}
