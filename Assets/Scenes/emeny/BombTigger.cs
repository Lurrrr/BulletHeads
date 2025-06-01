using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombtigger : MonoBehaviour
{
    [Header("基本参数")]
    public BoxCollider2D boxCollider;
    public float timer;
    public float counter;
    public bool isActive;

    void Start()
    {
        // 获取或添加BoxCollider2D组件
        boxCollider = GetComponent<BoxCollider2D>();
        
            isActive = false;
            timer = 0f;
        
        
    }

    void Update()
    {
        // 计时器逻辑
        timer += Time.deltaTime;
        
        if (timer >= counter)
        {
            // 切换碰撞器状态
            isActive = true;
            boxCollider.enabled = isActive;
            
            //Debug.Log("box启动了");
            Invoke("ResetTimer", 0.5f); 
           
        }
        else
        {
            isActive = false;
            boxCollider.enabled = isActive;
            
           // Debug.Log("box关闭了");
        }
    }

    void ResetTimer()
    {
        timer = 0;
    }

}
