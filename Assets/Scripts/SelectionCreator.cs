using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCreator : MonoBehaviour
{
    [SerializeField]
    Transform creatingPoint;

    [SerializeField]
    GameObject selectionPrefab;

    [SerializeField]
    float creatingInterval = 5f;

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
            // ������ ������Ʈ ����
            //GameObject go = Instantiate(selectionPrefab, transform);
            //go.transform.position = creatingPoint.position;
            CreateSelection(4);

            yield return new WaitForSeconds(creatingInterval);
        }
    }

    void CreateSelection(int count)
    {        
        float floorWidth = 10;
        float size = floorWidth / count;
        float minX = creatingPoint.position.x - (floorWidth / 2f);
        
        Debug.Log("minX : " + minX);
        Debug.Log("size : " + size);

        for (int i = 0; i < count; i++)
        {            
            // ������ ������Ʈ ����
            GameObject go = Instantiate(selectionPrefab, transform);

            // ��ġ ����
            Vector3 pos = creatingPoint.position;
            float pointX = minX + ( (size * i) + (size * (i + 1)) ) / 2f;
            go.transform.position = new Vector3(pointX, pos.y, pos.z);                        

            // ������ ����
            SelectionObject selection = go.GetComponent<SelectionObject>();
            selection.SetSize(size);

            // �� ����. �ϵ� �ڵ���
            Color color;
            if (i % 3 == 0) color = Color.red;
            else if (i % 2 ==0) color = Color.green;
            else color = Color.blue;
            selection.SetMateralColor(color);
        }                
    }
}
