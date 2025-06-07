using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("传送设置")]
    [SerializeField] private Transform topExit;    // 上方出口
    [SerializeField] private Transform bottomExit; // 下方出口
    [SerializeField] private float teleportCooldown = 0.5f; // 传送冷却时间
    [SerializeField] protected float smoothTime = 0.3f;     // 平滑跟随时间
    public Vector2 currentVelocity;

    private float lastTeleportTime;
    private Collider2D portalCollider;

    void Awake()
    {
        portalCollider = GetComponent<Collider2D>();
        lastTeleportTime = -teleportCooldown; // 确保初始可以传送
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        // 冷却检查
        //if (Time.time < lastTeleportTime + teleportCooldown) return;

        // 忽略触发器
       // if (other.isTrigger) return;

        if(other.tag == "Props")
        {
            if (!other.GetComponent<Props>().isFollow)
            {
                // 确定进入方向
                Vector2 entryDirection = GetEntryDirection(other.transform.position);

                // 选择出口
                Transform exitPoint = entryDirection.y > 0 ? bottomExit : topExit;
                Debug.Log(exitPoint);

                // 执行传送
                TeleportObject(other.gameObject, exitPoint.position);
                // 可选: 添加传送后的小推力
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                Vector2 velocity = rb.velocity;

                rb.gravityScale = 9f;

                // 更新冷却时间
                lastTeleportTime = Time.time;

            }

        }

    }

    Vector2 GetEntryDirection(Vector2 entryPosition)
    {
        // 计算进入点相对于传送门中心的位置向量
        Vector2 relativePosition = entryPosition - (Vector2)transform.position;

        // 标准化方向(主要判断上下)
        return relativePosition.normalized;
    }

    void TeleportObject(GameObject obj, Vector2 targetPosition)
    {

        // 传送对象
        obj.transform.position =  Vector2.SmoothDamp(
                targetPosition,
                targetPosition,
                ref currentVelocity,
                smoothTime,3f);

        // 对物理对象处理
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 保持原有速度但反转Y方向
            Vector2 velocity = rb.velocity;
            velocity.y *= -0.8f; // 稍微减少垂直速度防止震荡
            rb.velocity = velocity;


        }

    }


    // 可视化辅助
    void OnDrawGizmos()
    {
        if (topExit != null && bottomExit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, topExit.position);
            Gizmos.DrawWireCube(topExit.position, Vector3.one * 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, bottomExit.position);
            Gizmos.DrawWireCube(bottomExit.position, Vector3.one * 0.5f);
        }
    }
}
