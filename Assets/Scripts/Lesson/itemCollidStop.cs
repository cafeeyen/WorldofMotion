using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollidStop : MonoBehaviour
{
    public GameObject force2, RedBox, accArr, accArr2;
    private Rigidbody arrowRigid, rbRigid, force2Rigid;
    public ProblemGenerator pg;

    void Start()
    {
        arrowRigid = gameObject.GetComponent<Rigidbody>();
        rbRigid = RedBox.GetComponent<Rigidbody>();
        force2Rigid = force2.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RedCube")
        {
            if (PlayerPrefs.GetInt("LessonTask") == 1 && pg.randomQuestion < 3)
            {
                accArr.SetActive(true);

                arrowRigid.isKinematic = true;
                rbRigid.isKinematic = false;

                rbRigid.transform.localEulerAngles = new Vector3(0, 0, 0);
                arrowRigid.velocity = new Vector3(0, 0, 0);
                force2Rigid.velocity = new Vector3(0, 0, 0);
                rbRigid.velocity = new Vector3(5, 0, 0);
            }
            else if (PlayerPrefs.GetInt("LessonTask") == 1 && pg.randomQuestion == 3)
            {
                arrowRigid.isKinematic = true;
                force2Rigid.isKinematic = true;
                rbRigid.isKinematic = false;
                accArr2.SetActive(true);
                arrowRigid.velocity = new Vector3(0, 0, 0);
                force2Rigid.velocity = new Vector3(0, 0, 0);
                rbRigid.velocity = new Vector3(4, 0, 2);
            }
        }
    }
}
