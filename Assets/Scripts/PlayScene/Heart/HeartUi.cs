using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUi : MonoBehaviour
{
    [SerializeField]
    Image fillImage;

    public void EnableFillImage(bool enable)
    {
        fillImage.enabled = enable;
    }
}
