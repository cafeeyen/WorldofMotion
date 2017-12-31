using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public GameObject target;
    public Transform sparkle;
    public Transform hitWord;

    private AudioSource audioSource;
    private AudioClip HitEffect;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        HitEffect = (AudioClip)Resources.Load("Audios/TargetHit", typeof(AudioClip));
        sparkle.GetComponent<ParticleSystem>().enableEmission = false;
        hitWord.GetComponent<ParticleSystem>().enableEmission = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall" && target.GetComponent<TargetDetail>().Detected)
        {
            AudioSource.PlayClipAtPoint(HitEffect, this.transform.position);
            hitWord.GetComponent<ParticleSystem>().enableEmission = true;
            sparkle.GetComponent<ParticleSystem>().enableEmission = true;
            
            StartCoroutine(showHitText());
        }
    }

    IEnumerator showHitText()
    {
        yield return new WaitForSeconds(0.1f);
        //set sparkle off
        hitWord.GetComponent<ParticleSystem>().enableEmission = false;
        sparkle.GetComponent<ParticleSystem>().enableEmission = false;
    }
}
