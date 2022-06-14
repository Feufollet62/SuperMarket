using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Info")]
    public int id;
    // Remplacer par scriptableojbect maybe ?
    [SerializeField] private Material[] playerMats;
    [SerializeField] private Transform model;
    [SerializeField] private Transform grabPoint;
    
    [Header("Mouvement")]
    [SerializeField] private float maxAccel = 30f;
    [SerializeField] private float maxAirAccel = 10f;
    [SerializeField] private float maxSpeed = 8f;

    [SerializeField] [Range(0,1)] private float minGroundDotProduct = .9f;
    
    [SerializeField] private float dashStrength = 1f;
    [SerializeField] private float dashLength = .5f;
    
    [SerializeField] private float throwStrengthItem = 3f;
    [SerializeField] private float throwStrengthPlayer = 5f;

    [Header("Animation")]
    [SerializeField] private float animIntensity = 2f;
    [SerializeField] private float animMaxAngle = 20f;
    
    public bool controllable = true;

    // Private
    private Rigidbody _rb;
    private ParticleSystem _particleSystem;

    public List<Interactible> interactibles;
    private Interactible _grabbedObject;

    private Vector3 _velocity, _desiredVelocity, _inputMovement;
    private Vector3 contactNormal;

    private int groundContactCount;
    private bool Grounded => groundContactCount > 0;
    
    private bool _inputDash, _inputInteract;
    private bool _dashing;
    private float _dashTimer;

    private float _animTimer;

    #endregion

    #region Built-in Functions
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        
        // Assigne les bons matériaux aux bons joueurs
        GetComponentInChildren<MeshRenderer>().material = playerMats[id];
    }

    private void Update()
    {
        GetInput();
        Animate();
    }

    private void FixedUpdate()
    {
        if(!_dashing) Move();
        
        if (Grounded)
        {
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
            Dash();
        }
        else
        {
            contactNormal = Vector3.up;
        }
        
        Interact();
        
        // Doit rester à la fin
        ClearState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        EvaluateCollision(collisionInfo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactible") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Interactible thisInteractible = other.GetComponent<Interactible>();
            
            if (!interactibles.Contains(thisInteractible))
            {
                interactibles.Add(thisInteractible);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactible") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Interactible thisInteractible = other.GetComponent<Interactible>();
            
            if (interactibles.Contains(thisInteractible))
            {
                interactibles.Remove(thisInteractible);
            }
        }
    }

    #endregion

    #region Custom Functions

    // Focntionnement input system: https://youtu.be/5tOOstXaIKE
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (controllable)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            _inputMovement = new Vector3(rawInput.x, 0, rawInput.y);
        }
        else
        {
            _inputMovement = Vector3.zero;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        // True si controllable = true et context.action.triggered = true
        _inputDash = controllable && context.action.triggered;
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        _inputInteract = controllable && context.action.triggered;
    }

    public void SetControllable(bool isControllable)
    {
        if (isControllable)
        {
            controllable = true;
        }
        else
        {
            controllable = false;
            _velocity = Vector3.zero;
        }
    }
    
    private void GetInput()
    {
        _desiredVelocity = _inputMovement * maxSpeed;
    }
    
    private void Move()
    {
        // Ci dessous code pour prendre les pentes de manière smooth
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(_velocity, xAxis);
        float currentZ = Vector3.Dot(_velocity, zAxis);
        
        float accel = Grounded ? maxAccel : maxAirAccel;
        float maxSpeedChange = accel * Time.deltaTime;
        
        float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);
        
        _velocity = _rb.velocity;
        _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

        //transform.position += velocity * Time.deltaTime;
        _rb.velocity = _velocity;
    }

    private void Dash()
    {
        // Already dashing
        if(_dashing)
        {
            _dashTimer += Time.fixedDeltaTime;
            if (_dashTimer >= dashLength)
            {
                _dashing = false;
            }
            return;
        }
        
        // No input
        if (!_inputDash) return;
        
        // Start dashing
        _inputDash = false;
        _dashing = true;
        _dashTimer = 0f;
        
        _rb.AddForce(_inputMovement * dashStrength, ForceMode.Impulse);
    }

    private void Interact()
    {
        if(!_inputInteract) return;
        
        // Pas ouf niveau opti une comparaison avec null
        if (_inputInteract)
        {
            if (_grabbedObject == null)
            {
                InteractWithNearest();
            }
            else
            {
                Vector3 throwVector = new Vector3(model.forward.x, 0, model.forward.z) + _velocity;
                
                if (_grabbedObject.type == InteractType.Player)
                {
                    throwVector *= throwStrengthPlayer;
                }
                else
                {
                    throwVector *= throwStrengthItem;
                }


                _grabbedObject.Throw(throwVector);
                _grabbedObject = null;
            }

            _inputInteract = false; 
        }
    }

    private void InteractWithNearest()
    {
        // Si rien est à portée ou si on porte déja un truc, rien ne se passe 
        if(interactibles.Count == 0 || !(_grabbedObject == null)) return;
        
        // Si c'est le seul à portée on cherche pas plus loin
        if (interactibles.Count == 1 && interactibles[0].type != InteractType.Static)
        {
            _grabbedObject = interactibles[0];
            _grabbedObject.Interact(grabPoint);
            return;
        }

        // Sinon on vérifie tout le reste
        // Le premier obj sera forcément plus près que l'infini
        float closestDistance = 99999f;
        
        // Peu importe ce qu'il y a là dedans car ce sera forcément remplacé
        Interactible closest = interactibles[0];
        
        for (int i = 0; i < interactibles.Count; i++)
        {
            // Si pas throwable on skip
            if (interactibles[i].type == InteractType.Static) continue;

            float distance = Vector3.Distance(transform.position, interactibles[i].transform.position);
            
            if (distance < closestDistance)
            {
                closest = interactibles[i];
                closestDistance = distance;
            }
        }
        
        closest.Interact(grabPoint);
    }

    private void Animate()
    {
        if(_inputMovement == Vector3.zero)
        {
            model.Rotate(0,0,0);
            _particleSystem.Stop();
            _animTimer = 0;
            return;
        }

        _animTimer += Time.deltaTime;
        if (_animTimer > .5f)
        {
            _particleSystem.Play();
            _animTimer = 0f;
        }
        
        // On retrouve l'accélération
        Vector3 accel = _inputMovement*maxSpeed - _rb.velocity;

        model.forward = _rb.velocity;
        float angle = accel.magnitude * animIntensity;

        Mathf.Clamp(0, animMaxAngle, angle);
        
        model.Rotate(-angle,0,0);
    }

    private void EvaluateCollision(Collision col)
    {
        for (int i = 0; i < col.contactCount; i++)
        {
            Vector3 normal = col.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct) {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }
    
    private void ClearState() {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }
    
    private Vector3 ProjectOnContactPlane(Vector3 vector) 
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }
    
    private void AdjustVelocity() 
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right);
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward);
    }
    
    #endregion
}
