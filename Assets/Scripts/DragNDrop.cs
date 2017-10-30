using UnityEngine;
using TouchScript.Gestures.TransformGestures;

public class DragNDrop : MonoBehaviour
{
    private TransformGesture gesture;
    private Vector3 curPos;

    private void OnEnable()
    {
        gesture = GetComponent<TransformGesture>();
        curPos = transform.localPosition;
        gesture.Transformed += transfromMoveHandler;
    }

    private void OnDisable()
    {
        gesture.Transformed -= transfromMoveHandler;
    }

    private void transfromMoveHandler(object sender, System.EventArgs e)
    {
        curPos += transform.rotation * gesture.DeltaPosition;

        // Set block 1(app):10(Unity)
        transform.localPosition = new Vector3(Mathf.Round(curPos.x / 10) * 10, Mathf.Round(curPos.y / 10) * 10, Mathf.Round(curPos.z / 10) * 10);
    }
}
