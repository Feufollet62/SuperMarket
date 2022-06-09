using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClient : MonoBehaviour
{
    public CapsuleCollider ClientCollider;
    public CapsuleCollider ClientBlockCollider;
    void Start()
    {
        Physics.IgnoreCollision(ClientCollider, ClientBlockCollider,true);
        
    }

}
