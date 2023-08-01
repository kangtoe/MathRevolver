using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("옵션")]
    [SerializeField]
    GameObject optionUI;
    public static bool isOnUI;
    float timeScaleOnMenu = 0.5f;

    [Header("화면 점멸")]
    [SerializeField]
    Image flashPanel;
    [SerializeField]
    float flashDuration = 0.5f;
    [SerializeField]
    float flashMaxAlpha = 0.5f;
    Coroutine m_FlashCr;

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

    public void Flash()
    {
        if (m_FlashCr != null) StopCoroutine(FlashCr());
        m_FlashCr = StartCoroutine(FlashCr());
    }

    IEnumerator FlashCr()
    {
        float t = 0;
        Color color = flashPanel.color;

        while (true)
        {
            color.a = Mathf.Lerp(flashMaxAlpha, 0, t);
            flashPanel.color = color;

            t += Time.deltaTime / flashDuration;

            if (t >= 1) break;

            yield return null;
        }
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
