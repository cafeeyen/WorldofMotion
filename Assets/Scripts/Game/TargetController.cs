using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public TargetDetail target;
    public ParticleSystem sparkle;
    public GameObject cannon;

    private AudioClip HitEffect;
    private SkinnedMeshRenderer meshRen;
    private int hit = 0;

    private void OnEnable()
    {
        HitEffect = (AudioClip)Resources.Load("Audios/TargetHit", typeof(AudioClip));
        meshRen = GetComponentInChildren<SkinnedMeshRenderer>();
        StartCoroutine(setNewPosition());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall" && target.Detected)
        {
            AudioSource.PlayClipAtPoint(HitEffect, this.transform.position);
            sparkle.Play();
            meshRen.enabled = false;
            hit++;
            if (hit < 5)
                StartCoroutine(setNewPosition());
            else
                Debug.Log("Finish level");
            /*
             * Plan
             * 1. Show score(star)
             * 2. Send back to level selection
            */    
        }
    }

    IEnumerator setNewPosition()
    {
        yield return new WaitForSeconds(3);
        if(PlayerPrefs.GetInt("CannonShooterMode") != 5)
        {
            var z = PlayerPrefs.GetInt("CannonShooterMode") == 2 ? 60 : Random.Range(15, 95);
            var y = PlayerPrefs.GetInt("CannonShooterMode") == 1 ? -1.8f : Random.Range(-1.8f, 25 * (95 - z) / 95);

            switch (PlayerPrefs.GetInt("CannonShooterMode"))
            {
                case 1:
                    transform.position = new Vector3(0, -1.8f, z);
                    break;
                case 2:
                    transform.position = new Vector3(0, y, z);
                    break;
                case 3:
                case 4:
                    transform.position = new Vector3(0, y, z);
                    break;
            }
            meshRen.enabled = true;
            cannon.GetComponent<CannonController>().setHeight(cannon.transform.position.y - transform.position.y);
        }
    }
}
