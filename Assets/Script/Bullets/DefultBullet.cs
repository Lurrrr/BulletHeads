using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefultBullet : Bullet
{
    // Start is called before the first frame update
    new
    void Start()
    {
        base.Start();
        //设置子弹参数
        Speed = 3f;

    }

    new
    // Update is called once per frame
    void Update()
    {
        move(Speed);
    }
}
