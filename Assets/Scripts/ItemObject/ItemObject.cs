using UnityEngine;
using TouchScript.Gestures;
using System.Linq;

public class ItemObject : MonoBehaviour // Subject for ItemObjectController
{
    private ItemObjectController ItemCon;
    private SurfaceType surType;
    private int mass;
    private bool gravity, overlapping;
    private Renderer baseRenderer;
    private Material baseMat;
    private TapGesture gesture;
    private Rigidbody rb;

    // Data
    private Vector3 pos, velocity = Vector3.zero, angularVelocity;
    private Quaternion rot;

    void Awake()
    {
        ItemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        surType = ItemCon.getFactory().getSurType("Wood");
        gravity = false;
        overlapping = false;

        baseRenderer = transform.GetComponent<Renderer>();
        baseRenderer.material = surType.getSurMat();
        baseMat = baseRenderer.material;
        rb = GetComponent<Rigidbody>();
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

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(surType.getSound(), transform.position, 1f);
    }

    private void Notify(object sender, System.EventArgs e)
    {
        // Observe by ItemObjectController
        ItemCon.setItemObject(gameObject);
    }

    public void checkCollider()
    {
        Collider[] colliders = Physics.OverlapBox(transform.localPosition, transform.localScale / 2, transform.localRotation);
        Collider last = colliders.Last();
        foreach (Collider col in colliders)
        {
            // OverlapBox detect self collider, check transfrom(position and etc.) to make sure this is not itself
            if (col.CompareTag("ItemObject") && col.transform!=transform)
            {
                overlapping = true;
                ItemCon.changeGrowColor();
                break;
            }

            if(col == last)
            {
                overlapping = false;
                ItemCon.changeGrowColor();
            }
        }
    }

    public void saveCurrentState()
    {
        pos = transform.localPosition;
        rot = transform.localRotation;
        velocity = rb.velocity;
        angularVelocity = rb.angularVelocity;
    }

    public void returnState()
    {
        transform.localPosition = pos;
        transform.localRotation = rot;
        rb.velocity = velocity;
        rb.angularVelocity = angularVelocity;
    }

    public SurfaceType getSurType() { return surType; }
    public void setSurType(string st_name)
    {
        surType = ItemCon.getFactory().getSurType(st_name);
        BaseMat = surType.getSurMat();
        PhyMat = surType.getPhyMat();
        ItemCon.changeGrowMaterialTexture();
    }
    public bool IsGravity { get { return gravity; } set { gravity = value; } }
    public Material BaseMat { get { return baseMat; } set { baseMat = value; } }
    public PhysicMaterial PhyMat { get { return GetComponent<Collider>().material; } set { GetComponent<Collider>().material = value; } }
    public Renderer BaseRenderer { get { return baseRenderer; } set { baseRenderer = value; } }
    public bool IsOverlap { get { return overlapping; } }
    public float Mass { get { return rb.mass; } }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; rb.velocity = velocity; } }
}
