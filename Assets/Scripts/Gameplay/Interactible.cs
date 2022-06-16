using UnityEngine;

public enum InteractType {Spawner, Throwable, Player, Comptoir}

public class Interactible : MonoBehaviour
{
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
            _thisPlayer = GetComponent<PlayerController>();
            _collider = GetComponent<CapsuleCollider>();
        }
        else
        {
            _collider = GetComponent<Collider>();
        }
    }

    public void Spawner(Transform grabPoint)
    {
        // obj = Instatiate(...).getComponent<Interactible>
        // obj.pickup(grabPoint)
        
        
    }

    public void PickUp(Transform grabPoint)
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
}
