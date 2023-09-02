using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCreator : MonoBehaviour
{
    #region 싱글톤
    public static SelectionCreator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SelectionCreator>();
            }
            return instance;
        }
    }
    private static SelectionCreator instance;
    #endregion

    [SerializeField]
    GameObject selectionPrefab_Power;
    [SerializeField]
    GameObject selectionPrefab_Math;

    [SerializeField]
    Transform creatingPoint;

    [Header("선택지/문제풀이 발판 출현 간격")]
    [SerializeField]
    float creatingInterval = 5f;

    [Header("Mathpid 출현 카운트")]
    [SerializeField]
    int adventCount = 1; // '전투력 선택지: 매스피드 선택지' 출현비에서 에서 전투력 선택지의 비율
    int spwanCount = 0;

    [Header("인스턴스화된 전투력 선택지 섹션")]
    [SerializeField]
    List<SelectionObject_Power> SelectionObject_PowerList;

    [Header("전투력 선택지 마진")]
    [SerializeField]
    float planeMargin = 0.05f;
    [SerializeField]
    float textMargin = 0.5f;

    [Header("전투력 선택지 색")]
    [SerializeField]
    Color selectionColor;

    [Header("전투력 합연산 선택지 무작위 출력 배수 (최적화 점수 비례)")]
    [SerializeField]
    float randomMult = 2;
    public float RandimMult => randomMult;

    bool OnActiveAllOptimalVFX;

    // Start is called before the first frame update
    void Start()
    {        
        
    }    

    // Update is called once per frame
    public void CreateStart()
    {
        StartCoroutine(CreateSelectionCr());
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

    // 모든 최적 선택지 VFX 활성화 제어
    public void ActiveAllOptimalVFX(bool active)
    {
        OnActiveAllOptimalVFX = active;
        UpdateOptimalSelectVFX();
    }

    // OnActiveAllOptimalVFX -> 모든 선택지 섹션에 반영
    void UpdateOptimalSelectVFX()
    {
        // 현존하는 모든 최적 선택지 표시
        foreach (SelectionObject_Power selection in SelectionObject_PowerList)
        {
            selection.OptimalSelect.EnableVFX(OnActiveAllOptimalVFX);
        }
    }

    #region 전투력 선택지

    void CreateSelection_Power()
    {        
        // 선택지 오브젝트 생성
        GameObject go = Instantiate(selectionPrefab_Power, transform);

        // 위치 설정
        go.transform.position = creatingPoint.position;

        SelectionObject_Power selection = go.GetComponent<SelectionObject_Power>();
        selection.CreateElement(3, planeMargin, textMargin, selectionColor);

        UpdateOptimalSelectVFX();
    }

    #endregion

    #region 리스트 관리

    public void AddToList(SelectionObject_Power SelectionObject_Power)
    {
        SelectionObject_PowerList.Add(SelectionObject_Power);
    }

    public void DeleteOnList(SelectionObject_Power SelectionObject_Power)
    {
        SelectionObject_PowerList.Remove(SelectionObject_Power);
    }

    #endregion
}
