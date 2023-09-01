using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultPercent))]
public class MultPercentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MultPercent spwanPercent = (MultPercent)target;

        if (GUILayout.Button("곱연산 확률 분배"))
        {
            spwanPercent.DistributeMult();
        }
    }
}
