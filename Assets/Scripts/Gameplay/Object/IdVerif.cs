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
            Debug.Log("Ya un truc ?");
            if (other.gameObject.CompareTag("Object"))
            {
                Debug.Log("Verification en Cours");
                VerificationID(other.gameObject.GetComponent<ObjectDefinition>());
            }
        }
        void VerificationID(ObjectDefinition ObjectComptoire)
        {
            Debug.Log("L'ID de mon Objet = " + ObjectComptoire.iD);
            Debug.Log("L'ID du Clients 1 = " + clientsWait[0].iDaleatoire);
            Debug.Log("L'ID du Clients 2 = " + clientsWait[1].iDaleatoire);
            Debug.Log("L'ID du Clients 3 = " + clientsWait[2].iDaleatoire);

            for (int i = 0; i < clientsWait.Count; i++)
            {
                if (clientsWait[i].iDaleatoire == ObjectComptoire.iD)
                {
                    Debug.Log("tabon pour le " + i);
                    //clientsWait[i].iDEgal = true;
                    //Debug.Log("etat du client " + clientsWait[i].iDEgal);
                    clientsWait[i].InExitQueue();
                }
                else
                {
                    Debug.Log("ya pas en file");
                }
            }
        }
        
    }
}
