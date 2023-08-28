using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 옵션 창 자체를 끄고 키는 기능은 각 Scene의 UI Managaer들이 담당
// 그 외 모든 옵션 관련 UI 및 기능 연동
public class OptionControl : MonoBehaviour
{    
    [Header("언어 버튼 ui")]
    [SerializeField]
    Button button_kr;
    [SerializeField]
    Button button_en;

    [Header("버튼 색")]
    [SerializeField]
    Color buttonSelect;
    [SerializeField]
    Color buttonDeselect;

    [Header("볼륨 슬라이더 ui")]
    [SerializeField]
    Slider slider_sfx;
    [SerializeField]
    Slider slider_bgm;

    [Header("필요 컴포넌트")]
    [SerializeField]
    LanguageChanger language;
    [SerializeField]
    VolumeControl volume;

    private void OnEnable()
    {
        // 버튼 사운드 게이지 설정값대로 UI에 표시
        switch (LanguageSetting.currentLanguage)
        {
            case Language.Korea:
                OnClickButton_Kr();
                break;
            case Language.English:
                OnClickButton_En();
                break;
            default:
                Debug.Log("");
                break;
        }

        // 언어 버튼 설정
        // 현재 언어 설정에 따라 버튼 누른 효과 반영

        // 현재 볼륨에 따라 슬라이더 값 할당
    }

    private void Start()
    {
        button_kr.onClick.AddListener(delegate
        {
            OnClickButton_Kr();
        });

        button_en.onClick.AddListener(delegate
        {
            OnClickButton_En();
        });
    }

    // 언어 버튼 클릭 시 언어 설정, 사운드 바 조절 시 사운드 반영
    void OnClickButton_Kr()
    {
        language.SetKr();

        // 버튼 색 설정
        {
            button_kr.targetGraphic.color = buttonSelect;
            button_kr.GetComponentInChildren<TextMeshProUGUI>().color = buttonSelect;

            button_en.targetGraphic.color = buttonDeselect;
            button_en.GetComponentInChildren<TextMeshProUGUI>().color = buttonDeselect;
        }
        
    }

    void OnClickButton_En()
    {
        language.SetEn();

        // 버튼 색 설정
        {
            button_kr.targetGraphic.color = buttonDeselect;
            button_kr.GetComponentInChildren<TextMeshProUGUI>().color = buttonDeselect;

            button_en.targetGraphic.color = buttonSelect;
            button_en.GetComponentInChildren<TextMeshProUGUI>().color = buttonSelect;
        }        
    }
}
