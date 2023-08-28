using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerPrefs를 통한 정보 저장 & 불러오기
public static class SaveManager
{
    static string diagonosticCompletedKey = "diagonosticCompleted";
    static string bestScoreKey = "bestScore";    
    static string usingLanguageKey = "usingLanguage";
    static string sfxVolumeKey = "sfxVolume";
    static string bgmVolumeKey = "bgmVolume";

    // 진단 평가 풀었나?
    public static bool DiagonosticCompleted
    {
        get
        {
            int i = PlayerPrefs.GetInt(diagonosticCompletedKey);
            return i == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(diagonosticCompletedKey, value ? 1 : 0);
        }
    }

    // play Scene에서 최고 점수
    public static int BestScore
    {
        get
        {
            return PlayerPrefs.GetInt(bestScoreKey);
        }
        set
        {
            PlayerPrefs.SetInt(bestScoreKey, value);
        }
    }    

    // 사용 언어
    static Language UsingLanguage_default = Language.Korea;
    public static Language UsingLanguage
    {
        get
        {
            string str;

            // 이전 저장 데이터 없는 경우 기본 값 할당
            if (!PlayerPrefs.HasKey(usingLanguageKey))
            {
                str = EnumLanguageToString(UsingLanguage_default);
                PlayerPrefs.SetString(usingLanguageKey, str);
            }            
                        
            str = PlayerPrefs.GetString(usingLanguageKey);
            return StringToEnumLanguage(str);
        }
        set
        {
            string str = EnumLanguageToString(value);
            PlayerPrefs.SetString(usingLanguageKey, str);
        }
    }

    // 배경 볼륨
    static float SfxVolume_default = 0.5f;
    public static float SfxVolume
    {
        get 
        {
            // 이전 저장 데이터 없는 경우 기본 값 할당
            if (!PlayerPrefs.HasKey(sfxVolumeKey))
            {
                PlayerPrefs.SetFloat(sfxVolumeKey, SfxVolume_default);
            }

            return PlayerPrefs.GetFloat(sfxVolumeKey);
        }
        set
        {
            PlayerPrefs.SetFloat(sfxVolumeKey, value);
        }
    }

    // 이팩트 볼륨
    static float BgmVolume_default = 0.5f;
    public static float BgmVolume
    {
        get
        {
            // 이전 저장 데이터 없는 경우 기본 값 할당
            if (!PlayerPrefs.HasKey(bgmVolumeKey))
            {
                PlayerPrefs.SetFloat(bgmVolumeKey, BgmVolume_default);
            }

            return PlayerPrefs.GetFloat(bgmVolumeKey);

        }
        set
        {
            PlayerPrefs.SetFloat(bgmVolumeKey, value);
        }
    }

    // 모든 저장 데이터 삭제 메소드
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    #region Enum <-> String 변환

    static string EnumLanguageToString(Language lan)
    {
        return lan.ToString();
    }

    static Language StringToEnumLanguage(string str)
    {
        return (Language)Enum.Parse(typeof(Language), str);
    }

    #endregion
}
