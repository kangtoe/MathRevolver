using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CalcType
{
    Undefind = 0,
    Add,
    Substract,
    Multiply,
    Divide
}

public class SelectionObject_Power_Element : MonoBehaviour
{
    ScoreManager ScoreManager => ScoreManager.Instance;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    MeshRenderer mesh;

    [SerializeField]
    GameObject vfx;

    [Header("연산 종류")]
    [SerializeField]
    CalcType type;
    [Header("합연산 계수")]
    [SerializeField]
    int plusVal;
    [Header("곱연산 계수")]
    [SerializeField]
    int multVal;

    public void SetColor(Color new_color, bool changeAlpha = false)
    {
        Debug.Log("set colot alpha : " + new_color.a);

        Color color = new_color;
        if (!changeAlpha) color.a = mesh.material.color.a;
        mesh.material.color = color;
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetSize(float X, float Z = float.NaN)
    {
        Vector3 scale = mesh.transform.localScale;
        scale.x = X / 10f;
        if (!float.IsNaN(Z)) scale.y = Z / 10f;
        mesh.transform.localScale = scale;
    }

    // 완전 무작위 연산
    public void SetCalc()
    {
        // Undefind를 제외한 무작위 연산 타입 구하기
        CalcType _type = (CalcType)Random.Range(1, System.Enum.GetValues(typeof(CalcType)).Length);
        SetCalc(_type);        
    }

    // 연산 타입만 고정
    public void SetCalc(CalcType _type)
    {
        //int score = ScoreManager.GetScore();
        int optimalScore = ScoreManager.GetOptimalScore();

        // 합연산 변수 구하기
        int _pulsVal = 0;
        int ran1 = Random.Range(optimalScore / 2, optimalScore * 2);
        // 합연산 변수 -> 앞 두자리까지 반올림
        int numLen = optimalScore.ToString().Length;        
        float factor = Mathf.Pow(10, numLen - 2);        
        float roundedNumber = Mathf.Round(ran1 / factor) * factor;        
        _pulsVal = (int)roundedNumber;

        // 합연산 변수 디버깅
        //Debug.Log("ran1 : " + ran1);
        //Debug.Log("numLen : " + numLen);
        //Debug.Log("factor : " + factor);
        //Debug.Log("roundedNumber : " + roundedNumber);

        // 곱연산 변수 구하기
        int _multVal;
        int ran2 = Random.Range(0, 100);
        if (ran2 < 10) _multVal = 4;
        else if (ran2 < 30) _multVal = 3;
        else _multVal = 2;

        SetCalc(_type, _pulsVal, _multVal);
    }

    // 연산 타입, 변수들 모두 고정됨
    public void SetCalc(CalcType _type, int _plusVal, int _multVal)
    {
        type = _type;
        plusVal = _plusVal;
        multVal = _multVal;

        // 텍스트에 정보 매핑
        switch (_type)
        {
            case CalcType.Add:
                text.text = "+" + plusVal;
                break;
            case CalcType.Substract:
                text.text = "-" + plusVal;
                break;
            case CalcType.Multiply:
                text.text = "*" + multVal;
                break;
            case CalcType.Divide:
                text.text = "/" + multVal;
                break;
            default:
                Debug.Log("calc type undefind : " + _type);
                break;
        }
    }

    // 선택지 결과 반영
    public void OnSelected()
    {
        int score = ScoreManager.GetScore();

        // 텍스트에 정보 매핑
        int result = PreCalc(score);
        ScoreManager.SetScore(result);


    }

    // 선택지 연산 결과 계산해보기
    public int PreCalc(int i)
    {        
        // 텍스트에 정보 매핑
        switch (type)
        {
            case CalcType.Add:
                return i + plusVal;                
            case CalcType.Substract:
                return i - plusVal;                
            case CalcType.Multiply:
                return i * multVal;                
            case CalcType.Divide:
                return i / multVal;
            default:
                Debug.Log("calc type undefind : " + type);
                return -1;
        }
    }

    public void EnableVFX(bool enable)
    {
        vfx.SetActive(enable);
    }

    public void FadeColor(float duration, float targetAlpha, float startAlpha = -1)
    {
        // 시작 알파 값 지정 없는 경우, 기존 알파값 사용
        if (startAlpha == -1) startAlpha = mesh.material.color.a;

        StopAllCoroutines();        
        StartCoroutine(FadeColorCr(duration, targetAlpha, startAlpha));
    }

    IEnumerator FadeColorCr(float duration, float targetAlpha, float startAlpha)
    {
        Debug.Log("startAlpha : " + startAlpha);

        float interval = 0.1f;

        float t = 0;
        while (true)
        {
            t += interval / duration;

            if (t > 1) t = 1;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t);
            Debug.Log("a :" + a);

            Color color = mesh.material.color;
            color.a = a;
            SetColor(color, true);

            if (t == 1) break;

            yield return new WaitForSeconds(interval);
        }        
    }
}
