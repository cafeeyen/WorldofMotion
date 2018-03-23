using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public LessonItem item;
    public Text timer;

    private float time;

    private void OnEnable()
    {
        time = 0;
    }

    void FixedUpdate ()
    {
        time -= Time.deltaTime;
        float minutes = time / 60;
        float seconds = time % 60;
        float fraction = (time * 100) % 100;
        timer.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);

        if (time <= 0)
        {
            Time.timeScale = 0;
            item.checkAns();
        }
    }

    public void setTimer(float second)
    {
        timer.text = "00 : 00 : 000";
        time = second;
        float minutes = time / 60;
        float seconds = time % 60;
        float fraction = (time * 100) % 100;
        timer.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
    }
}
