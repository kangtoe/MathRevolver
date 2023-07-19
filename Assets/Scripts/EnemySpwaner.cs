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
        GameObject go = Instantiate(enemyPrefab, null);
        go.transform.position = pos;

        //Enemy enemy = go.GetComponent<Enemy>();        
    }

    public void DebugFunc()
    {
        Enemy enemy = GetClosestEnemy();
        float z = enemy.transform.position.z;
        Debug.Log("GetClosestEnemy z : " + z);       
    }

    Enemy GetClosestEnemy()
    {        
        // list ��� ���� �˻�
        if (enemies.Count == 0)
        {
            Debug.Log("enemies.Count == 0");
            return null;
        }        

        int closestIndex = 0;
        float closestRange = enemies[0].transform.position.z;

        for (int i = 0; i < enemies.Count; i++)
        {
            float range = enemies[i].transform.position.z;

            // �ִܰŸ� ����
            if (range < closestRange)
            {
                closestRange = range;
                closestIndex = i;
                Debug.Log("�ִܰŸ� ����");
            }
        }

        Debug.Log("closestIndex : " + closestIndex);
        Debug.Log("closestRange : " + closestRange);
        return enemies[closestIndex];
    }
}
