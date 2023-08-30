using System;
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

    [SerializeField]
    TMP_Text currentScoreText;
    [SerializeField]
    TMP_Text bestScoreText;
            
    [Header("숫자 카운팅 효과")]
    [SerializeField]
    float countDuration = 0.33f;
    
    [Header("디버깅 : 최적해 점수 (가장 높은 결과값의 선택지를 택했을 때의 값)")]
    [SerializeField]
    double optimalScore = 0;
    public double OptimalScore => optimalScore;

    // 제한 점수
    static double MinScore = 1;
    static double MaxScore = 999999999;

    // 최고 득점 점수
    double bestScore; 
    string bestScoreName = "BestScore";
   
    // 현재 점수
    double currentScore = 0;
    public double CurrentScore => currentScore;

    // 시작 점수
    double startScore => SaveManager.DiagonosticScore;

    private void Awake()
    {
        // 최고 점수 설정
        bestScoreText.enabled = false;
        bestScore = SaveManager.BestScore;
                
        SetScore(startScore, false);
        SetOptimalScore(currentScore);
    }

    #region double 연산

    double ClampDouble(double val, double min, double max)
    {
        if (val < min)
        {
            //Debug.Log("val < MinScore");
            return min;
        }
        if (max < val)
        {
            //Debug.Log("MaxScore < val");
            return max;
        }
        return val;
    }

    double Interpolate(double a, double b, double t)
    {
        t = ClampDouble(t, 0, 1);

        //Debug.Log(" t = " + t);
        double interval = b - a;
        //Debug.Log("interval : " + interval);
        double val1 = (double)(1d - t);
        //Debug.Log("val1 : " + val1);
        double val2 = val1 * interval;
        //Debug.Log("val2 : " + val2);

        return b - val2;
    }

    #endregion

    public void SetScore(double _score, bool countEffect = true)
    {
        // 점수 제한 -> 1 이하로 떨어지지 않음
        //f = Mathf.Clamp(f, MinScore, MaxScore);
        _score = ClampDouble(_score, MinScore, MaxScore);
        //Debug.Log("_score :" + _score);

        // 카운트 효과            
        float duration = countDuration;        
        if (!countEffect) duration = 0;        
        StopAllCoroutines();
        StartCoroutine(CountTextCr(currentScoreText, currentScore, _score, duration));

        // 현재 점수 갱신
        currentScore = Math.Truncate(_score); // 소수점 이하를 버림으로써 오차의 누적을 방지

        // 최고 점수 갱신
        if (currentScore >= bestScore)
        {
            SoundManager.Instance.PlaySound("bestscore");
            bestScoreText.enabled = true;
            bestScore = Math.Round(currentScore);            
            SaveManager.BestScore = (int)Math.Floor(currentScore);
        }
        else
        {
            // 이전 최고 기록 텍스트가 활성화 중이었다면 비활성화
            bestScoreText.enabled = false;
        }
    }

    public double GetCurrentScore()
    {
        return currentScore;
    }

    public void SetOptimalScore(double _score)
    {
        //f = Mathf.Clamp(f, MinScore, MaxScore);
        _score = ClampDouble(_score, MinScore, MaxScore);
        optimalScore = Math.Truncate(_score);  // 소수점 이하를 버림으로써 오차의 누적을 방지
    }

    public double GetOptimalScore()
    {
        return optimalScore;
    }

    IEnumerator CountTextCr(TMP_Text text, double start, double end, float duration = 1)
    {
        //Debug.Log(text.name + ": start = " + start);
        //Debug.Log(text.name + ": end = " + end);        

        double current;
        double t = 0;
        while (true)
        {
            t += Time.deltaTime / duration;
            if (t > 1) t = 1;
            //Debug.Log(text.name + ": t = " + t);

            // 카운트 중간의 값 구하기
            current = Interpolate(start, end, t);

            // 아주 큰 수에서는 lerp 과정에서 오차가 발생함. 이를 보정            
            //int i = start;
            //if (start < end) i = Mathf.Clamp((int)current, start, end); // 증가 카운트
            //if (start > end) i = Mathf.Clamp((int)current, end, start); // 감소 카운트            
            //UpdataText(text, i);

            //Debug.Log(text.name + ": current = " + current);
            UpdataText(text, current);

            yield return null;

            if (t == 1) break;            
        }        
    }
    
    void UpdataText(TMP_Text text, double val)
    {
        // 소수부 버림하여 정수부만 표기
        //string str = Mathf.FloorToInt(f).ToString();
        string str = Math.Truncate(val).ToString();
        //Debug.Log(text.name + ": val rounded = " + str);
        text.text = str;
    }
}
