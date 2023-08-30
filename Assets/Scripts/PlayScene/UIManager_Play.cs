using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Play : MonoBehaviour
{
    public static UIManager_Play Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager_Play>();
            }
            return instance;
        }
    }
    private static UIManager_Play instance;

    [Header("업그레이드")]
    [SerializeField]
    GameObject upgradeUI;

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
        //ActiveOptionUI(false);        
    }

    public void ActiveUpgradeUI(bool active)
    {
        SoundManager.Instance.PlaySound("upgrade");
        upgradeUI.SetActive(active);

        // 시간 스케일 조정
        if (active == true)
        {
            TimeManager.Instance.SetScale(0);
        }
        if (active == false)
        {
            TimeManager.Instance.SetScale(1);
        }
    }

    public void ActiveOverUI(bool active)
    {
        overUI.SetActive(active);
    }

    public void ActiveOptionUI(bool active)
    {
        SoundManager.Instance.PlaySound("click");
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

    void CreateCurrectUI()
    { 
    
    }

    void CreateIncurrectUI()
    { 
    
    }

    public void Flash(Color _color)
    {
        if (m_FlashCr != null) StopCoroutine(m_FlashCr);
        m_FlashCr = StartCoroutine(FlashCr(_color));
    }

    IEnumerator FlashCr(Color _color)
    {
        float t = 0;
        Color color = flashPanel.color = _color;

        while (true)
        {
            t += Time.deltaTime / flashDuration;
            if (t > 1) t = 1;

            color.a = Mathf.Lerp(flashMaxAlpha, 0, t);
            flashPanel.color = color;            

            if (t == 1) break;

            yield return null;
        }
    }
}
