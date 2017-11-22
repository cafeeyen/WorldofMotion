using UnityEngine;

public class Rubber : SurfaceType
{
    public Rubber() { }

    private void OnEnable()
    {
        typeName = "Rubber";
        setBaseValue();
    }
}
