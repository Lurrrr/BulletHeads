using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireRate_Prop : Props
{


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        addon = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        follow();

    }



    protected override void Function(Collider2D collider)
    {
        if (!eated)
        {
            BigPlayer bigplayer = collider.gameObject?.GetComponent<BigPlayer>();
            SmallPlayer smallplayer = collider.gameObject?.GetComponent<SmallPlayer>();

            if (bigplayer != null)
            {
                //Debug.Log($"吃到前firerate: {bigplayer.FireRate}");
                bigplayer.FireRate = bigplayer.FireRate + addon;
                //Debug.Log($"吃到后firerate: {bigplayer.FireRate}");
                eated = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
            if (smallplayer != null)
            {
                // Debug.Log($"吃到前firerate: {smallplayer.FireRate}");
                smallplayer.FireRate = smallplayer.FireRate + addon;
                //Debug.Log($"吃到后firerate: {smallplayer.FireRate}");
                eated = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }

    }

    protected override void Strengthen()
    {
        base.Strengthen();
        selfpv.RPC("PRCStrengthen",RpcTarget.All);
        //addon += addon * 1.5f;

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        //使用道具
        if (collision.transform.tag == "Player")
        {
            Function(collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //抓取道具
        if (collision.gameObject.CompareTag("Player"))
        {
            // 获取玩家Transform
            playerTransform = collision.transform;
            playerpv = collision.gameObject.GetComponent<PhotonView>();
            //获取丢置位置
            if (collision.transform.name == "BigCharacter(Clone)")
            {
                throwTarget = GameObject.Find("buttom").transform;
            }
            else
            {
                throwTarget = GameObject.Find("top").transform;

            }

            // 停止物理模拟
            rb.isKinematic = true;
            circleCollider.enabled = false;
            rb.velocity = Vector3.zero;

            // 开始跟随
            shouldFollow = true;
        }


    }

    [PunRPC]

    private void PRCStrengthen()
    {
        addon += addon * 1.5f;
    }

}
