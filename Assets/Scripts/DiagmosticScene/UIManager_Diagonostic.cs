using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager_Diagonostic : MonoBehaviour
{
    #region 싱글톤
    public static UIManager_Diagonostic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager_Diagonostic>();
            }
            return instance;
        }
    }
    private static UIManager_Diagonostic instance;
    #endregion

    [SerializeField] 
    BulletMag bulletMag;

    [SerializeField]
    TextMeshProUGUI LeftQuestionCount;

    [SerializeField]
    TextMeshProUGUI DiagonosisScore;

    public void DoBulletUsingEffect()
    {
        bulletMag.UseBullet();
    }

    public void SetLeftQuestionCount(int i)
    {
        LeftQuestionCount.text = i.ToString();
    }

    public void SetDiagonosisScore(int i)
    {
        DiagonosisScore.text = i.ToString();
    }
}
