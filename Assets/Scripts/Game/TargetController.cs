using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject target;
    public Transform sparkle;
    public Transform hitWord;

    private AudioClip HitEffect;
    private ParticleSystem.EmissionModule sparkEm, hitEm;

    private void OnEnable()
    {
        HitEffect = (AudioClip)Resources.Load("Audios/TargetHit", typeof(AudioClip));
        sparkEm = sparkle.GetComponent<ParticleSystem>().emission;
        hitEm = hitWord.GetComponent<ParticleSystem>().emission;

        sparkEm.enabled = false;
        hitEm.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall" && target.GetComponent<TargetDetail>().Detected)
        {
            AudioSource.PlayClipAtPoint(HitEffect, this.transform.position);
            sparkEm.enabled = true;
            hitEm.enabled = true;

            StartCoroutine(showHitText());
        }
    }

    IEnumerator showHitText()
    {
        yield return new WaitForSeconds(0.1f);
        //set sparkle off
        sparkEm.enabled = false;
        hitEm.enabled = false;
    }
}
