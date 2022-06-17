using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusionable : MonoBehaviour
{
    #region Variables

    //liste possible de fusion (scritableObject)
    [SerializeField] FusionData[] fusions;
    //liste de position (plus pratique pour l'anim et pour rechopper l'objet suplementaire)
    [SerializeField] Transform posObjet1;
    [SerializeField] Transform posObjet2;
    [SerializeField] Transform posRejet;
    //temps de conception
    [SerializeField] float timeToReceip;
    //Prend les objet poser permettant de regarder son id
    [SerializeField] List<GameObject> objectOnLabo;

    //condition pour :
    //savoir si la premiere place est prise
    private bool pos1 = false;
    //si l'objet est en construction
    private bool useFabric = false;
    //verif si mes objet son dans ma collection
    private bool item1Verif = false;
    private bool item2Verif = false;

    //creation d'objet pour l'anim (et l'appeler dans d'autre fonction)
    private GameObject Item1;
    private GameObject Item2;
    private GameObject newItem;

    #endregion

    #region Custom Function

    private void OnTriggerEnter(Collider other)
    {
        //si mon objet est de type objet 
        //que la fabrication n'est pas en cours
        //et si les objet sont craftable
        if (other.gameObject.CompareTag("Object") && !useFabric && other.gameObject.GetComponent<ObjectDefinition>().dataObject.craftable)
        {
            //si ma 1ere place n'est pas prise
            if (!pos1)
            {
                //ma place une est prise
                pos1 = true;
                //j'ajoute un objet dans ma liste
                objectOnLabo.Add(other.gameObject);
                //destruction de ma liste
                Destroy(other.gameObject);
                //creation d'un objet pour animé 
                Item1 = Instantiate(objectOnLabo[0], posObjet1.position, posObjet1.rotation);
            }
            if (pos1)
            {
                //j'ajoute un objet dans ma liste
                objectOnLabo.Add(other.gameObject);
                //destruction de ma liste
                Destroy(other.gameObject);
                //creation d'un objet pour animé 
                Item2 = Instantiate(objectOnLabo[1], posObjet2.position, posObjet2.rotation);
                //on se met en fabrication
                useFabric = true;
                //lancement de la fabrique
                StartCoroutine(FusionObjet());
            }
        }
    }

    IEnumerator FusionObjet()
    {
        //temps de fabrique
        yield return new WaitForSeconds(timeToReceip);

        if (useFabric)
        {
            for (int i = 0; i < fusions.Length; i++)
            {
                //connaitre mon premier objet que je met
                if (Item1.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item1.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    //mon item est bon
                    item1Verif = true;
                }
                //connaitre mon deuxieme objet que je met
                if (Item2.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item2.GetComponent<ObjectDefinition>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    //mon second item est bon
                    item2Verif = true;
                    newItem = Instantiate(fusions[i].objetResult, posRejet.position, posRejet.rotation);
                }
                //pour plus tard pour des objet complexe comme deux objet different
                /*
                //si mes deux item sont bon
                if (item1Verif && item2Verif)
                {
                    //crée l'objet
                    newItem = Instantiate(fusions[i].objetResult, posRejet.position, posRejet.rotation);
                }*/
            }
            //elever tout ce qui as sur mon atelier
            Destroy(Item1);
            Destroy(Item2);
            objectOnLabo.Clear();
        }
        //dire que c'est vide
        pos1 = false;
        useFabric = false;
    }
    #endregion
}
