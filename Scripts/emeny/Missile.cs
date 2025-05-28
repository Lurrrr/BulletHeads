using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Character character;
    private Rigidbody2D rb;
    public Vector3 startPosition;
    public Vector3 movePosition;
    public ForceMode2D forceMode = ForceMode2D.Force;

    [Header("物理参数")] public float upwardForce = 15f; // 向上的力大小（建议15-25）

    [Header("状态")] private bool isActive = true; // 导弹是否激活

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("缺失Rigidbody2D组件");
        startPosition = transform.position;
        movePosition = new Vector3(startPosition.x + 25f, startPosition.y, startPosition.z);
        character = GetComponent<Character>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        isActive = false;
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            // 持续施加向上的力（抵消下落速度）
            rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Force);


        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*Debug.Log($"碰撞对象名称: {other.gameObject.name}");
        Debug.Log($"碰撞对象层级: {LayerMask.LayerToName(other.gameObject.layer)}");
        Debug.Log($"自身对象名称: {gameObject.name}");
        */

        if (LayerMask.LayerToName(other.gameObject.layer) == "missile trigger")
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                Debug.Log("导弹激活！物理模拟已开启");
                rb.MovePosition(movePosition);
                isActive = true;
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
    

    


