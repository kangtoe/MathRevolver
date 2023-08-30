using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUICreator : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    [Header("보스 경고 UI")]
    [SerializeField]
    GameObject bossWarningUI;

    [Header("정답/오답 표시 UI")]
    [SerializeField]
    GameObject currectUI;
    [SerializeField]
    GameObject incurrectUI;

    public void CreateCurrectUI()
    {
        Instantiate(currectUI, canvas.transform);
    }

    public void CreateIncurrectUI()
    {
        Instantiate(incurrectUI, canvas.transform);
    }

    public void CreateBossWarningUI(Vector2? point = null)
    {
        GameObject go = Instantiate(bossWarningUI, canvas.transform);
        RectTransform tf = go.GetComponent<RectTransform>();
        if (point != null) tf.anchoredPosition = point.Value;
    }
}
