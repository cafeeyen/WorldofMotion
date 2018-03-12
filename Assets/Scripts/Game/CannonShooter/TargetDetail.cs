using UnityEngine;
using UnityEngine.UI;

public class TargetDetail : MonoBehaviour
{
    public Transform cannon;
    public Text distText, heightText;

    private bool detected = false;

	// Update is called once per frame
	void Update ()
    {
		if(GetComponentInChildren<Collider>().enabled)
        {
            detected = true;
            distText.text = (transform.position.z - cannon.position.z).ToString("F2") + "m";
            heightText.text = (transform.position.y - cannon.position.y).ToString("F2") + "m";
        }
        else
        {
            detected = false;
            distText.text = "----";
            heightText.text = "----";
        }
	}

    public bool Detected { get { return detected; } }
}
