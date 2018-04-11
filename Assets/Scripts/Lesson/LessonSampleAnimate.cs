using UnityEngine;

public class LessonSampleAnimate : MonoBehaviour
{

    public GameObject redBox, blueBox, accArrG1, accArrG2, slope, plane, playBtt;
    public GameObject forceArrow, forceArrow2, froceText, accText;
    public ProblemGenerator pg;
    public Camera mainCam;

    private Rigidbody redRb, blueRb, forceArrowRb, forceArrow2Rb;
    private float timer = 0;

    void Start()
    {
        redRb = redBox.GetComponent<Rigidbody>();
        blueRb = blueBox.GetComponent<Rigidbody>();

        switch (PlayerPrefs.GetInt("LessonTask"))
        {
            case 1:
                forceArrowRb = forceArrow.GetComponent<Rigidbody>();
                forceArrow2Rb = forceArrow2.GetComponent<Rigidbody>();

                mainCam.transform.position = new Vector3(2, 30, 7.5f);
                mainCam.transform.localEulerAngles = new Vector3(90, 0, 0);
                break;

            case 2:
                forceArrowRb = forceArrow.GetComponent<Rigidbody>();
                froceText.transform.localPosition = new Vector3(0.7f, 0, -2.5f);
                froceText.transform.localEulerAngles = new Vector3(-90, -90, 0);
                accText.transform.localPosition = new Vector3(0, -1.4f, -5.5f);
                accText.transform.localEulerAngles = new Vector3(180, -90, 0);

                mainCam.transform.position = new Vector3(0, 2.8f, -12);
                mainCam.transform.localEulerAngles = Vector3.zero;
                break;
            case 3:
                mainCam.transform.position = new Vector3(3.6f, 1.9f, -11.4f);
                mainCam.transform.localEulerAngles = Vector3.zero;
                break;
            case 4:
                mainCam.transform.position = new Vector3(3.6f, 1.9f, -11.4f);
                mainCam.transform.localEulerAngles = Vector3.zero;
                break;
        }
    }

    public void playSample()
    {
        resetSample();
        redBox.SetActive(true);
        switch (PlayerPrefs.GetInt("LessonTask"))
        {
            case 1: //Force
                forceArrow.SetActive(true);
                switch (pg.randomQuestion)
                {
                    case 1:
                    case 2:
                        forceArrowRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 3:
                        forceArrow2.SetActive(true);
                        forceArrowRb.velocity = new Vector3(3, 0, 0);
                        forceArrow2Rb.velocity = new Vector3(0, 0, 3);
                        break;
                }
                break;

            case 2: // Friction
                switch (pg.randomQuestion)
                {
                    case 1:
                    case 2:
                        plane.SetActive(true);
                        forceArrow.SetActive(true);
                        forceArrowRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 3:
                        slope.SetActive(true);
                        break;
                }
                break;

            case 3: // Gravity + Motion
                switch (pg.randomQuestion)
                {
                    case 1:
                        blueBox.SetActive(true);
                        break;
                    case 2:
                        redBox.transform.Rotate(45, 45, 0);
                        break;
                    case 3:
                        redRb.velocity = new Vector3(0, 10, 0);
                        redBox.transform.Rotate(45, 45, 0);
                        break;
                }
                break;

            case 4: // Momentum
                switch (pg.randomQuestion)
                {
                    case 1:
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(15, 0, 0);
                        break;
                    case 2:
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(20, 0, 0);
                        blueRb.velocity = new Vector3(8, 0, 0);
                        break;
                    case 3:
                        redRb.velocity = new Vector3(10, 0, 0);
                        break;
                }
                break;
        }
    }

    public void resetSample()
    {
        redBox.SetActive(false);
        redRb.isKinematic = false;
        redRb.velocity = Vector3.zero;
        redBox.transform.localEulerAngles = Vector3.zero;

        blueBox.SetActive(false);
        blueRb.velocity = Vector3.zero;
        blueBox.transform.localEulerAngles = Vector3.zero;

        //no velo when start anew
        if (PlayerPrefs.GetInt("LessonTask") == 1)
        {
            forceArrow.SetActive(false);
            forceArrow2.SetActive(false);
            accArrG1.SetActive(false);
            accArrG2.SetActive(false);

            forceArrow.transform.position = new Vector3(-3, 1, 6.5f);
            forceArrowRb.velocity = Vector3.zero;
            forceArrowRb.isKinematic = false;
            
            forceArrow2.transform.position = new Vector3(0, 1, 3.5f);
            forceArrow2Rb.velocity = Vector3.zero;
            forceArrow2Rb.isKinematic = false;

            redRb.isKinematic = true;
            redBox.transform.position = new Vector3(0f, 0.75f, 6.5f);
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 2)
        {
            slope.SetActive(false);
            plane.SetActive(false);
            accArrG1.SetActive(false);
            forceArrow.SetActive(false);

            forceArrow.transform.position = new Vector3(-6, 2.35f, 12);
            forceArrowRb.velocity = Vector3.zero;
            forceArrowRb.isKinematic = false;

            if (pg.randomQuestion != 3)
            {
                redRb.isKinematic = true;
                redBox.transform.position = new Vector3(-3, 2.35f, 12);
                redBox.transform.Rotate(0, 0, 0);
            }
            else
            {
                redRb.isKinematic = false;
                redBox.transform.position = new Vector3(-1.35f, 2.8f, 12);
                redBox.transform.Rotate(0, 0, 60);
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 3 && pg.randomQuestion < 3) //drop item in gravity
        {
            redBox.transform.position = new Vector3(0f, 5.5f, 6.5f);
            blueBox.transform.position = new Vector3(3f, 5.5f, 6.5f);
        }
    }
}
