using System;
using UnityEngine;

public class ItemObjectController : Observer
{
    public Material growMat;
    public UIController UIcon;
    public PropWindow propWin;
    public GameObject axisTransition;

    private GameObject ItemObject;
    private Renderer baseRenderer;
    private Material baseMat;

    private void Update()
    {
        if(ItemObject != null)
        {
            // Use base color as emission base color
            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color finalColor = baseMat.color * emission;
            growMat.SetColor("_EmissionColor", finalColor);
        }
    }

    public override void selectedItemObject(GameObject itemObject)
    {
        /*** Can select Itemobject only 1 at same time ***/

        // Make sure this is not duplicate tap to same object
        if (ItemObject != itemObject)
        {
            // Reset to base material | disable drag and drop
            if (ItemObject != null)
            {
                baseRenderer.material = baseMat;
                ItemObject.GetComponent<DragNDrop>().enabled = false;
            }

            // Get material from Itemobject and set color to grow material
            baseRenderer = itemObject.transform.GetComponent<Renderer>();
            baseMat = baseRenderer.material;
            growMat.SetTexture("_MainTex", baseMat.mainTexture);
            growMat.SetColor("_Color", new Color(baseMat.color.r, baseMat.color.g, baseMat.color.b, baseMat.color.a));

            // Change material to grow material | enable drag and drop
            ItemObject = itemObject;
            baseRenderer.material = growMat;
            itemObject.GetComponent<DragNDrop>().enabled = true;
            itemObject.GetComponent<DragNDrop>().setAxisTransition(axisTransition);
            axisTransition.SetActive(true);
            /* Axis scale not ready yet*/
            //axisTransition.transform.localScale = itemObject.transform.localScale / 10;
            axisTransition.GetComponent<AxisTransition>().setSelectedItemObject(itemObject);
        }
    }
}
