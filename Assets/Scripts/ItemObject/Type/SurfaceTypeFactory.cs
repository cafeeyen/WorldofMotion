using UnityEngine;

public class SurfaceTypeFactory
{
    SurfaceType st_wood = Wood.CreateInstance<Wood>();
    SurfaceType st_ice = Ice.CreateInstance<Ice>();
    SurfaceType st_metal = Metal.CreateInstance<Metal>();
    SurfaceType st_rubber = Rubber.CreateInstance<Rubber>();

    public SurfaceType getSurType(string name)
    {
        switch (name)
        {
            case "Wood": return st_wood;
            case "Ice": return st_ice;
            case "Metal": return st_metal;
            case "Rubber": return st_rubber;
            default: return st_wood;// Set Wood as default
        }
    }
}
