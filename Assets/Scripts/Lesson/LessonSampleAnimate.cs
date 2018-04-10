using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonSampleAnimate : MonoBehaviour {

    public GameObject redBox, blueBox,accArr1,accArr2,accAtext,slope ,playBtt;
    public GameObject force, force2; //ForceArr in lesson1
    public ProblemGenerator pg;
    public Camera mainCam;

    private Rigidbody rbRigid, bbRigid,forceRigid, force2Rigid;
    private float timer = 0;
    private Vector3 force2v;

    
    // Use this for initialization
    void Start()
    {
        rbRigid = redBox.GetComponent<Rigidbody>();
        bbRigid = blueBox.GetComponent<Rigidbody>();

        switch(PlayerPrefs.GetInt("LessonTask"))//change camera view
        {
            case 1:
                forceRigid = force.GetComponent<Rigidbody>();
                force2Rigid = force2.GetComponent<Rigidbody>();
                mainCam.transform.position = new Vector3(-1.62f, 28.35f, -4.6f);
                mainCam.transform.localEulerAngles = new Vector3(67, 0, 0);
                force2v = force2.transform.position;
                break;

            case 2:
                mainCam.transform.position = new Vector3(3.6f, 1.9f, -11.4f);
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
                force.SetActive(true);
                
                switch (pg.randomQuestion)
                {
                    case 1:
                        forceRigid.velocity = new Vector3(3, 0, 0);
                        //rbRigid.velocity = new Vector3(9, 0, 0);
                        break;
                    case 2:                      
                        forceRigid.velocity = new Vector3(3, 0, 0);                      
                        //rbRigid.velocity = new Vector3(9, 0, 0);
                        break;
                    case 3:
                        force2.SetActive(true);
                        forceRigid.velocity = new Vector3(3, 0, 0);
                        force2Rigid.velocity = new Vector3(0, 0, 4);
                        //rbRigid.velocity = new Vector3(10, 0, 10);
                        break;

                }
                break;

            case 2: // Friction
                switch (pg.randomQuestion)
                {
                    case 1:
                        rbRigid.velocity = new Vector3(7, 0, 0);
                        break;
                    case 2:
                        rbRigid.velocity = new Vector3(12, 0, 0);
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
                        rbRigid.velocity = new Vector3(0, 10, 0);
                        redBox.transform.Rotate(45, 45, 0);
                        break;

                }
                break;

            case 4: // Momentum
                switch(pg.randomQuestion)
                {
                    case 1:
                        blueBox.SetActive(true);
                        rbRigid.velocity = new Vector3(15, 0, 0);
                        break;
                    case 2:
                        blueBox.SetActive(true);
                        rbRigid.velocity = new Vector3(20, 0, 0);
                        bbRigid.velocity = new Vector3(8, 0, 0);
                        break;
                    case 3:
                        rbRigid.velocity = new Vector3(10, 0, 0);
                        break;

                }
                break;

        }
    }

    public void resetSample()
    {
        if (PlayerPrefs.GetInt("LessonTask") == 2 && pg.randomQuestion == 3) //slop friction
        {
            redBox.transform.position = new Vector3(-1.49f, 2.54f, 6.34f);
            redBox.transform.Rotate(0, 0, 60);
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 3 && pg.randomQuestion < 3) //drop item in gravity
        {   
            redBox.transform.position = new Vector3(-0.85f, 5.5f, 6.34f);
            blueBox.transform.position = new Vector3(1.46f, 5.5f, 6.34f);
        }

        //else if (PlayerPrefs.GetInt("LessonTask") == 1)
        //{
            
        //}
        else
        {
            redBox.transform.position = new Vector3(-0.85f, 0.5f, 6.34f);
            blueBox.transform.position = new Vector3(1.46f, 0.5f, 6.34f);
        }

        force2.SetActive(false);
        accArr1.SetActive(false);
        accArr2.SetActive(false);
        accAtext.SetActive(false);
        slope.SetActive(false);
        blueBox.SetActive(false);

        //no velo when start anew
        forceRigid.velocity = new Vector3(0, 0, 0);
        rbRigid.velocity = new Vector3(0, 0, 0);
        bbRigid.velocity = new Vector3(0, 0, 0);

        //reset position
        force.transform.position = new Vector3(-3f, 0.5f, 6.34f);
        force2.transform.position = force2v;
        redBox.transform.localEulerAngles = Vector3.zero;
        blueBox.transform.localEulerAngles = Vector3.zero;
    }
}
