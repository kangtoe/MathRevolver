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
        int score = ScoreManager.GetScore();

        // 합연산 변수 구하기
        int _pulsVal = 0;
        int ran1 = Random.Range(score / 2, score * 2);        
        int numLen = score.ToString().Length;        
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

    public void OnSelected()
    {
        int score = ScoreManager.GetScore();        

        // 텍스트에 정보 매핑
        switch (type)
        {
            case CalcType.Add:
                ScoreManager.SetScore(score + plusVal);
                break;
            case CalcType.Substract:
                ScoreManager.SetScore(score - plusVal);
                break;
            case CalcType.Multiply:
                ScoreManager.SetScore(score * multVal);
                break;
            case CalcType.Divide:
                ScoreManager.SetScore(score / multVal);
                break;
            default:
                Debug.Log("calc type undefind : " + type);
                break;
        }
    }
}
