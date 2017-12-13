using UnityEngine;
using TouchScript.Gestures.TransformGestures;

public class DragNDrop : MonoBehaviour
{
    private TransformGesture gesture;
    private Vector3 curPos;
    private GameObject axisTransition;

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
        curPos += axisTransition.transform.rotation * gesture.DeltaPosition;
        transform.localPosition = new Vector3(Mathf.Round(curPos.x * 100) / 100, Mathf.Round(curPos.y * 100) / 100, Mathf.Round(curPos.z * 100) / 100);
        axisTransition.GetComponent<AxisTransition>().updatePosition(transform.localPosition);
        triggerColliderCheck();
    }

    public void setAxisTransition(GameObject axis)
    {
        axisTransition = axis;
    }

    public void updatePosition(Vector3 pos)
    {
        // Update position from axis
        transform.localPosition = pos;
        curPos = pos;
        triggerColliderCheck();
    }

    private void triggerColliderCheck()
    {
        gameObject.GetComponent<ItemObject>().checkCollider();
    }
}
