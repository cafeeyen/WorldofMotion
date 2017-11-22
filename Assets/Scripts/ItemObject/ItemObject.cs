using UnityEngine;
using TouchScript.Gestures;

public class ItemObject : MonoBehaviour // Subject for ItemObjectController
{
    private ItemObjectController ItemCon;
    private SurfaceType surType;
    private int mass;
    private bool gravity, gyro, breakable, player;
    private Renderer baseRenderer;
    private Material baseMat;
    private TapGesture gesture;

    void Start ()
    {
        ItemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        surType = ItemCon.getFactory().getSurType("Wood");
        gravity = false;
        gyro = false;
        player = false;
        breakable = false;

        baseRenderer = transform.GetComponent<Renderer>();
        baseMat = baseRenderer.material;
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
    }

    public string getSurType() { return surType.getName(); }
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
}
