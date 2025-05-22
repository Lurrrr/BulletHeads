using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    
    [Header("属性")]
    [SerializeField] protected float HP;
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float JumpForce;
    [SerializeField] protected float MoveSpeed;
    [Header("开火属性")]
    [SerializeField] protected float FireRate;
    [SerializeField] protected Transform FirePosition;
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected float nextFireTime;


    [Header("组件")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float HorizontalInput;
    [SerializeField] protected bool isGrounded;


    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        //跳跃逻辑
        if(isGrounded)
        {
            Jump();
        }
        //移动逻辑
        Movement();
        Flip();
        //开火逻辑
        Fire(FireRate);
    }

    protected void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x,JumpForce);
        }
    }

    protected void Movement()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(MoveSpeed * HorizontalInput, rb.velocity.y);
    }

    protected void Flip()
    {
        if(HorizontalInput<0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); // 朝左
        }
        if(HorizontalInput>0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // 朝右
        }
    }


    protected void Fire(float FireRate)
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            PhotonNetwork.Instantiate("Bullet/DefualtBullet", FirePosition.position, FirePosition.rotation);
            nextFireTime = Time.time + 1f / FireRate; // 计算下次可射击时间
        }
        
    }

}
