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

    [SerializeField]
    float countDuration = 0.5f;

    // 최적해 점수 : 가장 높은 결과값의 선택지만을 택했을 때 낼 수 있는 값
    [SerializeField]
    int optimalScore = 0;
    public int OptimalScore => optimalScore;

    [SerializeField]
    TMP_Text scoreText;

    private void Awake()
    {
        SetScore(100, false);
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

    public void SetScore(int i, bool countEffect = true)
    {
        float duration = 0;
        if (countEffect) duration = countDuration;
        StopAllCoroutines();
        StartCoroutine(CountTextCr(scoreText, currentScore, i, duration));
        currentScore = i;

        //LimitScore();
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

    IEnumerator CountTextCr(TMP_Text text, int start, int end, float duration = 1)
    {
        float current = start;

        float t = 0;
        while (true)
        {
            t += Time.deltaTime / duration;
            if (t > 1) t = 1;

            current = Mathf.Lerp(start, end, t);
            text.text = ((int)current).ToString();

            yield return null;

            if (t == 1) break;
        }        
    }
}
