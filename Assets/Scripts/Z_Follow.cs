using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ��ũ��Ʈ�� ����� ������Ʈ����, �÷��̾�� z������ ������ �Ÿ��� �����ϵ��� ��
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
