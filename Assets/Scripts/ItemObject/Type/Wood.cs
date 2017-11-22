using UnityEngine;

public class Wood : SurfaceType
{
    public Wood() { }

    private void OnEnable()
    {
        typeName = "Wood";
        setBaseValue();
    }
}
