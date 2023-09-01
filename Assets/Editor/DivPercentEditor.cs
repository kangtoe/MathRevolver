using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DivPercent))]
public class DivPercentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DivPercent spwanPercent = (DivPercent)target;

        if (GUILayout.Button("나누기 연산 확률 분배"))
        {
            spwanPercent.DistributeMult();
        }
    }
}
