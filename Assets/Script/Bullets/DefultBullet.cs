using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefultBullet : Bullet
{

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
        if(collision.tag == "Enemy")
        {
            worm wormscript = collision?.GetComponent<worm>();
            Bomb bombscript = collision?.GetComponent<Bomb>();
            Missile missilescript = collision?.GetComponent<Missile>();
            normal normalscript = collision?.GetComponent<normal>();

            if(wormscript!= null)
            {
                //扣血
            }
        }
    }
}
