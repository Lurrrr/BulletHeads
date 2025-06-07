using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [Header("跟随设置")]
    [SerializeField] protected string playerTag = "Player"; // 玩家标签
    [SerializeField] protected float followSpeed = 3f;      // 跟随速度
    [SerializeField] protected float followDistance = 1f;   // 跟随距离
    [SerializeField] protected float smoothTime = 0.3f;     // 平滑跟随时间

    protected Transform playerTransform;    // 玩家Transform引用
    protected bool shouldFollow = false;    // 是否开始跟随
    protected Vector2 currentVelocity;      // 当前速度(用于平滑阻尼)

    [Header("投掷设置")]
    [SerializeField] protected KeyCode throwKey = KeyCode.E;
    [SerializeField] protected Transform throwTarget; // 投掷目标位置
    [SerializeField] protected float throwDuration = 1f; // 投掷时间
    [SerializeField] protected AnimationCurve throwCurve; // 投掷运动曲线

    protected CircleCollider2D circleCollider;
    protected Vector2 throwStartPosition;
    protected float throwStartTime;

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] public bool isFollow;
    [SerializeField] protected bool isThrowing = false;
    [SerializeField] protected bool eated = false;
    [SerializeField] protected PhotonView playerpv;
    [SerializeField] protected PhotonView selfpv;
    [SerializeField] protected float addon;


    protected virtual void Start()
    {
        selfpv = GetComponent<PhotonView>();


        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        //给一个初始力
        GiveForce();
        // 初始化曲线(如果未设置)
        if (throwCurve == null || throwCurve.length == 0)
        {
            throwCurve = new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(0.5f, 1.2f), // 抛物线高点
                new Keyframe(1, 0));
        }
    }

    protected virtual void Update()
    {
        if (isFollow && !isThrowing && Input.GetKeyDown(KeyCode.E) && throwTarget != null && playerpv.IsMine)
        {
            Debug.Log("丢");
            if (selfpv.IsMine)
            {
                Throw();
            }


        }
        else if (isFollow && !isThrowing && Input.GetKeyDown(KeyCode.Q) && playerpv.IsMine)
        {
            Debug.Log("使用");
            circleCollider.isTrigger = true;
            circleCollider.enabled = true;


        }
        else if (isThrowing)
        {
            // 计算投掷进度(0-1)
            float progress = (Time.time - throwStartTime) / throwDuration;

            if (progress < 1f)
            {
                // 使用曲线控制投掷路径
                float curveValue = throwCurve.Evaluate(progress);
                Vector2 currentPos = Vector2.Lerp(throwStartPosition, throwTarget.position, progress);
                currentPos.y += curveValue * 2f; // 抛物线高度
                transform.position = currentPos;
            }
            else
            {
                // 投掷完成
                transform.position = throwTarget.position;
                isThrowing = false;

                // 可选: 到达后恢复为碰撞体
                // circleCollider.isTrigger = false;
            }
        }
    }

    private void GiveForce()
    {
        rb.AddForce(new Vector2(300, 200));
    }


    protected virtual void Function(Collider2D collider)
    {

    }

    protected virtual void Strengthen()
    {

    }

    protected void follow()
    {
        if (shouldFollow && playerTransform != null)
        {
            float facingSign = Mathf.Sign(playerTransform.localScale.x); // 或检查SpriteRenderer.flipX

            // 计算真正的后方位置
            Vector2 targetPosition = (Vector2)playerTransform.position -
                                   new Vector2(facingSign, 0) * followDistance * -1f;

            // 使用平滑阻尼移动
            transform.position = Vector2.SmoothDamp(
                transform.position,
                targetPosition,
                ref currentVelocity,
                smoothTime,
                followSpeed);

            // 可选：使球体始终朝向玩家
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            
            isFollow = true;
            //selfpv.RPC("ChangeParameter", RpcTarget.All, true);

        }
    }

    protected void Throw()
    {
        //selfpv.RPC("ChangeParameter", RpcTarget.All, false);
        isFollow = false;

        isThrowing = true;
        shouldFollow = false;

        // 记录投掷开始状态
        throwStartPosition = transform.position;
        throwStartTime = Time.time;

        StartCoroutine("changetrigger");
        Strengthen();
    }
    IEnumerator changetrigger()
    {

        yield return new WaitForSeconds(1f);
        // 设置为Trigger以便穿过其他物体
        circleCollider.isTrigger = true;
        circleCollider.enabled = true;

        // 禁用物理(因为我们手动控制投掷运动)
        rb.isKinematic = true;
    }

    [PunRPC]
    protected void ChangeParameter(bool tof)
    {
        isFollow = tof;
    }
}
