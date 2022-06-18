using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFallingPrefab : MonoBehaviour
{
    [SerializeField] float timeFall;
    [SerializeField] GameObject flaque;
    void Start()
    {
        StartCoroutine(Falling());
    }

    IEnumerator Falling()
    {
        Debug.Log("Je tombe");
        yield return new WaitForSeconds(timeFall);

        Vector3 posFlaque = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 0.35f, this.gameObject.transform.position.z);
        Instantiate(flaque, posFlaque, this.gameObject.transform.rotation);
        Debug.Log("Je casse");
    }
}
