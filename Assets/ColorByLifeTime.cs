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
    }

    // Update is called once per frame
    void Update()
    {
        OnLife += Time.deltaTime;
        if (OnLife > 1) OnLife = 1;
        SetColor();

        if (OnLife == 1) Destroy(gameObject);
    }

    private void OnValidate()
    {
        SetColor();
    }

    void SetColor()
    {
        Image.color = gradient.Evaluate(OnLife);
    }
}
