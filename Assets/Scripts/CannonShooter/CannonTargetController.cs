using System.Collections;
using UnityEngine;

public class CannonTargetController : MonoBehaviour
{
    /* Game Object */
    public GameObject ARTarget;
    public MeshRenderer targetMesh;

    /* Game Parameters */
    private int hitCnt = 0;

    /* Effect */
    public AudioClip hitSound;
    public ParticleSystem particle;

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
    private void Update()
    {
        if(PlayerPrefs.GetString("CannonShooterMode") == "AR")
        {
            if(targetMesh.enabled)
                UIController.setTargetDetail(ARTarget.transform.position.z - 8, ARTarget.transform.position.y - -1.8f);
            else
                UIController.setTargetDetail(-1, -1);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        particle.Play();

        if (PlayerPrefs.GetString("CannonShooterMode") != "AR")
        {
            hitCnt++;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            if (hitCnt == 5 && PlayerPrefs.GetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star") < 3)
                UIController.endStage();
            else
                StartCoroutine(setNewPosition());
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
    public void setShootedTargetPosition()
    {
        if(targetMesh.enabled)
            transform.position = ARTarget.transform.position;
        else
            transform.position = Vector3.forward * -100;
    }
}
