using UnityEngine;
using TouchScript.Gestures;

public class SelectItemObject : MonoBehaviour 
{
    private TapGesture gesture;

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        transform.parent.GetComponent<ItemObjectController>().setSelectedItemObject(gameObject);
    }

}
