using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsConrol : MonoBehaviour
{
    [SerializeField]
    Transform heartUiParent;

    [SerializeField]
    GameObject heartUiPrefab;

    [SerializeField]
    int maxHeart = 5;
    int leftHeart;

    [SerializeField]
    List<HeartUi> heartUiList;

    // Start is called before the first frame update
    void Start()
    {
        leftHeart = maxHeart;
        ClearList();
        CreateHeartUI();
        UpdateHeartUI();
    }

    // 하트  UI 모두 삭제
    void ClearList()
    {        
        foreach (HeartUi heart in heartUiList)
        {            
            Destroy(heart.gameObject);
        }
        heartUiList.Clear();
    }

    // maxHeart만큼 하트 UI 만들기
    void CreateHeartUI()
    {
        for (int i = 0; i < maxHeart; i++)
        {
            GameObject go = Instantiate(heartUiPrefab, heartUiParent);
            HeartUi heart = go.GetComponent<HeartUi>();
            // 리스트에 추가
            heartUiList.Add(heart);
            // 시작시 빈 하트 상태
            heart.EnableFillImage(false);
        }
    }

    // leftHeart만큼 UI 채우기
    void UpdateHeartUI()
    {
        for (int i = 0; i < heartUiList.Count; i++)
        {
            if (i < leftHeart) heartUiList[i].EnableFillImage(true);
            else heartUiList[i].EnableFillImage(false);
        }
    }

    public void HeartLost()
    {
        leftHeart--;
        if (leftHeart < 0) leftHeart = 0;
        UpdateHeartUI();

        CameraManager.Instance.ShakeDebug();
        UIManager.Instance.Flash();

        if (leftHeart == 0)
        {
            Debug.Log("게임 오버");
        }
    }
}
