using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{    
    #region 싱글톤
    public static GameOverManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameOverManager>();
            }
            return instance;
        }
    }
    private static GameOverManager instance;
    #endregion

    [SerializeField]
    TextMeshProUGUI currentscoreTxt;
    [SerializeField]
    TextMeshProUGUI bestscoreTxt;

    public void GameOver()
    {
        currentscoreTxt.text = ScoreManager.Instance.CurrentScore.ToString("N0");
        bestscoreTxt.text = SaveManager.BestScore.ToString("N0");

        Player.Instance.OnDead();
        UIManager_Play.Instance.ActiveOverUI(true);
        SelectionCreator.Instance.StopAllCoroutines();
        EnemySpwaner.Instance.StopAllCoroutines();
        SkillManager.Instance.StopAllCoroutines();
    }
}
