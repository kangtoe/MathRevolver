using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 스크립트가 적용된 오브젝트에게, 플레이어와 z축으로 일정한 거리를 유지하도록 함
public class Z_Follow : MonoBehaviour
{ 
    float zInterval;

    Vector3 playerPos => Player.Instance.transform.position;

    // Start is called before the first frame update
    void Start()
    {        
        zInterval = transform.position.z - playerPos.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.z = playerPos.z + zInterval;
        transform.position = pos;

    }
}
