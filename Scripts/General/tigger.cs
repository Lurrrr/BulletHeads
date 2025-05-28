using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tigger : MonoBehaviour
{
    public Character character;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    public CapsuleCollider2D capsuleCollider;
    public PolygonCollider2D polygonCollider;
    public Vector3 startposition;
    public Vector3 position;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        character=GetComponent<Character>();
        startposition = transform.position; 
        
        boxCollider.enabled = true;
        polygonCollider.enabled = false;
        capsuleCollider.enabled = false;
    }
    
    private void OnTriggerStay2D(Collider2D other) //other是被攻击的人
    {
        ;
       
            // 方法2：输出Layer名称（推荐）
            Debug.Log($"检测到对象：{other.name} | Layer名称：{LayerMask.LayerToName(other.gameObject.layer)}");

            // 检查是否为Missile层
            if (LayerMask.LayerToName(other.gameObject.layer) == "missile trigger")
            {

                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    boxCollider.enabled = false;
                    polygonCollider.enabled = true;
                    capsuleCollider.enabled = false;
                }
            }

            if (LayerMask.LayerToName(other.gameObject.layer) == "player")
            {

                if (rb != null)
                {
                    position = transform.position; 
                    Vector3 playerPosition = other.gameObject.transform.position;
                    
                    // 计算方向向量（保持Y轴为0，纯2D平面计算）
                    Vector2 toPlayer = (playerPosition - position).normalized;
                    Vector2 missileForward = transform.right; // 假设导弹朝X轴正方向
    
                    // 计算带符号角度（-180°~180°）
                    float angle = Vector2.SignedAngle(missileForward, toPlayer);
    
                    // 限制最大偏转角度（±15度）
                    float clampedAngle = Mathf.Clamp(angle, -15f, 15f);
    
                    // 施加旋转力（基于扭矩更符合物理规律）
                    float torque = clampedAngle * rb.mass * 0.5f; // 0.5f为灵敏度系数
                    rb.AddTorque(torque, ForceMode2D.Force);
                    ExecuteColliderLogic();
                
                }
                Invoke("ExecuteColliderLogic", 3f); // 3秒后强制执行
            }

            string layerName = LayerMask.LayerToName(other.gameObject.layer);
            if (layerName == "player" || layerName == "platform") 
            {
            
                boxCollider.enabled = true;
                polygonCollider.enabled = false;
                capsuleCollider.enabled = false;
                transform.position=startposition;
                character.currentHealth = character.maxHealth;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        
        }
    
    
    void ExecuteColliderLogic() 
    {
        boxCollider.enabled = false;
        polygonCollider.enabled = true;
        capsuleCollider.enabled = false;
        
    }
}