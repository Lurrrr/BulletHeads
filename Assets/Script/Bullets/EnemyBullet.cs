using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        moveup(-Speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Character bigcharacter = collision.GetComponent<BigPlayer>();
            Character smallplayer = collision.GetComponent<SmallPlayer>();


            if (bigcharacter != null)
            {
                if (bigcharacter.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    bigcharacter.TakeDamage(damage);
                    Destroyself();
                }
            }
            if (smallplayer != null)
            {
                if (smallplayer.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    smallplayer.TakeDamage(damage);
                    Destroyself();
                }
            }
        }
    }
}
