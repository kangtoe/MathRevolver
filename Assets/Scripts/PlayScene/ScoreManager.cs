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
        //Debug.Log("i :" + i);

        // 카운트 효과            
        float duration = countDuration;        
        if (!countEffect) duration = 0;        
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
        i = Mathf.Clamp(i, MinScore, MaxScore);
        optimalScore = i;
    }

    public int GetOptimalScore()
    {
        return optimalScore;
    }

    IEnumerator CountTextCr(TMP_Text text, int start, int end, float duration = 1)
    {
        //Debug.Log(text.name + ": start = " + start);
        //Debug.Log(text.name + ": end = " + end);        

        float current;
        float t = 0;
        while (true)
        {
            t += Time.deltaTime / duration;
            if (t > 1) t = 1;

            // 카운트 중간의 값 구하기
            current = Mathf.Lerp(start, end, t);
            // 아주 큰 수에서는 lerp 과정에서 오차가 발생함. 이를 보정            
            int i = start;
            if (start < end) i = Mathf.Clamp((int)current, start, end); // 증가 카운트
            if (start > end) i = Mathf.Clamp((int)current, end, start); // 감소 카운트
            //Debug.Log(text.name + ": i = " + i);
            UpdataText(text, i);

            yield return null;

            if (t == 1) break;            
        }        
    }

    void UpdataText(TMP_Text text, int i)
    {
        //Debug.Log(text.name + ": set = " + i);
        text.text = i.ToString();
    }
}
