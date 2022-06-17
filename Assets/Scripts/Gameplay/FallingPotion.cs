using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPotion : MonoBehaviour
{
    [SerializeField] GameObject potionsFall;
    [SerializeField] Transform posInitial;
    private GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Je capte un truc");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player = other.gameObject;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            Debug.Log("Je capte mon joueur");
            if (player._dashing == true)
            {
                Debug.Log("Je fait tomber ma potion");
                Instantiate(potionsFall, posInitial);
            }
        }
    }
}
