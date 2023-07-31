using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCreator : MonoBehaviour
{
    [SerializeField]
    GameObject selectionPrefab_Power;
    [SerializeField]
    GameObject selectionPrefab_Math;

    [SerializeField]
    Transform creatingPoint;    

    [SerializeField]
    float creatingInterval = 5f;

    [SerializeField]
    int adventCount = 1; // '전투력 선택지: 매스피드 선택지' 출현비에서 에서 전투력 선택지의 비율
    int spwanCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateSelectionCr());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreateSelectionCr()
    {
        while (true)
        {
            // 선택지 오브젝트 생성
            if (spwanCount > adventCount)
            {
                Debug.Log("spwanCount error : " + spwanCount);
            }

            if (spwanCount == adventCount)
            {
                CreateSelection_Math();
                spwanCount = 0;
            }            
            else
            {
                CreateSelection_Power();
                spwanCount++;
            }            

            yield return new WaitForSeconds(creatingInterval);
        }
    }

    void CreateSelection_Math()
    {
        // 선택지 오브젝트 생성
        GameObject go = Instantiate(selectionPrefab_Math, transform);

        // 위치 설정
        Vector3 pos = creatingPoint.position;
        go.transform.position = pos;        
    }

    #region 전투력 선택지

    void CreateSelection_Power()
    {        
        // 선택지 오브젝트 생성
        GameObject go = Instantiate(selectionPrefab_Power, transform);

        // 위치 설정
        go.transform.position = creatingPoint.position;

        SelectionObject_Power selection = go.GetComponent<SelectionObject_Power>();
        selection.CreateElement(3);
    }



    #endregion
}
