using System.Collections;
using UnityEngine;

public class CannonTargetController : MonoBehaviour
{
    public AudioClip hitSound;
    public ParticleSystem particle;

    /* Game Parameters */
    private int hitCnt = 0;

    /* Scripts */
    public CannonUIController UIController;

    /* Unity Functions */
    private void OnEnable()
    {
        if (PlayerPrefs.GetString("CannonShooterMode") != "AR")
        {
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(setNewPosition());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        hitCnt++;
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        particle.Play();

        if (PlayerPrefs.GetString("CannonShooterMode") != "AR")
        {
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            if (hitCnt == 5 && PlayerPrefs.GetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star") < 3)
                UIController.endStage();
            else
                StartCoroutine(setNewPosition());
        }
        else
        {

        }
    }

    /* My Functions */
    IEnumerator setNewPosition()
    {
        yield return new WaitForSeconds(3);
        if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
        {
            var z = PlayerPrefs.GetString("CannonShooterMode") == "Lv2" ? 58 : Random.Range(15, 95);
            var y = PlayerPrefs.GetString("CannonShooterMode") == "Lv1" ? -1.8f : Random.Range(-1.8f, 25 * (95 - z) / 95);

            switch (PlayerPrefs.GetString("CannonShooterMode"))
            {
                case "Lv1":
                    transform.position = new Vector3(0, -1.8f, z);
                    UIController.setTargetDetail(z - 8, 0);
                    break;
                case "Lv2":
                case "Lv3":
                    transform.position = new Vector3(0, y, z);
                    UIController.setTargetDetail(z - 8, 27 - y);
                    break;
            }
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }
    }
}
