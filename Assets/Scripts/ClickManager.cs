using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    #region �̱���
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.isOnUI) return;
        ClickCheck();
    }

    // Ŭ�� ���� ���鿡 Ŭ�� ������ ǥ��
    void ClickCheck()
    {        
        // ���콺 �Է� ó��
        if (Input.GetMouseButtonDown(0))
        {
            Vector3? point = GetClickPoint();
            if (point == null)
            {
                Debug.Log("Ŭ�� ���� Ground ��ü ����");
                return;
            }

            ClickPointer.Instance.SetPointer(point.Value);
            clickPointObject.position = point.Value;
            Player.Instance.MovePosX_Smooth(point.Value.x);
        }
    }

    // ���콺 Ŭ�� ���� �˾Ƴ���
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
