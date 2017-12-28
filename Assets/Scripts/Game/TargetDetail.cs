using UnityEngine;
using UnityEngine.UI;

public class TargetDetail : MonoBehaviour
{
    public Transform cannon;
    public Text distText, heightText;

	// Update is called once per frame
	void Update ()
    {
		if(GetComponentInChildren<Collider>().enabled)
        {
            distText.text = (transform.position.z - cannon.position.z).ToString("F2") + "m";
            heightText.text = (transform.position.y - cannon.position.y).ToString("F2") + "m";
        }
        else
        {
            distText.text = "----";
            heightText.text = "----";
        }
	}
}
