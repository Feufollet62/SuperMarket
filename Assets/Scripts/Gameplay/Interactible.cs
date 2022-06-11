using UnityEngine;

public enum InteractType {Static, Throwable, Player}

public class Interactible : MonoBehaviour
{
    public InteractType type;

    private Rigidbody _rb;
    private Collider _collider;

    private PlayerController _player;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        if (type == InteractType.Player)
        {
            _player = GetComponent<PlayerController>();
            _collider = GetComponent<CapsuleCollider>();
        }
        else
        {
            _collider = GetComponent<Collider>();
        }
    }

    public void Interact(Transform grabPoint)
    {
        // ajouter comportement ici
        if(type == InteractType.Static) return;
        if(type == InteractType.Player) _player.SetControllable(false);
                
        _rb.isKinematic = true;
        _collider.enabled = false;
        
        transform.parent = grabPoint;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0,0,0);
    }

    public void Throw(Vector3 force)
    {
        if(type == InteractType.Static) return;
        
        if(type == InteractType.Player)
        {
            _player.SetControllable(true);
        }

        transform.SetParent(null, true);
        
        _rb.isKinematic = false;
        _collider.enabled = true;
        
        _rb.AddForce(force,ForceMode.Impulse);
    }
}
