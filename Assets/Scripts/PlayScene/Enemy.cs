using System.Collections;
using UnityEngine;
//using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    VisibleCheck visibleCheck;

    [SerializeField]
    Animator anim;

    // 체력
    [Header("체력")]
    //int maxHp;
    [SerializeField]    
    int currentHp;
    [SerializeField]
    TMPro.TextMeshPro hpText;

    [Header("체력 배수")]    
    [SerializeField]
    float healthMult = 1; // 체력 계수
    public float HealthMult => healthMult;

    [Header("이동속도")]
    [SerializeField]
    float moveSpeed = 0.1f;

    [Header("플레이어에 대한 상대 속도로 이동하는가?")]
    [SerializeField]
    bool isMoveRelative;

    [Header("격파 시 스킬 쿨타임 감소")]
    [SerializeField]
    float skillCooltimeReduce = 1f;

    [Header("하트 손실")]
    [SerializeField]
    int heartLost = 1;

    [Header("사망")]
    [SerializeField]
    GameObject dieVfx;    

    [Header("보스인가?")]
    [SerializeField]
    public bool isBoss = false;

    // 현재 공격 가능한가?
    public bool CanBeAttacked => IsFrontToPlayer && visibleCheck.Visible;

    // (z축) player를 지나치지 않았는가?
    bool IsFrontToPlayer => transform.position.z > Player.Instance.transform.position.z;

    // 플레이어와 z 간격
    float zInterval;

    SkillManager skill => SkillManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        AddSelfOnList();

        // 플레이어와 z 간격 구하기
        zInterval = Player.Instance.transform.position.z - transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // player를 지나쳐 카메라 벗어남 -> 삭제
        if (!visibleCheck.Visible && !IsFrontToPlayer)
        {
            Debug.Log("적 지나침 : " + gameObject.name);
            HeartsManager.Instance.HeartLost(heartLost);
            DeleteEnemy();
        }

        // 이동 처리
        if (isMoveRelative)
        {                        
            Vector3 pos = transform.position;
            pos.z = Player.Instance.transform.position.z - zInterval;
            transform.position = pos;

            zInterval += moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }        
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter : " + collision);

        if (collision.gameObject.tag == "Player")
        {
            // 플레이어와 충돌 시, 지나쳐 간 것과 동일한 처리
            HeartsManager.Instance.HeartLost(heartLost);
            OnDie();
        }
    }

    public void Init(int _maxHp)
    {
        //maxHp = _maxHp;
        currentHp = _maxHp;
        hpText.text = currentHp.ToString();
    }

    public void OnHit(int damage)
    {
        //Debug.Log(gameObject.name + " || OnHit : " + damage);

        DoHitEffect();

        // 피해량 텍스트로 표기
        //Vector3 textPos = transform.position + Vector3.right * -0.3f + Vector3.up * 0.5f;
        //Text3dMaker.Instance.MakeText(damage.ToString(), textPos);

        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // UI 반영
        hpText.text = currentHp.ToString();

        // 적 처치 성공
        if (currentHp == 0)
        {
            // 스킬 쿨타임 중에만
            if (skill.IsOnCoolTime)
            {
                // 남은 쿨타임 감소
                skill.AdjustCoolTimeLeft(-skillCooltimeReduce);
            }
            
            OnDie();
        } 
    }

    void OnDie()
    {
        SoundManager.Instance.PlaySound("monsterdie");
        Vector3 pos = transform.position + Vector3.up * 0.5f;
        Instantiate(dieVfx, pos, Quaternion.identity);

        DeleteEnemy();
    }

    // 사라질 때 처리 (점수 감소 연출 등..)
    void DeleteEnemy()
    {
        DeleteSelfOnList();
        Destroy(gameObject);
    }

    #region 적 리스트 관리

    // 적 리스트에 등록
    void AddSelfOnList()
    {
        EnemySpwaner.Instance.enemies.Add(this);
    }

    // 적 리스트에서 해제
    void DeleteSelfOnList()
    {
        EnemySpwaner.Instance.enemies.Remove(this);
    }

    #endregion

    #region 피격 연출

    // 흔들림 연출 코루틴 호출
    public void DoHitEffect()
    {
        // 지속시간
        float duration = 0.15f;

        StopAllCoroutines();
        StartCoroutine(ShackPosCr(duration, anim.transform, 0.1f));
        StartCoroutine(ShackPosCr(duration, hpText.transform, 0.05f));
        StartCoroutine(ColorChangeCr(duration));
    }

    IEnumerator ShackUiCr()
    {
        yield return null;
    }

    // todo : 색상 변경 코루틴
    IEnumerator ColorChangeCr(float duration)
    {
        yield return null;
    }

    // 흔들림 연출 코루틴
    IEnumerator ShackPosCr(float duration, Transform tf, float amount = 0.1f, float interval = 0.05f)
    {
        Vector3 startPos = tf.localPosition;
        float leftTime = duration;

        bool plusMinus = true;

        while (true)
        {
            leftTime -= interval;

            // 남은 시간 종료 : 원래 위치로
            if (leftTime < 0)
            {
                tf.localPosition = startPos;
                break;
            }

            // 흔들림 구하기
            // 초기에는 무작위 값을 사용했으나, 고정 값이 연출상 더 나은 것으로 사료됨
            Vector3 shake;
            if (plusMinus) shake = Vector3.right;
            else shake = Vector3.left;
            shake *= amount;

            // 흔들림 방향 반전
            plusMinus = !plusMinus;

            // 흔들림 적용
            tf.localPosition += shake;

            yield return new WaitForSeconds(interval);
        }
    }

    #endregion


}
