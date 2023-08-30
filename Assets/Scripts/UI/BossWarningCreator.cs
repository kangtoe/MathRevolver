using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarningCreator : MonoBehaviour
{
    [SerializeField]
    Transform parent;

    [Header("보스 경고 UI")]
    [SerializeField]
    GameObject bossWarningUI;

    public void CreateBossWarningUI(Vector2? point = null)
    {
        GameObject go = Instantiate(bossWarningUI, parent);
        RectTransform tf = go.GetComponent<RectTransform>();
        if (point != null) tf.anchoredPosition = point.Value;
    }
}
