using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickManager : MonoBehaviour
{
    #region 싱글톤
    public static ClickManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ClickManager>();
            }
            return instance;
        }
    }
    private static ClickManager instance;
    #endregion

    public Transform ClickPointObject => clickPointObject;
    [SerializeField]
    Transform clickPointObject;
    
    [SerializeField]
    LayerMask groundLayerMask;
   
    [Header("클릭 가능 영역")]
    [SerializeField]
    GameObject ClickAreaXAnchor; // 영역 x좌표 기준 : 플레이어 케릭터
    [SerializeField]
    Vector2 clickAreaSize = new Vector2(10, 15);
    [SerializeField]
    Vector3 clickAreaOffset = new Vector3(0, 0, 5);
    Bounds checkArea
    {
        get
        {
            Vector3 vec = new Vector3(clickAreaSize.x, 1, clickAreaSize.y);
            Vector3 center = new Vector3(0,0, ClickAreaXAnchor.transform.position.z) + clickAreaOffset;
            return new Bounds(center, vec);
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        ClickCheck();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(checkArea.center, checkArea.size);
    }

    // 클릭 지점 지면에 클릭 포인터 표시
    void ClickCheck()
    {
        // ui 클릭시 지면 이동 입력은 무시
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

            // 마우스 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            Vector3? point = GetClickPoint();
            if (point == null)
            {
                Debug.Log("클릭 지점 Ground 물체 없음");
                return;
            }

            // 클릭 영역 외부 클릭 시, 이동 위치 영역 내로 재조정
            if (checkArea.Contains(point.Value) == false)
            {
                point = checkArea.ClosestPoint(point.Value);                
            }

            ClickPointer.Instance.SetPointer(point.Value);
            clickPointObject.position = point.Value;
            Player.Instance.MovePosX_Smooth(point.Value.x);
        }
    }

    // 마우스 클릭 지점 알아내기
    Vector3? GetClickPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            return hit.point;
        }
        return null;
    }
}
