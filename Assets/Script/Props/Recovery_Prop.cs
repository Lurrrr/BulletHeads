using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Recovery_Prop : Props
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        addon = 0.2f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        follow();

    }

    protected override void Strengthen()
    {
        base.Strengthen();
        selfpv.RPC("PRCStrengthen", RpcTarget.All);
        //addon += addon * 2f;
    }

    protected override void Function(Collider2D collider)
    {
        base.Function(collider);
        if (!eated)
        {
            BigPlayer bigplayer = collider.gameObject?.GetComponent<BigPlayer>();
            SmallPlayer smallplayer = collider.gameObject?.GetComponent<SmallPlayer>();

            if (bigplayer != null)
            {
                //Debug.Log($"吃到前HP: {bigplayer.HP}");
                bigplayer.HP = bigplayer.HP *(1+addon);
                //Debug.Log($"吃到后HP: {bigplayer.HP}");
                eated = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
            if (smallplayer != null)
            {
                // Debug.Log($"吃到前HP: {smallplayer.HP}");
                bigplayer.HP = bigplayer.HP * (1 + addon);
                //Debug.Log($"吃到后HP: {smallplayer.HP}");
                eated = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
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
        addon += addon * 2f;
    }

}
