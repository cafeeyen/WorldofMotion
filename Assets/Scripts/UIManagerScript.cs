using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour
{
    public void ChangeBoolAnimator(Animator anim)
    {
        if(anim.GetBool("IsDisplayed") == true)
            anim.SetBool("IsDisplayed", false);
        else
            anim.SetBool("IsDisplayed", true);
    }
}