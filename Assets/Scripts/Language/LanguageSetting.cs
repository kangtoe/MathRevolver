using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{ 
    Undefind,
    Korea,
    English
}

public static class LanguageSetting
{
    public delegate void m_Delegate();
    public static event m_Delegate onChangeLanguage;

    public static Language language;

    public static void SetLanguage(Language _language)
    {
        language = _language;        
    }

    // mathpid 언어 설정을 위한 문자열 반환
    public static string GetCurrentLanguageString()
    {
        switch (language)
        {
            case Language.Korea:
                return "KO";                
            case Language.English:
                return "EN";                
            default:
                Debug.Log("대응되는 언어 설정 없음");
                return "EN";
        }
    }
}
