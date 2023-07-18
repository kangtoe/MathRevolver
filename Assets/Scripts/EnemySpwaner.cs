using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    Transform spwanPoint;

    [SerializeField]
    float spwanInterval = 5f;

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

        // spwanPoint���� ������ x ��ġ ���ϱ�
        float floorWidth = 10;        
        float minX = spwanPoint.position.x - (floorWidth / 2f);
        float maxX = spwanPoint.position.x + (floorWidth / 2f);
        pos.x = Random.Range(minX, maxX);

        // �� ����
        GameObject go = Instantiate(enemyPrefab, transform);
        go.transform.position = pos;
    }
}
