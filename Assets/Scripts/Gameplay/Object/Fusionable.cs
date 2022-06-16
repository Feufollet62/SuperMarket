using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusionable : MonoBehaviour
{
    [SerializeField] FusionData[] fusions;
    [SerializeField] Transform posObjet1;
    [SerializeField] Transform posObjet2;
    [SerializeField] Transform posRejet;
    [SerializeField] float timeToReceip;
    [SerializeField] List<GameObject> objectOnLabo;

    private bool pos1 = false;
    private bool useFabric = false;
    private bool canFuse = true;

    private GameObject Item1;
    private GameObject Item2;
    private GameObject newItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object") && !useFabric && other.gameObject.GetComponent<ObjectDefinition>().dataObject.craftable)
        {
            if (!pos1)
            {
                objectOnLabo.Add(other.gameObject);
                Destroy(other.gameObject);
                Debug.Log("Pos1Prise");
                Item1 = Instantiate(objectOnLabo[0], posObjet1.position, posObjet1.rotation);
                pos1 = true;
            }
            if (pos1)
            {
                objectOnLabo.Add(other.gameObject);
                Destroy(other.gameObject);
                Debug.Log("Pos2Prise");
                Item2 = Instantiate(objectOnLabo[1], posObjet2.position, posObjet2.rotation);
                useFabric = true;
                StartCoroutine(FusionObjet());
            }
        }
    }
    void Verification()
    {
        if (useFabric)
        {
            for (int i = 0; i < fusions.Length; i++)
            {
                if (Item1.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item1.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    Debug.Log("Premiere Verif Faite");
                    Debug.Log(Item1.GetComponent<ObjectDefinition>().dataObject.iD);
                    
                    Debug.Log(fusions[i].objetFusion1.iD);
                }
                if (Item2.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion1.iD || 
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    Debug.Log("Second Verif Faite");
                    Debug.Log(Item2.GetComponent<ObjectDefinition>().dataObject.iD);
                }/*
                if (Item1.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion1.iD ||
                    Item1.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion2.iD ||
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion1.iD ||
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion2.iD)
                {
                    canFuse = false;
                }*/
            }
            StartCoroutine(FusionObjet());
        }
    }

    IEnumerator FusionObjet()
    {

        yield return new WaitForSeconds(timeToReceip);

        if (useFabric)
        {
            for (int i = 0; i < fusions.Length; i++)
            {
                if (Item1.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item1.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    Debug.Log("Premiere Verif Faite");
                    Debug.Log(Item1.GetComponent<ObjectDefinition>().dataObject.iD);

                    Debug.Log(fusions[i].objetFusion1.iD);
                }
                if (Item2.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    Debug.Log("Second Verif Faite");
                    Debug.Log(Item2.GetComponent<ObjectDefinition>().dataObject.iD);
                    newItem = Instantiate(fusions[i].objetResult, posRejet.position, posRejet.rotation);
                }/*
                if (Item1.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion1.iD ||
                    Item1.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion2.iD ||
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion1.iD ||
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD != fusions[i].objetFusion2.iD)
                {
                    canFuse = false;
                }*/
            }
            //StartCoroutine(FusionObjet());


            Destroy(Item1);
            Destroy(Item2);
            objectOnLabo.Clear();
        }

        //if (canFuse)
        //{
            //newItem = Instantiate(fusions[0].objetResult, posRejet.position, posRejet.rotation);
        //}

        pos1 = false;
        useFabric = false;
    }
}
