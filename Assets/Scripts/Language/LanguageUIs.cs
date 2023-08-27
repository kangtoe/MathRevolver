using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class LanguageUi_text
{
    [SerializeField]
    Text text;
    
    [Header("한글 설정")]    
    [SerializeField]
    [TextArea]
    string textKr;

    [Header("영문 설정")]    
    [SerializeField]
    [TextArea]
    string textEn;

    //[SerializeField]
    //Font fontKr;
    //[SerializeField]
    //Font fontEn;    

    public void SetKr()
    {
        //text.font = fontKr;
        text.text = textKr;
    }

    public void SetEn()
    {
        //text.font = fontEn;
        text.text = textEn;
    }
}

[Serializable]
public class LanguageUi_tmp
{
    [SerializeField]
    TextMeshProUGUI text;

    [Header("한글 설정")]
    [SerializeField]
    [TextArea]
    string textKr;

    [Header("영문 설정")]
    [SerializeField]
    [TextArea]
    string textEn;

    //[SerializeField]
    //TMP_FontAsset fontKr;
    //[SerializeField]
    //TMP_FontAsset fontEn;

    public void SetKr()
    {
        //text.font = fontKr;
        text.text = textKr;
    }

    public void SetEn()
    {
        //text.font = fontEn;
        text.text = textEn;
    }
}

[Serializable]
public class LanguageUi_image
{
    [SerializeField]
    Image image;
    
    [Header("한글 설정")]
    [SerializeField]
    Sprite spriteKr;

    [Header("영문 설정")]
    [SerializeField]
    Sprite spriteEn;

    public void SetKr()
    {
        image.sprite = spriteKr;
    }

    public void SetEn()
    {
        image.sprite = spriteEn;
    }
}