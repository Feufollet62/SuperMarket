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


        flaque.SetActive(true);
        Debug.Log("Je casse");
    }
}
