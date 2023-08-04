using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    #region 싱글톤
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
            }
            return instance;
        }
    }
    private static ScoreManager instance;
    #endregion

    int currentScore = 0;

    // 최적해 점수 : 가장 높은 결과값의 선택지만을 택했을 때 낼 수 있는 값
    [SerializeField]
    int optimalScore = 0;
    public int OptimalScore => optimalScore;

    [SerializeField]
    TMP_Text scoreText;

    private void Awake()
    {
        SetScore(100);
        SetOptimalScore(currentScore);
    }

    void LimitScore()
    {
        if (currentScore < 1)
        {
            Debug.Log("score limited");
            currentScore = 1;
        } 
    }

    void SyncUI()
    {        
        string str = "SCORE? : ";
        scoreText.text = str + currentScore;
    }  

    public void SetScore(int i)
    {
        currentScore = i;
        LimitScore();
        SyncUI();
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void SetOptimalScore(int i)
    {
        optimalScore = i;
    }

    public int GetOptimalScore()
    {
        return optimalScore;
    }
}
