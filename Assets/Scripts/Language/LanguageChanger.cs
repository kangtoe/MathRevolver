using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LanguageChanger : MonoBehaviour
{
    [SerializeField]
    List<LanguageUi_text> textList;

    [SerializeField]
    List<LanguageUi_tmp> tmpList;

    [SerializeField]
    List<LanguageUi_image> imageList;
    
    void Start()
    {
        //LanguageSetting.onChangeLanguage += ChangeLanguage;
    }

    public void DebugKr()
    {
        ChangeLanguage(Language.Korea);
    }

    public void DebugEn()
    {
        ChangeLanguage(Language.English);
    }

    // 언어 설정에 따라 UI(이미지, 텍스트, TexDraw 등) 일괄 변경
    public void ChangeLanguage(Language _language)
    {
        // 이미 기존 언어 설정과 같음
        if (LanguageSetting.language == _language) return;

                
        switch (_language)
        {
            case Language.Korea:
                foreach (LanguageUi_text text in textList) text.SetKr();
                foreach (LanguageUi_tmp tmp in tmpList) tmp.SetKr();
                foreach (LanguageUi_image image in imageList) image.SetKr();
                break;

            case Language.English:
                foreach (LanguageUi_text text in textList) text.SetEn();
                foreach (LanguageUi_tmp tmp in tmpList) tmp.SetEn();
                foreach (LanguageUi_image image in imageList) image.SetEn();
                break;

            default:
                Debug.Log("");
                return;
        }
        

        LanguageSetting.language = _language;
    }
}
