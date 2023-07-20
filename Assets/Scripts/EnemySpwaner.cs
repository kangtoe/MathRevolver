using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    public static EnemySpwaner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemySpwaner>();
            }
            return instance;
        }
    }
    private static EnemySpwaner instance;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    Transform spwanPoint;

    [SerializeField]
    float spwanInterval = 5f;

    public List<Enemy> enemies = new List<Enemy>();

    // 디버그용
    int enemyNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpwanEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpwanEnemy()
    {
        while (true)
        {
            // 선택지 오브젝트 생성
            //GameObject go = Instantiate(selectionPrefab, transform);
            //go.transform.position = creatingPoint.position;
            Spwan(enemyPrefab);

            yield return new WaitForSeconds(spwanInterval);
        }
    }
     
    void Spwan(GameObject enemyPrefab)
    {
        Vector3 pos = spwanPoint.position;

        // 추후 전역에서 사용하는 변수로 수정할 것
        float floorWidth = 10;
        float margin = 1;

        // spwanPoint상의 무작위 x 위치 구하기
        float spwanWidth = floorWidth - margin;        
        float minX = spwanPoint.position.x - (spwanWidth / 2f);
        float maxX = spwanPoint.position.x + (spwanWidth / 2f);
        pos.x = Random.Range(minX, maxX);

        // 적 생성
        GameObject go = Instantiate(enemyPrefab);
        go.name += enemyNumber; enemyNumber++;
        go.transform.position = pos;
        go.transform.rotation = enemyPrefab.transform.rotation;

        //Enemy enemy = go.GetComponent<Enemy>();        
    }

    public void DebugFunc()
    {
        Enemy enemy = GetClosestEnemy();
        float z = enemy.transform.position.z;
        Debug.Log("GetClosestEnemy z : " + z);       
    }

    // 공격 가능한 가장 가까운 적 반환 (없으면 null)
    public Enemy GetClosestEnemy()
    {        
        // list 요소 개수 검사
        if (enemies.Count == 0)
        {
            Debug.Log("enemies.Count == 0");
            return null;
        }        

        // 가장 나중에 리스트에 추가된(생성된) 적 = 가장 멀리있는 적
        int closestIndex = -1;
        float closestPosZ = 0;

        for (int i = 0; i < enemies.Count; i++)
        {            
            // 공격 가능한 적인가?
            if (!enemies[i].CanBeAttacked) continue;

            float posZ = enemies[i].transform.position.z;

            // 초기화
            if (closestIndex == -1)
            {
                closestPosZ = posZ;
                closestIndex = i;
                continue;
            }                        

            // 최단거리 갱신
            if (posZ < closestPosZ)
            {
                closestPosZ = posZ;
                closestIndex = i;
                //Debug.Log("최단거리 갱신");
            }
        }

        // 조건에 해당하는 적 없음
        if (closestIndex == -1)
        {
            Debug.Log("no enemy can attack : " + closestIndex);
            return null;
        }

        //Debug.Log("closestIndex : " + closestIndex);
        //Debug.Log("closestRange : " + closestPosZ);
        return enemies[closestIndex];
    }
}
