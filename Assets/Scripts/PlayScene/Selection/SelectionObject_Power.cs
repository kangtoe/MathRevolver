using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObject_Power : MonoBehaviour
{
    ScoreManager ScoreManager => ScoreManager.Instance;

    [Header("디버그용 : 섹션에 해당하는 선택지들")]
    [SerializeField]
    List<SelectionObject_Power_Element> elements = new List<SelectionObject_Power_Element>();

    [SerializeField]
    GameObject elementPrefab;

    [SerializeField]
    SelectionObject_Power_Element optimalSelect;
    public SelectionObject_Power_Element OptimalSelect => optimalSelect;

    private void OnEnable()
    {
        SelectionCreator.Instance.AddToList(this);
    }

    private void OnDisable()
    {
        SelectionCreator.Instance.DeleteOnList(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject + ": OnTriggerEnter = " + other);

        if (other.tag == "Player")
        {
            // 어느 선택지에 가장 가까운지 판별하여, 해당 선택지에 해당하는 효과 적용
            SelectionObject_Power_Element selection = GetClosestSelection(other.transform.position);
            selection.OnSelected();

            // 선택지 페이드 아웃
            foreach (SelectionObject_Power_Element element in elements)
            {
                if (element == selection) element.FadeColor(2f, 0f, 1, -1, true);
                else element.FadeColor(0.5f, 0f, -1, -1, true);
            }

            // 플레이어 택스트 생성
            Player.Instance.MakeText(selection.Text);


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

    // 실제 선택지 생성    
    public void CreateElement(int count)
    {        
        float floorWidth = 10;
        float size = floorWidth / count;
        float minX = transform.position.x - (floorWidth / 2f);

        // 최적해
        int optimalScore = ScoreManager.Instance.GetOptimalScore();
        int optimalScore_tmp = 0;

        // 해당 인덱스의 요소는 무조건 Add 연산
        int addIdx = Random.Range(0, count);

        for (int i = 0; i < count; i++)
        {           
            // 선택지 오브젝트 생성
            GameObject go =  Instantiate(elementPrefab, transform);
            SelectionObject_Power_Element element = go.GetComponent<SelectionObject_Power_Element>();

            element.EnableVFX(false);

            // 선택지 내용 설정
            if (i == addIdx) element.SetCalc(CalcType.Add);
            else element.SetCalc();
                
            // 사이즈 설정            
            element.SetSize(size);

            // 위치 설정
            Vector3 pos = transform.position;
            float pointX = minX + ((size * i) + (size * (i + 1))) / 2f;
            go.transform.position = new Vector3(pointX, pos.y, pos.z);

            // 색 설정. 하드 코딩됨
            Color color;
            if (i % 3 == 0) color = Color.red;
            else if (i % 2 == 0) color = Color.green;
            else color = Color.blue;
            element.SetMeshColor(color);

            // 최적해 구하기
            int preCalc = element.PreCalc(optimalScore);
            if (preCalc > optimalScore_tmp)
            {
                optimalScore_tmp = preCalc;
                optimalSelect = element;
            } 

            elements.Add(element);
        }

        // 최적해 갱신
        ScoreManager.SetOptimalScore(optimalScore_tmp);
        //optimalSelect.EnableVFX(true);
    }
}
