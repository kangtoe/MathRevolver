using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    void OnCollectEnable()
    {
        gameObject.SetActive(true);
    }

    void OnCollectDisable()
    {
        gameObject.SetActive(false);
    }
}
