using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public void OnCollectEnable()
    {
        gameObject.SetActive(true);
    }

    public void OnCollectDisable()
    {
        gameObject.SetActive(false);
    }
}
