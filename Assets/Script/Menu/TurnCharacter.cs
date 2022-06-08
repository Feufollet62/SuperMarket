using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCharacter : MonoBehaviour
{
    public Vector3 rotate;
    void Update()
    {
        transform.Rotate(rotate * Time.deltaTime);
    }
}
