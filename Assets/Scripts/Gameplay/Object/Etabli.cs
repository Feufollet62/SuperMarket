using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etabli : MonoBehaviour
{
    #region Variables

    int[,] dataarrey = new int[,] {  { 17,14 }, { 19, 16}, { 18, 15} };

    // Recettes possibles sur cet etabli
    [SerializeField] private FusionData[] fusions;
    [SerializeField] private Interactible interactible;

    // Positions de placement des objets
    [SerializeField] private Transform posObjet1;
    [SerializeField] private Transform posObjet2;
    [SerializeField] private Transform posRejet;
    
    // Temps de craft
    [SerializeField] private float timeToCraft;
    
    // Prend les objet posés permettant de regarder son id
    [SerializeField] private List<GameObject> objetsSurEtabli;

    // Conditions pour :
    
    // Savoir si la première place est prise
    private bool pos1 = false;
    
    // Si l'objet est en cours de construction
    private bool currentlyCrafting = false;
    
    // Verif si mes objet sont dans ma collection
    private bool item1Verif = false;
    private bool item2Verif = false;

    // Creation d'objet pour l'anim (et l'appeler dans d'autre fonction)
    private GameObject Item1;
    private GameObject Item2;
    private GameObject newItem;

    #endregion

    #region Build-in Function
    #endregion

    #region Custom Function

    private void OnTriggerEnter(Collider other)
    {
        // Si mon objet est de type objet 
        // Que la fabrication n'est pas en cours
        // Et si les objet sont craftable
        /*
        if (other.gameObject.CompareTag("Object") && !currentlyCrafting /*&& other.gameObject.GetComponent<Interactible>().dataObject.craftable/)
        {
            //si ma 1ere place n'est pas prise
            if (!pos1)
            {
                //ma place une est prise
                pos1 = true;
                other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;

                

                //j'ajoute un objet dans ma liste
                objetsSurEtabli.Add(other.gameObject);
                //destruction de ma liste
                Destroy(other.gameObject);
                //creation d'un objet pour animé 
                Item1 = Instantiate(objetsSurEtabli[0], posObjet1.position, posObjet1.rotation);
            }
            if (pos1)
            {
                other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                //j'ajoute un objet dans ma liste
                objetsSurEtabli.Add(other.gameObject);
                //destruction de ma liste
                Destroy(other.gameObject);
                //creation d'un objet pour animé 
                Item2 = Instantiate(objetsSurEtabli[1], posObjet2.position, posObjet2.rotation);
                //on se met en fabrication
                currentlyCrafting = true;
                //lancement de la fabrique
                StartCoroutine(FusionObjet());
            }
        }*/
        if (other.gameObject.CompareTag("Object") && !currentlyCrafting && other.gameObject.GetComponent<Interactible>().dataObject.craftable)
        {
            if (!pos1 && other.gameObject.GetComponent<Interactible>().dataObject.weapon)
            {
                //ma place une est prise
                pos1 = true;
                other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                //j'ajoute un objet dans ma liste
                objetsSurEtabli.Add(other.gameObject);
                //destruction de ma liste
                Destroy(other.gameObject);
                //creation d'un objet pour animé 
                Item1 = Instantiate(objetsSurEtabli[0], posObjet1.position, posObjet1.rotation);
            }
            if (pos1 && !other.gameObject.GetComponent<Interactible>().dataObject.weapon)
            {
                other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                //j'ajoute un objet dans ma liste
                objetsSurEtabli.Add(other.gameObject);
                //destruction de ma liste
                Destroy(other.gameObject);
                //creation d'un objet pour animé 
                Item2 = Instantiate(objetsSurEtabli[1], posObjet2.position, posObjet2.rotation);
                //on se met en fabrication
                currentlyCrafting = true;
                //lancement de la fabrique
                StartCoroutine(FusionObjet());
            }
        }
    }

    IEnumerator FusionObjet()
    {
        //temps de fabrique
        yield return new WaitForSeconds(timeToCraft);

        if (currentlyCrafting)
        {
            Debug.Log(Item1.GetComponent<Interactible>().dataObject.iD);
            Debug.Log(Item2.GetComponent<Interactible>().dataObject.iD);
            Debug.Log(dataarrey[Item1.GetComponent<Interactible>().dataObject.iD, Item2.GetComponent<Interactible>().dataObject.iD]);
            /*for (int i = 0; i < fusions.Length; i++)
            {
                //connaitre mon premier objet que je met
                if (Item1.GetComponent<Interactible>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item1.GetComponent<Interactible>().dataObject.iD == fusions[i].objetFusion2.iD)
                {
                    switch (dataarrey)
                    {
                        //case Item1.GetComponent<Interactible>().dataObject.iD = 4:
                            
                        default:
                            break;
                    }
                    ;
                    //mon item est bon
                    item1Verif = true;
                }
                //connaitre mon deuxieme objet que je met
                if (Item2.GetComponent<Interactible>().dataObject.iD == fusions[i].objetFusion1.iD ||
                    Item2.GetComponent<Interactible>().dataObject.iD == fusions[i].objetFusion2.iD)
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
                }/
            }*/
            //elever tout ce qui as sur mon atelier
            Destroy(Item1);
            Destroy(Item2);
            objetsSurEtabli.Clear();
        }
        //dire que c'est vide
        pos1 = false;
        currentlyCrafting = false;
    }
    #endregion
}
