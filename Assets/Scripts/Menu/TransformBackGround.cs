using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBackGround : MonoBehaviour
{
    public Vector3 rotate;
    void Update()
    {
        transform.Translate(rotate * Time.deltaTime);
    }
}
