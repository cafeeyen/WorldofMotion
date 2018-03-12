using System.Collections;
using UnityEngine;

public class CannonTargetController : MonoBehaviour
{
    public AudioClip hitSound;

    private SkinnedMeshRenderer meshRen;

    /* Unity Functions */
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("CannonShooterMode") != 5)
        {
            meshRen = GetComponentInChildren<SkinnedMeshRenderer>();
            meshRen.enabled = false;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(setNewPosition());
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    /* My Functions */
    IEnumerator setNewPosition()
    {
        yield return new WaitForSeconds(3);
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
        //meshRen.enabled = true;
        //GetComponent<Collider>().enabled = true;
        //cannon.GetComponent<CannonController>().setHeight(cannon.transform.position.y - transform.position.y);
    }
}
