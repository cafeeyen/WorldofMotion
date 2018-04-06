using UnityEngine;
using UnityEngine.UI;

public class LessonItem : MonoBehaviour
{
    public Text cntText,qNoText;
    public GameObject SuccessWords,bronze,silver,gold,fail,blueBox;
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

        if (PlayerPrefs.GetInt("Lesson") == 2)
        {
            rb2 = blueBox.GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate(){}

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

    public void checkAns(int boxNum)
    {
        if(boxNum == problemGenerator.trueAnswer)
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
        }

        //check quality
        if (cnt >= 5 && qCnt <= 6)
        {
            //Gold
            SuccessWords.SetActive(true);
            gold.SetActive(true);
            gold.GetComponent<Animator>().SetBool("Show", true);
        }
        else if (cnt >= 5 && qCnt <= 8)
        {
            //Silver
            SuccessWords.SetActive(true);
            silver.SetActive(true);
            silver.GetComponent<Animator>().SetBool("Show", true);
        }
        else if (cnt >= 5 && qCnt <= 11)
        {
            //Bronze
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
            problemGenerator.newProblem();
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
}

