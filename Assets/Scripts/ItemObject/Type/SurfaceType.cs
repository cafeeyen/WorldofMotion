using UnityEngine;

public abstract class SurfaceType : ScriptableObject
{
    // Parameters
    protected string typeName = "";
    protected Material surMat;
    protected PhysicMaterial phyMat;
    protected AudioClip collideSound;

    // Methods
    protected void setBaseValue()
    {
        surMat = (Material)Resources.Load("Materials/" + typeName + "Mat", typeof(Material));
        phyMat = (PhysicMaterial)Resources.Load("PhysicMaterials/" + typeName + "Phy", typeof(PhysicMaterial));
        collideSound = (AudioClip)Resources.Load("Audios/" + typeName + "Sound", typeof(AudioClip));
    }

    public string getName() { return typeName; }
    public float getDynamicFiction() { return phyMat.dynamicFriction; }
    public float getStaticFiction() { return phyMat.staticFriction; }
    public Material getSurMat() { return surMat; }
    public AudioClip getSound() { return collideSound; }
}
