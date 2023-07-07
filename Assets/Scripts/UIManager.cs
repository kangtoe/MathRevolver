using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    private static UIManager instance;

    [SerializeField]
    GameObject optionUI;

    public static bool isOnUI;
    float timeScaleOnMenu = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        OffOptionMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOptionMenu()
    {
        isOnUI = true;
        optionUI.SetActive(true);
        Time.timeScale = timeScaleOnMenu;
    }

    public void OffOptionMenu()
    {
        isOnUI = false;
        optionUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
