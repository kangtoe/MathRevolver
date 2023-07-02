using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Transform chaseTarget;

    [SerializeField]
    Vector3 offset = new Vector3(0, 0, 2.5f);

    // Start is called before the first frame update
    void Start()
    {
        chaseTarget = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        // z축으로만 추적함
        pos.z = chaseTarget.position.z + offset.z;
        transform.position = pos;
    }
}
