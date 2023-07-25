using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionObject_Power : MonoBehaviour
{    
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    MeshRenderer mesh;

    [SerializeField]
    int score = 100;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject + ": OnTriggerEnter = " + other);

        if (other.tag == "Player")
        {            
            ScoreManager.Instance.AddScore(score);
        }        
    }

    public void SetMateralColor(Color new_color, bool changeAlpha = false)
    {
        Color color = new_color;
        if (!changeAlpha) color.a = mesh.material.color.a;
        mesh.material.color = color;
    }

    public void SetSize(float X, float Z = float.NaN)
    {
        Vector3 scale = mesh.transform.localScale ;
        scale.x = X / 10f;
        if (!float.IsNaN(Z)) scale.y = Z / 10f;
        mesh.transform.localScale = scale;
    }
}
