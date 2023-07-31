using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// 웅진 API 샘플과 결합하여 활용
public class DiagomosticManager : MonoBehaviour
{
    #region 싱글톤
    public static DiagomosticManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DiagomosticManager>();
            }
            return instance;
        }
    }
    private static DiagomosticManager instance;
    #endregion

    [Header("문제당 풀이시간")]
    [SerializeField]
    float timeLimit = 10;

    [SerializeField]
    Image timeGage;

    [SerializeField]
    public UnityEvent onTimeEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimeBar()
    {
        StopAllCoroutines();
        StartCoroutine(TimeBarCr());
    }

    public void InitTimeBar()
    {
        StopAllCoroutines();
    }

    IEnumerator TimeBarCr()
    {
        float leftTime = timeLimit;
        while (true)
        {
            leftTime -= Time.deltaTime;
            float ratio = Mathf.Clamp01(leftTime / timeLimit);
            timeGage.fillAmount = ratio;
            yield return null;
            if (ratio == 0) break;
        }

        Debug.Log("time end");
        onTimeEnd.Invoke();
    }
}
