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
                CreateSelection_Power(4);
                spwanCount++;
            }

            

            yield return new WaitForSeconds(creatingInterval);
        }
    }

    void CreateSelection_Power(int count)
    {        
        float floorWidth = 10;
        float size = floorWidth / count;
        float minX = creatingPoint.position.x - (floorWidth / 2f);
        
        //Debug.Log("minX : " + minX);
        //Debug.Log("size : " + size);

        for (int i = 0; i < count; i++)
        {            
            // 선택지 오브젝트 생성
            GameObject go = Instantiate(selectionPrefab_Power, transform);

            // 위치 설정
            Vector3 pos = creatingPoint.position;
            float pointX = minX + ( (size * i) + (size * (i + 1)) ) / 2f;
            go.transform.position = new Vector3(pointX, pos.y, pos.z);                        

            // 사이즈 설정
            SelectionObject_Power selection = go.GetComponent<SelectionObject_Power>();
            selection.SetSize(size);

            // 색 설정. 하드 코딩됨
            Color color;
            if (i % 3 == 0) color = Color.red;
            else if (i % 2 ==0) color = Color.green;
            else color = Color.blue;
            selection.SetMateralColor(color);
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
}
