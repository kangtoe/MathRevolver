using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectCreator : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    [Header("정답/오답 표시 UI")]
    [SerializeField]
    GameObject currectUI;
    [SerializeField]
    GameObject incurrectUI;

    public void CreateCurrectUI()
    {
        Instantiate(currectUI, canvas.transform);
    }

    public void CreateIncurrectUI()
    {
        Instantiate(incurrectUI, canvas.transform);
    }

}
