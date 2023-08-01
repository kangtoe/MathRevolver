using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private static GameManager instance;
    #endregion

    public bool DiagonosticCompleted => diagonosticCompleted;
    bool diagonosticCompleted = false;

    // Start is called before the first frame update
    void Awake()
    {
        // 모든 씬에서 하나만 유지
        if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        Application.targetFrameRate = 60;

        int setWidth = 1920; // 화면 너비
        int setHeight = 1080; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        //Screen.SetResolution(setWidth, setHeight, true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnDiagonosticComplete()
    {
        diagonosticCompleted = true;
    }
}
