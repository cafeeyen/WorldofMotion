using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private Text timer;
    private float time;

    private void OnEnable()
    {
        timer = GetComponent<Text>();
        time = 0;
    }

    void Update ()
    {
        time += Time.deltaTime;
        float minutes = time / 60;
        float seconds = time % 60;
        float fraction = (time * 100) % 100;
        timer.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
    }
}
