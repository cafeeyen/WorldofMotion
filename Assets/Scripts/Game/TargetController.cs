using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public TargetDetail target;
    public ParticleSystem sparkle;
    public GameObject cannon, star1, star2, star3, overlay, particle,levelbtt, homebtt;

    private AudioClip HitEffect;
    private SkinnedMeshRenderer meshRen;
    private int hit = 0;
    private Animator star;

    private void OnEnable()
    {
        HitEffect = (AudioClip)Resources.Load("Audios/TargetHit", typeof(AudioClip));
        if (PlayerPrefs.GetInt("CannonShooterMode") != 4)
        {
            meshRen = GetComponentInChildren<SkinnedMeshRenderer>();
            meshRen.enabled = false;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(setNewPosition());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall")
        {
            if(PlayerPrefs.GetInt("CannonShooterMode") != 4 && target.Detected)
            {
                AudioSource.PlayClipAtPoint(HitEffect, Camera.main.transform.position);
                sparkle.Play();
                meshRen.enabled = false;
                GetComponent<Collider>().enabled = false;
                hit++;
                if (hit < 5 || PlayerPrefs.GetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star") == 3)
                    StartCoroutine(setNewPosition());
                else
                {
                    cannon.SetActive(false);
                    PlayerPrefs.SetInt("CSLv" + (PlayerPrefs.GetInt("CannonShooterMode") + 1), 1);
                    var cnt = cannon.GetComponent<CannonController>().getShootCnt();
                    if (cnt == 5)
                    {
                        star3.SetActive(true);
                        star = star3.GetComponent<Animator>();
                        PlayerPrefs.SetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star", 3);
                    }
                    else if (cnt <= 10)
                    {
                        star2.SetActive(true);
                        star = star2.GetComponent<Animator>();
                        if (PlayerPrefs.GetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star") < 2)
                            PlayerPrefs.SetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star", 2);
                    }
                    else
                    {
                        star1.SetActive(true);
                        star = star1.GetComponent<Animator>();
                        if (PlayerPrefs.GetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star") < 1)
                            PlayerPrefs.SetInt("CsLv" + PlayerPrefs.GetInt("CannonShooterMode") + "Star", 1);
                    }
                    overlay.SetActive(true);
                    star.SetBool("Show", true);
                    levelbtt.SetActive(true);
                    homebtt.SetActive(true);
                    particle.SetActive(true);
                }
            }
            else
            {
                AudioSource.PlayClipAtPoint(HitEffect, this.transform.position);
                sparkle.Play();
            }
        }
    }

    IEnumerator setNewPosition()
    {
        yield return new WaitForSeconds(3);
        if(PlayerPrefs.GetInt("CannonShooterMode") != 5)
        {
            var z = PlayerPrefs.GetInt("CannonShooterMode") == 2 ? 55.5f : Random.Range(15, 95);
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
            GetComponent<Collider>().enabled = true;
            cannon.GetComponent<CannonController>().setHeight(cannon.transform.position.y - transform.position.y);
        }
    }
}
