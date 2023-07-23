using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Revolver : MonoBehaviour
{    
    [SerializeField]
    GameObject movePoint;

    [SerializeField]
    GameObject lookPoint;

    [SerializeField]
    float moveSpeed = 5;

    [SerializeField]
    float rotatingSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR        
        if (!Application.isPlaying)
        {
            transform.position = movePoint.transform.position;
        }
#endif
        Move();
    }

    // movePoint를 목표로 부드럽게 이동
    void Move()
    {        
        transform.position = Vector3.Lerp(transform.position, movePoint.transform.position, Time.deltaTime * moveSpeed);
    }

    // ookPoint를 목표로 부드럽게 회전
    void Look()
    { 
    
    }
}
