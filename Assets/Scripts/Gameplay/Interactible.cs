using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InteractType {Spawner, Throwable, Player, Comptoir, PassePlat, Etabli}

namespace _Script
{
    public class Interactible : MonoBehaviour
    {
        #region Variable
        public InteractType type;

        // Bientôt un editor script pour ça

        // Seulement pour les spawners: prefab throwable
        [SerializeField] public GameObject throwablePrefab;


        // Seulement pour les throwables / players
        public ObjectData dataObject;
        public float iD;

        public Sprite imageObjet;

        private Rigidbody _rb;
        private Collider _collider;

        private bool _isHeld;

        // Seulement pour les players
        private PlayerController _thisPlayer;

        [Header("Passe Plat / Comptoire")]
        //seulement le comptoire
        [SerializeField] public List<ClientController> clientsWait;// chez le GM y'a la même
        [SerializeField] public GameManager gM;
        [SerializeField] private Transform posPassePlat;
        private Interactible ItemADeposer;

        [Header("Etabli")]
        //seulement pour etabli
        int[,] dataarrey = new int[,] {
        { 8,11,13,0,0,0,0,0 },
        { 11,9,12,0,0,0,0,0 },
        { 13,12,10,0,0,0,0,0},
        { 0,0,0,0,0,17,19,18},
        { 0,0,0,0,0,14,16,15},
        { 0,0,0,17,14,0,0,0 },
        { 0,0,0,19,16,0,0,0 },
        { 0,0,0,18,15,0,0,0 }};

        [Header("Etabli")]
        // Recettes possibles sur cet etabli
        [SerializeField] private FusionData[] fusions;
        //[SerializeField] private ObjectData[] ObjectsFusionable;
        //[SerializeField] private GameObject throwablePrefab;

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
        //private bool item1Verif = false;
        //private bool item2Verif = false;

        // Creation d'objet pour l'anim (et l'appeler dans d'autre fonction)
        private Interactible Item1;
        private Interactible Item2;
        private Interactible newItem;
        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            // if(type == InteractType.Throwable) SetupThrowable(dataObject);

            if (type == InteractType.Player)
            {
                // Le gameobject est un joueur, donc collider capsule
                _thisPlayer = GetComponent<PlayerController>();
                _collider = GetComponent<CapsuleCollider>();
            }
            else
            {
                _collider = GetComponent<Collider>();
            }
        }

        public bool Interact(PlayerController player)
        {
            // On return true si l'interaction est faisable, et false si elle ne l'est pas

            switch (type)
            {
                case InteractType.Spawner:
                    if (player.grabbing) return false;

                    // Ceci est foireux
                    player.grabbing = true;
                    ActivateSpawner(player);
                    return true;

                case InteractType.Throwable:
                case InteractType.Player:
                    if (player.grabbing) return false;

                    player.grabbing = true;
                    player._grabbedObject = this;
                    PickUp(player.grabPoint);
                    return true;

                case InteractType.PassePlat:
                    if (!player.grabbing) return false;
                    PassePlat(player);

                    print("Je pose");
                    return true;

                case InteractType.Etabli:
                    if (!player.grabbing) return false;
                    Etabli(player);

                    return true;
                    /*
                case InteractType.Comptoir:
                    if (!player.grabbing) return false;
                    Comptoire(player);
                    // to do: comportement comptoir ici
                    print("Interaction comptoir");
                    return true;*/
                


                default:
                    return false;
            }

            // Normalement on n'arrive jamais ici
            return false;
        }

        public void Throw(Vector3 force)
        {
            if (type == InteractType.Spawner || !_isHeld) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(true);

            _isHeld = false;
            transform.SetParent(null, true);

            _rb.isKinematic = false;
            _collider.enabled = true;

            _rb.AddForce(force, ForceMode.Impulse);
        }

        public void SetupThrowable(ObjectData data) // Faire setup global avec switch case plz
        {
            if (type != InteractType.Throwable) return;

            dataObject = data;

            MeshFilter mFilter = GetComponent<MeshFilter>();
            MeshRenderer mRender = GetComponent<MeshRenderer>();

            mFilter.sharedMesh = dataObject.model;
            mRender.sharedMaterial = dataObject.material;

            gameObject.name = dataObject.name;
            imageObjet = dataObject.image;

            iD = dataObject.iD;
        }
        private void Etabli(PlayerController player)
        {
            Debug.Log("Je veux avoir un objet");
            if (player._grabbedObject.dataObject.craftable && !currentlyCrafting)
            {
                if (isAlchimisteTable)
                {
                    Debug.Log("je capte un Objet");
                    if (pos1)
                    {
                        Debug.Log("je pose un Objet 2");
                        Item2 = player._grabbedObject;
                        player.DropItem();
                        Item2.PlaceIt(posObjet2);
                        pos2 = true;
                    }

                    else
                    {
                        Debug.Log("je pose un Objet 1");
                        Item1 = player._grabbedObject;
                        player.DropItem();
                        Item1.PlaceIt(posObjet1);
                        pos1 = true;

                        //objetsSurEtabli.Add(other.gameObject);
                    }

                    if (pos1 && pos2)
                    {
                        currentlyCrafting = true;
                        StartCoroutine(FusionObjet(player));

                    }


                }
                if (!isAlchimisteTable)
                {
                    if (player._grabbedObject.dataObject.weapon && !pos1)
                    {
                        Item1 = player._grabbedObject;
                        player.DropItem();
                        Item1.PlaceIt(posObjet1);
                        pos1 = true;
                    }
                    if (!player._grabbedObject.dataObject.weapon && !pos2)
                    {
                        Item2 = player._grabbedObject;
                        player.DropItem();
                        Item2.PlaceIt(posObjet2);
                        pos2 = true;
                    }
                    if (pos1 && pos2)
                    {
                        currentlyCrafting = true;
                        StartCoroutine(FusionObjet(player));

                    }
                }
            }
            /*
            if (pos1 && pos2)
            {
                currentlyCrafting = true;
                StartCoroutine(FusionObjet(player));

            }*/
        }

        private void PassePlat(PlayerController player)
        {
            ItemADeposer = player._grabbedObject;

            if (!player.grabbing) return;
            player._grabbedObject = null;
            player.grabbing = false;
            
            ItemADeposer.PlaceIn(posPassePlat);
        }
        private void Comptoire(PlayerController player)
        {
            if (player.grabbing)
            {
                VerificationID(player._grabbedObject.GetComponent<Interactible>());
            }

        }
        void VerificationID(Interactible interactible)
        {
            for (int i = 0; i < clientsWait.Count; i++)
            {
                if (clientsWait[i].iDAleatoire == interactible.iD)
                {
                    gM.score++;
                    clientsWait[i].ExitQueue();
                }
            }
        }

        void Drop()
        {
            if (type == InteractType.Spawner || !_isHeld) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(true);

            _isHeld = false;
            transform.SetParent(null, true);

            _rb.isKinematic = true;
            _collider.enabled = true;
        }

        private void ActivateSpawner(PlayerController player)
        {
            if (type != InteractType.Spawner) return;

            Interactible newObject = Instantiate(throwablePrefab).GetComponent<Interactible>();
            newObject.SetupThrowable(dataObject);

            player._grabbedObject = newObject;
            newObject.PickUp(player.grabPoint);
        }

        private void PickUp(Transform grabPoint)
        {
            if (type == InteractType.Spawner || _isHeld) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(false);

            _isHeld = true;

            _rb.isKinematic = true;
            _collider.enabled = false;

            transform.parent = grabPoint;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        private void PlaceIt(Transform placeposition)
        {
            if (type == InteractType.Spawner) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(false);

            _isHeld = true;

            _rb.isKinematic = true;
            _collider.enabled = false;

            transform.parent = placeposition;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        private void PlaceIn(Transform placeposition)
        {
            if (type == InteractType.Spawner) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(false);

            _isHeld = false;

            _rb.isKinematic = true;
            _collider.enabled = true;

            transform.parent = placeposition;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one;
        }
        IEnumerator FusionObjet(PlayerController player)
        {
            Debug.Log("Je demarre ma production");
            //temps de fabrique
            yield return new WaitForSeconds(timeToCraft);

            if (currentlyCrafting)
            {
                for (int i = 0; i < fusions.Length; i++)
                {
                    if (fusions[i].objetFusion1.iD == Item1.iD && fusions[i].objetFusion2.iD == Item2.iD || fusions[i].objetFusion1.iD == Item2.iD && fusions[i].objetFusion2.iD == Item1.iD)
                    {
                        Interactible newObject = Instantiate(throwablePrefab, posRejet).GetComponent<Interactible>();
                        newObject.SetupThrowable(fusions[i].objetResult);

                        MeshFilter mFilter = throwablePrefab.GetComponent<MeshFilter>();
                        MeshRenderer mRender = throwablePrefab.GetComponent<MeshRenderer>();

                        mFilter.sharedMesh = fusions[i].objetResult.model;
                        mRender.sharedMaterial = fusions[i].objetResult.material;

                        gameObject.name = fusions[i].objetResult.name;
                        throwablePrefab.GetComponent<Interactible>().imageObjet = fusions[i].objetResult.image;

                        throwablePrefab.GetComponent<Interactible>().iD = fusions[i].objetResult.iD;
                    }
                }

                player.interactibles.Remove(Item1);
                Destroy(Item1.gameObject);
                player.interactibles.Remove(Item2);
                Destroy(Item2.gameObject);
                //objetsSurEtabli.Clear();
            }
            //dire que c'est vide
            pos1 = false;
            pos2 = false;
            currentlyCrafting = false;
        }
    }
}