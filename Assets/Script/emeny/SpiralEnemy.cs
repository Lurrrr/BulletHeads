using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiralEnemy : EnemyCharacter
{
    [Header("冲锋属性")]
    public float chargeSpeed = 10f;          // 冲锋速度
    public float normalSpeed = 3f;          // 正常移动速度
    public float chargeDistance = 8f;       // 触发冲锋的最小距离
    public float chargeCooldown = 5f;       // 冲锋冷却时间
    public float chargeDuration = 1f;       // 冲锋持续时间

    [Header("冲锋伤害")]
    public float chargeDamage = 20f;        // 冲锋撞击伤害
    public float chargeKnockback = 5f;      // 击退力度

    private float lastChargeTime;           // 上次冲锋时间
    private bool isCharging = false;        // 是否正在冲锋
    private Vector2 chargeDirection;        // 冲锋方向
    private Rigidbody2D rb;
    private Transform target;              // 目标玩家
    public PhotonView pv;

    new
     void Start()
    {
        base.Start(); // 调用父类Start方法初始化基础属性
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        FindNearestPlayer();
        if (!pv.IsMine)
        {
            Destroy(rb);
        }
    }

    new
     void Update()
    {
        base.Update(); // 调用父类Update方法

        // 1. 基础组件检查
        if (this == null || photonView == null) return;

        // 2. 网络权限检查
        if (!photonView.IsMine) return;

        // 3. 目标检查与恢复
        if (target == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                FindNearestPlayer();
            }
            if (target == null)
            {
                // 可以添加等待状态或闲置行为
                rb.velocity = Vector2.zero;
                return;
            }
        }

        // 如果不是冲锋状态，正常移动
        if (!isCharging)
        {
            MoveTowardsPlayer();
            CheckChargeCondition();

        }
    }

    private void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null || players.Length == 0)
        {
            target = null;
            return;
        }
        if (players.Length > 0)
        {
            float minDistance = float.MaxValue;
            foreach (GameObject player in players)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = player.transform;
                }
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // 只有 Master Client 控制移动
        if (!PhotonNetwork.IsMasterClient) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * normalSpeed;

        // 同步位置给其他客户端
        photonView.RPC("SyncPosition", RpcTarget.Others, transform.position);
    }

    [PunRPC]
    void SyncPosition(Vector3 newPosition)
    {
        if (!photonView.IsMine)
        {
            transform.position = newPosition;
        }
    }

    private void CheckChargeCondition()
    {
        // 检查冷却和距离条件
        if (Time.time > lastChargeTime + chargeCooldown &&
            Vector2.Distance(transform.position, target.position) <= chargeDistance)
        {
            StartCharge();
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        lastChargeTime = Time.time;
        chargeDirection = (target.position - transform.position).normalized;

        // 设置冲锋速度
        rb.velocity = chargeDirection * chargeSpeed;

        // 冲锋结束后恢复
        Invoke(nameof(EndCharge), chargeDuration);
    }

    private void EndCharge()
    {
        isCharging = false;
        rb.velocity = Vector2.zero;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine || !isCharging) return;

        // 冲锋状态下撞击玩家造成额外伤害
        if (collision.gameObject.CompareTag("Player"))
        {
            BigPlayer playerHealth = collision.gameObject.GetComponent<BigPlayer>();
            SmallPlayer smalplayer = collision.gameObject.GetComponent<SmallPlayer>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(chargeDamage);
                dead();
            }
            if (smalplayer != null)
            {
                smalplayer.TakeDamage(chargeDamage);
                dead();
            }
        }
    }
}
