using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    
    
    [Header("基本属性")]
   
    public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")]
    
    public float invulnerableDuration;//无敌时间
    
    public float invulnerableCounter;//计时器
    
    public bool invulnerable;//是否受伤
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (invulnerable == true)
            return;
        if(currentHealth - damage<=0)
        {
            currentHealth = currentHealth - damage; //当前血量减去收到的伤害
            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            //触发死亡
            
        }
    }

    public void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}

