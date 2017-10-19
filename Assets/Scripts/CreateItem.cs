using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
    public GameObject item;

    public void createItem()
    {
        Debug.Log("Nta 5555");
        Instantiate(item, Input.mousePosition, new Quaternion(0, 0, 0, 0));
    }
}
