using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpwanPercent : MonoBehaviour
{
    [Header("최대 합 선택지 배수")]
    [SerializeField]
    float max;
    [Header("최소 합 선택지 배수")]
    [SerializeField]
    float min;

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

        float addAvg = (max + min) / 2f;
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
}
