using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 음향 재생
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }
    private static SoundManager instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
