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

    int score = 0;

    [SerializeField]
    TMP_Text scoreText;

    private void Awake()
    {
        SetScore(100);
    }

    void LimitScore()
    {
        if (score < 1) score = 1;
    }

    void SyncUI()
    {        
        string str = "SCORE? : ";
        scoreText.text = str + score;
    }  

    public void SetScore(int i)
    {
        score = i;
        LimitScore();
        SyncUI();
    }

    public int GetScore()
    {
        return score;
    }
}
