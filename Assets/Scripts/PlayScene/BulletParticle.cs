using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{ 
    undefined = 0,
    nomal = 1,
    skill = 2
}

public class BulletParticle : MonoBehaviour
{
    public BulletType type;    
    ParticleSystem ps;    

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        //Debug.Log($"Effect Collision : {other.name}");

        Enemy enemy = other.GetComponent<Enemy>();
        
        // 적에게 피해
        if (enemy)
        {
            // 일반 피해량
            int damage = (int)Player.Instance.damageOnNomal;
            if (type == BulletType.skill)
            {
                // 스킬 발동 중 피해량
                if (enemy.isBoss) damage = Player.Instance.damageOnSkillToBoss;
                else damage = Player.Instance.damageOnSkillToEnemy;
            }

            // 피해주기
            enemy.OnHit(damage);
        }                
    }

    public void Shoot(Vector3? point)
    {
        if(point != null) transform.LookAt(point.Value);
        ps.Play();
    }
}
