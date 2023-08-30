using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectCreator : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    [Header("정답/오답 표시 UI")]
    [SerializeField]
    GameObject correctUI;
    [SerializeField]
    GameObject incorrectUI;

    public void CreateCorrectUI()
    {
        Instantiate(correctUI, canvas.transform);
    }

    public void CreateIncorrectUI()
    {
        Instantiate(incorrectUI, canvas.transform);
    }

}
