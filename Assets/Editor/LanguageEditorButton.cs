using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LanguageChanger))]
public class CubeGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LanguageChanger changer = (LanguageChanger)target;

        if (GUILayout.Button("Set Kr"))
        {
            changer.SetKr();
        }

        if (GUILayout.Button("Set En"))
        {
            changer.SetEn();
        }
    }
}