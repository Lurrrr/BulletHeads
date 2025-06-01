using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("基本属性")]
    public float damage;

    public float attackRange;//攻击范围
    
    public float damageRate;//攻击速度

    private void OnTriggerStay2D(Collider2D other)//other是被攻击的人
    {
       
        other.GetComponent<EnemyCharacter>()?.TakeDamage(this);
        other.GetComponent <BigPlayer>()?.TakeDamage(this);
        other.GetComponent<SmallPlayer>()?.TakeDamage(this);
    }
}
