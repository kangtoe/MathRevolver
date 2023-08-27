using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        if (!SaveManager.DiagonosticCompleted)
        {
            Debug.Log("한번 이상의 진단평가 풀이 필요");
            return;
        }

        SceneChanger.Instance.SceneChange("Play");
    }

}
