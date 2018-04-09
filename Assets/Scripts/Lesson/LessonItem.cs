using UnityEngine;
using UnityEngine.UI;

public class LessonItem : MonoBehaviour
{
    public Text cntText, qNoText;
    public GameObject SuccessWords, bronze, silver, gold, fail;
    public ProblemGenerator problemGenerator;
    private int cnt = 0, qCnt = 1;

    void FixedUpdate() { }

    public void checkAns(int boxNum)
    {
        if (boxNum == problemGenerator.trueAnswer)
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

