using UnityEngine;
using TouchScript.Gestures;

public class SelectItemObject : MonoBehaviour 
{
    public GameObject growEffect;

    private bool state = false;
    private TapGesture gesture;

    private void OnEnable()
    {
        growEffect.SetActive(false);
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += tapHandler;
    }

    private void OnDisable()
    {
        gesture.Tapped -= tapHandler;
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        state = true;
        growEffect.SetActive(true);
        GetComponent<DragNDrop>().enabled = true;
    }
}
