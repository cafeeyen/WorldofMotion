using UnityEngine;
using UnityEngine.UI;

public class LessonItem : MonoBehaviour
{
    public Text cntText,qNoText;
    public GameObject SuccessWords,bronze,silver,gold,fail,blueBox;
    public InputField veloZ;
    public Slider sliderBar;
    public Timer timer;
    public ProblemGenerator problemGenerator;
    public int speed = 2;

    private State state;
    private Rigidbody rb,rb2;
    private int cnt = 0, qCnt = 1;
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
        
        if (PlayerPrefs.GetInt("Lesson") == 1)
        {
            veloZ.onEndEdit.AddListener(changeVelocity);

            sliderBar.onValueChanged.AddListener(changeSlideValue);
        }
        else if (PlayerPrefs.GetInt("Lesson") == 2)
        {
            rb2 = blueBox.GetComponent<Rigidbody>();;
        }
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

    public void changePropertyMomentum()
    {
            try
            {
                rb.velocity = new Vector3(12 ,0 ,0);
                rb.mass = 30 ;
                rb2.mass = 70;
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
        if(PlayerPrefs.GetInt("Lesson") == 2)
        {
            reset();
            changePropertyMomentum();
        }
    }
    public void reset()
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb2.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(-0.85f, 0.5f, 6.34f);
        rb2.transform.position = new Vector3(1.46f, 0.5f, 6.34f);
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
            //cnt = 0;
        }
        cntText.text = "Problem Solved : " + cnt;
        transform.position = startPoint;
    }

    public void checkMomentumAns(int boxNum)
    {
        if(boxNum == problemGenerator.randomChoice)
        {
            cnt++;
            qCnt++;
            cntText.text = cnt.ToString();
            qNoText.text = qCnt.ToString();
        }
        else
        {
            qCnt++;
            qNoText.text = qCnt.ToString();
            //problemGenerator.newProblem();
        }

        //check quality
        if (cnt >= 5 && qCnt <= 6)
        {
            //gold
            SuccessWords.SetActive(true);
            gold.SetActive(true);
            gold.GetComponent<Animator>().SetBool("Show", true);
        }
        else if (cnt >= 5 && qCnt <= 8)
        {
            //silver
            SuccessWords.SetActive(true);
            silver.SetActive(true);
            silver.GetComponent<Animator>().SetBool("Show", true);
        }
        else if (cnt >= 5 && qCnt <= 10)
        {
            //bronze
            SuccessWords.SetActive(true);
            bronze.SetActive(true);
            bronze.GetComponent<Animator>().SetBool("Show", true);
        }
        else if (qCnt > 10)
        {
            SuccessWords.SetActive(true);
            fail.SetActive(true);
            fail.GetComponent<Animator>().SetBool("Show", true);
        }
        else
        {
            problemGenerator.newProblem();
        }
    }

    public void Replay()
    {
        if(PlayerPrefs.GetInt("Lesson") == 1)
        {
            SuccessWords.SetActive(false);
            cnt = 0;
            cntText.text = "Problem Solved : " + cnt;
        }
        else if (PlayerPrefs.GetInt("Lesson") > 1)
        {
            SuccessWords.SetActive(false);
            gold.SetActive(false);
            silver.SetActive(false);
            bronze.SetActive(false);
            cnt = 0;
            qCnt = 1;
            cntText.text = cnt.ToString();
            qNoText.text = qCnt.ToString();
        }

    }

    public void speedUp()
    {
        Time.timeScale = speed;
    }
}

