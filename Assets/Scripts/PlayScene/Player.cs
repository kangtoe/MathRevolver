using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 싱글톤
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    private static Player instance;
    #endregion

    [Header("이동")]    
    [SerializeField]
    float forwardSpeed = 1f;
    [SerializeField]
    float sideSpeed = 1f;

    [Header("공격")]    
    //[SerializeField]
    //int damage = 10;
    [SerializeField]
    float attackInterval = 1;
    //float lastAttackTime = 0;
    bool canAttack = true;

    [Header("공격 이펙트")]
    [SerializeField]
    ParticleSystem nozzle;
    [SerializeField]
    ParticleSystem trail;

    // 코루틴
    Coroutine moveCr;
    Coroutine attackCr;


    // Start is called before the first frame update
    void Start()
    {
        attackCr = StartCoroutine(AttackCheckCr(attackInterval));   
    }

    // Update is called once per frame
    void Update()
    {
        // 전방으로 이동 (z축 이동)
        transform.Translate(forwardSpeed * transform.forward * Time.deltaTime);
    }

    #region X축 이동 제어

    public void MovePosX_Smooth(float targetX)
    {
        //Debug.Log("MovePosY_Smooth || targetY :" + targetX);
        
        if (moveCr != null) StopCoroutine(moveCr);        
        moveCr = StartCoroutine(MovePosX_SmoothCr(targetX, sideSpeed));
    }

    IEnumerator MovePosX_SmoothCr(float targetX, float speed = 1)
    {
        while (true)
        {
            float currentX = transform.position.x;

            // 현재 X 위치를 목표 위치 방향으로 조금씩 이동
            if (targetX > currentX) currentX += Time.fixedDeltaTime * speed;
            else if (targetX < currentX) currentX -= Time.fixedDeltaTime * speed;

            bool closeEnough = Mathf.Abs(targetX - currentX) <= Time.fixedDeltaTime * speed;
            if (closeEnough) currentX = targetX;

            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            if (currentX == targetX) yield break;

            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    #region 공격

    // 공격 검사
    IEnumerator AttackCheckCr(float interval)
    {
        while (true)
        {
            if (canAttack)
            {
                TryAtack();
            }

            yield return new WaitForSeconds(interval);
        }
    }

    void TryAtack()
    {
        //Enemy target = EnemySpwaner.Instance.GetClosestEnemy_Z();
        Enemy target = EnemySpwaner.Instance.GetClosestEnemy_Transform(transform);

        //bug.Log("TryAtack");

        // 가장 가까운 적 공격
        if (target is null)
        {
            Debug.Log("target is null");
            return;
        }
        //else Debug.Log("target :" + target.name);

        //lastAttackTime = Time.time;
        Attack(target);
    }

    void Attack(Enemy target)
    {
        // 사격 시각 효과
        nozzle.Play();
        trail.Play();
        trail.transform.LookAt(target.transform.position);

        int damage = ScoreManager.Instance.GetScore();
        target.OnHit(damage);
    }

    #endregion


}
