using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SurfaceType surType;
    private int mass;
    private bool gravity, gyro, breakable, player;

	void Start ()
    {
        /*
        In development
        surType = new Wood();
        */
        gravity = false;
        gyro = false;
        player = false;
        breakable = false;
	}

    public bool IsGravity { get { return gravity; } set { gravity = value; } }
    public bool IsGyro { get { return gyro; } set { gyro = value; } }
    public bool IsBreakable { get { return breakable; } set { breakable = value; } }
    public bool IsPlayer { get { return player; } set { player = value; } }

}
