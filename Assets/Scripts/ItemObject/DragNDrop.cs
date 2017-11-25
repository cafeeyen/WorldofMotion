using UnityEngine;
using TouchScript.Gestures.TransformGestures;

public class DragNDrop : MonoBehaviour
{
    private TransformGesture gesture;
    private Vector3 curPos;
    private GameObject axisTransition;
    private Rigidbody rb;

    private void OnEnable()
    {
        gesture = GetComponent<TransformGesture>();
        curPos = transform.localPosition;
        gesture.Transformed += transfromMoveHandler;
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        gesture.Transformed -= transfromMoveHandler;
    }

    private void transfromMoveHandler(object sender, System.EventArgs e)
    {
        curPos += axisTransition.transform.rotation * gesture.DeltaPosition;
        rb.MovePosition(new Vector3(Mathf.Round(curPos.x * 100) / 100, Mathf.Round(curPos.y * 100) / 100, Mathf.Round(curPos.z * 100) / 100));
        axisTransition.GetComponent<AxisTransition>().updatePosition(transform.localPosition);
    }

    public void setAxisTransition(GameObject axis)
    {
        axisTransition = axis;
    }

    public void updatePosition(Vector3 pos)
    {
        // Update position from axis
        rb.MovePosition(pos);
        curPos = pos;
    }
}
