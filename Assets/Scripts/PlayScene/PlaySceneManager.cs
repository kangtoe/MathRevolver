using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    #region 싱글톤
    public static PlaySceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlaySceneManager>();
            }
            return instance;
        }
    }
    private static PlaySceneManager instance;
    #endregion

    public void GameOver()
    {
        Player.Instance.OnDead();
        UIManager.Instance.ActiveOverUI(true);
        SelectionCreator.Instance.StopAllCoroutines();
        EnemySpwaner.Instance.StopAllCoroutines();
    }
}
