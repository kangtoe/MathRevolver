using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MultPercent : MonoBehaviour
{
    [Header("연산 확률")]
    [Range(0, 100)]
    [SerializeField]
    int Mult2Per;
    [Range(0, 100)]
    [SerializeField]
    int Mult3Per;
    [Range(0, 100)]
    [SerializeField]
    int Mult4Per;
    
    [Header("디버그 : 연산 확률 합 미달/초과")]
    [SerializeField]
    int multPerLeft;

    [Header("디버그 : 연산 평균")]
    [SerializeField]
    float multAvg;
    public static float MultAvg;

    private void Start()
    {
        if (multPerLeft != 0) Debug.LogError("확률합 0이 아님! 자동 조절됨");
        DistributeMult();
    }

    private void OnValidate()
    {
        // multAvg 구하기
        UpdateMultPerLeft();
        UpdateAvg();
    }

    void UpdateMultPerLeft()
    {
        // 확률 오차 갱신
        int perSum = Mult2Per + Mult3Per + Mult4Per;
        multPerLeft = 100 - perSum;
    }

    // 곱연산 평균
    void UpdateAvg()
    {
        if (Mult2Per + Mult3Per + Mult4Per == 0)
        {
            Mult2Per = 1;
            Mult3Per = 1;
            Mult4Per = 1;
        }

        int perSum = Mult2Per + Mult3Per + Mult4Per;

        // 각 비율 구하기
        float mult2Ratio = (float)Mult2Per / perSum;
        float mult3Ratio = (float)Mult3Per / perSum;
        float mult4Ratio = (float)Mult4Per / perSum;

        multAvg = mult2Ratio * 2 + mult3Ratio * 3 + mult4Ratio * 4;
        MultAvg = multAvg;
    }

    // 남은 곱연산 확률 분배
    public void DistributeMult()
    {
        if (Mult2Per + Mult3Per + Mult4Per == 0)
        {
            Mult2Per = 1;
            Mult3Per = 1;
            Mult4Per = 1;
        }

        int perSum = Mult2Per + Mult3Per + Mult4Per;

        // 각 비율 구하기
        float mult2Ratio = (float)Mult2Per / perSum;
        float mult3Ratio = (float)Mult3Per / perSum;
        float mult4Ratio = (float)Mult4Per / perSum;

        Mult2Per += (int)(multPerLeft * mult2Ratio);
        Mult3Per += (int)(multPerLeft * mult3Ratio);
        Mult4Per += (int)(multPerLeft * mult4Ratio);
        UpdateMultPerLeft();

        // 남은 확률 오차 재할당
        Mult2Per += multPerLeft;
        UpdateMultPerLeft();
        UpdateAvg();
    }
}
