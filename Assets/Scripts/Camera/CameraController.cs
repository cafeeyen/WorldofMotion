using UnityEngine;
using TouchScript.Gestures.TransformGestures;

// Base on CameraController.cs in Camera Example from TouchScript asset

public class CameraController : MonoBehaviour
{
    public Transform pivot, cam;
    public ScreenTransformGesture MoveGesture;
    public ScreenTransformGesture MultiTouchGesture;

    private float moveSpeed = 0.05f;
    private float rotateSpeed = 50f;
    private float zoomSpeed = 30f;

    private void OnEnable()
    {
        MoveGesture.Transformed += moveHandler;
        if (PlayerPrefs.GetInt("Lesson") == 0)
            MultiTouchGesture.Transformed += multiTouchHandler;
    }

    private void OnDisable()
    {
        MoveGesture.Transformed -= moveHandler;
        if (PlayerPrefs.GetInt("Lesson") == 0)
            MultiTouchGesture.Transformed -= multiTouchHandler;
    } 

    private void moveHandler(object sender, System.EventArgs e)
    {
        // Move camera
        if (PlayerPrefs.GetInt("Lesson") == 0)
            pivot.localPosition += cam.rotation * MoveGesture.DeltaPosition * -moveSpeed;
        else
            pivot.localPosition += new Vector3(MoveGesture.DeltaPosition.x, 0, MoveGesture.DeltaPosition.y) * -moveSpeed;
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
    }
}
