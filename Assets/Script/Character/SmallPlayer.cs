using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmallPlayer : Character
{
    // Start is called before the first frame update
    new
    void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        HorizontalFirePosition = transform.Find("HorizontalFirePosition");
        FirePosition = transform.Find("UpFirePosition");
        base.Start();
        //设置角色属性
        HP = 40f;
        MaxHP = 40f;
        JumpForce = 4f;
        MoveSpeed = 6f;
        FireRate = 3f;

        //删除其他玩家在本地的rigidbody
        if(!PV.IsMine)
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
            Movement();
            Flip();
            //开火逻辑
            Fire(FireRate,this.gameObject);
            Jump();
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
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
