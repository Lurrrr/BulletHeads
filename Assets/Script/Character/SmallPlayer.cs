using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPlayer : Character
{
    // Start is called before the first frame update
    new
    void Start()
    {
        base.Start();
        //设置角色属性
        HP = 40f;
        MaxHP = 40f;
        JumpForce = 8f;
        MoveSpeed = 10f;
        FireRate = 1f;

        FirePosition = transform.Find("FirePosition").transform;

    }

    new
    // Update is called once per frame
    void Update()
    {
        base.Update();
        //开火逻辑
        Fire(FireRate);
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
