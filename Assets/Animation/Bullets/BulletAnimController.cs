using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Animator cylinder;
    //[SerializeField] BulletMag bulletMag;

    void CylinderFlash()
    {
        cylinder.SetTrigger("flash");
        //bulletMag.UseBullet();
    }

    void NextProblem()
    {
        anim.SetTrigger("appear");
        WJ_Sample_Diagonostic.Instance.UpdateQuestionUI();
    }
}
