using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlayer : Character 
{
    public PhotonView PV;
    new
        // Start is called before the first frame update
        void Start()
    {
        PV = GetComponent<PhotonView>();
        base.Start();
        //设置角色属性
        HP = 100f;
        MaxHP = 100f;
        JumpForce = 5f;
        MoveSpeed = 6f;
        FireRate = 0.5f;

        FirePosition = transform.Find("FirePosition").transform;

        //删除其他玩家在本地的rigidbody
        if (!PV.IsMine)
        {
            Destroy(PV.transform.GetComponent<Rigidbody2D>());
        }

    }

    new
    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine)
        {
            //移动逻辑
            Movement();
            Flip();
            //开火逻辑
            Fire(FireRate);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
