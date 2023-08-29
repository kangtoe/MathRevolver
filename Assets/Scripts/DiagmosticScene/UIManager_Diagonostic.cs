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
    Canvas canvas;

    [Header("정답/오답 표시 UI")]
    [SerializeField]
    GameObject currectUI;
    [SerializeField]
    GameObject incurrectUI;

    [SerializeField] 
    BulletMag bulletMag;

    [SerializeField]
    TextMeshProUGUI LeftQuestionCount;

    public void CreateCurrectUI()
    {
        Instantiate(currectUI, canvas.transform);
    }

    public void CreateIncurrectUI()
    {
        Instantiate(incurrectUI, canvas.transform);
    }

    public void DoBulletUsingEffect()
    {
        bulletMag.UseBullet();
    }

    public void SetLeftQuestionCount(int i)
    {
        LeftQuestionCount.text = i.ToString();
    }
}
