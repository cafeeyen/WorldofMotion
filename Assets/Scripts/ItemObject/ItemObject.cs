using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SurfaceType surType;
    private int mass;
    private bool gravity, gyro, player, breakable;


	// Use this for initialization
	void Start ()
    {
        surType = new Wood();
        gravity = false;
        gyro = false;
        player = false;
        breakable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
