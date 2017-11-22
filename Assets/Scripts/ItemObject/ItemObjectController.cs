using UnityEngine;

public class ItemObjectController : MonoBehaviour
{
    public Material growMat;
    public GameObject axisTransition;

    // We control only one ItemObject at the same time
    // Share with PropWindow, UIController and AxisTransition
    private GameObject itemObject;
    private ItemObject itemObjectSc;
    private PropWindow propWin;
    private UIController UICon;
    private static SurfaceTypeFactory surTypeFac;

    private void Start()
    {
        propWin = GameObject.Find("PropWindow").GetComponent<PropWindow>();
        UICon = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    private void Update()
    {
        if(itemObject != null)
        {
            // Use base color as emission base color
            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color finalColor = itemObjectSc.BaseMat.color * emission;
            growMat.SetColor("_EmissionColor", finalColor);
        }
    }

    public void setItemObject(GameObject selectedItemObject)
    {
        // Make sure this is not duplicate tap to same object
        if (itemObject != selectedItemObject)
        {
            // Reset old ItemObject to base material | disable drag and drop
            if (itemObject != null)
            {
                itemObjectSc.BaseRenderer.material = itemObjectSc.BaseMat;
                itemObject.GetComponent<DragNDrop>().enabled = false;
            }

            itemObject = selectedItemObject;
            itemObjectSc = itemObject.GetComponent<ItemObject>();

            changeGrowMaterialTexture();

            // Change material to grow material | enable drag and drop
            itemObjectSc.BaseRenderer.material = growMat;
            itemObject.GetComponent<DragNDrop>().enabled = true;
            itemObject.GetComponent<DragNDrop>().setAxisTransition(axisTransition);

            // Set active and update axis position
            axisTransition.SetActive(true);
            axisTransition.GetComponent<AxisTransition>().setItemObject(itemObject);

            // Trigger change state in PropWindow
            propWin.setPropValue(itemObject);

            // Send object to UI Controller
            UICon.setItemObject(itemObject);
        }
    }

    public void DestroyItemObject()
    {
        if (itemObject != null)
        {
            axisTransition.SetActive(false);
            Destroy(itemObject);
        }
    }

    public SurfaceTypeFactory getFactory()
    {
        if(surTypeFac == null)
            surTypeFac = new SurfaceTypeFactory();
        return surTypeFac;
    }

    public void changeGrowMaterialTexture()
    {
        // Get material from Itemobject and set color to grow material
        growMat.SetTexture("_MainTex", itemObjectSc.BaseMat.mainTexture);
        growMat.SetColor("_Color", new Color(itemObjectSc.BaseMat.color.r, itemObjectSc.BaseMat.color.g, itemObjectSc.BaseMat.color.b, itemObjectSc.BaseMat.color.a));
    }
}
