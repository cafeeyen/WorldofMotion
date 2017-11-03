using UnityEngine;
using TouchScript.Gestures.TransformGestures;

public class AxisTransition : MonoBehaviour
{
    private TransformGesture axisX, axisY, axisZ;
    private Vector3 curPos;
    private GameObject selectedItemObject;

    private void OnEnable()
    {
        axisX = transform.Find("Axis X").GetComponent<TransformGesture>();
        axisY = transform.Find("Axis Y").GetComponent<TransformGesture>();
        axisZ = transform.Find("Axis Z").GetComponent<TransformGesture>();

        axisX.Transformed += transfromMoveHandler;
        axisY.Transformed += transfromMoveHandler;
        axisZ.Transformed += transfromMoveHandler;
    }

    private void OnDisable()
    {
        axisX.Transformed -= transfromMoveHandler;
        axisY.Transformed -= transfromMoveHandler;
        axisZ.Transformed -= transfromMoveHandler;
    }

    private void transfromMoveHandler(object sender, System.EventArgs e)
    {

        curPos += transform.rotation * new Vector3(axisX.DeltaPosition.x, axisY.DeltaPosition.y, axisZ.DeltaPosition.z);
        transform.localPosition = new Vector3(Mathf.Round(curPos.x / 10) * 10, Mathf.Round(curPos.y / 10) * 10, Mathf.Round(curPos.z / 10) * 10);
        selectedItemObject.GetComponent<DragNDrop>().updatePosition(transform.localPosition);
    }

    public void setSelectedItemObject(GameObject itemObject)
    {
        selectedItemObject = itemObject;
        updatePosition(itemObject.transform.localPosition);
    }

    public void updatePosition(Vector3 pos)
    {
        // Update position from drag and drop
        transform.localPosition = pos;
        curPos = pos;
    }
}
