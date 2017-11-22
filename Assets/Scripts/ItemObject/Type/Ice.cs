using UnityEngine;

public class Ice : SurfaceType
{
    public Ice() {}

    private void OnEnable()
    {
        typeName = "Ice";
        setBaseValue();
    }
}
