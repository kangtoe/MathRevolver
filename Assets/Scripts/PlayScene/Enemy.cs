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

    // �̵�
    [SerializeField]
    float moveSpeed = 0.1f;

    // (z��) player�� ����ġ�� �ʾҴ°�?
    bool IsFrontToPlayer => transform.position.z > Player.Instance.transform.position.z;

    // ���� ���� �����Ѱ�?
    public bool CanBeAttacked => IsFrontToPlayer && visibleCheck.Visible;

    // Start is called before the first frame update
    void Start()
    {
        AddSelfOnList();
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // player�� ������ ī�޶� ��� -> ����
        if (!visibleCheck.Visible && !IsFrontToPlayer) DeleteEnemy();

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void OnHit(int damage)
    {
        // todo: �ð��� ���� ȿ�� ���ϱ�
        DoHitEffect();

        Vector3 textPos = transform.position + Vector3.right * -0.3f + Vector3.up * 0.5f;
        Text3dMaker.Instance.MakeText(damage.ToString(), textPos);

        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // todo : UI �ݿ�

        if (currentHp == 0) OnDie();
    }

    void OnDie()
    {
        // todo : animator �Ķ���� ���� �Ǵ� die prefab(��ƼŬ �ý��� ��) ����

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

    #region �ǰ� ����

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

    #endregion


}
