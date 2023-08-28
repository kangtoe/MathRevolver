using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // 옵션 버튼 클릭 시 : 판넬 활성화 제어는 UI Manager에서 담당
    // 그 외 나머지 버튼의 동작 구현

    public void OnClickButton_Diagnostic()
    {
        SoundManager.Instance.PlaySound("SampleSFX");

        SceneChanger.Instance.SceneChange("Diagnostic");
    }

    public void OnClickButton_Play()
    {
        SoundManager.Instance.PlaySound("SampleSFX");

        if (!SaveManager.DiagonosticCompleted)
        {
            Debug.Log("한번 이상의 진단평가 풀이 필요");
            return;
        }

        SceneChanger.Instance.SceneChange("Play");
    }

    public void OnClickButton_Quit()
    {
        SoundManager.Instance.PlaySound("SampleSFX");

        Application.Quit();
    }
}
