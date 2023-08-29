using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitOnceOnPlay
{
    // 매번 앱 시작 시 한번만 실행되는 함수
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethodAtOnce()
    {
        // 이전 세이브 기록을 무시하고 초기화
        SaveManager.DiagonosticCompleted = false;
        SaveManager.DiagonosticScore = 0;
    }
}
