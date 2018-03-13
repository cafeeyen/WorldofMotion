using UnityEngine;
using UnityEngine.UI;

public class LessonItem : MonoBehaviour
{
    public Text cntText;
    public GameObject SuccessWords;
    public InputField veloZ;
    public Slider sliderBar;
    public Timer timer;
    public ProblemGenerator problemGenerator;
    public int speed = 2;

    private State state;
    private Rigidbody rb;
    private int cnt = 0;
    private bool check = false;
    private float time;
    private Vector3 startPoint = new Vector3(0, 0.5f, 0);

    enum State
    {
        Play,
        Stop
    }

    void OnEnable()
    {
        state = State.Stop;
        rb = GetComponent<Rigidbody>();

        veloZ.onEndEdit.AddListener(changeVelocity);

        sliderBar.onValueChanged.AddListener(changeSlideValue);
    }

    void FixedUpdate(){}

    private void changeSlideValue(float value)
    {
        veloZ.text = sliderBar.value.ToString();
        rb.velocity = new Vector3(0, 0, sliderBar.value);
    }

    private void changeVelocity(string value)
    {
        if (string.IsNullOrEmpty(veloZ.text))
            veloZ.text = "0";
        try
        {
            var z = Mathf.Clamp(float.Parse(veloZ.text), -30.0f, 30.0f);
            veloZ.text = z.ToString();

            rb.velocity = new Vector3(0, 0, z);
        }
        catch
        {
            Debug.Log("Power input error.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == State.Play && other.name == "CheckPoint")
            check = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (state == State.Play && other.name == "CheckPoint")
            check = false;
    }

    public void Play()
    {
        state = State.Play;
        Time.timeScale = 1;
    }

    public void setTime(float second)
    {
        time = second;
        timer.setTimer(time);
    }

    public void checkAns()
    {
        state = State.Stop;
        if (check)
        {
            cnt++;
            if (cnt == 5 && PlayerPrefs.GetInt("LessonMotion") == 0)
            {
                PlayerPrefs.SetInt("LessonMotion", 1);
                //Open UI
                SuccessWords.SetActive(true);
            }
            else
                problemGenerator.newProblem();
        }
        else
        {
            timer.setTimer(time);
            cnt = 0;
        }
        cntText.text = "Problem Solved : " + cnt;
        transform.position = startPoint;
    }

    public void Replay()
    {
        SuccessWords.SetActive(false);
        cnt = 0;
        cntText.text = "Problem Solved : " + cnt;
    }

    public void speedUp()
    {
        Time.timeScale = speed;
    }
}

