using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worm : MonoBehaviour
{
    public Character character;
    private Rigidbody2D rb;
    public Vector3 startPosition;
    public Vector3 movePosition;
    public GameObject wormtigger;
    public string currentName ;
    public GameObject wormTrigger;
    [Header("物理参数")] 
    public float upwardForce = 15f;
    public float moveSpeed = 1f;
    public Vector2 newDirection;
    [Header("状态")] 
    public bool isActive; // 导弹是否激活
    public bool isAtStart;
    [Header("边界设置")]
    public float minX = -9f;
    public float maxX = 11f;
    public float minY = -2f;
    public float maxY = 4f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        startPosition = transform.position;
        
        character = GetComponent<Character>();
       
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        isActive = false;
        
        isAtStart = true;

        currentName = gameObject.name;


    }

    void FixedUpdate()
    {
        if (isActive)
        {
           
            // 计算新位置
            Vector2 newPos = rb.position + newDirection.normalized * moveSpeed * Time.fixedDeltaTime;
        
            // 边界反弹检测
            if (newPos.x < minX || newPos.x > maxX)
            {
                newDirection.x *= -1; // X轴反向
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
                Debug.Log($"{currentName} X轴反弹");
            }
        
            if (newPos.y < minY || newPos.y > maxY)
            {
                newDirection.y *= -1; // Y轴反向
                newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
                Debug.Log($"{currentName} Y轴反弹");
            }

            rb.MovePosition(newPos);


        }
    }
    
    
  
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"碰撞对象名称: {other.gameObject.name}");
        // Debug.Log($"碰撞对象层级: {LayerMask.LayerToName(other.gameObject.layer)}");
        // Debug.Log($"自身对象名称: {gameObject.name}");
        //

        if (LayerMask.LayerToName(other.gameObject.layer) == "wormTrigger")
        {
            
            if (rb != null)
            {
               
                //Debug.Log("虫子激活！物理模拟已开启");
                wormtigger worm = wormtigger.GetComponent<wormtigger>();// 假设脚本类名为Worm
                movePosition= worm.movePosition;
                newDirection= worm.newDirection;
                // 根据名称执行顺序延迟
                if (currentName == "worm1") 
                {
                    StartCoroutine(DelayedMove(1f)); // worm1延迟1秒
                    
                    Debug.Log("名称是"+currentName);
                }
                
                if (currentName == "worm2") 
                {
                    StartCoroutine(DelayedMove(2f)); // worm1延迟1秒
                    
                    Debug.Log("名称是"+currentName);
                }
                if (currentName == "worm3") 
                {
                    StartCoroutine(DelayedMove(3f)); // worm1延迟1秒
                    
                    Debug.Log("名称是"+currentName);
                }
                if (currentName == "worm4") 
                {
                    StartCoroutine(DelayedMove(4f)); // worm1延迟1秒
                    
                    Debug.Log("名称是"+currentName);
                }
                if (currentName == "worm5") 
                {
                    StartCoroutine(DelayedMove(5f)); // worm1延迟1秒
                    
                    Debug.Log("名称是"+currentName);
                }
                 if (currentName == "worm6") 
                {
                    StartCoroutine(DelayedMove(6f)); 
                   
                    Debug.Log("名称是"+currentName);
                }
               

               
                
              
                
            }

        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "player")
        {
            /*Debug.Log("识别到了玩家");*/
            rb.MovePosition(startPosition);
            character.currentHealth = character.maxHealth;
            transform.position = startPosition;
            
            /*Debug.Log("导弹关闭！物理模拟已关闭");*/
            isActive = false;
            isAtStart = true;

        }

        if (LayerMask.LayerToName(other.gameObject.layer) == "platform")
        {
            /*Debug.Log("识别到了地面");*/
            rb.MovePosition(startPosition);
            character.currentHealth = character.maxHealth;
            transform.position = startPosition;
           
            /*Debug.Log("导弹关闭！物理模拟已关闭");*/
            isActive = false;
            isAtStart = true;
        }
    }
    
    // 协程实现延迟移动
    IEnumerator DelayedMove(float delayTime) 
    {
        // 第一阶段：立即冻结移动
        isActive = false;
        yield return new WaitForSeconds(delayTime); // 独立延迟计时
    
        // 第二阶段：精确瞬移
        rb.MovePosition(movePosition);
        yield return new WaitForFixedUpdate(); // 关键：确保物理系统同步
    
        // 第三阶段：激活持续移动
        isActive = true;
        Debug.Log($"{currentName} 瞬移完成于 {Time.time}");
        StartCoroutine(ScaleUpOverTime(1f)); 
    }
    
    IEnumerator ScaleUpOverTime(float duration) 
    {
        float timer = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        while (timer < duration)
        {
            // 线性插值计算当前scale
            transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale; // 确保最终scale为1
    }
}