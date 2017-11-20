using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class ItemObject : MonoBehaviour // Subject
{
    public List<Observer> observers = new List<Observer>();

    private SurfaceType surType;
    private int mass;
    private bool gravity, gyro, breakable, player;
    private TapGesture gesture;
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

    private void OnEnable()
    {
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += Notify;
    }

    private void OnDisable()
    {
        gesture.Tapped -= Notify;
    }

    private void Notify(object sender, System.EventArgs e)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].selectedItemObject(gameObject);
        }
    }

    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }

    public bool IsGravity { get { return gravity; } set { gravity = value; } }
    public bool IsGyro { get { return gyro; } set { gyro = value; } }
    public bool IsBreakable { get { return breakable; } set { breakable = value; } }
    public bool IsPlayer { get { return player; } set { player = value; } }

}
