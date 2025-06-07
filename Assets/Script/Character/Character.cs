using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    
    [Header("属性")]
    [SerializeField] public float HP;
    [SerializeField] protected float MaxHP;
    [SerializeField] protected float JumpForce;
    [SerializeField] protected float MoveSpeed;
    [Header("开火属性")]
    [SerializeField] public float FireRate;
    [SerializeField] protected Transform FirePosition;
    [SerializeField] protected Transform HorizontalFirePosition;
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected float nextFireTime;


    [Header("组件")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float HorizontalInput;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected Animator animator;
    protected PhotonView PV;

    [Header("变量")]
    public bool invulnerable;//是否受伤
    public float invulnerableDuration;//无敌时间
    public float invulnerableCounter;//计时器
    public bool isattack;
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
        Flip();
        //开火逻辑
        Fire(FireRate,gameObject);

        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
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
        if (!isattack)
        {
            if (HorizontalInput > 0)
            {
                rb.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            }
            if (HorizontalInput < 0)
            {
                rb.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), rb.transform.localScale.y, rb.transform.localScale.z);
            }


            if (HorizontalInput == 0)
            {
                if (AnimatorHasParameter(animator,"walk"))
                {
                    animator.SetBool("walk", false);
                }
            }
            else
            {
                if(AnimatorHasParameter(animator, "walk"))
                {
                    animator.SetBool("walk", true);
                }
                    
            }
        }
        

    }

    protected bool AnimatorHasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
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


    protected void Fire(float FireRate, GameObject owner)
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && transform.localScale.x > 0)
        {
            Debug.Log(owner.name);
            if(owner.name == "BigCharacter(Clone)")
            {
                StartCoroutine("Leftattack");
            }
            if(owner.name == "SmallCharacter(Clone)")
            {
                StartCoroutine("Leftattack_Small");
            }
            nextFireTime = Time.time + 1f / FireRate; // 计算下次可射击时间
        }
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && transform.localScale.x <= 0)
        {
            if (owner.name == "BigCharacter(Clone)")
            {
                StartCoroutine("Rightattack");
            }
            if (owner.name == "SmallCharacter(Clone)")
            {
                StartCoroutine("Rightattack_Small");
            }
            nextFireTime = Time.time + 1f / FireRate; // 计算下次可射击时间
        }
        if (Input.GetMouseButton(1) && Time.time >= nextFireTime)
        {
            //Debug.Log("向上攻击");
            if (owner.name == "BigCharacter(Clone)")
            {
                StartCoroutine("Upattack");
            }
            if (owner.name == "SmallCharacter(Clone)")
            {
                StartCoroutine("Upattack_Small");
            }
            nextFireTime = Time.time + 1f / FireRate; // 计算下次可射击时间
        }
    }

    IEnumerator Upattack()
    {
        isattack = true;
        animator.SetBool("upattack", true);
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Bullet/UpDefualtBullet", FirePosition.position, Quaternion.identity);
        animator.SetBool("upattack", false);
        isattack = false;
    }

    IEnumerator Leftattack()
    {
        isattack = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Bullet/LeftBullet", HorizontalFirePosition.position, HorizontalFirePosition.rotation);
        animator.SetBool("attack", false);
        isattack = false;
    }
    IEnumerator Rightattack()
    {
        isattack = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Bullet/RightBullet", HorizontalFirePosition.position, HorizontalFirePosition.rotation);
        animator.SetBool("attack", false);
        isattack = false;
    }


    IEnumerator Upattack_Small()
    {
        isattack = true;
        animator.SetBool("upattack", true);
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Bullet/UpDefualtBullet_Small", FirePosition.position, Quaternion.identity);
        animator.SetBool("upattack", false);
        isattack = false;
    }

    IEnumerator Leftattack_Small()
    {
        isattack = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Bullet/LeftBullet_Small", HorizontalFirePosition.position, HorizontalFirePosition.rotation);
        animator.SetBool("attack", false);
        isattack = false;
    }

    IEnumerator Rightattack_Small()
    {
        isattack = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Bullet/RightBullet_Small", HorizontalFirePosition.position, HorizontalFirePosition.rotation);
        animator.SetBool("attack", false);
        isattack = false;
    }

    public void TakeDamage(float damage)
    {
        /*
        if (invulnerable == true)
            return;
        */
        if (HP - damage >= 0)
        {
            HP -= damage; //当前血量减去收到的伤害
            TriggerInvulnerable();
            //Debug.Log($"现在的血量：{HP}");
        }
        else
        {
            HP = 0;
            //触发死亡
            //Debug.Log($"现在的血量：{HP},死了");
            dead();
        }
    }

    public void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    protected void dead()
    {
        StartCoroutine("IEdead");
    }

    IEnumerator IEdead() 
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.4f);
        if(PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
