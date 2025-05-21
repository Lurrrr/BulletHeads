using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPlayer : Character 
{
    new
        // Start is called before the first frame update
        void Start()
    {
        base.Start();
        //设置角色属性
        HP = 100f;
        MaxHP = 100f;
        JumpForce = 5f;
        MoveSpeed = 6f;
        FireRate = 0.5f;

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
