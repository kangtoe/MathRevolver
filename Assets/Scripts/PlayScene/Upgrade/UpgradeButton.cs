using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    public Button Button => button;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    TextMeshProUGUI descText;

    public void SetLevelText(string str)
    {
        if(levelText)
        {
            levelText.text = str;
        }
    }

    public void SetDescText(string str)
    {
        if (descText)
        {
            descText.text = str;
        }
    }
}