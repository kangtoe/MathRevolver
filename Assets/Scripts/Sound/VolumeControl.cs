using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 볼륨 제어
public class VolumeControl : MonoBehaviour
{
    private void OnEnable()
    {
        // bgmVolume = 실제 볼륨
        // sfxVolume = 실제 볼륨
    }

    [Range(0, 1)]
    [SerializeField]
    float bgmVolume;
    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
            // 실제 볼륨 제어
        }
    }

    [Range(0, 1)]
    [SerializeField]
    float sfxVolume;
    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = value;
            // 실제 볼륨 제어
        }
    }
}
