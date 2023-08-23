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
    [SerializeField]
    float timeScaleOnMenu = 0.5f;

    [Header("게임오버")]
    [SerializeField]
    GameObject overUI;

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
        ActiveOptionUI(false);        
    }

    public void ActiveOverUI(bool active)
    {
        overUI.SetActive(active);
    }


    public void ActiveOptionUI(bool active)
    {
        optionUI.SetActive(active);

        // 시간 스케일 조정
        if (active == true)
        {
            TimeManager.Instance.SetScale(timeScaleOnMenu);
        }
        if (active == false)
        {
            TimeManager.Instance.SetScale(1);
        }
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
