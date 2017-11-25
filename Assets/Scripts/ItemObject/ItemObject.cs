using UnityEngine;
using TouchScript.Gestures;
using System.Collections.Generic;

public class ItemObject : MonoBehaviour // Subject for ItemObjectController
{
    private ItemObjectController ItemCon;
    private SurfaceType surType;
    private int mass;
    private bool gravity, gyro, breakable, player, overlapping;
    private Renderer baseRenderer;
    private Material baseMat;
    private TapGesture gesture;
    private Rigidbody rb;
    private Collider collide;
    private List<Collider> collideList = new List<Collider>();

    void Awake()
    {
        ItemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        surType = ItemCon.getFactory().getSurType("Wood");
        gravity = false;
        gyro = false;
        player = false;
        breakable = false;
        overlapping = false;

        baseRenderer = transform.GetComponent<Renderer>();
        baseRenderer.material = surType.getSurMat();
        baseMat = baseRenderer.material;

        rb = GetComponent<Rigidbody>();
        collide = GetComponent<Collider>();
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
        // Observe by ItemObjectController
        ItemCon.setItemObject(gameObject);
        collide.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check when selected only
        if (other.tag == "ItemObject" && ItemCon.getCurrentObject() == gameObject)
        {
            if(!collideList.Contains(other))
                collideList.Add(other);

            if(!overlapping)
            {
                overlapping = true;
                ItemCon.changeGrowColor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ItemObject")
        {
            if (collideList.Contains(other))
                collideList.Remove(other);

            if(collideList.Count == 0)
            {
                overlapping = false;
                ItemCon.changeGrowColor();
            }
        }
    }

    public SurfaceType getSurType() { return surType; }
    public void setSurType(string st_name)
    {
        surType = ItemCon.getFactory().getSurType(st_name);
        BaseMat = surType.getSurMat();
        ItemCon.changeGrowMaterialTexture();
    }
    public bool IsGravity { get { return gravity; } set { gravity = value; } }
    public bool IsGyro { get { return gyro; } set { gyro = value; } }
    public bool IsBreakable { get { return breakable; } set { breakable = value; } }
    public bool IsPlayer { get { return player; } set { player = value; } }
    public Material BaseMat { get { return baseMat; } set { baseMat = value; } }
    public Renderer BaseRenderer { get { return baseRenderer; } set { baseRenderer = value; } }
    public bool IsOverlap { get { return overlapping; } }
    public Collider Collider { get { return collide; } }
    public float Mass { get { return rb.mass; } }
}
