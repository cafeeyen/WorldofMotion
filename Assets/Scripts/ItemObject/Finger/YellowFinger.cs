using UnityEngine;

public class YellowFinger : FingerController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "BlueFinger")
        {
            if (curState == state.Cursor)
            {
                itemCon.setItemObject(other.gameObject);
            }
            else if (curState == state.Grab)
            {
                itemObject = other.gameObject;
                itemObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
