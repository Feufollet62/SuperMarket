using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Client"))
        {
            Debug.Log("Je veux destroy");
            Destroy(other.gameObject);
        }
    }
}
