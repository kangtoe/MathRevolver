using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Image fill;

    [SerializeField]
    float coolTime = 10;

    [SerializeField]
    float coolTimeLeft;

    [SerializeField]
    float skillActiveTime = 3f;

    public bool IsSkillActive => isSkillActive;
    [SerializeField] // 디버그용
    bool isSkillActive;

    // 디버그용
    [SerializeField] 
    Text activeCheck;

    private void Start()
    {
        activeCheck.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustCoolTimeLeft(-Time.deltaTime);
    }

    // time 변수만큼 남은 쿨타임 조절
    public void AdjustCoolTimeLeft(float time)
    {
        coolTimeLeft += time;

        // 유효값으로 제한
        coolTimeLeft = Mathf.Clamp(coolTimeLeft, 0, coolTime);

        // UI 반영
        float ratio = 1 - coolTimeLeft / coolTime;
        fill.fillAmount = ratio;
    }

    public void SetSkillActiveTime(float time)
    {
        skillActiveTime = time;
    }

    public void UseSkill()
    {
        if (coolTimeLeft > 0)
        {
            return;
        }
        // 이전 스킬 코루틴 중지
        StopAllCoroutines();
        Debug.Log("스킬 사용");
        StartCoroutine(SkillCr());
        coolTimeLeft = coolTime;
    }


    IEnumerator SkillCr()
    {
        // 스킬효과 활성화
        isSkillActive = true;
        SelectionCreator.Instance.ActiveAllOptimalVFX(true);
        activeCheck.enabled = true;

        yield return new WaitForSeconds(skillActiveTime);

        // 스킬효과 비활성화
        isSkillActive = false;
        SelectionCreator.Instance.ActiveAllOptimalVFX(false);
        activeCheck.enabled = false;
    }
}
