﻿using UnityEngine;

public class ItemObjectController : MonoBehaviour
{
    public GameObject axisTransition;
    public Animator propBar;

    // We control only one ItemObject at the same time
    // Share with PropWindow, UIController and AxisTransition
    private GameObject itemObject;
    private ItemObject itemObjectSc;
    private PropWindow propWin;
    private UIController UICon;
    private static SurfaceTypeFactory surTypeFac;
    private float emission;
    private Color finalColor, growColor;
    private Material growMat;

    private void Awake()
    {
        propWin = GameObject.Find("PropWindow").GetComponent<PropWindow>();
        UICon = GameObject.Find("Canvas").GetComponent<UIController>();
        growMat = (Material)Resources.Load("Materials/GrowMat", typeof(Material));
    }

    private void Update()
    {
        if (itemObject != null)
        {
            // Use base color as emission base color
            emission = Mathf.PingPong(Time.unscaledTime, 1.0f);
            finalColor = growColor * emission;
            growMat.SetColor("_EmissionColor", finalColor);
        }
    }

    public void setItemObject(GameObject selectedItemObject)
    {
        /*
         * ----- Brief description -----
         * 1. Check new item is not current item
         *      1.1. Yes -> Go to 2.
         *      1.2. No -> Don othing
         *      
         * 2. Check this item is current item?
         *      2.1. Yes -> Go to 3.
         *      2.2. No -> Cancle select this item
         *      
         * 3. Check we have current item?
         *      3.1. Yes -> Reset current item to normal state, then go to 4.
         *      3.2. No -> Do nothing
         * 
         * 4. Set new item to current item  
         *      
         * 5. Check app is in play mode?
         *      5.1. Yes -> Skip to 7.
         *      5.2. No -> Go to 6.
         * 
         * 6. Set current item to selected mode and notify propWin/UICon that we have new one
         * 
         * 7. Set item material to GrowMat, tell user that he/she select this one
         */

        // Check if current object is not overlapping of no object is selected
        if (itemObjectSc == null || !itemObjectSc.IsOverlap)
        {
            // If tap new object -> select new object | tap old object -> cancle select this object
            if (itemObject != selectedItemObject)
            {
                // Reset old ItemObject to base material | disable drag and drop
                if (itemObject != null)
                    cancleSelectObject();

                itemObject = selectedItemObject;
                itemObjectSc = itemObject.GetComponent<ItemObject>();

                if (UICon.state == UIController.mode.Edit)
                {
                    itemObjectSc.checkCollider();

                    // Enable DragNDrop
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
                changeGrowMaterialTexture();
                changeGrowColor();
                itemObjectSc.BaseRenderer.material = growMat;
            }
            else
            {
                cancleSelectObject();
                UICon.setItemObject(null);
                UICon.displayWindows(propBar);
                itemObject = null;
            }
        }
    }

    public void DestroyItemObject()
    {
        if (itemObject != null)
        {
            cancleSelectObject();
            Destroy(itemObject);
        }
    }

    private void cancleSelectObject()
    {
        itemObjectSc.BaseRenderer.material = itemObjectSc.BaseMat;
        itemObject.GetComponent<DragNDrop>().enabled = false;
        axisTransition.SetActive(false);
    }

    public SurfaceTypeFactory getFactory()
    {
        if (surTypeFac == null)
            surTypeFac = new SurfaceTypeFactory();
        return surTypeFac;
    }

    public void changeGrowMaterialTexture()
    {
        // Get material from Itemobject and set color to grow material
        growMat.SetTexture("_MainTex", itemObjectSc.BaseMat.mainTexture);
        growMat.SetColor("_Color", new Color(itemObjectSc.BaseMat.color.r, itemObjectSc.BaseMat.color.g, itemObjectSc.BaseMat.color.b, itemObjectSc.BaseMat.color.a));
    }

    public void changeGrowColor()
    {
        if (itemObjectSc.IsOverlap)
            growColor = Color.red;
        else
            growColor = itemObjectSc.BaseMat.color;
    }

    public GameObject getCurrentObject() { return itemObject; }
}
