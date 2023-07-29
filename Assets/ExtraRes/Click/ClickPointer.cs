using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPointer : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static ClickPointer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ClickPointer>();
            }
            return instance;
        }
    }
    private static ClickPointer instance;
    #endregion

    public GameObject clickPointer;
    public float surfaceOffset = 0.1f;
    //public float liveTime = 2.0f;

    public void SetPointer(Vector3 pos)
    {
        GameObject pointer = Instantiate(clickPointer);
        pointer.transform.position = pos + Vector3.up * surfaceOffset;

        //Destroy(pointer, liveTime);
    }
}
