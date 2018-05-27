using UnityEngine;
using TouchScript.Gestures;
using System.Linq;
using System.Collections.Generic;

public class ItemObject : MonoBehaviour // Subject for ItemObjectController
{
    private ItemObjectController ItemCon;
    private TapGesture gesture;
    private Rigidbody rb;
    private Vector3 pos, scale, velocity = Vector3.zero;
    private Quaternion rot;
    private bool kinematic;
    private List<Collider> collidedObjects;
    public SurfaceType surType;

    public float acc, spd, movetime, dist, disp, Fst, Fsl;
    private Vector3 lastvelo, lastpos;
    private float mass;

    void Awake()
    {
        ItemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        kinematic = false;
        IsOverlap = false;

        surType = ItemCon.getFactory().getSurType("Wood");
        BaseRenderer = transform.GetComponent<Renderer>();
        BaseRenderer.material = surType.getSurMat();
        BaseMat = BaseRenderer.material;
        rb = GetComponent<Rigidbody>();

        collidedObjects = new List<Collider>();
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

    private void FixedUpdate()
    {
        acc = (rb.velocity.magnitude - lastvelo.magnitude) / Time.fixedDeltaTime;
        /*
         * A = F / M
         * Fst = Ust * Fn (Static force)
         * Fsl = Usl * Fn (Dymanic force)
         * F = Fapp - Ffr --> Fapp = -Ffr - F
         * acc = (Fapp - Usl * MG) / m
         */
        /*
       var Fst = surType.getStaticFiction() * velocity.magnitude;
       var Fsl = surType.getDynamicFiction() * velocity.magnitude;
       */

        spd = rb.velocity.magnitude;
        if (spd > 0)
            movetime += Time.fixedDeltaTime;
        dist += Vector3.Distance(transform.position, lastpos);
        disp = Vector3.Distance(pos, transform.localPosition);

        lastvelo = rb.velocity;
        lastpos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
         * Using f = u * m * 9.81f
         * 
         * F static = surType.getStaticFiction() * mass * 9.81f; 
         * F static use u of item that is below.
         * 
         * F slidding = surType.getDynamicFiction() * mass * 9.81f;
         * F slidding use u of item that is moving.
         */
        if (!collidedObjects.Contains(collision.collider) && collision.gameObject.tag == "ItemObject")
        {
            AudioSource.PlayClipAtPoint(surType.getSound(), transform.position, 1f);
            mass = gameObject.GetComponent<Rigidbody>().mass;
            if(collision.gameObject.transform.localPosition.y < transform.localPosition.y)
                Fst = collision.gameObject.GetComponent<Collider>().material.staticFriction * mass * 9.81f;
            Fsl = surType.getDynamicFiction() * mass * 9.81f;//currently using only u of itself to cal.
            collidedObjects.Add(collision.collider);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        collidedObjects.Remove(collision.collider);
        if(collidedObjects.Count == 0)
        {
            Fst = 0;
            Fsl = 0;
        }
    }

    public void Notify(object sender, System.EventArgs e)
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
            if (col.CompareTag("ItemObject") && col.transform != transform)
            {
                IsOverlap = true;
                ItemCon.changeGrowColor();
                break;
            }

            if (col == last)
            {
                IsOverlap = false;
                ItemCon.changeGrowColor();
            }
        }
    }

    public void saveCurrentState()
    {
        pos = transform.localPosition;
        rot = transform.localRotation;
        rb.velocity = velocity;
        rb.angularVelocity = Vector3.zero;

        lastvelo = velocity;
        lastpos = pos;
    }

    public void returnState()
    {
        transform.localPosition = pos;
        transform.localRotation = rot;
        rb.velocity = velocity;
        rb.angularVelocity = Vector3.zero;

        acc = 0;
        spd = 0;
        movetime = 0;
        dist = 0;
        disp = 0;
    }

    public SurfaceType getSurType() { return surType; }
    public void setSurType(string st_name)
    {
        surType = ItemCon.getFactory().getSurType(st_name);
        BaseMat = surType.getSurMat();
        PhyMat = surType.getPhyMat();
        ItemCon.changeGrowMaterialTexture();
    }
    public bool IsKinematic { get { return kinematic; } set { kinematic = value; rb.isKinematic = value; } }
    public Material BaseMat { get; set; }
    public PhysicMaterial PhyMat { get { return GetComponent<Collider>().material; } set { GetComponent<Collider>().material = value; } }
    public Renderer BaseRenderer { get; set; }
    public bool IsOverlap { get; private set; }
    public Vector3 Velocity { get { return velocity; } set { velocity = value; rb.velocity = value; } }
    public string ItemType { get; set; }
}
