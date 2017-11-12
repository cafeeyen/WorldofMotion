using UnityEngine;

public class ItemObjectController : MonoBehaviour
{
    public Material growMat;
    public UIController UIcon;
    public PropWindow propWin;

    private GameObject axisTransition;
    private GameObject selectedItemObject;
    private Renderer baseRenderer;
    private Material baseMat;

    private void Start()
    {
        axisTransition = transform.Find("Axis").gameObject;
    }

    private void Update()
    {
        if(selectedItemObject != null)
        {
            // Use base color as emission base color
            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color finalColor = baseMat.color * emission;
            growMat.SetColor("_EmissionColor", finalColor);
        }
    }

    private void tapHandler(object sender, System.EventArgs e)
    {
        baseRenderer = transform.GetComponent<Renderer>();
        baseMat = baseRenderer.material;
        growMat.SetTexture("_MainTex", baseMat.mainTexture);
        growMat.SetColor("_Color", new Color(baseMat.color.r, baseMat.color.g, baseMat.color.b, baseMat.color.a));
        
    }

    public void setSelectedItemObject(GameObject itemObject)
    {
        /*** Can select Itemobject only 1 at same time ***/
        
        // Make sure this is not duplicate tap to same object
        if (selectedItemObject != itemObject)
        {
            // Reset to base material | disable drag and drop
            if (selectedItemObject != null)
            {
                baseRenderer.material = baseMat;
                selectedItemObject.GetComponent<DragNDrop>().enabled = false;
            }

            // Get material from Itemobject and set color to grow material
            baseRenderer = itemObject.transform.GetComponent<Renderer>();
            baseMat = baseRenderer.material;
            growMat.SetTexture("_MainTex", baseMat.mainTexture);
            growMat.SetColor("_Color", new Color(baseMat.color.r, baseMat.color.g, baseMat.color.b, baseMat.color.a));

            // Change material to grow material | enable drag and drop
            selectedItemObject = itemObject;
            baseRenderer.material = growMat;
            itemObject.GetComponent<DragNDrop>().enabled = true;
            itemObject.GetComponent<DragNDrop>().setAxisTransition(axisTransition);
            axisTransition.SetActive(true);
            /* Axis scale not ready yet*/
            //axisTransition.transform.localScale = itemObject.transform.localScale / 10;
            axisTransition.GetComponent<AxisTransition>().setSelectedItemObject(itemObject);
            UIcon.setSelectedItemObject(itemObject);
            propWin.setSelectedItemObject(itemObject);
        }
    }
}
