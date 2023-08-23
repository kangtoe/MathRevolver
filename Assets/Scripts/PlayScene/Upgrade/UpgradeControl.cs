using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeControl : MonoBehaviour
{
    #region 싱글톤
    public static UpgradeControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UpgradeControl>();
            }
            return instance;
        }
    }
    private static UpgradeControl instance;
    #endregion

    [Header("레벨 한계")]
    [SerializeField]
    int maxLv = 10;

    [Header("공격 속도")] // 초당 공격 횟수
    [SerializeField]
    int attackSpeedLv;
    [SerializeField]
    float attackSpeedMin = 1; 
    [SerializeField]
    float attackSpeedMax = 0.5f;
    float CurrentAttackSpeed => Mathf.Lerp(attackSpeedMin, attackSpeedMax, (float)attackSpeedLv / maxLv);

    [Header("스킬 지속 시간")]
    [SerializeField]
    int skillTimeLv;
    [SerializeField]
    float skillTimeMin = 3;
    [SerializeField]
    float skillTimeMax = 6;
    float CurrentSkillTime => Mathf.Lerp(skillTimeMin, skillTimeMax, (float)skillTimeLv / maxLv);

    [Header("문제 풀이 시간")]        
    [SerializeField]
    int solvingTimeLv;
    [SerializeField]
    float solvingTimeMin = 5;
    [SerializeField]
    float solvingTimeMax = 10;
    float CurrentSlovingTime => Mathf.Lerp(solvingTimeMin, solvingTimeMax, (float)solvingTimeLv / maxLv);

    [Header("제어할 UI")]
    [SerializeField]
    UpgradeButton attackSpeedBtn;
    [SerializeField]
    UpgradeButton skillTimeBtn;
    [SerializeField]
    UpgradeButton solvingTimeBtn;
    [SerializeField]
    UpgradeButton recoverHeartBtn;

    [Header("레벨 숫자 앞 문자열")]
    [SerializeField]
    string levelText = "Lv.";

    void Start()
    {
        //Debug.Log("Start");
        InitButtons();
    }

    #region 텍스트 갱신

    void UpdateAttackSpeedText()
    {
        string str;

        // level 텍스트 갱신
        str = levelText;
        if (attackSpeedLv == maxLv) str += "MAX";
        else str += attackSpeedLv;
        attackSpeedBtn.SetLevelText(str);

        // desc 텍스트 갱신
        str = "공격 간격 : " +  CurrentAttackSpeed + "초";
        attackSpeedBtn.SetDescText(str);
    }

    void UpdateSkillTimeText()
    {
        string str;

        // level 텍스트 갱신
        str = levelText;
        if (skillTimeLv == maxLv) str += "MAX";
        else str += skillTimeLv;
        skillTimeBtn.SetLevelText(str);

        // desc 텍스트 갱신
        str = "스킬 지속 : " + CurrentSkillTime + "초";
        skillTimeBtn.SetDescText(str);
    }

    void UpdateSlovingTimeText()
    {
        string str;

        // level 텍스트 갱신
        str = levelText;
        if (solvingTimeLv == maxLv) str += "MAX";
        else str += solvingTimeLv;
        solvingTimeBtn.SetLevelText(str);

        // desc 텍스트 갱신
        str = "풀이 시간 : " + CurrentSlovingTime + "초";
        solvingTimeBtn.SetDescText(str);
    }

    #endregion


    void InitButtons()
    {
        UpdateAttackSpeedText();
        UpdateSkillTimeText();
        UpdateSlovingTimeText();

        // attackSpeedBtn 버튼 초기화
        attackSpeedBtn.SetLevelText(levelText + attackSpeedLv);
        attackSpeedBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 레벨 증가
            attackSpeedLv++;
            attackSpeedLv = Mathf.Clamp(attackSpeedLv, 0, maxLv);

            // 텍스트 갱신            
            UpdateAttackSpeedText();            

            // UI 비활성화
            //UIManager.Instance.ActiveUpgradeUI(false);
        });

        // skillTimeBtn 버튼 초기화
        skillTimeBtn.SetLevelText(levelText + skillTimeLv);
        skillTimeBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 레벨 증가
            skillTimeLv++;
            skillTimeLv = Mathf.Clamp(skillTimeLv, 0, maxLv);

            // 텍스트 갱신
            UpdateSkillTimeText();

            // UI 비활성화
            //UIManager.Instance.ActiveUpgradeUI(false);
        });

        // solvingTimeBtn 버튼 초기화
        solvingTimeBtn.SetLevelText(levelText + solvingTimeLv);
        solvingTimeBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 레벨 증가
            solvingTimeLv++;
            solvingTimeLv = Mathf.Clamp(solvingTimeLv, 0, maxLv);

            // 텍스트 갱신
            UpdateSlovingTimeText();

            // UI 비활성화
            //UIManager.Instance.ActiveUpgradeUI(false);
        });

        // recoverHeartBtn 버튼 초기화
        recoverHeartBtn.SetLevelText(levelText + "--");
        recoverHeartBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 회복 처리 및 시각 효과

            // UI 비활성화
            //UIManager.Instance.ActiveUpgradeUI(false);
        });
    }
}
