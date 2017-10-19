using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 dist;
    private float posX, posY;

    private void OnMouseDown()
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
    }

    private void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        // Set 1 in app = 10 in unity
        Vector3 simPos = new Vector3(Mathf.Round(worldPos.x / 10) * 10, Mathf.Round(worldPos.y / 10) * 10, Mathf.Round(worldPos.z / 10) * 10);
        transform.position = simPos;
    }
}
