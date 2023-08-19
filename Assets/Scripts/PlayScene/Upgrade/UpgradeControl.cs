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

    [SerializeField]
    GameObject upgradeUI;

    [Header("레벨 한계")]
    [SerializeField]
    int maxLv = 10;

    [Header("현재 레벨")]
    [SerializeField]
    int attackSpeedLv;
    [SerializeField]
    int skillTimeLv;
    [SerializeField]
    int solvingTimeLv;

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

    void InitButtons()
    {
        // attackSpeedBtn 버튼 초기화
        attackSpeedBtn.SetLevelText(levelText + attackSpeedLv);
        attackSpeedBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 레벨 증가
            attackSpeedLv++;
            attackSpeedLv = Mathf.Clamp(attackSpeedLv, 0, maxLv);

            // 텍스트 갱신
            string str = levelText;
            if (attackSpeedLv == maxLv) str += "MAX";
            else str += attackSpeedLv;
            attackSpeedBtn.SetLevelText(str);
            
            //upgradeUI.SetActive(false);
        });

        // skillTimeBtn 버튼 초기화
        skillTimeBtn.SetLevelText(levelText + skillTimeLv);
        skillTimeBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 레벨 증가
            skillTimeLv++;
            skillTimeLv = Mathf.Clamp(skillTimeLv, 0, maxLv);

            // 텍스트 갱신
            string str = levelText;
            if (skillTimeLv == maxLv) str += "MAX";
            else str += skillTimeLv;
            skillTimeBtn.SetLevelText(str);

            //upgradeUI.SetActive(false);
        });

        // solvingTimeBtn 버튼 초기화
        solvingTimeBtn.SetLevelText(levelText + solvingTimeLv);
        solvingTimeBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 레벨 증가
            solvingTimeLv++;
            solvingTimeLv = Mathf.Clamp(solvingTimeLv, 0, maxLv);

            // 텍스트 갱신
            string str = levelText;
            if (solvingTimeLv == maxLv) str += "MAX";
            else str += solvingTimeLv;
            solvingTimeBtn.SetLevelText(str);

            //upgradeUI.SetActive(false);
        });

        // recoverHeartBtn 버튼 초기화
        recoverHeartBtn.SetLevelText(levelText + "--");
        recoverHeartBtn.Button.onClick.AddListener(delegate // 클릭 시 처리
        {
            // 회복 처리 및 시각 효과

            //upgradeUI.SetActive(false);
        });
    }
}
