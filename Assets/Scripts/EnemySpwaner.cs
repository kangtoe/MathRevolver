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

    // ����׿�
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
            // ������ ������Ʈ ����
            //GameObject go = Instantiate(selectionPrefab, transform);
            //go.transform.position = creatingPoint.position;
            Spwan(enemyPrefab);

            yield return new WaitForSeconds(spwanInterval);
        }
    }
     
    void Spwan(GameObject enemyPrefab)
    {
        Vector3 pos = spwanPoint.position;

        // ���� �������� ����ϴ� ������ ������ ��
        float floorWidth = 10;
        float margin = 1;

        // spwanPoint���� ������ x ��ġ ���ϱ�
        float spwanWidth = floorWidth - margin;        
        float minX = spwanPoint.position.x - (spwanWidth / 2f);
        float maxX = spwanPoint.position.x + (spwanWidth / 2f);
        pos.x = Random.Range(minX, maxX);

        // �� ����
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

    // ���� ������ ���� ����� �� ��ȯ (������ null)
    public Enemy GetClosestEnemy()
    {        
        // list ��� ���� �˻�
        if (enemies.Count == 0)
        {
            Debug.Log("enemies.Count == 0");
            return null;
        }        

        // ���� ���߿� ����Ʈ�� �߰���(������) �� = ���� �ָ��ִ� ��
        int closestIndex = -1;
        float closestPosZ = 0;

        for (int i = 0; i < enemies.Count; i++)
        {            
            // ���� ������ ���ΰ�?
            if (!enemies[i].CanBeAttacked) continue;

            float posZ = enemies[i].transform.position.z;

            // �ʱ�ȭ
            if (closestIndex == -1)
            {
                closestPosZ = posZ;
                closestIndex = i;
                continue;
            }                        

            // �ִܰŸ� ����
            if (posZ < closestPosZ)
            {
                closestPosZ = posZ;
                closestIndex = i;
                //Debug.Log("�ִܰŸ� ����");
            }
        }

        // ���ǿ� �ش��ϴ� �� ����
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
