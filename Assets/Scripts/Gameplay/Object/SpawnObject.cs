using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject objet;
    [SerializeField] Transform spawnerObject;

    public void SpawnObjectAction()
    {
        Instantiate(objet,spawnerObject.position, spawnerObject.rotation);
    }
}
