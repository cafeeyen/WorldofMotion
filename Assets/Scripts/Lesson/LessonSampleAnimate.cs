using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonSampleAnimate : MonoBehaviour {

    public GameObject redBox, blueBox;
    public ProblemGenerator pg;
    private Rigidbody rbRigid, bbRigid;
    
    // Use this for initialization
    void Start()
    {
        rbRigid = redBox.GetComponent<Rigidbody>();
        bbRigid = blueBox.GetComponent<Rigidbody>();
    }

    public void playSample()
    {
        resetSample();
        switch (PlayerPrefs.GetInt("LessonTask"))
        {
            case 1: //Force
                switch (pg.randomQuestion)
                {
                    case 1:
                        blueBox.SetActive(false);
                        rbRigid.velocity = new Vector3(15, 0, 0);
                        break;
                    case 2:
                        rbRigid.velocity = new Vector3(10, 0, 0);
                        break;
                    case 3:
                        blueBox.SetActive(false);
                        rbRigid.velocity = new Vector3(10, 0, 10);
                        break;

                }
                break;
            case 2: // Friction
                break;
            case 3: // Gravity + Motion
                switch (pg.randomQuestion)
                {
                    case 1:
                        break;
                    case 2:
                        blueBox.SetActive(false);
                        redBox.transform.Rotate(45, 45, 0);
                        break;
                    case 3:
                        blueBox.SetActive(false);
                        rbRigid.velocity = new Vector3(0, 15, 0);
                        redBox.transform.Rotate(45, 45, 0);
                        break;

                }
                break;
            case 4: // Momentum
                switch(pg.randomQuestion)
                {
                    case 1:
                        rbRigid.velocity = new Vector3(15, 0, 0);
                        break;
                    case 2:
                        rbRigid.velocity = new Vector3(20, 0, 0);
                        bbRigid.velocity = new Vector3(8, 0, 0);
                        break;
                    case 3:
                        blueBox.SetActive(false);
                        rbRigid.velocity = new Vector3(10, 0, 0);
                        break;

                }
                break;

        }
    }

    public void resetSample()
    {
        if(PlayerPrefs.GetInt("LessonTask") == 3 && pg.randomQuestion < 3)
        {   
            redBox.transform.position = new Vector3(-0.85f, 5.5f, 6.34f);
            blueBox.transform.position = new Vector3(1.46f, 5.5f, 6.34f);
        }
        else
        {
            redBox.transform.position = new Vector3(2.85f, 0.5f, 6.34f);
            blueBox.transform.position = new Vector3(4.46f, 0.5f, 6.34f);
        }
        blueBox.SetActive(true);
        rbRigid.velocity = new Vector3(0, 0, 0);
        bbRigid.velocity = new Vector3(0, 0, 0);
    }
}
