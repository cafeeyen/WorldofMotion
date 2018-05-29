﻿using UnityEngine;

public class LessonSampleAnimate : MonoBehaviour
{

    public GameObject redBox, blueBox, accArrG1, accArrG2, slope, plane, playBtt;
    public GameObject forceArrow, forceArrow2, forceArrow3, forceArrow4, forceArrow5, forceArrow6, forceArrow7, forceFrictionArrow, forceFrictionArrow2, forceFrictionArrow3, froceText, accText;
    public GameObject GravityRedArrow, GravityRedArrow2, GravityBlueRedArrow, veloUpArr, veloDownArr, veloDownArr2, veloRightArr, accRightArr, ground,wall,table,mass1,mass2,mass3 ; //gravity section
    public GameObject V1Arr, V2Arr, V3Arr, V4Arr, V5Arr,V6Arr,boom; //Momentum Section
    public ProblemGenerator pg;
    public Camera mainCam;

    private Rigidbody redRb, blueRb, forceArrowRb, forceArrow2Rb, forceArrow5Rb, veloRb;
    private Vector3 gravityA, veloA;
    private Quaternion rotation;

    void Start()
    {
        redRb = redBox.GetComponent<Rigidbody>();
        blueRb = blueBox.GetComponent<Rigidbody>();
        veloRb = veloDownArr2.GetComponent<Rigidbody>();
        gravityA = GravityRedArrow2.transform.position;
        veloA = veloDownArr2.transform.position;

        switch (PlayerPrefs.GetInt("LessonTask"))
        {
            case 1:
                forceArrowRb = forceArrow.GetComponent<Rigidbody>();
                forceArrow2Rb = forceArrow2.GetComponent<Rigidbody>();
                forceArrow5Rb = forceArrow5.GetComponent<Rigidbody>();

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
                mainCam.transform.position = new Vector3(3.6f, 4f, -24f);
                mainCam.transform.localEulerAngles = Vector3.zero;
                break;
            case 4:
                mainCam.transform.position = new Vector3(10f, 3.5f, -24f);
                mainCam.transform.localEulerAngles = Vector3.zero;
                break;
        }
    }

    void Awake()
    {
        rotation = mass3.transform.rotation;
    }

    void LateUpdate()
    {
        mass3.transform.rotation = rotation;
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("LessonTask") == 1 && (pg.randomQuestion == 4 || pg.randomQuestion == 9))
        {
            if (forceArrow5.transform.position.x >= -1.8f && redBox.transform.position.x <= 1)
            {
                forceArrow5Rb.isKinematic = true;
                forceArrow3.SetActive(true);
                redRb.velocity = new Vector3(5, 0, 0);
                 
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 3 && (pg.randomQuestion == 3|| pg.randomQuestion == 7|| pg.randomQuestion == 8))
        {
            if(redBox.transform.position.y >= 3.4f)
            {
                veloUpArr.SetActive(false);
                veloDownArr.SetActive(true);
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 3 && pg.randomQuestion == 2)
        {
            if(veloDownArr2.transform.position.y <= 0.6f)
            {
                veloRb.isKinematic = true;
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 4 && pg.randomQuestion == 1)
        {
            if (blueRb.transform.position.x > 5f)
            {
                V2Arr.SetActive(true);
                if (blueRb.transform.position.x < 7f)
                {
                    boom.SetActive(true);
                }
                else
                {
                    boom.SetActive(false);
                }
            }    
                     
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 4 && (pg.randomQuestion == 2|| pg.randomQuestion == 7 || pg.randomQuestion == 8))
        {
            if (blueRb.transform.position.x > 8f && blueRb.transform.position.x < 9f)
            {
                blueRb.velocity = new Vector3(0, 0, 0);
                redRb.velocity = new Vector3(0, 0, 0);
                
                if(pg.randomQuestion == 7)
                {
                    blueRb.velocity = new Vector3(13f, 0, 0);
                    redRb.velocity = new Vector3(10f, 0, 0);
                }
                else
                {
                    blueRb.velocity = new Vector3(13f, 0, 0);
                    redRb.velocity = new Vector3(13f, 0, 0);
                }
                
                boom.SetActive(true);
                    
            }
            if (blueRb.transform.position.x >= 11f)
            {
                boom.SetActive(false);
            }
            if (blueRb.transform.position.x > 17.5f)
            {
                Time.timeScale = 0;
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 4 && pg.randomQuestion == 4)
        {
            if (redRb.transform.position.x > 6f && redRb.transform.position.x < 6.1f)
            {
                redRb.velocity = new Vector3(0, 0, 0);
                V1Arr.SetActive(false);
                V5Arr.SetActive(true);
                forceArrow6.SetActive(true);
                redRb.useGravity = false;
                redRb.velocity = new Vector3(5f, 5f, 0);

            }
            if (redRb.transform.position.y > 3f && redRb.transform.position.y < 3.1f)
            {
                redRb.isKinematic = true;
                redRb.useGravity = true;
            }
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 4 && (pg.randomQuestion == 5|| pg.randomQuestion == 10))
        {
            if (redRb.transform.position.x > 4.9f)
            {
                V3Arr.SetActive(false);
                V4Arr.SetActive(false);
                V6Arr.SetActive(true);
                if (blueRb.transform.position.x < 8f)
                {
                    boom.SetActive(true);
                }
                else
                {
                    boom.SetActive(false);
                }
            }
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
                    case 4:
                        blueBox.SetActive(true);
                        redRb.isKinematic = false;
                        blueRb.isKinematic = false;
                        forceArrow.SetActive(false);
                        forceArrow5.SetActive(true);
                        forceArrow5Rb.velocity = new Vector3(5, 0, 0);
                        break;
                    case 5:
                        forceArrow4.SetActive(true);
                        forceArrow.SetActive(false);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(6, 0, 0);
                        break;
                    case 6:
                        forceArrowRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 7:
                        forceArrowRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 8:
                        forceArrow7.SetActive(true);
                        forceArrow.SetActive(false);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(6, 0, 0);
                        break;
                    case 9:
                        blueBox.SetActive(true);
                        redRb.isKinematic = false;
                        blueRb.isKinematic = false;
                        forceArrow.SetActive(false);
                        forceArrow5.SetActive(true);
                        forceArrow5Rb.velocity = new Vector3(5, 0, 0);
                        break;
                    case 10:
                        forceArrow4.SetActive(true);
                        forceArrow.SetActive(false);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(6, 0, 0);
                        break;

                }
                break;

            case 2: // Friction
                switch (pg.randomQuestion)
                {
                    case 1:
                        plane.SetActive(true);
                        forceFrictionArrow.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 2:
                        plane.SetActive(true);
                        forceArrow.SetActive(true);
                        forceArrowRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 3:
                        slope.SetActive(true);
                        break;
                    case 4:
                        plane.SetActive(true);
                        forceFrictionArrow2.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 5:
                        plane.SetActive(true);
                        forceFrictionArrow3.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 6:
                        plane.SetActive(true);
                        forceFrictionArrow.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 7:
                        slope.SetActive(true);
                        break;
                    case 8:
                        plane.SetActive(true);
                        forceFrictionArrow.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 9:
                        plane.SetActive(true);
                        forceFrictionArrow2.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 10:
                        plane.SetActive(true);
                        forceFrictionArrow3.SetActive(true);
                        redRb.isKinematic = false;
                        redRb.velocity = new Vector3(3, 0, 0);
                        break;
                }
                break;

            case 3: // Gravity + Motion
                Time.timeScale = 0.6f;
                switch (pg.randomQuestion)
                {
                    case 1:
                        mass1.SetActive(true);
                        mass2.SetActive(true);
                        blueBox.SetActive(true);
                        GravityRedArrow.SetActive(true);
                        GravityBlueRedArrow.SetActive(true);
                        ground.SetActive(true);
                        wall.SetActive(true);
                        break;
                    case 2:
                        mass3.SetActive(true);
                        mass1.SetActive(false);
                        veloDownArr2.SetActive(true);
                        ground.SetActive(true);
                        table.SetActive(true);
                        break;
                    case 3:
                        redRb.velocity = new Vector3(0, 7, 0);
                        mass1.SetActive(true);
                        GravityRedArrow.SetActive(true);
                        veloUpArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 4:
                        redRb.velocity = new Vector3(7, 0, 0);
                        mass1.SetActive(true);
                        veloRightArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 5:
                        redRb.velocity = new Vector3(7, 0, 0);
                        mass1.SetActive(true);
                        veloRightArr.SetActive(true);
                        accRightArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 6:
                        redRb.velocity = new Vector3(7, 0, 0);
                        mass1.SetActive(true);
                        veloRightArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 7:
                        redRb.velocity = new Vector3(0, 7, 0);
                        mass1.SetActive(true);
                        GravityRedArrow.SetActive(true);
                        veloUpArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 8:
                        redRb.velocity = new Vector3(0, 7, 0);
                        mass1.SetActive(true);
                        GravityRedArrow.SetActive(true);
                        veloUpArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 9:
                        redRb.velocity = new Vector3(7, 0, 0);
                        mass1.SetActive(true);
                        veloRightArr.SetActive(true);
                        accRightArr.SetActive(true);
                        ground.SetActive(true);
                        break;
                    case 10:
                        redRb.velocity = new Vector3(7, 0, 0);
                        mass1.SetActive(true);
                        veloRightArr.SetActive(true);
                        ground.SetActive(true);                    
                        break;
                }
                break;

            case 4: // Momentum
                switch (pg.randomQuestion)
                {
                    case 1:
                        redRb.drag = 0.5f;
                        
                        V1Arr.SetActive(true);
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(10, 0, 0);
                        break;
                    case 2:
                        redRb.drag = 0.3f;
                        blueRb.drag = 0.3f;
                        V1Arr.SetActive(true);
                        V2Arr.SetActive(true);
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(15, 0, 0);
                        blueRb.velocity = new Vector3(5, 0, 0);
                        break;
                    case 3:
                        V1Arr.SetActive(true);
                        redRb.velocity = new Vector3(7, 0, 0);
                        break;
                    case 4:
                        V1Arr.SetActive(true);
                        redRb.velocity = new Vector3(7, 0, 0);
                        break;
                    case 5:
                        redRb.drag = 0.3f;
                        blueRb.drag = 0.3f;
                        V3Arr.SetActive(true);
                        V4Arr.SetActive(true);
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(10, 0, 0);
                        blueRb.velocity = new Vector3(-7, 0, 0);
                        break;
                    case 6:
                        V1Arr.SetActive(true);
                        redRb.velocity = new Vector3(7, 0, 0);
                        break;
                    case 7:
                        redRb.drag = 0.3f;
                        blueRb.drag = 0.3f;
                        V1Arr.SetActive(true);
                        V2Arr.SetActive(true);
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(15, 0, 0);
                        blueRb.velocity = new Vector3(3, 0, 0);
                        break;
                    case 8:
                        redRb.drag = 0.3f;
                        blueRb.drag = 0.3f;
                        V1Arr.SetActive(true);
                        V2Arr.SetActive(true);
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(15, 0, 0);
                        blueRb.velocity = new Vector3(5, 0, 0);
                        break;
                    case 9:
                        V1Arr.SetActive(true);
                        redRb.velocity = new Vector3(7, 0, 0);
                        break;
                    case 10:
                        redRb.drag = 0.3f;
                        blueRb.drag = 0.3f;
                        V3Arr.SetActive(true);
                        V4Arr.SetActive(true);
                        blueBox.SetActive(true);
                        redRb.velocity = new Vector3(10, 0, 0);
                        blueRb.velocity = new Vector3(-7, 0, 0);
                        break;
                }
                break;
        }
    }

    public void resetSample()
    {
        redBox.SetActive(false);
        redRb.isKinematic = true;
        redRb.velocity = Vector3.zero;
        redBox.transform.localEulerAngles = Vector3.zero;
        redRb.isKinematic = false;


        blueBox.SetActive(false);
        blueRb.velocity = Vector3.zero;
        blueBox.transform.localEulerAngles = Vector3.zero;

        //no velo when start anew
        if (PlayerPrefs.GetInt("LessonTask") == 1)
        {
            forceArrow.SetActive(false);
            forceArrow2.SetActive(false);
            forceArrow3.SetActive(false);
            forceArrow4.SetActive(false);
            forceArrow5.SetActive(false);
            forceArrow7.SetActive(false);
            forceFrictionArrow.SetActive(false);
            accArrG1.SetActive(false);
            accArrG2.SetActive(false);

            forceArrow.transform.position = new Vector3(-3, 1, 6.5f);
            forceArrow5.transform.position = new Vector3(-5.55f, 1, 5.2f);
            forceArrowRb.velocity = Vector3.zero;
            forceArrowRb.isKinematic = false;
            forceArrow5Rb.isKinematic = false;

            forceArrow2.transform.position = new Vector3(0, 1, 3.5f);
            forceArrow2Rb.velocity = Vector3.zero;
            forceArrow2Rb.isKinematic = false;

            redRb.isKinematic = true;
            blueBox.SetActive(false);
            blueRb.isKinematic = true;
            blueBox.transform.position = new Vector3(1.5f, 0.75f, 5.2f);
            if (pg.randomQuestion != 4 && pg.randomQuestion != 9)
                redBox.transform.position = new Vector3(0f, 0.75f, 6.5f);
            else
                redBox.transform.position = new Vector3(0f, 0.75f, 5.2f);
        }
        else if (PlayerPrefs.GetInt("LessonTask") == 2)
        {
            slope.SetActive(false);
            plane.SetActive(false);
            accArrG1.SetActive(false);
            forceArrow.SetActive(false);
            forceFrictionArrow.SetActive(false);
            forceFrictionArrow2.SetActive(false);
            forceFrictionArrow3.SetActive(false);

            forceArrow.transform.position = new Vector3(-6, 2.35f, 12);
            forceArrowRb.velocity = Vector3.zero;
            forceArrowRb.isKinematic = false;

            if (pg.randomQuestion != 3 && pg.randomQuestion != 7)
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
        else if (PlayerPrefs.GetInt("LessonTask") == 3) //drop item in gravity
        {
            if (pg.randomQuestion < 3)
            {
                redBox.transform.position = new Vector3(0f, 4f, 6.5f);
                blueBox.transform.position = new Vector3(3f, 4f, 6.5f);
            }
            else
            {
                redBox.transform.position = new Vector3(0f, 1.25f, 6.5f);
                blueBox.transform.position = new Vector3(3f, 1.25f, 6.5f);
            }

            veloRb.isKinematic = false;
            veloRb.velocity = Vector3.zero;
            redRb.transform.localEulerAngles = Vector3.zero;
            redRb.velocity = Vector3.zero;
            
            GravityRedArrow2.transform.position = gravityA;
            veloDownArr2.transform.position = veloA;
            veloDownArr2.SetActive(false);
            veloRightArr.SetActive(false);
            accRightArr.SetActive(false);
            wall.SetActive(false);
            mass1.SetActive(false);
            mass2.SetActive(false);
            mass3.SetActive(false);
            boom.SetActive(false);
        }

        else if (PlayerPrefs.GetInt("LessonTask") == 4)
        {
            redBox.transform.position = new Vector3(0f, 0.75f, 6.5f);
            if(pg.randomQuestion == 5 || pg.randomQuestion == 10)//blue box run from another direction.
            {
                blueBox.transform.position = new Vector3(10f, 0.75f, 6.5f);
            }
            else
            {
                blueBox.transform.position = new Vector3(5f, 0.75f, 6.5f);
            }
            V1Arr.SetActive(false);
            V2Arr.SetActive(false);
            V3Arr.SetActive(false);
            V4Arr.SetActive(false);
            V5Arr.SetActive(false);
            V6Arr.SetActive(false);
            forceArrow6.SetActive(false);
        }
            Time.timeScale = 1;
            redRb.drag = 0;
            blueRb.drag = 0;
            redRb.useGravity = true;
            GravityRedArrow.SetActive(false);
            GravityBlueRedArrow.SetActive(false);
            blueBox.SetActive(false);
            veloDownArr.SetActive(false);
            veloUpArr.SetActive(false);
            ground.SetActive(false);
            table.SetActive(false);
        
    }
}
