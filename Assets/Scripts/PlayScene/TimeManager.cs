using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // 싱글톤
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimeManager>();
            }
            return instance;
        }
    }
    private static TimeManager instance;

    // 디버그용
    [SerializeField]
    float currentTimeScale;

    private void OnEnable()
    {
        SetScale(1);
    }

    private void OnDisable()
    {
        SetScale(1);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeScale = Time.timeScale;
    }    

    public void SetScale(float f)
    {
        Time.timeScale = f;
    }
}
