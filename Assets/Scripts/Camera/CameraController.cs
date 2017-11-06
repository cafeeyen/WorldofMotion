using UnityEngine;
using TouchScript.Gestures.TransformGestures;

// Base on CameraController.cs in Camera Example from TouchScript asset

public class CameraController : MonoBehaviour
{
    public Transform pivot;
    public Transform cam;
    public ScreenTransformGesture MoveGesture;
    public ScreenTransformGesture MultiTouchGesture;
    public AxisTransition axisTransition;

    private float moveSpeed = 0.1f;
    private float rotateSpeed = 100f;
    private float zoomSpeed = 50f;

    private void OnEnable()
    {
        MoveGesture.Transformed += moveHandler;
        MultiTouchGesture.Transformed += multiTouchHandler;
    }

    private void OnDisable()
    {
        MoveGesture.Transformed -= moveHandler;
        MultiTouchGesture.Transformed -= multiTouchHandler;
    } 

    private void moveHandler(object sender, System.EventArgs e)
    {
        // Move camera
        pivot.localPosition += cam.rotation * MoveGesture.DeltaPosition * -moveSpeed;
        getDistance();
    }

    private void multiTouchHandler(object sender, System.EventArgs e)
    {
        // Rotate camera
        Quaternion rotation = Quaternion.Euler(-MultiTouchGesture.DeltaPosition.y/Screen.height*rotateSpeed,
            MultiTouchGesture.DeltaPosition.x/Screen.width*rotateSpeed,
            MultiTouchGesture.DeltaRotation);
        pivot.localRotation *= rotation;

        // Zoom camera
        cam.localPosition += Vector3.forward * (MultiTouchGesture.DeltaScale - 1f) * zoomSpeed;
        getDistance();
    }

    private void getDistance()
    {
        /* Not ready to use
        // Get distance betweem camera and axis in world space
        float zoom = Vector3.Distance(cam.position, axisTransition.transform.position);
        Debug.Log(zoom);
        if(axisTransition.isActiveAndEnabled)
            axisTransition.updateScale(zoom);
        */
    }

}
