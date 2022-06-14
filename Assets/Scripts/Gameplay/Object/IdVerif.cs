using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script
{
    public class IdVerif : MonoBehaviour
    {
        [SerializeField] public List<ClientController> clientsWait;

        void VerificationID(ObjectDefinition ObjectComptoire)
        {
            if (clientsWait[0].iDaleatoire == ObjectComptoire.iD)
            {
                Debug.Log("tabon");
                clientsWait[0].iDEgal = true;
            }
            if (clientsWait[1].iDaleatoire == ObjectComptoire.iD)
            {
                Debug.Log("tabon");
                clientsWait[1].iDEgal = true;
            }
            if (clientsWait[2].iDaleatoire == ObjectComptoire.iD)
            {
                Debug.Log("tabon");
                clientsWait[2].iDEgal = true;
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Object")
            {
                Destroy(other.gameObject);
                Debug.Log("Verification en Cours");
                VerificationID(other.gameObject.GetComponent<ObjectDefinition>());
            }
        }
    }
}
