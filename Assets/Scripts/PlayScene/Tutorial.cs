using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    int idx = 0;

    [SerializeField]
    List<GameObject> tutorialPageList;

    [SerializeField]
    Button skipButton;

    private void Start()
    {
        if (SaveManager.ShowTutorial)
        {
            idx = 0;
            ActiveImage(0);

            skipButton.gameObject.SetActive(true);
            skipButton.onClick.AddListener(() => Skip());
        }
        else Skip();
    }

    public void Skip()
    {
        idx = tutorialPageList.Count;
        OnClickImage();        
    }

    // 버튼 이벤트로 호출
    public void OnClickImage()
    {
        SoundManager.Instance.PlaySound("click");
        
        idx++;        
        ActiveImage(idx);

        // 튜토리얼 종료
        if (idx >= tutorialPageList.Count)
        {
            SaveManager.ShowTutorial = false;
            PlayManager.Instance.PlayGame();
            skipButton.gameObject.SetActive(false);
        } 
    }

    // idx 이미지 활성화, 그 외 모든 이미지 비활성화
    void ActiveImage(int idx)
    {
        for (int i = 0; i < tutorialPageList.Count; i++)
        {
            if (i == idx) tutorialPageList[i].SetActive(true);
            else tutorialPageList[i].SetActive(false);
        }
    }
}
