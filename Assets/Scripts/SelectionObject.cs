using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionObject : MonoBehaviour
{
    public Color color;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    MeshRenderer mesh;

    [SerializeField]
    int score = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        Color col = text.color;
        col.a = color.a;
        text.color = col;

        //mesh.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject + ": OnTriggerEnter = " + other);

        if (other.tag == "Player")
        {
            

            ScoreManager.Instance.AddScore(score);
        }
        
    }
}
