using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField]
    float speed;

    // Update is called once per frame
    void Update()
    {
        // 전방으로 이동 (z축 이동)
        transform.Translate(speed * transform.forward * Time.deltaTime);
    }
}
