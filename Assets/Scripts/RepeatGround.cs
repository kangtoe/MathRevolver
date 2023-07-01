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
    GameObject groundPrefabs;

    [SerializeField]
    float groundSize = 10;

    [Header("디버깅 용")]
    [SerializeField]
    List<GameObject> spwanedGrounds;

    //[SerializeField]
    Player currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        int preSpwaned = 2;

        // player의 위치 검사
        Vector3 playerPos = currentPlayer.transform.position;
        Vector3 spwanCheckPos = spwanedGrounds[spwanedGrounds.Count - preSpwaned].transform.position;
        if (playerPos.z >= spwanCheckPos.z)
        {
            Vector3 spwanPoint = spwanCheckPos + new Vector3(0, 0, groundSize * preSpwaned);

            // 새로운 그라운드 스폰
            GameObject go = Instantiate(groundPrefabs, transform);
            go.transform.position = spwanPoint;
            spwanedGrounds.Add(go);
        }

        // TODO
        // spwanedGrounds 중 카메라에서 완전히 벗어난 것은 제거할 것
    }
}
