using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollidStop : MonoBehaviour {
    public GameObject force2,RedBox,accArr,accArr2,accAtext;
    private Rigidbody arrowRigid, rbRigid, force2Rigid;
    public ProblemGenerator pg;

    // Use this for initialization
    void Start () {
        arrowRigid = gameObject.GetComponent<Rigidbody>();
        rbRigid = RedBox.GetComponent<Rigidbody>();
        force2Rigid = force2.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RedCube")
        {
            if(PlayerPrefs.GetInt("LessonTask") == 1 && pg.randomQuestion < 3)
            {
                accArr.SetActive(true);
                arrowRigid.velocity = new Vector3(0, 0, 0);
                force2Rigid.velocity = new Vector3(0, 0, 0);
                rbRigid.velocity = new Vector3(5, 0, 0);
            }
            else if (PlayerPrefs.GetInt("LessonTask") == 1 && pg.randomQuestion == 3)
            {
                accArr2.SetActive(true);
                accAtext.SetActive(true);
                arrowRigid.velocity = new Vector3(0, 0, 0);
                force2Rigid.velocity = new Vector3(0, 0, 0);
                rbRigid.velocity = new Vector3(4, 0, 2);
            }
        }
    }
}
