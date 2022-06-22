using _Script;
using UnityEngine;

public class Flaque : MonoBehaviour
{
    [SerializeField] private float timeBeforeDespawn = 10;

    private void FixedUpdate()
    {
        timeBeforeDespawn -= Time.fixedDeltaTime;
        if(timeBeforeDespawn < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>()._slipping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerController>()._slipping = false;
    }
}
