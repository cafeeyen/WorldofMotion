using UnityEngine;
using UnityEngine.UI;

public class ProblemGenerator : MonoBehaviour
{
    public Text QuestionText, QWarningText;
    public Text[] ansText;
    public int trueAnswer, randomQuestion;

    private float distance, time, Nforce;
    private float mass1, mass2, vBefore1, vBefore2, vAfter2, answer, mew, weight, gravity, acc;
    private float[] falseAnswer = { 0, 0, 0 };
    private int cntAnswer = 0;

    public void newProblem()
    {
        randomQuestion = Random.Range(1, 11);
        if (randomQuestion == 0) { randomQuestion = 1; }

        if (PlayerPrefs.GetInt("LessonTask") == 1) // Force
        {
            switch (randomQuestion)
            {
                case 1:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(5, 25);
                    answer = (mass1/9.8f) * vBefore1;
                    QuestionText.text = "จงหาแรงที่จะทำให้วัตถุหนัก " + mass1 + " N เคลื่อนที่ด้วยความเร่ง " + vBefore1 + " m/s^2";
                    QWarningText.text = "*อย่าลืมแปลงหน่วยน้ำหนักจากNเป็นกิโลกรัมล่ะ (W / 9.8)";
                    break;

                case 2:
                    vBefore1 = Random.Range(5, 100);
                    vBefore2 = Random.Range(5, 100);
                    acc = Random.Range(2, 25);
                    answer = (vBefore2 / vBefore1) * acc;
                    QuestionText.text = "วัตถุหนึ่งถูกแรง " + vBefore1 + " N กระทำเกิดความเร่ง " + acc + " m/s^2 ถ้าวัตถุถูกกระทำด้วยแรง " + vBefore2 + " N จะเกิดความเร่งเท่าใด?";
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
                case 4:
                    mass1 = Random.Range(2, 15);
                    mass2 = Random.Range(2, 15);
                    acc = Random.Range(10, 100);
                    answer = (mass1+mass2)*acc;
                    QuestionText.text = "วัตถุสองชิ้นมวล "+ mass1 + " กิโลกรัมและ "+ mass2 + " กิโลกรัมวางติดกันบนพื้นที่ไม่มีแรงเสียดทาน ถูกแรงทำให้เกิดความเร่ง "+ acc + " m/s^2 แรงดังกล่าวมีขนาดเท่าไหร่?";
                    QWarningText.text = "*วัตถุอยุ่ติดกันดังนั้นให้นำน้ำหนักมารวมกันก่อนคำนวณ";
                    break;
                case 5:
                    mass1 = Random.Range(2, 30);
                    vBefore1 = Random.Range(10, 100);
                    vBefore2 = Random.Range(10, 100);
                    answer = (vBefore1 - vBefore2) / mass1;
                    QuestionText.text = "กล่องมีมวล "+ mass1 + " กิโลกรัม มีแรงที่1ขนาด "+ vBefore1 + " N ดึงไปข้างหน้าและแรงที่2ขนาด "+ vBefore2 + " N ดึงไปข้างหลัง วัตถุจะเคลื่อนที่ไปข้างหน้าด้วยความเร่งเท่าไหร่?";
                    QWarningText.text = "*ทั้ง 2 แรงที่มีทิศตรงข้ามกัน อย่าลืมนำแรงมาลบกันเพื่อหาแรงรวมก่อนล่ะ";
                    break;
                case 6:
                    mass1 = Random.Range(2, 30);
                    mass2 = Random.Range(2, 30);
                    acc = Random.Range(10, 100);
                    vBefore1 = mass1 * acc;
                    answer = vBefore1 / mass2;
                    QuestionText.text = "วัตถุมวล "+ mass1 + " กิโลกรัมถูกแรงกระทำเกิดความเร่ง "+ acc + " m/s^2 ถ้าวัตถุมีน้ำหนัก "+ mass2 + " กิโลกรัมถูกแรงกระทำความเร่งจะเป็นเท่าใด?";
                    QWarningText.text = "*แรงเป็นแรงเดิมและมีขนาดเท่าเดิม";
                    break;
                case 7:
                    mass1 = Random.Range(5, 25);
                    vBefore1 = Random.Range(10, 100);
                    answer = vBefore1/ mass1;
                    QuestionText.text = "วัตถุมวล "+ mass1 + " กิโลกรัมถูกแรง "+ vBefore1 + " Nกระทำ วัตถุนี้จะมีความเร่งเท่าไหร่?";
                    QWarningText.text = "";
                    break;
                case 8:
                    mass1 = Random.Range(2, 25);
                    vBefore1 = Random.Range(5, 100);
                    vBefore2 = Random.Range(5, 100);
                    answer = (vBefore2 + vBefore1) / mass1;
                    QuestionText.text = "วัตถุมวล "+ mass1 + " กิโลกรัมถูกแรงที่1ขนาด "+ vBefore1 + " Nและแรงที่2ขนาด "+ vBefore2 + " Nดึงไปในทางเดียวกัน วัตถุจะมีความเร่งเท่าไหร่?";
                    QWarningText.text = "*แรงไปทางเดียวกันนำมาบวกกันก่อนคำนวณ";
                    break;
                case 9:
                    mass1 = Random.Range(2, 15);
                    mass2 = Random.Range(2, 15);
                    vBefore1 = Random.Range(30, 180);
                    answer = vBefore1/(mass1 + mass2);
                    QuestionText.text = "วัตถุสองชิ้นมวล " + mass1 + " กิโลกรัมและ " + mass2 + " กิโลกรัมวางติดกันบนพื้นที่ไม่มีแรงเสียดทาน ถูกแรงขนาด "+ vBefore1 + " Nทำให้เคลื่อนที่จะเกิดความเร่งเท่าไหร่?";
                    QWarningText.text = "*วัตถุอยุ่ติดกันดังนั้นให้นำน้ำหนักมารวมกันก่อนคำนวณ";
                    break;
                case 10:
                    acc = Random.Range(2, 15);
                    vBefore1 = Random.Range(10, 100);
                    vBefore2 = Random.Range(10, 100);
                    answer = (vBefore1 - vBefore2) / acc;
                    QuestionText.text = "กล่องมีแรงที่1ขนาด " + vBefore1 + " N ดึงไปข้างหน้าและแรงที่2ขนาด " + vBefore2 + " N ดึงไปข้างหลัง มีความเร่ง "+ acc + " m/s^2 วัตถุมีมวลเท่าไหร่?";
                    QWarningText.text = "*ทั้ง 2 แรงที่มีทิศตรงข้ามกัน อย่าลืมนำแรงมาลบกันเพื่อหาแรงรวมก่อนล่ะ";
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 2) // Friction
        {
            switch (randomQuestion)
            {
                case 1:
                    vBefore1 = Random.Range(5, 150);
                    mew = Random.Range(0.1f, 1.1f);
                    answer = vBefore1 / mew;
                    QuestionText.text = "ออกแรง " + vBefore1 + " N ลากวัตถุทำให้วัตถุเริ่มขยับ ถ้าสัมประสิทธิ์แรงเสียดทานมีค่าเป็น " + mew.ToString("F2") + " วัตถุจะมีน้ำหนัก(N)เท่าไหร่ ?";
                    QWarningText.text = "";
                    break;

                case 2:
                    mass1 = Random.Range(5, 100);
                    mew = Random.Range(0.1f, 1.1f);
                    acc = Random.Range(2, 25);
                    answer = (mass1 * acc) + (mass1 * 9.8f * mew);
                    QuestionText.text = "วัตถุหนึ่งมีมวล " + mass1 + " กิโลกรัมถูกแรงกระทำเกิดความเร่ง " + acc + " m/s^2 และสัมประสิทธิ์แรงเสียดทานระหว่างวัตถุกับพื้นคือ " + mew.ToString("F2") + " จงหาขนาดของแรงที่กระทำกับวัตถุ";
                    QWarningText.text = "*อย่าลืมลบแรงเสียดทานจากแรงทั้งหมด(F-แรงเสียดทาน = ma) และ N = mg";
                    break;
                case 3:
                    mass1 = Random.Range(5, 100);
                    answer = (mass1 * 9.8f) / 2;
                    QuestionText.text = "วัตถุหนึ่งมีมวล " + mass1 + " กิโลกรัมวางบนพื้นเอียง 30 องศากำลังจะไถลลง จงคำนวณหาแรงเสียดทานที่เกิดขึ้น";
                    QWarningText.text = "*อย่าลืมแตกแรงของน้ำหนัก(mg)เป็น mgsinθ ซึ่งมีทิศทางตรงกันข้ามกับแรงเสียดทาน";
                    break;
                case 4:
                    mass1 = Random.Range(5, 30);
                    vBefore1 = Random.Range(5, 100);
                    mew = Random.Range(0.1f, 1.1f);
                    Nforce = (mass1*9.81f)- (vBefore1/2);
                    answer = mew*Nforce;
                    QuestionText.text = "ดึงวัตถุมวล "+ mass1 + " กิโลกรัมไปข้างหน้าด้วยแรง "+ vBefore1 + " Nทำมุม 30 องศากับพื้น และมีสัมประสิทธิ์แรงเสียดทานที่ "+ mew.ToString("F2") + " จะมีค่าแรงเสียดทานเกิดขึ้นเท่าไหร่?";
                    QWarningText.text = "*ทำการแตกแรงที่ทำมุมก่อน แรงที่ไปทางเดียวกันนำมาบวกเข้าด้วยกันในสมการ";
                    break;
                case 5:
                    mass1 = Random.Range(1, 30);
                    vBefore1 = Random.Range(2, 20);
                    vBefore2 = Random.Range(20, 100);
                    answer = (vBefore2 - vBefore1) / (mass1*9.81f);
                    QuestionText.text = "วัตถุมวล "+ mass1 + " กิโลกรัมมีแรงกระทำทางซ้าย "+ vBefore1 + " Nและทางขวา "+ vBefore2 + " Nจะต้องมีสัมประสิทธิ์แรงเสียดทานน้อยกว่าเท่าใด วัตถุจึงจะเคลื่อนที่ไปทางขวา?";
                    QWarningText.text = "*แรงทิศตรงกันข้ามกันมีขนาดเท่ากัน แรงเสียดทานกับแรงที่ไปทิศทางเดียวกันนำมาบวกกัน";
                    break;
                case 6:
                    mass1 = Random.Range(10, 80);
                    mew = Random.Range(0.1f, 1.1f);
                    answer = (mass1/9.81f) * mew;
                    QuestionText.text = "ถ้าวัตถุหนัก "+ mass1 + " กิโลกรัมเคลื่อนที่บนผิวที่มีค่าสัมประสิทธิ์แรงเสียดทานเป็น "+ mew.ToString("F2") + " จะเกิดแรงเสียดทานเท่าไหร่?";
                    QWarningText.text = "อย่าลืมแปลงน้ำหนักเป็นมวลเสียก่อน (w/9.81)";
                    break;
                case 7:
                    vBefore1 = Random.Range(10, 200);
                    answer = (2* vBefore1)/9.81f;
                    QuestionText.text = "วัตถุกำลังไถลลงจากเนินที่ทำมุม30องศา เกิดแรงเสียดทาน "+ vBefore1 + " N วัตถุนี้มีมวลกี่กิโลกรัม?";
                    QWarningText.text = "*อย่าลืมแตกแรงของน้ำหนัก(mg)เป็น mgsinθ ซึ่งมีทิศทางตรงกันข้ามกับแรงเสียดทาน";
                    break;
                case 8:
                    mass1 = Random.Range(1, 20);
                    vBefore1 = Random.Range(20, 100);
                    answer = vBefore1 / (mass1*9.81f);
                    QuestionText.text = "วัตถุเคลื่อนที่บนพื้นผิวเกิดแรงเสียดทาน "+ vBefore1 + " N ถ้าวัตถุมีน้ำหนัก "+ mass1 + " N ค่าสัมประสิทธิ์แรงเสียดทานจะเป็นเท่าไหร่?";
                    QWarningText.text = "";
                    break;
                case 9:
                    mass1 = Random.Range(5, 30);
                    vBefore1 = Random.Range(30, 100);
                    vBefore2 = Random.Range(5, 30);
                    mew = Random.Range(0.1f, 1.1f);
                    Nforce = (mass1 * 9.81f) - (vBefore1 / 2);
                    answer = vBefore2 / Nforce;
                    QuestionText.text = "ดึงวัตถุมวล " + mass1 + " กิโลกรัมไปข้างหน้าด้วยแรง " + vBefore1 + " Nทำมุม 30 องศากับพื้น เกิดแรงเสียดทาน "+" N สัมประสิทธิ์แรงเสียดทานต้องมีขนาดเท่าไหร่?";
                    QWarningText.text = "*ทำการแตกแรงที่ทำมุมก่อน แรงที่ไปทางเดียวกันนำมาบวกเข้าด้วยกันในสมการ";
                    break;
                case 10:
                    vBefore1 = Random.Range(2, 20);
                    vBefore2 = Random.Range(20, 100);
                    mew = Mathf.Floor(Random.Range(0.1f, 1.1f)*100)/100;
                    answer = (vBefore2 - vBefore1) / (mew * 9.81f);
                    QuestionText.text = "วัตถุมีแรงกระทำทางซ้าย " + vBefore1 + " Nและทางขวา " + vBefore2 + " N มีสัมประสิทธิ์แรงเสียดทานเท่ากับ "+ mew + " วัตถุต้องหนักเท่าใดจึงจะเคลื่อนที่ไปทางขวา?";
                    QWarningText.text = "*แรงทิศตรงกันข้ามกันมีขนาดเท่ากัน แรงเสียดทานกับแรงที่ไปทิศทางเดียวกันนำมาบวกกัน";
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 3) // Gravity + Motion
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
                    QWarningText.text = "*คำใบ้ : ความเร็วต้น(u)เป็น0 และ ใช้สูตรการเคลื่อนที่มาช่วย";
                    break;
                case 3:
                    vBefore1 = Random.Range(5, 100);
                    time = vBefore1 / 9.8f;
                    answer = (vBefore1 * time) + ((-9.8f * time * time) / 2);
                    QuestionText.text = "บนพื้นผิวโลก โยนวัตถุหนึ่งขึ้นด้วยความเร็วต้น " + vBefore1 + " m/s วัตถุจะขึ้นไปสูงสุดได้กี่เมตรก่อนตกลงมา?";
                    QWarningText.text = "*อย่าลืมหาเวลา ณ จุดสูงสุดก่อนหาระยะทางจากสูตรการเคลื่อนที่ โดยvที่จุดสูงสุดมีค่าเป็น0 และgมีค่าเป็นลบในที่นี้";
                    break;
                case 4:
                    mass1 = Random.Range(5, 50);
                    vBefore1 = Random.Range(5, 100);
                    time = Random.Range(2, 20);
                    acc = vBefore1 / time;
                    answer = mass1*acc;
                    QuestionText.text = "วางกล่องไม้มวล "+ mass1 + " กิโลกรัมไว้บนพื้นเรียบที่ไม่มีแรงเสียดทาน ถ้าต้องการให้กล่องไม้เคลื่อนที่จนมีความเร็ว "+ vBefore1 + " m/s ในเวลา "+ time + " วินาที แรงลัพธ์ที่กระทำต้องมีขนาดเท่าไหร่?";
                    QWarningText.text = "*นำสูตรเรื่องแรงเข้ามาช่วย และ u = 0";
                    break;
                case 5:
                    distance = Random.Range(5, 50);
                    time = Random.Range(2, 20);
                    answer = (2* distance)/(time*time);
                    QuestionText.text = "วัตถุหนึ่งเคลื่อนที่จากหยุดนิ่งไปตามพื้นด้วยความเร่งคงตัว ไปได้ไกล "+ distance + " เมตรภายใน "+ time + " วินาที ขนาดของความเร่งจะเป็นเท่าไหร่?";
                    QWarningText.text = "*ค่า u = 0 และพื้นไม่มีแรงเสียดทาน";
                    break;
                case 6:
                    distance = Random.Range(5, 100);
                    acc = Random.Range(2, 20);
                    answer = Mathf.Sqrt(2 * acc * distance); 
                    QuestionText.text = "วัตถุเคลื่อนที่จากหยุดนิ่งไปเป็นระยะ "+ distance + " เมตรโดยมีความเร่ง "+ acc + " m/s^2 ความเร็วปลายของวัตถุจะมีค่าเท่าใด?";
                    QWarningText.text = "*ค่า u = 0 และพื้นไม่มีแรงเสียดทาน";
                    break;
                case 7:
                    vBefore1 = Random.Range(5, 100);
                    answer = (0 - vBefore1)/-9.81f;
                    QuestionText.text = "โยนของขึ้นบนฟ้าด้วยอัตราเร็ว "+ vBefore1 + " m/s จะใช้เวลากี่วินาทีถึงขึ้นไปจุดสูงสุดก่อนตกลงมา?";
                    QWarningText.text = "*ค่า v = 0 ณ จุดสูงสุด และค่า g ติดลบเพราะสวนทางกับวัตถุที่โยนขึ้นไป";
                    break;
                case 8:
                    vAfter2 = Random.Range(5, 100);
                    distance = Random.Range(5, 100);
                    answer = Mathf.Sqrt((vAfter2* vAfter2) + (2 * acc * distance));
                    QuestionText.text = "เด็กขว้างวัตถุขึ้นไปในแนวดิ่ง เมื่อขึ้นไปได้สูง "+ distance + " เมตรอัตราเร็วของวัตถุเป็น "+ vAfter2 + " m/s อันตราเร็วเริ่มต้นมีค่าเท่าใด?";
                    QWarningText.text = "*gมีค่าติดลบเพราะสวนทางกับวัตถุที่โยนขึ้นไป";
                    break;
                case 9:
                    vAfter2 = Random.Range(5, 100);
                    acc = Random.Range(2, 20);
                    answer = vAfter2 / acc;
                    QuestionText.text = "กล่องเคลื่อนที่จากหยุดนิ่งไปถึงจุดๆหนึ่งมีอัตราเร็วปลาย "+ vAfter2 + " m/sโดยมีความเร่ง "+ acc +" m/s^2 ใช้เวลาไปเท่าไหร่ถึงจะถึงจุดดังกล่าว?";
                    QWarningText.text = "*ค่า u = 0 และพื้นไม่มีแรงเสียดทาน";
                    break;
                case 10:
                    vBefore1 = Random.Range(30, 100);
                    vAfter2 = Random.Range(2, 29);
                    time = Random.Range(2, 20);
                    answer = (vAfter2 - vBefore1) / time;
                    QuestionText.text = "วัตถุหนึ่งเคลื่อนที่ด้วยอัตราเร็ว "+ vBefore1 + " m / s ก่อนลดเหลือ "+ vAfter2 + " m / s ภายในเวลา "+ time + " วินาที เกิดความหน่วงเท่าไหร่?";
                    QWarningText.text = "*ความหน่วงคือความเร่งที่มีค่าลดลงหรือติดลบนั่นเอง";
                    break;
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 4) // Momentum
        {
            switch (randomQuestion)
            {
                case 1:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore1 = Random.Range(5, 25);
                    vAfter2 = Random.Range(2, 10);
                    answer = ((mass1 * vBefore1) + 0 - (mass2 * vAfter2)) / mass1;
                    QuestionText.text = "มีกล่องสีแดงและสีฟ้า สีแดงมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยความเร็ว " + vBefore1 + " m/s ใส่กล่องสีฟ้ามวล " + mass2 + " กิโลกรัมที่ *ตั้งอยู่* จนมีความเร็ว " + vAfter2 + " m/s หลังกระทบกล่องสีแดงมีความเร็วเท่าไหร่ ? ";
                    QWarningText.text = "";
                    break;
                case 2:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore1 = Random.Range(10, 25);
                    vBefore2 = Random.Range(1, 11);
                    answer = ((mass1 * vBefore1) + (mass2 * vBefore2)) / (mass1 + mass2);
                    QuestionText.text = "มีกล่องสีแดงและสีฟ้า สีแดงมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยความเร็ว " + vBefore1 + " m/s ใส่กล่องสีฟ้ามวล " + mass2 + " กิโลกรัมที่มีความเร็ว " + vBefore2 + " m/s ติดไปด้วยกันหลังกระทบจะมีความเร็วเท่าไหร่ ? ";
                    QWarningText.text = "";
                    break;
                case 3:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(100, 200);
                    answer = vBefore1 / mass1;
                    QuestionText.text = "มีกล่องมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยโมเมนตัม " + vBefore1 + " kg.m/s กล่องจะมีความเร็วเท่าไหร่ ? ";
                    QWarningText.text = "";
                    break;
                case 4:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(2, 30);
                    vBefore2 = Random.Range(2, 30);
                    answer = Mathf.Sqrt(((vBefore1*mass1)*(vBefore1 * mass1)) + ((vBefore2*mass1)*(vBefore2 * mass1)));
                    QuestionText.text = "วัตถุมวล "+ mass1 + " กิโลเคลื่อนที่ไปทางขวาด้วยความเร็ว "+ vBefore1 + " m/s ก่อนมีแรงกระทำทำให้วัตถุเคลื่อนที่ไปด้านบนด้วยความเร็ว "+ vBefore2 + " m/s จงหาโมเมนตัมที่เปลี่ยนไปของวัตถุนี้";
                    QWarningText.text = "*ลองคำนวณโมเมนตัมและทำการรวมเวกเตอร์ลัพธ์ มุมที่เปลี่ยนไปไม่ต้องนำมาคำนวณ";
                    break;
                case 5:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore1 = Random.Range(10, 100);
                    vBefore2 = Random.Range(10, 100);
                    Nforce = (mass1 * vBefore1) - (mass2 * vBefore2);
                    answer = Nforce / (mass1+mass2);
                    QuestionText.text = "มวล "+ mass1 + " กิโลกรัมเคลื่อนที่ด้วยความเร็ว "+ vBefore1 + " m/s ชนมวลที่2ขนาด "+ mass2 + " กิโลกรัมที่เคลื่อนที่ด้วยความเร็ว "+ vBefore2 + " m/s ในทิศที่สวนทางกันแล้วติดกันไป ภายหลังการชนจะมีความเร็วเท่าไหร่";
                    QWarningText.text = "*ทิศสวนทางกันกำหนดให้ทิศของมวลที่2ติดลบ";
                    break;
                case 6:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(2, 80);
                    answer = vBefore1 * mass1;
                    QuestionText.text = "มีกล่องน้ำหนัก "+ mass1 + " กิโลกรัมพุ่งมาด้วยความเร็ว "+ vBefore1 + " m/s จะเกิดโมเมนตัมเท่าไหร่ ?";
                    QWarningText.text = "";
                    break;
                case 7:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore1 = Random.Range(10, 25);
                    vBefore2 = Random.Range(1, 11);
                    vAfter2 = Random.Range(1, 11);
                    answer = ((mass1 * vBefore2) + (mass2 * vAfter2)) / (mass1 * mass2 * vBefore1);
                    QuestionText.text = "มีกล่องสีแดงมวล "+ mass1 + " กิโลกรัมพุ่งมาด้วยความเร็ว "+ vBefore1 + " m/s ใส่กล่องสีฟ้ามวล "+ mass2 + " กิโลกรัม หลังชนกล่องสีแดงมีความเร็ว "+ vBefore2 + " m/sและกล่องสีฟ้ามีความเร็ว "+ vAfter2 + " m/s ก่อนชนกล่องสีฟ้ามีความเร็วเท่าไหร่?";
                    QWarningText.text = "";
                    break;
                case 8:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore2 = Random.Range(1, 11);
                    vAfter2 = Random.Range(1, 11);
                    answer = (((mass1+mass2) * vAfter2) - (mass2 * vBefore2)) / mass1;
                    QuestionText.text = "กล่องสีแดงมวล "+ mass1 + " กิโลกรัมพุ่งชนกล่องสีฟ้ามวล "+ mass2 + " กิโลกรัมที่มีความเร็ว "+ vBefore2 + " m/sแล้วติดกันไป หลังชนมีความเร็ว "+ vAfter2 + " m/sกล่องสีแดงมีความเร็วก่อนชนเท่าไหร่?";
                    QWarningText.text = "";
                    break;
                case 9:
                    mass1 = Random.Range(5, 100);
                    vBefore1 = Random.Range(100, 200);
                    answer = vBefore1 / mass1;
                    QuestionText.text = "มีกล่องมวล " + mass1 + " กิโลกรัมพุ่งมาด้วยโมเมนตัม " + vBefore1 + " kg.m/s กล่องจะมีมวลเท่าไหร่ ? ";
                    QWarningText.text = "";
                    break;
                case 10:
                    mass1 = Random.Range(5, 100);
                    mass2 = Random.Range(5, 100);
                    vBefore2 = Random.Range(10, 100);
                    vAfter2 = Random.Range(2, 50);
                    Nforce = ((mass1 + mass2)* vAfter2) + (mass2* vBefore2);
                    answer = Nforce / mass1;
                    QuestionText.text = "มวลที่1ขนาด " + mass1 + " กิโลกรัมเคลื่อนที่ด้วยความเร็ว ชนมวลที่2ขนาด " + mass2 + " กิโลกรัมที่เคลื่อนที่ด้วยความเร็ว " + vBefore2 + " m/s ในทิศที่สวนทางกันติดกันไปด้วยความเร็ว "+ vAfter2 +" m/s มวลที่1มีความเร็วก่อนชนเท่าไหร่?";
                    QWarningText.text = "*ทิศสวนทางกันกำหนดให้ทิศของมวลที่2ติดลบ";
                    break;
            }
        }
        //answer = Mathf.Round(answer * 100f) / 100f;
        createChoices();
    }

    private void createChoices()
    {
        for (int i = 0; i < 3; i++)
        {
            float ranAns = Mathf.Round((answer * (float)(Random.Range(-15, 16) / 10.0)) * 100f) / 100f;
            if (Mathf.Abs(ranAns) == Mathf.Abs(Mathf.Round(answer * 100f) / 100f))
                i--;
            else
            {
                falseAnswer[i] = ranAns;

                for (int j = 0; j < i; j++) // Check for duplicate ans
                {
                    if (Mathf.Abs(ranAns) == Mathf.Abs(falseAnswer[j]))
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
                switch (PlayerPrefs.GetInt("LessonTask"))
                {
                    case 1: //Force
                        switch (randomQuestion)
                        {
                            case 1:
                                ansText[i].text = answer.ToString("F2") + " N";
                                break;
                            case 2:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 3:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 4:
                                ansText[i].text = answer.ToString("F2") + " N";
                                break;
                            case 5:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 6:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 7:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 8:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 9:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 10:                                
                                ansText[i].text = answer.ToString("F2") + " กิโลกรัม";
                                break;
                        }
                        break;

                    case 2: // Friction
                        if (randomQuestion != 5 && randomQuestion != 8 && randomQuestion != 9 && randomQuestion != 10) { ansText[i].text = answer.ToString("F2") + " N"; }
                        else if (randomQuestion == 10) { ansText[i].text = answer.ToString("F2") + " กิโลกรัม"; }
                        else { ansText[i].text = answer.ToString("F2"); }
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
                            case 4:
                                ansText[i].text = answer.ToString("F2") + " F";
                                break;
                            case 5:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                            case 6:
                                ansText[i].text = answer.ToString("F2") + " m/s";
                                break;
                            case 7:
                                ansText[i].text = answer.ToString("F2") + " วินาที";
                                break;
                            case 8:
                                ansText[i].text = answer.ToString("F2") + " m/s";
                                break;
                            case 9:
                                ansText[i].text = answer.ToString("F2") + " วินาที";
                                break;
                            case 10:
                                ansText[i].text = answer.ToString("F2") + " m/s^2";
                                break;
                        }

                        break;

                    case 4: // Momentum
                        //ansText[i].text = answer.ToString("F2") + " m/s";
                        if (randomQuestion != 4 && randomQuestion != 6 && randomQuestion != 9) { ansText[i].text = answer.ToString("F2") + " m/s"; }
                        else if (randomQuestion == 9) { ansText[i].text = answer.ToString("F2") + " กิโลกรัม"; }
                        else { ansText[i].text = answer.ToString("F2") + "kg.m/s"; }
                        break;
                }
                //cheats
                //Debug.Log("this is correct ans at i =" + i);
            }
            else
            {
                switch (PlayerPrefs.GetInt("LessonTask"))
                {
                    case 1: //Force
                        switch (randomQuestion)
                        {
                            case 1: // No negative value
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " N";
                                break;
                            case 2:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 3: // No negative value
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 4: // No negative value
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " N";
                                break;
                            case 5: // No negative value
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 6:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 7:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 8:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 9:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 10:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " กิโลกรัม";
                                break;
                        }
                        break;
                    case 2: // Friction | No negative value all
                        falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                        if (randomQuestion != 5 && randomQuestion != 8 && randomQuestion != 9 && randomQuestion != 10) { ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " N"; }
                        else if (randomQuestion == 10) { ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " กิโลกรัม"; }
                        else { ansText[i].text = falseAnswer[cntAnswer].ToString("F2"); }
                        break;
                    case 3: // Gravity + Motion
                        switch (randomQuestion)
                        {
                            case 1:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " กิโลกรัม";
                                break;
                            case 2:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s";
                                break;
                            case 3:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " เมตร";
                                break;
                            case 4:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " F";
                                break;
                            case 5:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                            case 6:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s";
                                break;
                            case 7:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " วินาที";
                                break;
                            case 8:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s";
                                break;
                            case 9:
                                falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " วินาที";
                                break;
                            case 10:
                                ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s^2";
                                break;
                        }
                        break;
                    case 4: // Momentum
                        if(randomQuestion == 3 || randomQuestion == 9)  // No negative value
                            falseAnswer[cntAnswer] = Mathf.Abs(falseAnswer[cntAnswer]);
                        if (randomQuestion != 4 && randomQuestion != 6 && randomQuestion != 9) { ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " m/s"; }
                        else if (randomQuestion == 9) { ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + " กิโลกรัม"; }
                        else { ansText[i].text = falseAnswer[cntAnswer].ToString("F2") + "kg.m/s"; }
                        break;
                }
                cntAnswer++;
            }
        }
        cntAnswer = 0;
    }
}
