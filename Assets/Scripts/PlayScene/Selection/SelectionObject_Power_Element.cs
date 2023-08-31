using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EPOOutline;
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
    public string Text => text.text;

    [SerializeField]
    MeshRenderer mesh;

    [SerializeField]
    GameObject vfx;

    [SerializeField]
    Outlinable outline;

    [Header("연산 종류")]
    [SerializeField]
    CalcType type;
    [Header("합연산 계수")]
    [SerializeField]
    int plusVal;
    [Header("곱연산 계수")]
    [SerializeField]
    int multVal;

    public void SetMeshColor(Color new_color, bool changeAlpha = false)
    {
        //Debug.Log("set colot alpha : " + new_color.a);

        Color color = new_color;
        if (!changeAlpha) color.a = mesh.material.color.a;
        mesh.material.color = color;
    }

    public void SetMashAlpha(float a)
    {
        Color color = mesh.material.color;
        color.a = a;
        mesh.material.color = color;
    }

    public void SetTextAlpha(float a)
    {
        Color color = text.color;
        color.a = a;
        text.color = color;
    }

    public void SetOutlineAlpha(float a)
    {
        if (outline.RenderStyle == RenderStyle.Single)
        {
            Color color = outline.OutlineParameters.Color;
            color.a = a;
            outline.OutlineParameters.Color = color;
        }
        else if (outline.RenderStyle == RenderStyle.FrontBack)
        {
            if (outline.FrontParameters.Enabled)
            {
                Color color = outline.FrontParameters.Color;
                color.a = a;
                outline.FrontParameters.Color = color;
            }
            if (outline.BackParameters.Enabled)
            {
                Color color = outline.BackParameters.Color;
                color.a = a;
                outline.BackParameters.Color = color;
            }
        }
        else Debug.Log("undefinded");
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetSize(float X, float? Z = null, float planeMargin = 0, float textMargin = 0)
    {
        // 플레인 사이즈 조정
        Vector3 scale = mesh.transform.localScale;
        scale.x = (X - planeMargin) / 10f;
        if (Z != null) scale.y = Z.Value / 10f;
        mesh.transform.localScale = scale;

        // 텍스트 사이즈 조정       
        float margin = planeMargin + textMargin; // textMargin -> 기존 플레인 마진 적용 값에서 텍스트 마진을 추가로 적용
        text.rectTransform.sizeDelta = Vector2.one * (X - margin);
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
        float randomMult = SelectionCreator.Instance.RandimMult;

        //int score = ScoreManager.GetScore();
        int optimalScore = (int)ScoreManager.GetOptimalScore();

        // 합연산 변수 구하기
        int _pulsVal = 0;
        int ran1 = (int)Random.Range(optimalScore / randomMult / 2, optimalScore * randomMult);
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

        string str = "";
        // 텍스트에 정보 매핑
        switch (_type)
        {
            case CalcType.Add:
                str = "+" + plusVal.ToString("N0");
                break;
            case CalcType.Substract:
                str = "-" + plusVal.ToString("N0");
                break;
            case CalcType.Multiply:
                str = "×" + multVal.ToString("N0");
                break;
            case CalcType.Divide:
                str = "÷" + multVal.ToString("N0");
                break;
            default:
                Debug.Log("calc type undefind : " + _type);
                break;
        }
        text.text = str;
    }

    // 선택지 결과 반영
    public void OnSelected()
    {
        double score = ScoreManager.GetCurrentScore();

        // 텍스트에 정보 매핑
        double result = PreCalc(score);
        ScoreManager.SetScore(result);
    }

    // 선택지 연산 결과 계산해보기 // int 오버플로우 방지
    public double PreCalc(double val)
    {    
        // 텍스트에 정보 매핑
        switch (type)
        {
            case CalcType.Add:
                val += plusVal;
                break;
            case CalcType.Substract:
                val -= plusVal;
                break;                       
            case CalcType.Multiply:
                val *= multVal;
                break;                
            case CalcType.Divide:
                val /= multVal;
                break;
            default:
                Debug.Log("calc type undefind : " + type);
                return -1;
        }

        //if (f > int.MaxValue) return int.MaxValue;
        //else return f;

        return val;
    }

    public void EnableOutline(bool enable)
    {
        outline.enabled = enable;
    }

    public void EnableVFX(bool enable)
    {
        vfx.SetActive(enable);
    }

    #region 페이드

    public void FadeColor(bool inactiveMeshOnEnd, float duration, float targetAlpha, float? meshStartAlpha = null, float? textStartAlpha = null, float? outlineStartAlpha = null)
    {
        // 시작 알파 값 지정 없는 경우, 기존 알파값 사용
        if (meshStartAlpha == null) meshStartAlpha = mesh.material.color.a;
        if (textStartAlpha == null) textStartAlpha = text.color.a;
        if (outlineStartAlpha == null) outlineStartAlpha = outline.OutlineParameters.Color.a;

        StopAllCoroutines();
        StartCoroutine(FadeColorCr(inactiveMeshOnEnd, duration, targetAlpha, meshStartAlpha.Value, textStartAlpha.Value, outlineStartAlpha.Value));
    }

    IEnumerator FadeColorCr(bool inactiveMeshOnEnd, float duration, float targetAlpha, float meshStartAlpha, float textStartAlpha, float outlineStartAlpha)
    {
        float interval = 0.05f;

        float t = 0;
        while (true)
        {
            t += interval / duration;

            if (t > 1) t = 1;
            float meshAlpha = Mathf.Lerp(meshStartAlpha, targetAlpha, t);
            float textAlpha = Mathf.Lerp(textStartAlpha, targetAlpha, t);
            float outlineAlpha = Mathf.Lerp(outlineStartAlpha, targetAlpha, t);            

            // 색 변경   
            SetMashAlpha(meshAlpha);            
            SetTextAlpha(textAlpha);
            SetOutlineAlpha(outlineAlpha);

            yield return new WaitForSeconds(interval);

            if (t == 1) break;
        }

        if (inactiveMeshOnEnd) mesh.enabled = false;
    }

    #endregion


}
