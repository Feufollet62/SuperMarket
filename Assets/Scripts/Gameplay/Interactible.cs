using UnityEngine;

public enum InteractType {Spawner, Throwable, Player, Comptoir}

public class Interactible : MonoBehaviour
{
    [SerializeField] public GameObject _objetPrefab;
    
    public InteractType type;

    private Rigidbody _rb;
    private Collider _collider;
    
    private bool isHeld;

    private PlayerController _thisPlayer;

    private void Start()
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
        if(type == InteractType.Spawner || !isHeld) return;
        if(type == InteractType.Player) _thisPlayer.SetControllable(true);

        isHeld = false;
        transform.SetParent(null, true);
        
        _rb.isKinematic = false;
        _collider.enabled = true;
        
        _rb.AddForce(force,ForceMode.Impulse);
    }

    private void ActivateSpawner(PlayerController player)
    {
        if (type != InteractType.Spawner) return;

        Interactible newObject = Instantiate(_objetPrefab).GetComponent<Interactible>();
        player._grabbedObject = newObject;
        newObject.PickUp(player.grabPoint);
    }

    private void PickUp(Transform grabPoint)
    {
        if(type == InteractType.Spawner || isHeld) return;
        if(type == InteractType.Player) _thisPlayer.SetControllable(false);

        isHeld = true;
        
        _rb.isKinematic = true;
        _collider.enabled = false;
        
        transform.parent = grabPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0,0,0);
    }
}
