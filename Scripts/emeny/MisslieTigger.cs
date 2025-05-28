using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisslieTigger : MonoBehaviour
{
    [Header("基本参数")]
    public float moveSpeed = 5f;    // 基础移动速度
    public float minX = -36f;       // 左边界
    public float maxX = -15f;       // 右边界

    private Rigidbody2D rb;
    private bool isMovingLeft = true; // 当前是否向左移动

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 关闭重力
    }

    void FixedUpdate()
    {
        // 1. 持续移动（根据方向决定左右）
        float currentDirection = isMovingLeft ? -1 : 1;
        rb.velocity = new Vector2(currentDirection * moveSpeed, 0);

        // 2. 边界检测（通俗易懂版）
        if (transform.position.x < minX) // 碰到左边界
        {
            isMovingLeft = false; // 改为向右
            //Debug.Log("碰到左墙，掉头！");
        }
        else if (transform.position.x > maxX) // 碰到右边界
        {
            isMovingLeft = true; // 改为向左
            //Debug.Log("碰到右墙，掉头！");
        }

        // 3. 强制位置修正（防止卡墙）
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector2(clampedX, transform.position.y);
    }
    
    
   
}
