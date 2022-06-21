using UnityEngine;

public enum InteractType {Spawner, Throwable, Player, Comptoir, PassePlat, Etabli}

public class Interactible : MonoBehaviour
{
    public InteractType type;
    
    // Bientôt un editor script pour ça ?
    
    // Seulement pour les spawners: prefab throwable
    [SerializeField] public GameObject throwablePrefab;

    // Seulement pour les throwables / players
    public ObjectData data;
    
    private Rigidbody _rb;
    private Collider _collider;
    
    private bool _isHeld;
    
    // Seulement pour les players
    private PlayerController _thisPlayer;

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
            
            case InteractType.Throwable: case InteractType.Player:
                if (player.grabbing) return false;
                
                player.grabbing = true;
                player._grabbedObject = this;
                PickUp(player.grabPoint);
                return true;
            
            case InteractType.Comptoir:
                if (!player.grabbing) return false;
                
                // to do: comportement comptoir ici
                print("Interaction comptoir");
                return true;
            
            default:
                return false;
        }
        
        // Normalement on n'arrive jamais ici
        return false;
    }
    
    public void Throw(Vector3 force)
    {
        if(type == InteractType.Spawner || !_isHeld) return;
        if(type == InteractType.Player) _thisPlayer.SetControllable(true);

        _isHeld = false;
        transform.SetParent(null, true);
        
        _rb.isKinematic = false;
        _collider.enabled = true;
        
        _rb.AddForce(force,ForceMode.Impulse);
    }

    public void SetupThrowable(ObjectData newData) // Faire setup global avec switch case plz
    {
        if(type != InteractType.Throwable) return;

        data = newData;
        
        MeshFilter mFilter = GetComponent<MeshFilter>();
        MeshRenderer mRender = GetComponent<MeshRenderer>();

        mFilter.sharedMesh = data.model;
        mRender.sharedMaterial = data.material;

        gameObject.name = data.name;
    }
    
    private void ActivateSpawner(PlayerController player)
    {
        if (type != InteractType.Spawner) return;

        Interactible newObject = Instantiate(throwablePrefab).GetComponent<Interactible>();
        newObject.SetupThrowable(data);

        player._grabbedObject = newObject;
        newObject.PickUp(player.grabPoint);
    }

    private void PickUp(Transform grabPoint)
    {
        if(type == InteractType.Spawner || _isHeld) return;
        if(type == InteractType.Player) _thisPlayer.SetControllable(false);

        _isHeld = true;
        
        _rb.isKinematic = true;
        _collider.enabled = false;
        
        transform.parent = grabPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0,0,0);
    }
}