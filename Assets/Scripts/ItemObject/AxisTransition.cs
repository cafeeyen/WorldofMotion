using UnityEngine;
using TouchScript.Gestures.TransformGestures;

public class AxisTransition : MonoBehaviour
{
    private TransformGesture axisX, axisY, axisZ;
    private Vector3 curPos;
    private GameObject itemObject;

    private void OnEnable()
    {
        axisX = transform.Find("AxisArrow X").GetComponent<TransformGesture>();
        axisY = transform.Find("AxisArrow Y").GetComponent<TransformGesture>();
        axisZ = transform.Find("AxisArrow Z").GetComponent<TransformGesture>();

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
        transform.localPosition = new Vector3(Mathf.Round(curPos.x * 100) / 100, Mathf.Round(curPos.y * 100) / 100, Mathf.Round(curPos.z * 100) / 100);
        itemObject.GetComponent<DragNDrop>().updatePosition(transform.localPosition);
    }

    public void setItemObject(GameObject selectedItemObject)
    {
        itemObject = selectedItemObject;
        updatePosition(itemObject.transform.localPosition);
    }

    public void updatePosition(Vector3 pos)
    {
        // Update position from drag and drop
        transform.localPosition = pos;
        curPos = pos;
    }

    public void updateScale(float zoom)
    {
        /* Not ready to use
        axisX.transform.localScale = new Vector3(zoom / 10, zoom / 4, zoom / 10);
        //axisX.transform.localPosition = new Vector3(10 + (zoom / 5), zoom, zoom / 5);

        axisY.transform.localScale = new Vector3(zoom / 10, zoom / 4, zoom / 10);

        //axisZ.transform.localScale = new Vector3(zoom / 10, zoom / 4, zoom / 10);
        */
    }
}
