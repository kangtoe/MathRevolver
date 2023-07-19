using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    VisibleCheck visibleCheck;

    [SerializeField]
    Animator anim;

    // (z��) player�� ����ġ�� �ʾҴ°�?
    bool isFrontToPlayer => transform.position.z > Player.Instance.transform.position.z;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("start");
        AddSelfOnList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!visibleCheck.Visible && !isFrontToPlayer) DeleteEnemy();
    }

    // ����� �� ó�� (���� ���� ���� ��..)
    void DeleteEnemy()
    {
        DeleteSelfOnList();
        Destroy(gameObject);
    }

    void AddSelfOnList()
    {
        EnemySpwaner.Instance.enemies.Add(this);
    }

    void DeleteSelfOnList()
    {
        EnemySpwaner.Instance.enemies.Remove(this);
    }
}
