using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script
{
    public class IdVerif : MonoBehaviour
    {
        [SerializeField] public List<ClientController> clientsWait;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Object"))
            {
                VerificationID(other.gameObject.GetComponent<ObjectDefinition>());
            }
        }
        void VerificationID(ObjectDefinition ObjectComptoire)
        {

            for (int i = 0; i < clientsWait.Count; i++)
            {
                if (clientsWait[i].iDaleatoire == ObjectComptoire.iD)
                {
                    clientsWait[i].InExitQueue();
                }
            }
        }
        
    }
}
