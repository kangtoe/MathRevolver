using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObject_Power : MonoBehaviour
{
    ScoreManager ScoreManager => ScoreManager.Instance;

    [SerializeField]
    GameObject elementPrefab;

    [Header("밝게 될 선택지를 표시하기 시작 할 플레이어와의 간격")]
    [SerializeField]
    float outlineInterval = 5;

    [Header("디버그용 : 섹션에 해당하는 선택지들")]
    [SerializeField]
    List<SelectionObject_Power_Element> elements = new List<SelectionObject_Power_Element>();

    [Header("디버그용 : 최적 선택지")]    
    [SerializeField]
    SelectionObject_Power_Element optimalSelect;
    public SelectionObject_Power_Element OptimalSelect => optimalSelect;

    [Header("디버그용 : 누르게 될 선택지")]    
    [SerializeField]
    SelectionObject_Power_Element selectionPredicted;

    // 상호작용 된 적 있나?
    bool isInteracted = false;

    private void OnEnable()
    {
        SelectionCreator.Instance.AddToList(this);
    }

    private void OnDisable()
    {
        SelectionCreator.Instance.DeleteOnList(this);
    }

    private void Update()
    {
        // 예상 선택지 표시
        if(!isInteracted)
        {
            float playerZ = Player.Instance.transform.position.z;
            float myZ = transform.position.z;

            if (myZ - playerZ < outlineInterval)
            {
                SelectionObject_Power_Element selectionPredicted_new = GetClosestSelection(Player.Instance.transform.position);

                if (selectionPredicted == null)
                {
                    selectionPredicted = selectionPredicted_new;
                    selectionPredicted.EnableOutline(true);
                }
                else if (selectionPredicted_new != selectionPredicted)
                {
                    selectionPredicted.EnableOutline(false);
                    selectionPredicted_new.EnableOutline(true);

                    selectionPredicted = selectionPredicted_new;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject + ": OnTriggerEnter = " + other);

        if (other.tag == "Player")
        {
            isInteracted = true;

            // 어느 선택지에 가장 가까운지 판별하여, 해당 선택지에 해당하는 효과 적용
            SelectionObject_Power_Element selection = GetClosestSelection(other.transform.position);
            selection.OnSelected();

            // 선택지 페이드 아웃
            foreach (SelectionObject_Power_Element element in elements)
            {
                if (element == selection) element.FadeColor(true, 2f, 0f, 1, null, null);
                else element.FadeColor(true, 0.5f, 0f, null, null, null);
            }

            // 플레이어 택스트 생성
            Player.Instance.MakeText(selection.Text);
        }        
    }

    private void OnDrawGizmos()
    {
        // 디버깅 : outlineInterval 범위 표시하기
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position - Vector3.forward * outlineInterval / 2f;
        Vector3 size = new Vector3(10, 1, outlineInterval);
        Gizmos.DrawWireCube(center, size);
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
    public void CreateElement(int count, float planeMargin, float textMargin, Color color)
    {        
        float floorWidth = 10;
        float size = floorWidth / count;
        float minX = transform.position.x - (floorWidth / 2f);

        // 최적해
        double optimalScore = ScoreManager.Instance.GetOptimalScore();
        double optimalScore_tmp = 0;

        // 해당 인덱스의 요소는 무조건 Add 연산
        int addIdx = Random.Range(0, count);

        for (int i = 0; i < count; i++)
        {           
            // 선택지 오브젝트 생성
            GameObject go =  Instantiate(elementPrefab, transform);
            SelectionObject_Power_Element element = go.GetComponent<SelectionObject_Power_Element>();

            element.EnableVFX(false);
            element.EnableOutline(false);

            // 선택지 내용 설정
            if (i == addIdx) element.SetCalc(CalcType.Add);
            else element.SetCalc();
                
            // 사이즈 설정            
            element.SetSize(size, null, planeMargin, textMargin);

            // 위치 설정
            Vector3 pos = transform.position;
            float pointX = minX + ((size * i) + (size * (i + 1))) / 2f;
            go.transform.position = new Vector3(pointX, pos.y, pos.z);

            // 색 설정. 하드 코딩됨
            //Color color;
            //if (i % 3 == 0) color = Color.red;
            //else if (i % 2 == 0) color = Color.green;
            //else color = Color.blue;
            element.SetMeshColor(color);

            // 최적해 구하기
            double preCalc = element.PreCalc(optimalScore);
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
