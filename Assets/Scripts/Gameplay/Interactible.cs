using UnityEngine;

public enum InteractType {Static, Throwable}

public class Interactible : MonoBehaviour
{
    public InteractType type;

    private Rigidbody _rb;
    private Collider _collider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void Grab(Transform grabPoint)
    {
        _rb.isKinematic = true;
        _collider.enabled = false;
        
        transform.parent = grabPoint;
        transform.localPosition = Vector3.zero;
        transform.Rotate(Vector3.zero);
    }

    public void Throw(Vector3 force)
    {
        _rb.isKinematic = false;
        _collider.enabled = true;
        
        _rb.AddForce(force,ForceMode.Impulse);
    }
}
