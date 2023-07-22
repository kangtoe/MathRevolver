using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Transform chaseTarget;

    [SerializeField]
    Vector3 offset = new Vector3(0, 0, 2.5f);

    Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        chaseTarget = Player.Instance.transform;

        originPos = Camera.main.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = chaseTarget.position;
        
        // z축으로만 추적함
        Vector3 camCenter = new Vector3(0, 0, pos.z);
               
        transform.position = camCenter + offset;
    }

    public void ShakeDebug()
    {
        Skake(0.1f, 0.2f);
    }

    public void Skake(float _amount, float _duration)
    {
        //Debug.Log("Skake");
        StopAllCoroutines();
        StartCoroutine(ShakeCr(_amount, _duration));
    }

    IEnumerator ShakeCr(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            Camera.main.transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            //yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        Camera.main.transform.localPosition = originPos;

    }
}
