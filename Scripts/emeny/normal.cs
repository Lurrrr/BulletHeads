using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal : MonoBehaviour
{
    public Character character;
    private Rigidbody2D rb;
    public Vector3 startPosition;
    public Vector3 movePosition;
    public ForceMode2D forceMode = ForceMode2D.Force;

    [Header("物理参数")] 
    public float upwardForce ; // 向上的力大小（建议15-25）
    public float rightForce ;    // 持续向右的力
    public float leftForce ;     // 到达x=20时施加的向左的力
    public float targetX ;     // 触发向左力的X坐标
    public float targetX2 ;
    
    
    [Header("状态")] 
    private bool isActive = true; // 导弹是否激活

    public bool isRight;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("缺失Rigidbody2D组件");
        startPosition = transform.position;
        
        character = GetComponent<Character>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        isActive = false;
        isRight = false;
       
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            // 持续施加向上的力（抵消下落速度）
            rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Force);
           

            // 3. 检测是否到达目标X坐标且未施加过向左的力
            if (isRight == false)
            {
                // 2. 持续施加向右的力
                rb.AddForce(Vector2.right * rightForce, ForceMode2D.Force);

                if (transform.position.x >= targetX )
                {
                    isRight = true;
                    
               
                }
            }

            if (isRight == true)
            {
                // 施加一个向左的力
                rb.AddForce(Vector2.left * leftForce, ForceMode2D.Force);
                
                Debug.Log("已到达x=20，施加向左的力");
            }
            if (transform.position.x <= targetX2&&isRight == true)
            {
                rb.MovePosition(startPosition);
                character.currentHealth = character.maxHealth;
                transform.position = startPosition;
                rb.bodyType = RigidbodyType2D.Kinematic;
                isActive = false;
                isRight = false;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*Debug.Log($"碰撞对象名称: {other.gameObject.name}");
        Debug.Log($"碰撞对象层级: {LayerMask.LayerToName(other.gameObject.layer)}");
        Debug.Log($"自身对象名称: {gameObject.name}");
        */

        if (LayerMask.LayerToName(other.gameObject.layer) == "normal trigger")
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                
                Vector3 randomOffset = new Vector3(
                    Random.Range(-2f, 2f), // x轴随机偏移（-2到2之间）
                    Random.Range(-2f, 2f) , // y轴随机偏移
                    0
                );
                rb.MovePosition(movePosition + randomOffset);
                
                isActive = true;
                Debug.Log("识别到了normal trigger");
            }

        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "player")
        {
            /*Debug.Log("识别到了玩家");*/
            rb.MovePosition(startPosition);
            character.currentHealth = character.maxHealth;
            transform.position = startPosition;
            rb.bodyType = RigidbodyType2D.Kinematic;
            /*Debug.Log("导弹关闭！物理模拟已关闭");*/
            isActive = false;

        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "platform")
        {
            /*Debug.Log("识别到了地面");*/
            rb.MovePosition(startPosition);
            character.currentHealth = character.maxHealth;
            transform.position = startPosition;
            rb.bodyType = RigidbodyType2D.Kinematic;
            /*Debug.Log("导弹关闭！物理模拟已关闭");*/
            isActive = false;
        }
    }
}