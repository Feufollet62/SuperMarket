using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etabli : MonoBehaviour
{
    #region Variables

    int[,] dataarrey = new int[,] {  
        { 8,11,13,0,0,0,0,0 }, 
        { 11,9,12,0,0,0,0,0 }, 
        { 13,12,10,0,0,0,0,0},
        { 0,0,0,0,0,17,19,18},
        { 0,0,0,0,0,14,16,15},
        { 0,0,0,17,14,0,0,0 },
        { 0,0,0,19,16,0,0,0 },
        { 0,0,0,18,15,0,0,0 }};

    // Recettes possibles sur cet etabli
    //[SerializeField] private FusionData[] fusions;
    [SerializeField] private ObjectData[] ObjectsFusionable;
    [SerializeField] private GameObject throwablePrefab;

    [SerializeField] private bool isAlchimisteTable;

    // Positions de placement des objets
    [SerializeField] private Transform posObjet1;
    [SerializeField] private Transform posObjet2;
    [SerializeField] private Transform posRejet;
    
    // Temps de craft
    [SerializeField] private float timeToCraft;
    
    // Prend les objet posés permettant de regarder son id
    //[SerializeField] private List<GameObject> objetsSurEtabli;

    // Conditions pour :

    // Savoir si la première place est prise
    private bool pos1 = false;
    private bool pos2 = false;

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
            /*
            if (!isAlchimisteTable)
            {
                if (other.gameObject.GetComponent<Interactible>().dataObject.weapon)
                {
                    //other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                    //j'ajoute un objet dans ma liste
                    objetsSurEtabli.Add(other.gameObject);
                    //destruction de ma liste
                    //Destroy(other.gameObject);
                    //creation d'un objet pour animé 
                    if (pos2)
                    {
                        objetsSurEtabli[1].GetComponent<Collider>().enabled = false;
                        objetsSurEtabli[1].transform.position = posObjet1.position;
                        //Item1 = Instantiate(objetsSurEtabli[0], posObjet1.position, posObjet1.rotation);
                        objetsSurEtabli[1].GetComponent<Rigidbody>().useGravity = false;
                        objetsSurEtabli[1].GetComponent<Rigidbody>().isKinematic = true;
                        //Destroy(other.gameObject);
                        pos1 = true;
                    }
                    else
                    {
                        objetsSurEtabli[0].GetComponent<Collider>().enabled = false;
                        objetsSurEtabli[0].transform.position = posObjet1.position;
                        //Item1 = Instantiate(objetsSurEtabli[0], posObjet1.position, posObjet1.rotation);
                        objetsSurEtabli[0].GetComponent<Rigidbody>().useGravity = false;
                        objetsSurEtabli[0].GetComponent<Rigidbody>().isKinematic = true;
                        //Destroy(other.gameObject);
                        pos1 = true;
                    }

                }
                if (!other.gameObject.GetComponent<Interactible>().dataObject.weapon)
                {
                    //other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                    //j'ajoute un objet dans ma liste
                    objetsSurEtabli.Add(other.gameObject);
                    //destruction de ma liste
                    //Destroy(other.gameObject);
                    //creation d'un objet pour animé 
                    if (pos1)
                    {
                        objetsSurEtabli[1].GetComponent<Collider>().enabled = false;
                        objetsSurEtabli[1].transform.position = posObjet2.position;
                        //Item2 = Instantiate(objetsSurEtabli[0], posObjet2.position, posObjet2.rotation);
                        objetsSurEtabli[1].GetComponent<Rigidbody>().useGravity = false;
                        objetsSurEtabli[1].GetComponent<Rigidbody>().isKinematic = true;
                        //Destroy(other.gameObject);
                        pos2 = true;
                    }
                    else
                    {
                    objetsSurEtabli[0].GetComponent<Collider>().enabled = false;
                        objetsSurEtabli[0].transform.position = posObjet2.position;
                        //Item2 = Instantiate(objetsSurEtabli[1], posObjet2.position, posObjet2.rotation);
                        objetsSurEtabli[0].GetComponent<Rigidbody>().useGravity = false;
                        objetsSurEtabli[0].GetComponent<Rigidbody>().isKinematic = true;
                        //Destroy(other.gameObject);
                        pos2 = true;
                    }
                    //on se met en fabrication
                    currentlyCrafting = true;
                    //lancement de la fabrique
                    if (pos1 && pos2)
                    {
                        StartCoroutine(FusionObjet());
                    }
                }
            
                
            }
            if (isAlchimisteTable)
            {
                if (pos1)
                {
                    //other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                    //j'ajoute un objet dans ma liste

                    //destruction de ma liste
                    //Destroy(other.gameObject);
                    //creation d'un objet pour animé 
                    other.gameObject.GetComponent<Collider>().enabled = false;

                    other.gameObject.transform.position = posObjet2.position;
                    //Item2 = Instantiate(objetsSurEtabli[0], posObjet2.position, posObjet2.rotation);
                    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    //Destroy(other.gameObject);
                    pos2 = true;
                    objetsSurEtabli.Add(other.gameObject);
                }
                if (!pos1)
                {
                    //other.gameObject.GetComponent<Interactible>().dataObject.craftable = false;
                    //j'ajoute un objet dans ma liste

                    //destruction de ma liste
                    //Destroy(other.gameObject);
                    //creation d'un objet pour animé 
                    other.gameObject.GetComponent<Collider>().enabled = false;

                    other.gameObject.transform.position = posObjet1.position;
                    //Item1 = Instantiate(objetsSurEtabli[0], posObjet1.position, posObjet1.rotation);
                    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    //Destroy(other.gameObject);
                    pos1 = true;
                    objetsSurEtabli.Add(other.gameObject);
                }
                    //ma place une est prise

            }*/
            if (!pos1)
            {
                Debug.Log("je pose un Objet 1");
                Item1 = other.gameObject;
                Item1.GetComponent<Collider>().enabled = false;
                Item1.transform.position = posObjet1.position;
                //Item1.GetComponent<Rigidbody>().useGravity = false;
                Item1.GetComponent<Rigidbody>().isKinematic = true;
                pos1 = true;
                //objetsSurEtabli.Add(other.gameObject);
            }
            Debug.Log("je capte un Objet");
            if (pos1 && !pos2)
            {
                Debug.Log("je pose un Objet 2");
                Item2 = other.gameObject;
                //Item1.GetComponent<Collider>().enabled = false;
                Item2.transform.position = posObjet2.position;
                //Item1.GetComponent<Rigidbody>().useGravity = false;
                Item2.GetComponent<Rigidbody>().isKinematic = true;
                pos2 = true;
                //objetsSurEtabli.Add(other.gameObject);
            }
            
            
            //on se met en fabrication
            currentlyCrafting = true;
            //lancement de la fabrique
            if (pos1 && pos2)
            {
                StartCoroutine(FusionObjet());
            }
        }
    }

    IEnumerator FusionObjet()
    {
        Debug.Log("Je demarre ma production");
        //temps de fabrique
        yield return new WaitForSeconds(timeToCraft);

        if (currentlyCrafting)
        {
            //Debug.Log(objetsSurEtabli[0].GetComponent<Interactible>().dataObject.iD);
            //Debug.Log(objetsSurEtabli[1].GetComponent<Interactible>().dataObject.iD);
            //Debug.Log(dataarrey[objetsSurEtabli[0].GetComponent<Interactible>().dataObject.iD, objetsSurEtabli[1].GetComponent<Interactible>().dataObject.iD]);

            for (int i = 0; i < ObjectsFusionable.Length; i++)
            {
                if (ObjectsFusionable[i].iD == dataarrey[Item1.GetComponent<Interactible>().dataObject.iD, Item2.GetComponent<Interactible>().dataObject.iD])
                {
                    Interactible newObject = Instantiate(throwablePrefab, posRejet).GetComponent<Interactible>();
                    newObject.SetupThrowable(ObjectsFusionable[i]);

                    MeshFilter mFilter = throwablePrefab.GetComponent<MeshFilter>();
                    MeshRenderer mRender = throwablePrefab.GetComponent<MeshRenderer>();

                    mFilter.sharedMesh = ObjectsFusionable[i].model;
                    mRender.sharedMaterial = ObjectsFusionable[i].material;

                    gameObject.name = ObjectsFusionable[i].name;
                    throwablePrefab.GetComponent<Interactible>().imageObjet = ObjectsFusionable[i].image;

                    throwablePrefab.GetComponent<Interactible>().iD = ObjectsFusionable[i].iD;
                }
            }
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
            //objetsSurEtabli.Clear();
        }
        //dire que c'est vide
        pos1 = false;
        pos2 = false;
        currentlyCrafting = false;
    }
    #endregion
}
