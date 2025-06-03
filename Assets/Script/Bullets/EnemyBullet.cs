using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{

    public float damage = 5f;
    // Start is called before the first frame update
    new
    void Start()
    {
        base.Start();
        //设置子弹参数
        Speed = 3f;

    }

    new
    // Update is called once per frame
    void Update()
    {
        move(Speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Character bigcharacter = collision.GetComponent<BigPlayer>();
            Character smallplayer = collision.GetComponent<SmallPlayer>();


            if (bigcharacter != null)
            {
                bigcharacter.TakeDamage(damage);
            }
            if(smallplayer!=null)
            {
                smallplayer.TakeDamage(damage);
            }
        }
    }
}
