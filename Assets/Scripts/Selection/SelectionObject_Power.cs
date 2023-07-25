using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObject_Power : MonoBehaviour
{
    [Header("디버그용 : 섹션에 해당하는 선택지들")]
    [SerializeField]
    List<SelectionObject_Power_Element> elements = new List<SelectionObject_Power_Element>();

    [SerializeField]
    GameObject elementPrefab;    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject + ": OnTriggerEnter = " + other);

        if (other.tag == "Player")
        {
            // 어느 선택지에 가장 가까운지 판별하여, 해당 선택지에 해당하는 효과 적용
            SelectionObject_Power_Element selection = GetClosestSelection(other.transform.position);
            selection.DoEffect();
        }        
    }

    // 가장 가까운 선택지 찾기
    SelectionObject_Power_Element GetClosestSelection(Vector3 pos)
    {
        float closestDist = float.MaxValue;
        SelectionObject_Power_Element closestelement = null;                

        foreach (SelectionObject_Power_Element element in elements)
        {
            float dist = Vector3.Distance(pos, element.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestelement = element;
            }
        }        

        return closestelement;
    }

    // 선택지에 대한 값 리스트를 받아 실제 선택지 생성
    // 임시로 int 리스트를 받지만, 추후에는 정의된 수식 클래스 인스턴스를 받아 해당하는 동작으로 처리 할 수 있도록 할것
    public void CreateElement(List<int> strs)
    {
        int count = strs.Count;
        float floorWidth = 10;
        float size = floorWidth / count;
        float minX = transform.position.x - (floorWidth / 2f);

        for (int i = 0; i < count; i++)
        {
            // 선택지 오브젝트 생성
            GameObject go =  Instantiate(elementPrefab, transform);
            SelectionObject_Power_Element element = go.GetComponent<SelectionObject_Power_Element>();

            // 사이즈 설정            
            element.SetSize(size);

            // 위치 설정
            Vector3 pos = transform.position;
            float pointX = minX + ((size * i) + (size * (i + 1))) / 2f;
            go.transform.position = new Vector3(pointX, pos.y, pos.z);

            // 점수 설정            
            element.SetScore(strs[i]);

            // 색 설정. 하드 코딩됨
            Color color;
            if (i % 3 == 0) color = Color.red;
            else if (i % 2 == 0) color = Color.green;
            else color = Color.blue;
            element.SetColor(color);

            elements.Add(element);
        }
    }
}
