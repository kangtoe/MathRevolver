using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeBar : MonoBehaviour
{
    [SerializeField]
    Image timeGage;

    [HideInInspector]
    public UnityEvent onTimeEnd;

    private void Start()
    {
        onTimeEnd.AddListener(() => { Debug.Log("TimeBar : onTimeEnd"); });
    }

    public void StartTimeBar(float time)
    {
        StopAllCoroutines();
        StartCoroutine(TimeBarCr(time));
    }

    public void StopTimeBar()
    {
        StopAllCoroutines();
    }

    IEnumerator TimeBarCr(float time, float interval = 0.01f)
    {        
        float leftTime = time;
        while (true)
        {
            leftTime -= interval;
            float ratio = Mathf.Clamp01(leftTime / time);
            if (ratio < 0) ratio = 0;
            timeGage.fillAmount = ratio;
            yield return new WaitForSecondsRealtime(interval);
            if (ratio == 0) break;
        }

        Debug.Log("time end");
        onTimeEnd.Invoke();
    }
}
