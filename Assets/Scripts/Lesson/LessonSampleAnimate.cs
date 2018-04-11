using UnityEngine;

public class LessonSampleAnimate : MonoBehaviour
{

    public GameObject redBox, blueBox, accArrG1, accArrG2, slope, playBtt;
    public GameObject forceArrow, forceArrow2; //ForceArr in lesson1
    public GameObject friction, frictionForce; //lesson2
    public ProblemGenerator pg;
    public Camera mainCam;

    private Rigidbody redRb, blueRb, forceRb, force2Rb;
    private float timer = 0;
    private Vector3 force1Start, force2Start;

    void Start()
    {
        redRb = redBox.GetComponent<Rigidbody>();
        blueRb = blueBox.GetComponent<Rigidbody>();

        switch (PlayerPrefs.GetInt("LessonTask"))
        {
            case 1:
                forceRb = forceArrow.GetComponent<Rigidbody>();
                force2Rb = forceArrow2.GetComponent<Rigidbody>();
                force1Start = forceArrow.transform.position;
                force2Start = forceArrow2.transform.position;

                mainCam.transform.position = new Vector3(2, 30, 7.5f);
                mainCam.transform.localEulerAngles = new Vector3(90, 0, 0);
                break;

            case 2:
                mainCam.transform.position = new Vector3(3f, 2f, -11f);
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
        switch (PlayerPrefs.GetInt("LessonTask"))
        {
            case 1: //Force
                forceArrow.SetActive(true);
                redBox.SetActive(true);
                switch (pg.randomQuestion)
                {
                    case 1:
                    case 2:
                        forceArrow.transform.position = force1Start;
                        forceRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 3:
                        forceArrow2.SetActive(true);
                        forceArrow.transform.position = force1Start;
                        forceArrow2.transform.position = force2Start;
                        forceRb.velocity = new Vector3(3, 0, 0);
                        force2Rb.velocity = new Vector3(0, 0, 3);
                        break;
                }
                break;

            case 2: // Friction
                friction.SetActive(true);
                switch (pg.randomQuestion)
                {
                    case 1:
                        frictionForce.SetActive(true);
                        redRb.velocity = new Vector3(5, 0, 0);
                        break;
                    case 2:
                        frictionForce.SetActive(true);
                        redRb.velocity = new Vector3(5, 0, 0);
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
        forceArrow.SetActive(false);
        forceArrow2.SetActive(false);
        accArrG1.SetActive(false);
        accArrG2.SetActive(false);
        slope.SetActive(false);
        blueBox.SetActive(false);
        redBox.SetActive(false);

        redRb.isKinematic = false;
        redRb.velocity = Vector3.zero;
        blueRb.velocity = Vector3.zero;
        redBox.transform.localEulerAngles = Vector3.zero;
        blueBox.transform.localEulerAngles = Vector3.zero;

        forceArrow.transform.position = force1Start;
        forceArrow2.transform.position = force2Start;

        //no velo when start anew
        if (PlayerPrefs.GetInt("LessonTask") == 1)
        {
            forceRb.velocity = Vector3.zero;
            forceRb.isKinematic = false;
            force2Rb.isKinematic = false;
            redRb.isKinematic = true;
            redBox.transform.position = new Vector3(0f, 0.5f, 6.5f);
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 2)
        {
            frictionForce.SetActive(false);
            friction.SetActive(false);
            if (pg.randomQuestion == 3)
            {
                slope.SetActive(true);
                redBox.transform.position = new Vector3(-1.5f, 2.5f, 6.5f);
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
