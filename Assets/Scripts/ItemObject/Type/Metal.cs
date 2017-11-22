using UnityEngine;

public class Metal : SurfaceType
{
    public Metal() {}

    private void OnEnable()
    {
        typeName = "Metal";
        setBaseValue();
    }
}
