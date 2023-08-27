using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 디버그 용 클래스
// save data를 에디터에서 볼수 있도록 함 
public class SaveDataViewer : MonoBehaviour
{
    #region 싱글톤
    public static SaveDataViewer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveDataViewer>();
            }
            return instance;
        }
    }
    private static SaveDataViewer instance;
    #endregion

    public int BestScore;
    public bool DiagonosticCompleted;
    public Language UsingLanguage;

    private void Awake()
    {
        // 모든 씬에서 하나만 유지
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // 저장 데이터 가져와 표시
    public void GetSave()
    {
        BestScore = SaveManager.BestScore;
        DiagonosticCompleted = SaveManager.DiagonosticCompleted;
        UsingLanguage = SaveManager.UsingLanguage;
    }

    // 현재 표시된 데이터로 저장 데이터 수정
    public void SetSave()
    {
        SaveManager.BestScore = BestScore;
        SaveManager.DiagonosticCompleted = DiagonosticCompleted;
        SaveManager.UsingLanguage = UsingLanguage;
    }
}
