using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefultBullet : Bullet
{
    public float damage = 10f;
    // Start is called before the first frame update
    new
    void Start()
    {
        base.Start();
        //设置子弹参数


    }

    new
    // Update is called once per frame
    void Update()
    {
        moveup(Speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            EnemyCharacter enemycharacter = collision.GetComponent<EnemyCharacter>();

            if(enemycharacter != null)
            {
                enemycharacter.TakeDamage(damage);
                Destroyself();
            }
        }
    }
}
