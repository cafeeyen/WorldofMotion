using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonSampleAnimate : MonoBehaviour {

    public GameObject redBox, blueBox,accArr1,accArr2,accAtext,slope ,playBtt;
    public GameObject force, force2; //ForceArr in lesson1
    public GameObject friction, frictionForce; //lesson2
    public ProblemGenerator pg;
    public Camera mainCam;

    private Rigidbody rbRigid, bbRigid,forceRigid, force2Rigid;
    private float timer = 0;
    private Vector3 force1v,force2v;

    
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
                mainCam.transform.position = new Vector3(1.78f, 20.84f, 7.7f);
                mainCam.transform.localEulerAngles = new Vector3(90, 0, 0);
                force2v = force2.transform.position;
                force1v = force.transform.position;
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
                force.SetActive(true);
                
                switch (pg.randomQuestion)
                {
                    case 1:
                        forceRigid.velocity = new Vector3(3, 0, 0);
                        break;
                    case 2:
                        forceRigid.velocity = new Vector3(3, 0, 0);                      
                        break;
                    case 3:
                        force2.SetActive(true);
                        forceRigid.velocity = new Vector3(3, 0, 0);
                        force2Rigid.velocity = new Vector3(0, 0, 4);
                        break;

                }
                break;

            case 2: // Friction
                friction.SetActive(true);
                switch (pg.randomQuestion)
                {
                    case 1:
                        frictionForce.SetActive(true);
                        rbRigid.velocity = new Vector3(5, 0, 0);
                        break;
                    case 2:
                        frictionForce.SetActive(true);
                        rbRigid.velocity = new Vector3(5, 0, 0);
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
        rbRigid.isKinematic = false;
        if (PlayerPrefs.GetInt("LessonTask") == 2 && pg.randomQuestion == 3) //slop friction
        {
            redBox.transform.position = new Vector3(-1.5f, 2.5f, 6.5f);
            redBox.transform.Rotate(0, 0, 60);
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 3 && pg.randomQuestion < 3) //drop item in gravity
        {   
            redBox.transform.position = new Vector3(0f, 5.5f, 6.5f);
            blueBox.transform.position = new Vector3(3f, 5.5f, 6.5f);
        }
        else
        {
            redBox.transform.position = new Vector3(0f, 0.5f, 6.5f);
            blueBox.transform.position = new Vector3(3f, 0.5f, 6.5f);
        }

        force2.SetActive(false);
        accArr1.SetActive(false);
        accArr2.SetActive(false);
        accAtext.SetActive(false);
        slope.SetActive(false);
        blueBox.SetActive(false);

        //no velo when start anew
        if (PlayerPrefs.GetInt("LessonTask") == 1)
        {
            forceRigid.velocity = new Vector3(0, 0, 0);
            forceRigid.isKinematic = false;
            force2Rigid.isKinematic = false;
            force.transform.localEulerAngles = Vector3.zero;
            force2.transform.localEulerAngles = new Vector3(-90, 0, 0);
            rbRigid.isKinematic = true;
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 2)
        {
            frictionForce.SetActive(false);
            friction.SetActive(false);
            if (pg.randomQuestion == 3) { slope.SetActive(true); }
            
        }

        rbRigid.velocity = new Vector3(0, 0, 0);
        bbRigid.velocity = new Vector3(0, 0, 0);

        //reset position
        force.transform.position = force1v;
        force2.transform.position = force2v;

        redBox.transform.localEulerAngles = Vector3.zero;
        blueBox.transform.localEulerAngles = Vector3.zero;
    }
}
