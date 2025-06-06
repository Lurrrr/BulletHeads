using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ZigZagEnemy : EnemyCharacter
{
    [Header("Movement Settings")]
    [Tooltip("水平移动速度")]
    [SerializeField] private float horizontalSpeed = 4f;  // 较快的水平速度

    [Tooltip("垂直下落速度")]
    [SerializeField] private float verticalSpeed = 0.5f;    // 较慢的垂直速度

    [Tooltip("Z字形移动的宽度")]
    [SerializeField] private float zigzagWidth = 3f;

    [Tooltip("改变方向的间隔时间")]
    [SerializeField] private float directionChangeInterval = 1f;

    [Header("Gizmos")]
    [SerializeField] private bool showDebugPath = true;


    [SerializeField] private float damage = 10;

    private float timer;
    private bool movingRight = true;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;

    }

    void Update()
    {
        FlipSprite();
        //获得开枪点
        FirePosition = transform.Find("FirePosition");
        Fire();
        timer += Time.deltaTime;


        // 独立控制水平和垂直速度
        Vector2 movement = Vector2.zero;
        movement.x = (movingRight ? horizontalSpeed : -horizontalSpeed) * Time.deltaTime;
        movement.y = -verticalSpeed * Time.deltaTime;  // 恒定下落速度

        transform.Translate(movement);

        // 方向改变逻辑
        if (timer >= directionChangeInterval)
        {
            timer = 0f;
            movingRight = !movingRight;
            FlipSprite(); // 调用翻转函数

        }

        // 边界检查 - 基于起始位置的相对位置
        float currentXOffset = transform.position.x - startPosition.x;
        if (Mathf.Abs(currentXOffset) > zigzagWidth)
        {
            movingRight = !movingRight;
            FlipSprite(); // 调用翻转函数

            // 确保不会超出边界
            float clampedX = startPosition.x + (movingRight ? -zigzagWidth : zigzagWidth);
            transform.position = new Vector2(clampedX, transform.position.y);
        }
    }

    void OnDrawGizmos()
    {
        if (showDebugPath && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Vector2 leftBound = startPosition + Vector2.left * zigzagWidth;
            Vector2 rightBound = startPosition + Vector2.right * zigzagWidth;
            Gizmos.DrawLine(leftBound, rightBound);
            Gizmos.DrawWireSphere(leftBound, 0.2f);
            Gizmos.DrawWireSphere(rightBound, 0.2f);
        }
    }

    private void FlipSprite()
    {
        // 如果朝右，localScale.x 为正；朝左则为负
        Vector3 newScale = transform.localScale;
        newScale.x = movingRight ? -Mathf.Abs(newScale.x) : Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            Character bigcharacter = collision.GetComponent<BigPlayer>();
            Character smallplayer = collision.GetComponent<SmallPlayer>();


            if (bigcharacter != null)
            {
                bigcharacter.TakeDamage(damage);

            }
            if (smallplayer != null)
            {
                smallplayer.TakeDamage(damage);

            }
        }
    }
}
