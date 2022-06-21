using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Script
{
    public class IdVerif : MonoBehaviour
    {
        private GameManager gM;

        private void Awake()
        {
            gM = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Object"))
            {
                VerificationID(other.gameObject.GetComponent<Interactible>());
            }
        }
        private void VerificationID(Interactible interactible)
        {
            for (int i = 0; i < gM.clientList.Count; i++)
            {
                if (gM.clientList[i].idCommande == interactible.data.iD)
                {
                    gM.score++;
                    gM.clientList[i].ExitQueue();
                }
            }
        }
        
    }
}
