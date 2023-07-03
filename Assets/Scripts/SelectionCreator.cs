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
            // 선택지 오브젝트 생성
            GameObject go = Instantiate(selectionPrefab, transform);
            go.transform.position = creatingPoint.position;

            yield return new WaitForSeconds(creatingInterval);
        }
    }

    void CreateSelection()
    {
        
    }
}
