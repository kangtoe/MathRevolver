using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    private static Player instance;
    #endregion


    [SerializeField]
    float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * transform.forward * Time.deltaTime);
    }


    public void MovePosX_Smooth(float targetX)
    {
        Debug.Log("MovePosY_Smooth || targetY :" + targetX);

        StopAllCoroutines();
        StartCoroutine(MovePosX_SmoothCr(targetX));
    }

    IEnumerator MovePosX_SmoothCr(float targetX, float duration = 1)
    {
        float t = 0;
        float StartX = transform.position.x;        

        while (true)
        {
            float lerpedX = Mathf.Lerp(StartX, targetX, t);            
            transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
            t += Time.fixedDeltaTime / duration;

            if (t >= 1)
            {
                transform.position = new Vector3(lerpedX, transform.position.y, transform.position.z);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }        
    }
}
