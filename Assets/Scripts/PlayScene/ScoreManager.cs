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

    static int MinScore = 1;
    static int MaxScore = 999999999;

    int currentScore = 0;

    int bestScore; // 최고 득점 점수
    string bestScoreName = "BestScore";

    [SerializeField]
    TMP_Text currentScoreText;
    [SerializeField]
    TMP_Text bestScoreText;

    [Header("시작 스코어")]
    [SerializeField]
    int startScore = 100;

    [Header("숫자 카운팅 효과")]
    [SerializeField]
    float countDuration = 0.33f;
    
    [Header("디버깅 : 최적해 점수 (가장 높은 결과값의 선택지를 택했을 때의 값)")]
    [SerializeField]
    int optimalScore = 0;
    public int OptimalScore => optimalScore;

    private void Awake()
    {
        // 최고 점수 설정
        bestScoreText.enabled = false;
        bestScore = SaveManager.BestScore;
                
        SetScore(startScore, false);
        SetOptimalScore(currentScore);
    }

    public void SetScore(int i, bool countEffect = true)
    {
        // 점수 제한 -> 1 이하로 떨어지지 않음
        i = Mathf.Clamp(i, MinScore, MaxScore);    

        // 카운트 효과
        float duration = 0;
        if (countEffect) duration = countDuration;
        StopAllCoroutines();
        StartCoroutine(CountTextCr(currentScoreText, currentScore, i, duration));
        
        // 현재 점수 갱신
        currentScore = i;

        // 최고 점수 갱신
        if (currentScore >= bestScore)
        {
            bestScoreText.enabled = true;
            bestScore = currentScore;
            SaveManager.BestScore = bestScore;
        }
        else
        {
            // 이전 최고 기록 텍스트가 활성화 중이었다면 비활성화
            bestScoreText.enabled = false;
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void SetOptimalScore(int i)
    {
        optimalScore = Mathf.Clamp(optimalScore, MinScore, MaxScore);
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
