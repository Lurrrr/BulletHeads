using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
   [Header("检测参数")]
   public Vector2 bottomOffest;

   public Vector2 leftOffest;
   
   public Vector2 rightOffest;
   
   public float checkRaduis;
   
   public LayerMask goundLayer;
   
  
   
   [Header("状态")]
   public bool isGround;

   public bool touchLeftWall;
   
   public bool touchRightWall;
   
  
   
   private void Update()
   {
      Check();
      
   }
   
   public void Check()
   {
      //检测地面
      isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffest, checkRaduis, goundLayer);
      touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffest, checkRaduis, goundLayer);
      touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffest, checkRaduis, goundLayer);
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffest, checkRaduis);//在辅助线界面画出isGround
      Gizmos.DrawWireSphere((Vector2)transform.position + leftOffest, checkRaduis);
      Gizmos.DrawWireSphere((Vector2)transform.position + rightOffest, checkRaduis);
   }
}
