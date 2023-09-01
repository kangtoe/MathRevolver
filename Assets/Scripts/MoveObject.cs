using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField]
    float speed;

    private void Start()
    {
        //Debug.Log("Time.timeScale : " + Time.timeScale);
    }

    // Update is called once per frame
    void Update()
    {
        // 전방으로 이동 (z축 이동)
        transform.Translate(speed * transform.forward * Time.deltaTime);
    }
}
