using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlayButton()
    {
        if (!GameManager.Instance.DiagonosticCompleted)
        {
            Debug.Log("한번 이상의 진단평가 풀이 필요");
            return;
        }

        SceneChanger.Instance.SceneChange("Play");
    }

}
