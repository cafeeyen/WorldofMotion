using UnityEngine;

public class FingerController : MonoBehaviour
{
    public GameObject blue, yellow;

    private YellowFinger yellowF;

    protected state curState;
    protected GameObject itemObject;

    protected enum state
    {
        Cursor,
        Grab
    }

    private void OnEnable()
    {
        yellowF = yellow.GetComponent<YellowFinger>();
    }

    void Update()
    {
        if (blue.activeSelf && yellow.activeSelf && Vector3.Distance(yellow.transform.position, blue.transform.position) <= 3.0f)
        {
            yellowF.curState = state.Grab;
            blue.GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            yellowF.curState = state.Cursor;
            blue.GetComponent<Collider>().isTrigger = false;

            if (itemObject != null)
            {
                itemObject.GetComponent<Rigidbody>().isKinematic = false;
                itemObject = null;
            }
        }

        if (curState == state.Grab && itemObject != null)
            itemObject.transform.position = (yellow.transform.position - blue.transform.position) * 0.5f + blue.transform.position;
    }
}
