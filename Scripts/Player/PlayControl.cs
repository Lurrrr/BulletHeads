using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControl : MonoBehaviour
{
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        physicsCheck = GetComponent<PhysicsCheck>();
    }
}
