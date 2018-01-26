using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private UIController UICon;
    private ItemObjectController itemCon;
    private List<GameObject> itemList;

    private void OnEnable()
    {
        if (itemList == null)
            itemList = new List<GameObject>();

        itemCon = GameObject.Find("ItemObjectController").GetComponent<ItemObjectController>();
        itemCon.setList(itemList);

        UICon = GameObject.Find("Canvas").GetComponent<UIController>();
        UICon.setWorld(this);
    }

    public void saveState()
    {
        foreach (GameObject item in itemList)
        {
            item.GetComponent<ItemObject>().saveCurrentState();
        }
    }

    public void loadState()
    {
        foreach (GameObject item in itemList)
        {
            item.GetComponent<ItemObject>().returnState();
        }
    }

    public List<GameObject> getItemList()
    {
        return itemList;
    }

    public void setItemList(List<GameObject> list)
    {
        itemList = list;
    }

}
