using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Diagonostic : MonoBehaviour
{
    #region 싱글톤
    public static UIManager_Diagonostic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager_Diagonostic>();
            }
            return instance;
        }
    }
    private static UIManager_Diagonostic instance;
    #endregion

    [SerializeField]
    Canvas canvas;

    [Header("정답/오답 표시 UI")]
    [SerializeField]
    GameObject currectUI;
    [SerializeField]
    GameObject incurrectUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCurrectUI()
    {
        Instantiate(currectUI, canvas.transform);
    }

    public void CreateIncurrectUI()
    {
        Instantiate(incurrectUI, canvas.transform);
    }
}
