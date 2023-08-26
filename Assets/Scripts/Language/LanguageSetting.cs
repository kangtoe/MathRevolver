using System.Collections;
using System.Collections.Generic;

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


}
