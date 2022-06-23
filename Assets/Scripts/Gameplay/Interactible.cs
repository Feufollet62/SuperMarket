using UnityEngine;
using System.Collections;

public enum InteractType {Spawner, Throwable, Player, Comptoir, PassePlat, Etabli}

namespace _Script
{
    public class Interactible : MonoBehaviour
    {
        #region Variables
        public InteractType type;
        
        // Seulement pour les spawners: prefab throwable
        [SerializeField] public GameObject throwablePrefab;


        // Seulement pour les throwables / players
        public ObjectData dataObject;
        public float iD;

        private Rigidbody _rb;
        private Collider _collider;

        private bool _isHeld;

        // Seulement pour les players
        private PlayerController _thisPlayer;

        [Header("Passe Plat / Comptoir")]
        [SerializeField] public GameManager manager;
        [SerializeField] private Transform posPassePlat;
        private Interactible ItemADeposer;
        
        [Header("Etabli")]
        // Recettes possibles sur cet etabli
        [SerializeField] private FusionData[] fusions;

        [SerializeField] private bool isAlchimisteTable;

        // Positions de placement des objets
        [SerializeField] private Transform posObjet1;
        [SerializeField] private Transform posObjet2;
        [SerializeField] private Transform posRejet;

        // Temps de craft
        [SerializeField] private float timeToCraft;

        // Conditions pour :
        // Savoir si la première place est prise
        private bool pos1 = false;
        private bool pos2 = false;

        // Si l'objet est en cours de construction
        private bool currentlyCrafting = false;
        
        // Creation d'objet pour l'anim (et l'appeler dans d'autre fonction)
        private Interactible Item1;
        private Interactible Item2;
        private Interactible newItem;
        #endregion

        #region Built-in functions
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
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
        private void Start()
        {
            manager = FindObjectOfType<GameManager>();
        }
        
        #endregion

        #region Custom functions

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
                    // Uhhhhh il y avait un truc ici avant
                    
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
                    
                case InteractType.Comptoir:
                    if (!player.grabbing) return false;
                    Comptoir(player);
                    // to do: comportement comptoir ici
                    print("Interaction comptoir");
                    return true;
                
                default:
                    return false;
            }
        }

        #region Objets physiques (Throwables, player)

        public void SetupThrowable(ObjectData data)
        {
            if (type != InteractType.Throwable) return;

            dataObject = data;

            MeshFilter mFilter = GetComponent<MeshFilter>();
            MeshRenderer mRender = GetComponent<MeshRenderer>();

            mFilter.sharedMesh = dataObject.model;
            mRender.sharedMaterial = dataObject.material;

            gameObject.name = dataObject.name;

            iD = dataObject.iD;
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
        
        private void Place(Transform placePosition)
        {
            if (type == InteractType.Spawner) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(false);

            _isHeld = true;

            _rb.isKinematic = true;
            _collider.enabled = false;

            transform.parent = placePosition;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
        // Diff entre celle ci ^^^^^ et celle là vvvvvv ? On peut sûrement en garder une seule
        private void PlaceIn(Transform placePosition)
        {
            if (type == InteractType.Spawner) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(false);

            _isHeld = false;

            _rb.isKinematic = true;
            _collider.enabled = true;

            transform.parent = placePosition;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = Vector3.one;
        }
        
        private void Drop()
        {
            if (type == InteractType.Spawner || !_isHeld) return;
            if (type == InteractType.Player) _thisPlayer.SetControllable(true);

            _isHeld = false;
            transform.SetParent(null, true);

            _rb.isKinematic = true;
            _collider.enabled = true;
        }

        #endregion

        #region Objets statiques (Spawners, etablis, comptoir)

        private void ActivateSpawner(PlayerController player)
        {
            if (type != InteractType.Spawner) return;

            Interactible newObject = Instantiate(throwablePrefab).GetComponent<Interactible>();
            newObject.SetupThrowable(dataObject);

            player._grabbedObject = newObject;
            newObject.PickUp(player.grabPoint);
        }
        
        private void PassePlat(PlayerController player)
        {
            ItemADeposer = player._grabbedObject;

            if (!player.grabbing) return;
            player._grabbedObject = null;
            player.grabbing = false;
            
            ItemADeposer.PlaceIn(posPassePlat);
        }
        
        private void Comptoir(PlayerController player)
        {
            ItemADeposer = player._grabbedObject;

            player.DropItem();

            ItemADeposer.PlaceIn(posPassePlat);
            VerificationID(ItemADeposer, player);
        }
        
        private void VerificationID(Interactible interactible, PlayerController player)
        {
            for (int i = 0; i < manager.clientList.Count; i++)
            {
                if (manager.clientList[i].iDAleatoire == interactible.iD && manager.clientList[i].PosActuelle == Position.Pos1)
                {
                    player.interactibles.Remove(ItemADeposer);
                    manager.score++;
                    manager.clientList[i].ExitQueue();
                    return;
                }
            }
        }
        
        private void Etabli(PlayerController player)
        {
            if (player._grabbedObject.dataObject.craftable && !currentlyCrafting)
            {
                if (isAlchimisteTable)
                {
                    if (pos1)
                    {
                        Item2 = player._grabbedObject;
                        player.DropItem();
                        Item2.Place(posObjet2);
                        pos2 = true;
                    }

                    else
                    {
                        Item1 = player._grabbedObject;
                        player.DropItem();
                        Item1.Place(posObjet1);
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
                        Item1.Place(posObjet1);
                        pos1 = true;
                    }
                    if (!player._grabbedObject.dataObject.weapon && !pos2)
                    {
                        Item2 = player._grabbedObject;
                        player.DropItem();
                        Item2.Place(posObjet2);
                        pos2 = true;
                    }
                    if (pos1 && pos2)
                    {
                        currentlyCrafting = true;
                        StartCoroutine(FusionObjet(player));
                    }
                }
            }
        }
        
        private IEnumerator FusionObjet(PlayerController player)
        {
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
                        
                        throwablePrefab.GetComponent<Interactible>().dataObject.image = fusions[i].objetResult.image;
                        throwablePrefab.GetComponent<Interactible>().iD = fusions[i].objetResult.iD;
                    }
                }

                player.interactibles.Remove(Item1);
                Destroy(Item1.gameObject);
                player.interactibles.Remove(Item2);
                Destroy(Item2.gameObject);
            }
            
            // Dire que c'est vide
            pos1 = false;
            pos2 = false;
            currentlyCrafting = false;
        }
        
        #endregion
        
        #endregion
    }
}