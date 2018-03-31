using UnityEngine;
using UnityEngine.UI;

public class ProblemGenerator : MonoBehaviour
{
    public LessonItem item;
    public Text distanceText;
    public Text QuestionText;
    public Text[] ansText;
    public int randomChoice;

    private float distance, time;
    private float mass1, mass2, vBefore1, vAfter2,answer;
    private float[] falseAnswer= { 0,0,0 };
    private int cntAnswer=0;

	public void newProblem()
    {
        if(PlayerPrefs.GetInt("Lesson") == 1)
        {
            distance = Random.Range(10, 100);
            time = Random.Range(5, 20);

            item.setTime(time);
            transform.position = new Vector3(0, 0, distance);
            distanceText.text = distance.ToString();
        }
        if (PlayerPrefs.GetInt("Lesson") == 2)
        {
            mass1 = Random.Range(5, 100);
            mass2 = Random.Range(5, 100);
            vBefore1 = Random.Range(5, 25);
            vAfter2 = Random.Range(2, 10);
            answer = ((mass1 * vBefore1) + 0 - (mass2 * vAfter2)) / mass1;
            falseAnswer[0] = answer + Random.Range(5, 200);
            falseAnswer[1] = Mathf.Abs(answer - Random.Range(5, 200));
            falseAnswer[2] = answer * Random.Range(2, 5);
            randomChoice = Random.Range(0, 4);
            //if (randomChoice == 0 ) { randomChoice = 1; }
            QuestionText.text = "มีกล่องสีแดงและสีฟ้า สีแดงมวล " + mass1 + " กรัมพุ่งมาด้วยความเร็ว " + vBefore1 + " m/s ใส่กล่องสีฟ้ามวล " + mass2 + " กรัมที่ตั้งอยู่จนมีความเร็ว " + vAfter2 + " m / s หลังกระทบกล่องสีแดงมีความเร็วเท่าไหร่ ? ";
            for (int i = 0;i<4; i++)
            {
                if(i == randomChoice)
                {
                    ansText[i].text = answer.ToString();
                    Debug.Log("this is correct ans at i =" + i);
                }
                else
                {
                    ansText[i].text = falseAnswer[cntAnswer].ToString();
                    cntAnswer++;
                }               
            }
            cntAnswer = 0;
            //Add set items.
        }

    }

    public float getVbefore(){return vBefore1;}
    public float getmass1() { return mass1; }
    public float getmass2() { return mass2; }
}
