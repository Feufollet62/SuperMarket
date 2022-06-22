using UnityEngine;

public class PotionFallingPrefab : MonoBehaviour
{
    [SerializeField] private GameObject flaquePrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            ContactPoint contact = collision.GetContact(0);
            
            GameObject flaque = Instantiate(flaquePrefab, contact.point, Quaternion.identity);
            flaque.transform.up = collision.GetContact(0).normal;

            Destroy(gameObject);
        }
    }
}
