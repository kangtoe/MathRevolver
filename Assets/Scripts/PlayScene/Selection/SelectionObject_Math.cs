using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObject_Math : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject + ": OnTriggerEnter = " + other);

        if (other.tag == "Player")
        {
            SoundManager.Instance.PlaySound("select");

            Debug.Log("매스피드 발판 밟음");
            WJ_Sample_Play.Instance.ActivePanel(true);
        }
    }
}
