using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionObject_Power_Element : MonoBehaviour
{
    [SerializeField]
    int score = 100;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    MeshRenderer mesh;

    public void SetScore(int i)
    {
        score = i;
        text.text = score.ToString();
    }

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

    // 선택지에 해당하는 효과 주기
    public void DoEffect()
    {
        ScoreManager.Instance.AddScore(score);
    }
}
