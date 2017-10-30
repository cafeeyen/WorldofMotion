using UnityEngine;

public class GrowingLightEffect : MonoBehaviour
{
    private Material growMat;
    private Material parentMat;

	void Start ()
    {
        Renderer growRenderer = GetComponent<Renderer>();
        growMat = growRenderer.material;

        Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        parentMat = parentRenderer.material;
    }

	void Update ()
    {
        // Use parent color as emission color base
        Color baseColor = parentMat.color;
        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color finalColor = baseColor * emission;
        growMat.SetColor("_EmissionColor", finalColor);
	}
}
