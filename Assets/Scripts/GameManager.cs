using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        Init();
    }

    void Init()
    {
        Application.targetFrameRate = 60;
    }
}
