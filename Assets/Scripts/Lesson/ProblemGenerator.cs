using UnityEngine;
using UnityEngine.UI;

public class ProblemGenerator : MonoBehaviour
{
    public Text QuestionText, QWarningText;
    public Text[] ansText;
    public int trueAnswer, randomQuestion;

    private float distance, time;
    private float mass1, mass2, vBefore1, vBefore2, vAfter2, answer, mew, weight, gravity, acc;
    private float[] falseAnswer = { 0, 0, 0 };
    private int cntAnswer = 0;

    public void newProblem()
    {
        randomQuestion = Random.Range(0, 4);
        if (randomQuestion == 0) { randomQuestion = 1; }

        if (PlayerPrefs.GetInt("Lesson") == 1) // Force
        {
            switch (randomQuestion)
            {
                case 1:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(5, 25);
                    answer = mass1 * vBefore1;
                    QuestionText.text = "จงหาแรงที่จะทำให้วัตถุหนัก " + mass1 + " N เคลื่อนที่ด้วยความเร็ว " + vBefore1 + " m / s";
                    QWarningText.text = "*อย่าลืมแปลงหน่วยน้ำหนักจากNเป็นกิโลกรัมล่ะ (N / 9.8)";
                    break;

                case 2:
                    vBefore1 = Random.Range(5, 100);
                    vBefore2 = Random.Range(5, 100);
                    acc = Random.Range(2, 25);
                    answer = (vBefore2 / vBefore1) * acc;
                    QuestionText.text = "วัตถุหนึ่งถูกแรง " + vBefore1 + " N กระทำเกิดควมเร่ง " + acc + " m/s2 ถ้าวัตถุถูกกระทำด้วยแรง " + vBefore2 + " N จะเกิดควมเร่งเท่าใด?";
                    QWarningText.text = "*วัตถุมีน้ำหนักเท่าเดิมและเป็นชิ้นเดิม";
                    break;
                case 3:
                    vBefore1 = Random.Range(5, 100);
                    vBefore2 = Random.Range(5, 100);
                    mass1 = Random.Range(2, 30);
                    vAfter2 = Mathf.Pow(vBefore1, 2) + Mathf.Pow(vBefore2, 2);
                    answer = Mathf.Sqrt(vAfter2) / mass1;
                    QuestionText.text = "วัตถุหนึ่งมวล " + mass1 + " กิโลกรัมถูกแรง " + vBefore1 + " N กระทำจากทางซ้ายและถูกแรง " + vBefore2 + " N กระทำจากด้านล่าง วัตถุจะมีความเร่งเท่าไหร่?";
                    QWarningText.text = "*อย่าลืมหาแรงลัพธ์ด้วยสูตร (a^2 * b^2)^1/2 ก่อน";
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("Lesson") == 2) // Friction
        {
            switch (randomQuestion)
            {
                case 1:
                    vBefore1 = Random.Range(5, 150);
                    mew = Random.Range(0.1f, 1.1f);
                    answer = vBefore1 / mew;
                    QuestionText.text = "ออกแรง " + vBefore1 + " N ลากวัตถุไปตามพื้นราบถ้าสัมประสิทธิ์แรงเสียดทาน มีค่าเป็น " + mew.ToString("F2") + " วัตถุจะมีน้ำหนักเท่าไหร่ ?";
                    QWarningText.text = "";
                    break;

                case 2:
                    mass1 = Random.Range(5, 100);
                    mew = Random.Range(0.1f, 1.1f);
                    acc = Random.Range(2, 25);
                    answer = (mass1 * acc) + (mass1 * 9.8f * mew);
                    QuestionText.text = "วัตถุหนึ่งมีมวล " + mass1 + " กิโลกรัมถูกแรงกระทำเกิดความเร่ง " + acc + " m/s2 และสัมประสิทธิ์แรงเสียดทานระหว่างวัตถุกับพื้นคือ " + mew.ToString("F2") + " จงหาขนาดของแรงที่กระทำกับวัตถุ";
                    QWarningText.text = "*อย่าลืมลบแรงเสียดทานจากแรงทั้งหมด(F-แรงเสียดทาน = ma) และ N = mg";
                    break;
                case 3:
                    mass1 = Random.Range(5, 100);
                    answer = (mass1 * 9.8f) / 2;
                    QuestionText.text = "วัตถุหนึ่งมีมวล " + mass1 + " กิโลกรัมวางบนพื้นเอียง 30 องศากำลังจะไถลลง จงคำนวณหาแรงเสียดทานที่เกิดขึ้น";
                    QWarningText.text = "*อย่าลืมแตกแรงของน้ำหนัก(mg)เป็น mgsinθ ซึ่งมีทิศทางตรงกันข้ามกับแรงเสียดทาน";
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("Lesson") == 3) // Gravity + Motion
        {
            switch (randomQuestion)
            {
                case 1:
                    mass1 = Random.Range(5, 50);
                    weight = mass1 * 9.8f;
                    gravity = Random.Range(2.5f, 30.5f);
                    answer = mass1 * gravity;
                    QuestionText.text = "หากวัตถุมีน้ำหนัก " + weight + " กิโลกรัมบนโลก ถ้าไปอยู่บนดาวที่มีค่าแรงโน้มถ่วง ที่ " + gravity.ToString("F2") + " เมตรต่อวินาทีกำลัง2 จะมีน้ำหนักเท่าไหร่ ?";
                    QWarningText.text = "*อย่าลืมหามวล(m) ของวัตถุบนโลกจากน้ำหนัก(w) ก่อนหาคำตอบนะ";
                    break;

                case 2:
                    time = Random.Range(1.4f, 30.2f);
                    answer = 9.8f * time;
                    QuestionText.text = "วัตถุหนึ่งร่วงลงมาจากโต๊ะกระทบพื้นในเวลา " + time.ToString("F2") + " วินาที จงหาความเร็วของวัตถุขณะกระทบพื้น(g = 9.8 m/s2)";
                    QWarningText.text = "*คำใบ้ : ความเร็วต้นเป็น0 และ ใช้สูตรการเคลื่อนที่มาช่วย";
                    break;
                case 3:
                    vBefore1 = Random.Range(5, 100);
                    time = vBefore1 / 9.8f;
                    answer = (vBefore1 * time) + ((-9.8f * time * time) / 2);
                    QuestionText.text = "โยนวัตถุหนึ่งขึ้นด้วยความเร็วต้น " + vBefore1 + " m/s วัตถุจะขึ้นไปสูงสุดได้กี่เมตรก่อนตกลงมา?";
                    QWarningText.text = "*อย่าลืมหาเวลา ณ จุดสูงสุดก่อนหาระยะทางจากสูตรการเคลื่อนที่ และgมีค่าเป็นลบในที่นี้";
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("Lesson") == 4) // Momentum
        {
            switch (randomQuestion)
            {
                case 1:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore1 = Random.Range(5, 25);
                    vAfter2 = Random.Range(2, 10);
                    answer = ((mass1 * vBefore1) + 0 - (mass2 * vAfter2)) / mass1;
                    QuestionText.text = "มีกล่องสีแดงและสีฟ้า สีแดงมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยความเร็ว " + vBefore1 + " m/s ใส่กล่องสีฟ้ามวล " + mass2 + " กิโลกรัมที่ *ตั้งอยู่* จนมีความเร็ว " + vAfter2 + " m / s หลังกระทบกล่องสีแดงมีความเร็วเท่าไหร่ ? ";
                    break;
                case 2:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore1 = Random.Range(10, 25);
                    vBefore2 = Random.Range(1, 11);
                    answer = ((mass1 * vBefore1) + (mass2 * vBefore2)) / (mass1 + mass2);
                    QuestionText.text = "มีกล่องสีแดงและสีฟ้า สีแดงมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยความเร็ว " + vBefore1 + " m/s ใส่กล่องสีฟ้ามวล " + mass2 + " กิโลกรัมที่มีความเร็ว " + vBefore2 + " m / s ติดไปด้วยกันหลังกระทบกล่องสีแดงมีความเร็วเท่าไหร่ ? ";
                    break;
                case 3:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(100, 200);
                    answer = vBefore1 / mass1;
                    QuestionText.text = "มีกล่องมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยโมเมนตัม " + vBefore1 + " kg.m/s กล่องจะมีความเร็วเท่าไหร่ ? ";
                    break;
            }
        }
        createChoices();
    }

    private void createChoices()
    {
        for (int i = 0; i < 3; i++)
        {
            float ranAns = answer * (float)(Random.Range(-15, 16) / 10.0);

            if (ranAns == answer)
                i--;
            else
            {
                falseAnswer[i] = ranAns;

                for (int j = 0; j < i; j++) // Check for duplicate ans
                {
                    if (ranAns == falseAnswer[j])
                    {
                        i--;
                        break;
                    }
                }
            }
        }
        trueAnswer = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            if (i == trueAnswer)
            {
                switch (PlayerPrefs.GetInt("Lesson"))
                {
                    case 1: //Force
                        switch (randomQuestion)
                        {
                            case 1:
                                ansText[i].text = answer.ToString("F2") + " N";
                                break;
                            case 2:
                                ansText[i].text = answer.ToString("F2") + " m/s2";
                                break;
                            case 3:
                                ansText[i].text = answer.ToString("F2") + " m/s2";
                                break;
                        }
                        break;

                    case 2: // Friction
                        ansText[i].text = answer.ToString("F2") + " N";
                        break;

                    case 3: // Gravity + Motion
                        switch (randomQuestion)
                        {
                            case 1:
                                ansText[i].text = answer.ToString("F2") + " กิโลกรัม";
                                break;
                            case 2:
                                ansText[i].text = answer.ToString("F2") + " m/s";
                                break;
                            case 3:
                                ansText[i].text = answer.ToString("F2") + " เมตร";
                                break;
                        }

                        break;

                    case 4: // Momentum
                        ansText[i].text = answer.ToString("F2") + " m/s";
                        break;
                }
                //cheats
                //Debug.Log("this is correct ans at i =" + i);
            }
            else
            {
                switch (PlayerPrefs.GetInt("Lesson"))
                {
                    case 1: //Force
                        switch (randomQuestion)
                        {
                            case 1:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " N";
                                break;
                            case 2:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s2";
                                break;
                            case 3:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s2";
                                break;
                        }
                        break;
                    case 2: // Friction
                        ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " N";
                        break;
                    case 3: // Gravity + Motion
                        switch (randomQuestion)
                        {
                            case 1:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " กิโลกรัม";
                                break;
                            case 2:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s";
                                break;
                            case 3:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " เมตร";
                                break;
                        }
                        break;
                    case 4: // Momentum
                        ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s";
                        break;
                }
                cntAnswer++;
            }
        }
        cntAnswer = 0;
    }
}
