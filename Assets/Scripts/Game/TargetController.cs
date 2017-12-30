using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public Animator hitText;

    private void OnEnable()
    {
        hitText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.height/2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CannonBall")
        {
            StartCoroutine(showHitText());
        }
    }

    IEnumerator showHitText()
    {
        hitText.gameObject.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        hitText.SetBool("Hit", true);
        yield return new WaitForSeconds(1);
        hitText.SetBool("Hit", false);
        yield return new WaitForSeconds(0.2f);
        hitText.gameObject.GetComponent<Image>().enabled = false;
    }
}
