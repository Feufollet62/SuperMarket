using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script
{
    public class FallingPotion : MonoBehaviour
    {
        [SerializeField] GameObject potionsFall;
        [SerializeField] PlayerController Player;
        //private GameObject Player;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Je capte un truc");
            if (other.CompareTag("armoire") && Player._dashing)
            {
                Debug.Log("Je fait tomber ma potion");
                Instantiate(potionsFall, other.gameObject.transform.GetChild(1).position, potionsFall.transform.rotation);
            }
        }
    }
}
