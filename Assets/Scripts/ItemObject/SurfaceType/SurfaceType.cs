using UnityEngine;

public abstract class SurfaceType
{
    // Parameters
    protected string name = "";
    protected float fiction, flexibility;
    public Material surMat;

    // Methods
    public string getName() { return name; }
    public float getFiction() { return fiction; }
    public float getFlexibility() { return flexibility; }
}
