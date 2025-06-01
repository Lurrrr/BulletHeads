using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float Speed;
    PhotonView pv;
    // Start is called before the first frame update
    protected void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        if(pv.IsMine)
        {
            StartCoroutine("IEDead");
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }


    protected void move(float Speed)
    {
        rb.velocity = new Vector2(0,Speed);
    }
    

    IEnumerator IEDead()
    {
        yield return new WaitForSeconds(4f);
        PhotonNetwork.Destroy(gameObject);
    }
}
