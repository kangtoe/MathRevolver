using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpwanPercent))]
public class SpwanPercentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpwanPercent spwanPercent = (SpwanPercent)target;

        if (GUILayout.Button("연산 확률 분배"))
        {
            spwanPercent.DistributeMult();
        }        
    }
}
