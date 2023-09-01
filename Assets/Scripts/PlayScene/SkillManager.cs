using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SkillState
{
    Undefined,
    OnCooltime,
    Flash,
    Ready,
    OnActive
}

public class SkillManager : MonoBehaviour
{
    #region 싱글톤
    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillManager>();
            }
            return instance;
        }
    }
    private static SkillManager instance;
    #endregion    
    
    [SerializeField]
    float coolTime = 10;

    [SerializeField]
    float coolTimeLeft;

    [Header("디버그 : 스킬 지속 시간")]
    [SerializeField]
    float skillActiveTime = 3f;
    [SerializeField]
    float skillActiveTimeLeft;

    [Header("디버그 : 스킬 상태")]
    [SerializeField]
    SkillState state;

    //[Header("디버그 : 스킬 사용 중 표시")]
    //[SerializeField]
    //Text activeCheck;

    [Header("스킬 스프라이트")]
    [SerializeField]
    Sprite empty;
    [SerializeField]
    Sprite loaded;
    [SerializeField]
    Sprite flash;

    [Header("스킬 fill 이미지")]
    [SerializeField]
    Image fill;

    [Header("플레이어 스킬 잔상 효과")]
    [SerializeField]
    SkinnedMeshAfterImage afterImage;

    public bool IsSkillOnActive => state == SkillState.OnActive;
    public bool IsOnCoolTime => state == SkillState.OnCooltime;

    private void Start()
    {
        coolTimeLeft = coolTime;
        StartCoolDown();
    }

    public void SetSkillActiveTime(float time)
    {
        skillActiveTime = time;
    }

    //time 변수만큼 남은 쿨타임 조절
    public void AdjustCoolTimeLeft(float time)
    {
        Debug.Log("AdjustCoolTimeLeft : " + time);        

        coolTimeLeft += time;

        // 유효값으로 제한
        coolTimeLeft = Mathf.Clamp(coolTimeLeft, 0, coolTime);

        if (state == SkillState.OnCooltime)
        {
            // UI 반영
            float ratio = 1 - coolTimeLeft / coolTime;
            fill.fillAmount = ratio;
        }        
    }

    // 스킬 쿨다운 시작 : 스킬 사용 종료 시
    void StartCoolDown()
    {        
        // 스킬 쿨다운 중 실행
        IEnumerator CoolDownCr()
        {
            // 스킬 준비 : 스킬 쿨다운 완료 시 1번
            IEnumerator SkillReadyCr()
            {
                Debug.Log("SkillReadyCr");

                state = SkillState.Flash;
                fill.sprite = flash;
                yield return new WaitForSeconds(0.1f);

                state = SkillState.Ready;
                fill.sprite = loaded;
                // 사운드 : 스킬 준비 완료
                SoundManager.Instance.PlaySound("skillready");
            }

            Debug.Log("CoolDownCr");

            state = SkillState.OnCooltime;

            while (true)
            {
                yield return new WaitForFixedUpdate();

                // 변수 값 감소
                coolTimeLeft -= Time.fixedDeltaTime;
                if (coolTimeLeft < 0) coolTimeLeft = 0;

                // ui 반영
                float ratio = 1 - coolTimeLeft / coolTime;
                fill.fillAmount = ratio;

                // 쿨타임 종료 : 스킬 사용 가능
                if (coolTimeLeft == 0)
                {
                    coolTimeLeft = coolTime;
                    StartCoroutine(SkillReadyCr());
                    break;
                }
            }
        }

        StopAllCoroutines();
        StartCoroutine(CoolDownCr());
    }

    // 스킬 사용 : 스킬 사용 버튼에서 호출?
    public void SkillActive()
    {
        // 스킬 사용 중 실행
        IEnumerator SkillActivatingCr()
        {
            Debug.Log("SkillActivatingCr");

            state = SkillState.OnActive;
            afterImage.enabled = true;

            while (true)
            {
                yield return new WaitForFixedUpdate();

                // 선택지 가이드 효과 활성화
                SelectionCreator.Instance.ActiveAllOptimalVFX(true);

                // 변수 값 감소
                skillActiveTimeLeft -= Time.fixedDeltaTime;
                if (skillActiveTimeLeft < 0) skillActiveTimeLeft = 0;

                // ui 반영
                float ratio = skillActiveTimeLeft / skillActiveTime;
                fill.fillAmount = ratio;

                // 스킬 사용 시간 종료 : 쿨다운 시작
                if (skillActiveTimeLeft == 0)
                {
                    afterImage.enabled = false;

                    // 선택지 가이드 효과 비활성화
                    SelectionCreator.Instance.ActiveAllOptimalVFX(false);
                    
                    fill.sprite = empty;

                    skillActiveTimeLeft = skillActiveTime;
                    StartCoolDown();
                    break;
                }
            }
        }

        if (state != SkillState.Ready)
        {
            Debug.Log("스킬 사용 상태가 아님 : " + state);
            return;
        }

        SoundManager.Instance.PlaySound("skill");
        StopAllCoroutines();
        StartCoroutine(SkillActivatingCr());
    }
}
