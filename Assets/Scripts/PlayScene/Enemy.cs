using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;

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
    [SerializeField]
    TMPro.TextMeshPro text;

    // 이동
    [SerializeField]
    float moveSpeed = 0.1f;

    // (z축) player를 지나치지 않았는가?
    bool IsFrontToPlayer => transform.position.z > Player.Instance.transform.position.z;

    // 현재 공격 가능한가?
    public bool CanBeAttacked => IsFrontToPlayer && visibleCheck.Visible;

    // Start is called before the first frame update
    void Start()
    {
        AddSelfOnList();        
    }

    // Update is called once per frame
    void Update()
    {
        // player를 지나쳐 카메라 벗어남 -> 삭제
        if (!visibleCheck.Visible && !IsFrontToPlayer) DeleteEnemy();

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void Init(int _maxHp)
    {
        currentHp = maxHp = _maxHp;
        text.text = currentHp.ToString();
    }

    public void OnHit(int damage)
    {
        // todo: 시각적 연출 효과 더하기
        DoHitEffect();

        Vector3 textPos = transform.position + Vector3.right * -0.3f + Vector3.up * 0.5f;
        Text3dMaker.Instance.MakeText(damage.ToString(), textPos);

        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // UI 반영
        text.text = currentHp.ToString();

        if (currentHp == 0) OnDie();
    }

    void OnDie()
    {
        // todo : animator 파라메터 조절 또는 die prefab(파티클 시스템 등) 생성

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

    #endregion


}
