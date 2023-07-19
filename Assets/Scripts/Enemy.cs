using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    VisibleCheck visibleCheck;

    [SerializeField]
    Animator anim;

    // 체력
    [SerializeField]
    int maxHp;
    [Header("디버그용 : 현재 체력")]
    [SerializeField]
    int currentHp;

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

    void Init()
    { 
        
    }

    void OnHit(int damage)
    {
        // todo: 시각적 연출 효과 더하기
        DoHitEffect();

        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // todo : UI 반영

        if (currentHp == 0) OnDie();
    }

    void OnDie()
    {
        // todo : animator 파라메터 조절

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



    // 흔들림 연출 코루틴 호출
    public void DoHitEffect()
    {
        // 지속시간
        float duration = 0.3f;
        
        StopAllCoroutines();
        StartCoroutine(ShackPosCr(duration));
        StartCoroutine(ColorChangeCr(duration));
    }

    // todo : 색상 변경 코루틴
    IEnumerator ColorChangeCr(float duration)
    {
        yield return null;
    }

    // 흔들림 연출 코루틴
    IEnumerator ShackPosCr(float duration)
    {
        // 코루틴 간격
        float interval = 0.05f;
        // 흔들림 정도
        float amount = 0.1f;

        // 자식 오브젝트 로컬 위치 구하기        
        Vector3 GetPosition()
        { 
            return anim.transform.localPosition;
        }
        // 자식 오브젝트 로컬 위치 정하기
        void SetPosition(Vector3 pos)
        {
            anim.transform.localPosition = pos;
        }    

        Vector3 startPos = GetPosition();
        float leftTime = duration;

        bool plusMinus = true;

        while (true)
        {            
            leftTime -= interval;            

            // 남은 시간 종료 : 원래 위치로
            if (leftTime < 0)
            {
                SetPosition(startPos);
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
            Vector3 pos = GetPosition() + shake;
            SetPosition(pos);
            
            yield return new WaitForSeconds(interval);
        }
    }
}
