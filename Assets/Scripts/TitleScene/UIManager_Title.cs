using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Title : MonoBehaviour
{
    public static UIManager_Title Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager_Title>();
            }
            return instance;
        }
    }
    private static UIManager_Title instance;

    [Header("옵션")]
    [SerializeField]
    GameObject optionUI;

    void Start()
    {
        ActiveOptionUI(false);
    }

    public void ActiveOptionUI(bool active)
    {
        optionUI.SetActive(active);
    }
}
