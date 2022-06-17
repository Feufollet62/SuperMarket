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
            Debug.Log("Je capte mon joueur");
            if (Player.GetComponent<PlayerController>()._inputDash)
            {
                Debug.Log("Je fait ma potion");
                Instantiate(potionsFall, posInitial);
            }
            /*
            PlayerController player = GetComponent<PlayerController>();
            if (player._inputDash)
            {
                Instantiate(potionsFall, posInitial);
            }*/

            
        }
    }
}
