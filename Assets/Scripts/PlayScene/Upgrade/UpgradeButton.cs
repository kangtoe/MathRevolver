using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    public Button Button => button;

    [SerializeField]
    Text levelText;

    public void SetLevelText(string str)
    {
        levelText.text = str;
    }
}
