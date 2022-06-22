using UnityEngine;

namespace _Script
{
    public class SpawnOnBonk : MonoBehaviour
    {
        [SerializeField] private GameObject spawnedPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private void OnCollisionEnter(Collision collision)
        {
            // Si c'est pas un joueur osef
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            
            // Si le joueur ne dash pas
            if(!collision.gameObject.GetComponent<PlayerController>()._dashing) return;
            
            Transform closestPoint = spawnPoints[0];

            if (spawnPoints.Length > 1)
            {
                // On cherche le point de spawn le plus proche du point de contact
                float closestLength = Vector3.Distance(collision.GetContact(0).point, closestPoint.position);
                    
                for (int i = 1; i < spawnPoints.Length; i++)
                {
                    float thisLength = Vector3.Distance(collision.GetContact(0).point, spawnPoints[i].position);

                    if (thisLength < closestLength)
                    {
                        closestPoint = spawnPoints[i];
                    }
                }
            }

            Instantiate(spawnedPrefab, closestPoint);
        }
    }
}
