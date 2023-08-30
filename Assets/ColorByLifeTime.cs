using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorByLifeTime : MonoBehaviour
{
    [SerializeField]
    Image Image;

    [SerializeField]
    Gradient gradient;

    [SerializeField]
    [Range(0, 1)]
    float OnLife;

    [SerializeField]
    float lifeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        OnLife = 0;
        SetColor();

        StartCoroutine(Cr());
    }    

    private void OnValidate()
    {
        SetColor();
    }

    IEnumerator Cr()
    {
        float interval = 0.01f;

        while (true)
        {
            OnLife += interval / lifeTime;
            if (OnLife > 1) OnLife = 1;
            SetColor();

            yield return new WaitForSecondsRealtime(interval);
            if (OnLife == 1) Destroy(gameObject);
        }
    }

    void SetColor()
    {
        Image.color = gradient.Evaluate(OnLife);
    }
}
