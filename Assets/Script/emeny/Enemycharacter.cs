using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyCharacter : MonoBehaviourPunCallbacks
{
    
    
    [Header("基本属性")]
   
    public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")]
    
    public float invulnerableDuration;//无敌时间
    
    public float invulnerableCounter;//计时器
    
    public bool invulnerable;//是否受伤

    [Header("参数")]
    public Transform FirePosition;
    public float nextFireTime;
    public float FireRate = 0.5f;

    protected void Start()
    {
        currentHealth = maxHealth;
    }

    protected void Update()
    {
       
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth - damage >= 0)
        {
            currentHealth = currentHealth - damage; //当前血量减去收到的伤害
        }
        else
        {
            currentHealth = 0;
            //触发死亡
            dead();


        }
    }

    protected void dead()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    protected void Fire()
    {
        if (Time.time >= nextFireTime)
        {
            PhotonNetwork.Instantiate("Bullet/EnemyBullet", FirePosition.position, FirePosition.rotation);
            nextFireTime = Time.time + 1f / FireRate; // 计算下次可射击时间
        }
    }
}

