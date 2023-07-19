using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    VisibleCheck visibleCheck;

    [SerializeField]
    Animator anim;

    // ü��
    [SerializeField]
    int maxHp;
    [Header("����׿� : ���� ü��")]
    [SerializeField]
    int currentHp;

    // (z��) player�� ����ġ�� �ʾҴ°�?
    bool isFrontToPlayer => transform.position.z > Player.Instance.transform.position.z;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("start");
        AddSelfOnList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!visibleCheck.Visible && !isFrontToPlayer) DeleteEnemy();
    }

    void Init()
    { 
        
    }

    void OnHit(int damage)
    {
        // todo: �ð��� ���� ȿ�� ���ϱ�
        DoHitEffect();

        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // todo : UI �ݿ�

        if (currentHp == 0) OnDie();
    }

    void OnDie()
    {
        // todo : animator �Ķ���� ����

        DeleteEnemy();
    }

    // ����� �� ó�� (���� ���� ���� ��..)
    void DeleteEnemy()
    {
        DeleteSelfOnList();
        Destroy(gameObject);
    }

    #region �� ����Ʈ ����

    // �� ����Ʈ�� ���
    void AddSelfOnList()
    {
        EnemySpwaner.Instance.enemies.Add(this);
    }

    // �� ����Ʈ���� ����
    void DeleteSelfOnList()
    {
        EnemySpwaner.Instance.enemies.Remove(this);
    }

    #endregion



    // ��鸲 ���� �ڷ�ƾ ȣ��
    public void DoHitEffect()
    {
        // ���ӽð�
        float duration = 0.3f;
        
        StopAllCoroutines();
        StartCoroutine(ShackPosCr(duration));
        StartCoroutine(ColorChangeCr(duration));
    }

    // todo : ���� ���� �ڷ�ƾ
    IEnumerator ColorChangeCr(float duration)
    {
        yield return null;
    }

    // ��鸲 ���� �ڷ�ƾ
    IEnumerator ShackPosCr(float duration)
    {
        // �ڷ�ƾ ����
        float interval = 0.05f;
        // ��鸲 ����
        float amount = 0.1f;

        // �ڽ� ������Ʈ ���� ��ġ ���ϱ�        
        Vector3 GetPosition()
        { 
            return anim.transform.localPosition;
        }
        // �ڽ� ������Ʈ ���� ��ġ ���ϱ�
        void SetPosition(Vector3 pos)
        {
            anim.transform.localPosition = pos;
        }    

        Vector3 startPos = GetPosition();
        float leftTime = duration;

        bool plusMinus = true;

        while (true)
        {            
            leftTime -= interval;            

            // ���� �ð� ���� : ���� ��ġ��
            if (leftTime < 0)
            {
                SetPosition(startPos);
                break;
            }

            // ��鸲 ���ϱ�
            // �ʱ⿡�� ������ ���� ���������, ���� ���� ����� �� ���� ������ ����
            Vector3 shake;
            if (plusMinus) shake = Vector3.right;
            else shake = Vector3.left;            
            shake *= amount;

            // ��鸲 ���� ����
            plusMinus = !plusMinus;

            // ��鸲 ����
            Vector3 pos = GetPosition() + shake;
            SetPosition(pos);
            
            yield return new WaitForSeconds(interval);
        }
    }
}
