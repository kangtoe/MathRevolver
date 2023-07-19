using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region �̱���
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
    float forwardSpeed = 1f;
    [SerializeField]
    float sideSpeed = 1f;

    [SerializeField]
    int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(forwardSpeed * transform.forward * Time.deltaTime);

        // ���� �˻�
    }

    #region X�� �̵� ����

    public void MovePosX_Smooth(float targetX)
    {
        //Debug.Log("MovePosY_Smooth || targetY :" + targetX);

        StopAllCoroutines();
        StartCoroutine(MovePosX_SmoothCr(targetX, sideSpeed));
    }

    IEnumerator MovePosX_SmoothCr(float targetX, float speed = 1)
    {
        while (true)
        {
            float currentX = transform.position.x;

            // ���� X ��ġ�� ��ǥ ��ġ �������� ���ݾ� �̵�
            if (targetX > currentX) currentX += Time.fixedDeltaTime * speed;
            else if (targetX < currentX) currentX -= Time.fixedDeltaTime * speed;

            bool closeEnough = Mathf.Abs(targetX - currentX) <= Time.fixedDeltaTime * speed;
            if (closeEnough) currentX = targetX;

            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            if (currentX == targetX) yield break;

            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    public void Attack()
    {
        Enemy target = EnemySpwaner.Instance.GetClosestEnemy();
        if (target is null)
        {
            Debug.Log("target is null");
            return;
        }

        Debug.Log("CanBeAttacked : " + target.CanBeAttacked); 
        target.OnHit(damage);
    }
}
