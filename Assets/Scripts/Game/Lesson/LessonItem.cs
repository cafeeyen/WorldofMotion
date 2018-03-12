using UnityEngine;
using UnityEngine.UI;

public class LessonItem : MonoBehaviour
{
    public Text cntText;
    public GameObject SuccessWords;
    public InputField veloX, veloY, veloZ;
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

        veloX.onEndEdit.AddListener(changeVelocity);
        veloY.onEndEdit.AddListener(changeVelocity);
        veloZ.onEndEdit.AddListener(changeVelocity);

        if(PlayerPrefs.GetInt("Lesson") == 1)
        {
            veloX.interactable = false;
            veloY.interactable = false;
        }
    }

    void FixedUpdate(){}

    private void changeVelocity(string value)
    {
        if (string.IsNullOrEmpty(veloX.text))
            veloX.text = "0";
        if (string.IsNullOrEmpty(veloY.text))
            veloY.text = "0";
        if (string.IsNullOrEmpty(veloZ.text))
            veloZ.text = "0";

        var test = 0;
        try
        {
            var x = Mathf.Clamp(float.Parse(veloX.text), -30.0f, 30.0f);
            test++;
            var y = Mathf.Clamp(float.Parse(veloY.text), -30.0f, 30.0f);
            test++;
            var z = Mathf.Clamp(float.Parse(veloZ.text), -30.0f, 30.0f);

            veloX.text = x.ToString();
            veloY.text = y.ToString();
            veloZ.text = z.ToString();

            rb.velocity = new Vector3(x, y, z);
        }
        catch
        {
            Debug.Log("Power input error.");
            if (test == 0)
                veloX.text = "0";
            else if (test == 1)
                veloY.text = "0";
            else
                veloZ.text = "0";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == State.Play && other.name == "CheckPoint")
            check = true;
        Debug.Log(check);
    }

    private void OnTriggerExit(Collider other)
    {
        if (state == State.Play && other.name == "CheckPoint")
            check = false;
        Debug.Log(check);
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
            cntText.text = "Problem Solved : " + cnt;
            problemGenerator.newProblem();

            if (cnt == 5)
            {
                PlayerPrefs.SetInt("LessonMotion", 1);
                //Open UI
                SuccessWords.SetActive(true);
            }
        }
        else
            cnt = 0;
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

