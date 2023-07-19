using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    VisibleCheck visibleCheck;

    [SerializeField]
    Animator anim;

    // (z축) player를 지나치지 않았는가?
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

    // 사라질 때 처리 (점수 감소 연출 등..)
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
