using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public TargetDetail target;
    public ParticleSystem sparkle;

    private AudioClip HitEffect;
    private SkinnedMeshRenderer meshRen;
    private int step = 1;

    private void OnEnable()
    {
        HitEffect = (AudioClip)Resources.Load("Audios/TargetHit", typeof(AudioClip));
        meshRen = GetComponentInChildren<SkinnedMeshRenderer>();
        PlayerPrefs.SetInt("CannonShooterMode", 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.tag == "CannonBall" && target.Detected)
        {
            AudioSource.PlayClipAtPoint(HitEffect, this.transform.position);
            sparkle.Play();
            meshRen.enabled = false;
            step++;
            StartCoroutine(setNewPosition());               
        }
    }

    IEnumerator setNewPosition()
    {
        yield return new WaitForSeconds(3);
        var z = Random.Range(15, 95);
        var y = Random.Range(-1.8f, 30 * (95 - z) / 95);
        switch (PlayerPrefs.GetInt("CannonShooterMode"))
        {
            case 1:
            case 2:
                transform.position = new Vector3(0, -1.8f, z);
                break;

            case 3:
            case 4:
                transform.position = new Vector3(0, y, z);
                break;
        }
        meshRen.enabled = true;
    }
}
