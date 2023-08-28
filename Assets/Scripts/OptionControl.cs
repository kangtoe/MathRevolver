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

    private void Start()
    {
        // 버튼 상호작용 설정 : 클릭 버튼에 따라 언어 설정
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

        // 슬라이더 상호작용 설정 : 변경한 슬라이더 값 적용 및 저장
        {
            slider_sfx.onValueChanged.AddListener(delegate
            {
                float val = slider_sfx.value;
                // 저장 값 수정
                SaveManager.SfxVolume = val;
                // 실제 볼륨 조절
                volume.SfxVolume = val;
            });
            slider_bgm.onValueChanged.AddListener(delegate
            {
                float val = slider_bgm.value;
                // 저장 값 수정
                SaveManager.BgmVolume = val;
                // 실제 볼륨 조절
                volume.BgmVolume = val;
            });
        }

        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    // UI를 세이브 데이터 대로 갱신
    void UpdateUI()
    {
        // 버튼 : 언어 설정에 따라 버튼 눌러진 상태로 설정
        switch (SaveManager.UsingLanguage)
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

        // 슬라이더 : 현재 볼륨에 따라 슬라이더 값 할당
        slider_sfx.value = SaveManager.SfxVolume;
        slider_bgm.value = SaveManager.BgmVolume;
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
