using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletMag : MonoBehaviour
{
    [Header("가하는 힘")]
    [SerializeField]
    Vector2 bulletForce = new Vector2(-100, 200);

    [Header("가하는 토크")]
    [SerializeField]
    float bulletTorqueMin = -90;
    [SerializeField]
    float bulletTorqueMax = 90;
    float bulletTorque => Random.Range(bulletTorqueMin, bulletTorqueMax);

    [Header("가하는 중력 스케일")]
    [SerializeField]
    float bulletGravScale = 100;

    [Header("디버그 : 탄환 UI")]
    [SerializeField]
    List<BulletUI> bulletUiList;

    [Header("탄환 트랜스폼 정렬 컴포넌트")]
    [SerializeField]
    LayoutGroup layoutGroup;
    
    public void UseBullet()
    {
        layoutGroup.enabled = false;

        if (bulletUiList.Count == 0)
        {
            Debug.Log("리스트 요소 없음");
            return;
        }

        int index = 0;

        bulletUiList[index].Use(bulletForce, bulletTorque, bulletGravScale);

        bulletUiList.RemoveAt(index);
    }

    // 모든 탄환 오브젝트 삭제 & 리스트 비우기
    void ClearBullets()
    {
        foreach (BulletUI bullet in bulletUiList)
        {
            Destroy(bullet.gameObject);
        }
        bulletUiList.Clear();
    }

    public void CreateBullets()
    {
        layoutGroup.enabled = true;

        // TODO : 탄환 UI 생성

        layoutGroup.enabled = false;
    }
}
