using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
        Vector3 pos = chaseTarget.position;
        
        // z축으로만 추적함
        Vector3 camCenter = new Vector3(0, 0, pos.z);
               
        transform.position = camCenter + offset;
    }
}
