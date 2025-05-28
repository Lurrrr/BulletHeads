using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormtigger : MonoBehaviour
{
    public Vector3 movePosition;
    public GameObject worm1;
    public GameObject worm2;
    public GameObject worm3;
    public GameObject worm4;
    public GameObject worm5;
    public GameObject worm6;
    
    [Header("基本参数")]
    public BoxCollider2D boxCollider;
    public float timer;
    public float counter;
    public bool isActive;
    [Header("方向参数")]
    public float minAngle = 0f;    // 最小角度（度）
    public float maxAngle = 360f;  // 最大角度（度）
    public Vector2 newDirection;
   
    
    void Start()
    {
        // 获取或添加BoxCollider2D组件
        boxCollider = GetComponent<BoxCollider2D>();
        
        isActive = false;
        timer = 0f;
        
        // 初始状态设为关闭
        boxCollider.enabled = false;
    }

    void Update()
    {
        
        if (worm1.GetComponent<worm>().isAtStart 
            && worm2.GetComponent<worm>().isAtStart
            && worm3.GetComponent<worm>().isAtStart
            && worm4.GetComponent<worm>().isAtStart
            && worm5.GetComponent<worm>().isAtStart
            && worm6.GetComponent<worm>().isAtStart)
        {
          // Debug.Log("执行isAtStart进来了"); // 计时器逻辑
            timer += Time.deltaTime;
            if (timer >= counter)
            {
                // 生成随机角度（弧度制）
                float randomAngle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;

                // 转换为单位方向向量
                newDirection = new Vector2(
                    Mathf.Cos(randomAngle),
                    Mathf.Sin(randomAngle)
                    
                ).normalized;
                
                
                isActive = true;
                boxCollider.enabled = isActive;
                movePosition= new Vector3(
                    Random.Range(-8.8f, 7f),
                    Random.Range(0f, 4f),
                    0f
                );
            
                Debug.Log("box启动了");
                Invoke("ResetTimer", 0.5f); 
           
            }
            else
            {
                isActive = false;
                boxCollider.enabled = isActive;
            
                Debug.Log("box关闭了");
            }
        }
    }

    



    void ResetTimer()
    {
        timer = 0;
    }
}
