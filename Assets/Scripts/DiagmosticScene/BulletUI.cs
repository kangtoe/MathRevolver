using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUI : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;

    public BulletUI Use(Vector2 force, float torque, float gravScale = 1)
    {
        //Debug.Log("BulletUI : " + gameObject.name + " || Use");

        Destroy(gameObject);

        // 물리엔진 연출
        {
            //// 힘 가하기
            //rb.simulated = true;
            //rb.AddForce(force * transform.up, ForceMode2D.Impulse);
            //rb.AddTorque(torque);
            //rb.gravityScale = gravScale;

            //Destroy(gameObject, 5f);
        }        

        return this;
    }
}
