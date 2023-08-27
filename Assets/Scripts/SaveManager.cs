using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerPrefs를 통한 정보 저장 & 불러오기
public static class SaveManager
{
    static string bestScoreKey = "bestScore";
    static string diagonosticCompletedKey = "diagonosticCompleted";
    static string usingLanguageKey = "usingLanguage";

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

    // 사용 언어
    public static Language UsingLanguage
    {
        get
        {
            string str = PlayerPrefs.GetString(usingLanguageKey);
            return (Language)Enum.Parse(typeof(Language), str);
        }
        set
        {
            PlayerPrefs.SetString(usingLanguageKey, value.ToString());
        }
    }
}
