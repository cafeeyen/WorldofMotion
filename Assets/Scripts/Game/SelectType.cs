using UnityEngine;

public class SelectType : MonoBehaviour
{
    public GameObject player;

    private SurfaceType curType;
    private SurfaceTypeFactory typeFac;

    private void OnEnable()
    {
        typeFac = new SurfaceTypeFactory();
        curType = typeFac.getSurType("Wood");
    }

    public void changeType(string name)
    {
        if(name != curType.getName())
        {
            Debug.Log("nya");
            curType = typeFac.getSurType(name);
            player.GetComponent<Collider>().material = curType.getPhyMat();
            player.GetComponent<Renderer>().material = curType.getSurMat();
        }
    }
}
