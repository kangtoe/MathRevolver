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

    private void Start()
    {
        idx = 0;
        ActiveImage(0);
    }

    // 버튼 이벤트로 호출
    public void OnClickImage()
    {
        SoundManager.Instance.PlaySound("click");
        
        idx++;        
        ActiveImage(idx);
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
