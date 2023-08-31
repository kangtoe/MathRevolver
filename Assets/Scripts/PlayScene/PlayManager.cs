using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayState
{ 
    OnTutorial,
    OnPlay,
    GameOver
}

public class PlayManager : MonoBehaviour
{
    #region 싱글톤
    public static PlayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayManager>();
            }
            return instance;
        }
    }
    private static PlayManager instance;
    #endregion

    public void PlayGame()
    {
        // 적 등장 시작
        EnemySpwaner.Instance.SpwanStart();

        // 선택지 등장 시작
        SelectionCreator.Instance.CreateStart();
    }
}
