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
    Transform spwanPoint;

    [SerializeField]
    float spwanInterval = 5f;

    [SerializeField]
    float spwanDelay = 3f;

    [Header("적 프리팹")]
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject enemyBossPrefab;
    
    [Header("보정 계수 : 최적해 * 보정계수 = 스폰 적 체력")]
    [SerializeField]
    float compensator = 0.5f;

    [Header("보스 출현 간격")]
    [SerializeField]
    int bossAdventCount = 10;

    [Header("디버깅 :보스 출현까지 남은 카운트 수")]
    [SerializeField]
    int currentBossAdventCount;

    [Header("현존하는 적 리스트")]
    public List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    FadeUICreator ui_Creator;

    // 디버그용
    int enemyNumber = 0;

    // 현재 하나 이상의 보스가 존재하는가?
    bool isBossExist
    {
        get{
            foreach (Enemy enemy in enemies)
            {
                if (enemy.isBoss && enemy.CanBeAttacked) return true;
            }

            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentBossAdventCount = bossAdventCount;
        StartCoroutine(SpwanEnemy());
    }    

    IEnumerator SpwanEnemy()
    {
        yield return new WaitForSeconds(spwanDelay);

        while (true)
        {
            // 보스가 필드에 존재하는 중 다른 적 스폰 중지 
            if (isBossExist == false)
            {                
                if (currentBossAdventCount <= 0)
                {                    
                    Spwan(enemyBossPrefab);
                    currentBossAdventCount = bossAdventCount;
                    ui_Creator.CreateBossWarningUI(Vector2.up * 250);
                }
                else
                {                 
                    Spwan(enemyPrefab);
                    currentBossAdventCount--;
                }                                
            }            
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

        // 체력 설정
        // 체력 = 최적해 점수 * 전체 보정 계수 * 개별 보정 계수
        Enemy enemy = go.GetComponent<Enemy>();
        int maxHealth = (int)(ScoreManager.Instance.OptimalScore * compensator * enemy.HealthMult);
        enemy.Init(maxHealth);
    }

    public void DebugFunc()
    {
        Enemy enemy = GetClosestEnemy_Z();
        float z = enemy.transform.position.z;
        Debug.Log("GetClosestEnemy z : " + z);       
    }

    // 공격 가능한 가장 가까운 적 반환 (없으면 null)
    public Enemy GetClosestEnemy_Z()
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

    public Enemy GetClosestEnemy_Transform(Transform tf, Bounds bounds, float? range = null) // range = -1 : 사거리 제한 없음
    {
        // list 요소 개수 검사
        if (enemies.Count == 0)
        {
            //Debug.Log("enemies.Count == 0");
            return null;
        }

        // 가장 나중에 리스트에 추가된(생성된) 적 = 가장 멀리있는 적
        int closestIndex = -1;
        float closestRange = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            // 공격 가능한 적인가?
            if (!enemies[i].CanBeAttacked) continue;

            // 공격 영역 내에 있는가?
            bool isContaion = bounds.Contains(enemies[i].transform.position);
            if (!isContaion) continue;            

            // 공격 가능 거리 검사
            float dist = Vector3.Distance(tf.position, enemies[i].transform.position);            
            if(range != null && dist > range) continue;

            // 초기화
            if (closestIndex == -1)
            {
                closestRange = dist;
                closestIndex = i;
                continue;
            }

            // 최단거리 갱신
            if (dist < closestRange)
            {
                closestRange = dist;
                closestIndex = i;
                //Debug.Log("최단거리 갱신");
            }
        }

        // 조건에 해당하는 적 없음
        if (closestIndex == -1)
        {
            //Debug.Log("no enemy can attack : " + closestIndex);
            return null;
        }

        //Debug.Log("closestIndex : " + closestIndex);
        //Debug.Log("closestRange : " + closestPosZ);
        return enemies[closestIndex];
    }
}
