using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Calc
{
    public CalcType type;
    public int val;
}

[ExecuteInEditMode]
public class SpwanPercent : MonoBehaviour
{
    #region 싱글톤
    public static SpwanPercent Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpwanPercent>();
            }
            return instance;
        }
    }
    private static SpwanPercent instance;
    #endregion

    [Header("최대 합 선택지 배수")]
    [SerializeField]
    float maxAddMult;
    public float MaxAddMult => maxAddMult;
    [Header("최소 합 선택지 배수")]
    [SerializeField]
    float minAddMult;
    public float MinAddMult => minAddMult;

    [Header("연산자 출현 확률")]
    [SerializeField]
    [Range(0, 100)]
    int plusPer;
    [SerializeField]
    [Range(0, 100)]
    int minusPer;
    [SerializeField]
    [Range(0, 100)]
    int dividePer;
    [SerializeField]
    [Range(0, 100)]
    int multPer;

    [Header("디버그 : 연산 확률 합 미달/초과")]
    [SerializeField]
    int perLeft;

    [Header("디버그 : 평균 결과 배수")]
    [SerializeField]
    float avg;

    [Space]
    [SerializeField]
    DivPercent div;
    [SerializeField]
    MultPercent mult;

    private void Start()
    {
        if (perLeft != 0) Debug.LogError("확률합 0이 아님! 자동 조절됨");
        DistributeMult();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //if (!UnityEditor.EditorApplication.isPlaying)
        {
            UpdateMultPerLeft();
            UpdateAvg();// 에디터 모드에서만 실행되는 코드
            // 여기에 코드를 작성하세요.
        }
#endif
    }

    private void OnValidate()
    {
        // multAvg 구하기
        UpdateMultPerLeft();
        UpdateAvg();
    }

    void UpdateAvg()
    {
        if (plusPer + minusPer + dividePer + multPer == 0)
        {
            plusPer = 1;
            minusPer = 1;
            dividePer = 1;
            multPer = 1;
        }

        int perSum = plusPer + minusPer + dividePer + multPer;

        // 각 비율 구하기
        float ratio1 = (float)plusPer / perSum;
        float ratio2 = (float)minusPer / perSum;
        float ratio3 = (float)dividePer / perSum;
        float ratio4 = (float)multPer / perSum;        

        float addAvg = (maxAddMult + minAddMult) / 2f;
        avg = (ratio1 * (1 + addAvg)) + (ratio2 * (1 - addAvg)) + (ratio3 * DivPercent.MultAvg) + (ratio4 * MultPercent.MultAvg);
    }

    void UpdateMultPerLeft()
    {
        // 확률 오차 갱신
        int perSum = plusPer + minusPer + dividePer + multPer;
        perLeft = 100 - perSum;
    }

    // 남은 곱연산 확률 분배
    public void DistributeMult()
    {
        if (plusPer + minusPer + dividePer + multPer == 0)
        {
            plusPer = 1;
            minusPer = 1;
            dividePer = 1;
            multPer = 1;
        }

        int perSum = plusPer + minusPer + dividePer + multPer;

        // 각 비율 구하기
        float ratio1 = (float)plusPer / perSum;
        float ratio2 = (float)minusPer / perSum;
        float ratio3 = (float)dividePer / perSum;
        float ratio4 = (float)multPer / perSum;

        plusPer += (int)(perLeft * ratio1);
        minusPer += (int)(perLeft * ratio2);
        dividePer += (int)(perLeft * ratio3);
        multPer += (int)(perLeft * ratio4);
        UpdateMultPerLeft();

        // 남은 확률 오차 재할당
        plusPer += perLeft;
        UpdateMultPerLeft();
        UpdateAvg();
    }

    // 확률과 값을 계산하여 구하기
    public Calc GetCalc(CalcType? calcType = null)
    {
        int _val = 0;
        CalcType _type;

        if (calcType != null)
        {
            // 연산 타입을 명시한 경우
            _type = calcType.Value;
        }
        else
        {
            // 연산 타입을 명시하지 않은 경우
            // 확률에 따른 타입 구하기
            int ran = Random.Range(0, 100);
            if (ran < plusPer) _type = CalcType.Add;
            else if (ran < minusPer + plusPer) _type = CalcType.Substract;
            else if (ran < minusPer + plusPer + dividePer) _type = CalcType.Divide;
            else if (ran < minusPer + plusPer + dividePer + multPer) _type = CalcType.Multiply;
            else
            {
                Debug.Log("확률오류?");
                _type = CalcType.Add;
            }
        }        

        // 합연산?
        if (_type == CalcType.Add || _type == CalcType.Substract)
        {
            //int score = ScoreManager.GetScore();
            int optimalScore = (int)ScoreManager.Instance.GetOptimalScore();

            // 합연산 변수 구하기            
            float plusMin = optimalScore * MinAddMult;
            float plusMax = optimalScore * MaxAddMult;
            int ran1 = (int)Random.Range(plusMin, plusMax);
            // 합연산 변수 -> 앞 두자리까지 반올림
            int numLen = optimalScore.ToString().Length;
            float factor = Mathf.Pow(10, numLen - 2);
            float roundedNumber = Mathf.Round(ran1 / factor) * factor;
            _val = (int)roundedNumber;


            // 합연산 변수 디버깅
            //Debug.Log("ran1 : " + ran1);
            //Debug.Log("numLen : " + numLen);
            //Debug.Log("factor : " + factor);
            //Debug.Log("roundedNumber : " + roundedNumber);
        }


        //// 곱연산 변수 구하기
        //int _multVal;
        //int ran2 = Random.Range(0, 100);
        //if (ran2 < 10) _multVal = 4;
        //else if (ran2 < 30) _multVal = 3;
        //else _multVal = 2;

        // 나누기 연산?
        if (_type == CalcType.Multiply)
        {
            _val = mult.GetValue();
        }

        // 곱연산?
        if (_type == CalcType.Divide)
        {
            _val = div.GetValue();
        }

        Calc calc = new Calc();
        calc.type = _type;
        calc.val = _val;
        return calc;
    }
}
