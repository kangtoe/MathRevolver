using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiColorGroup : MonoBehaviour
{
    [SerializeField]
    Color color;

    [SerializeField]
    List<TextMeshProUGUI> tmpList;

    [SerializeField]
    List<Image> imageList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        SetColor(color);
    }

    public void SetColor(Color _color)
    {
        color = _color;

        foreach (Image image in imageList) image.color = color;
        foreach (TextMeshProUGUI tmp in tmpList) tmp.color = color;
    }
}
