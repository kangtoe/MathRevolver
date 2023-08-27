using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatGround : MonoBehaviour
{
    #region 싱글톤
    public static RepeatGround Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RepeatGround>();
            }
            return instance;
        }
    }
    private static RepeatGround instance;
    #endregion


    [SerializeField]
    GameObject[] groundPrefabs;

    [SerializeField]
    float groundSize = 10;
    
    [SerializeField]
    List<GameObject> spwanedGrounds;

    [SerializeField]
    Transform checkTarget; // player?

    // Update is called once per frame
    void Update()
    {
        int preSpwaned = 2;

        // player의 위치 검사
        Vector3 target = checkTarget.position;
        Vector3 spwanCheckPos = spwanedGrounds[spwanedGrounds.Count - preSpwaned].transform.position;
        if (target.z >= spwanCheckPos.z)
        {
            Vector3 spwanPoint = spwanCheckPos + new Vector3(0, 0, groundSize * preSpwaned);

            // 생성할 그라운드 찾기
            int idx = Random.Range(0, groundPrefabs.Length);
            GameObject nextGround = groundPrefabs[idx];

            // 새로운 그라운드 스폰
            GameObject go = Instantiate(nextGround, transform);
            go.transform.position = spwanPoint;
            spwanedGrounds.Add(go);

            // 콜라이더 리셋
            MeshCollider collider =  go.GetComponent<MeshCollider>();
            Destroy(collider);
            go.AddComponent<MeshCollider>();            
        }

        // TODO
        // spwanedGrounds 중 카메라에서 완전히 벗어난 것은 제거할 것
    }

    
}
