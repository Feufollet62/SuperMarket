using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Info")]
    public int id;
    // Remplacer par scriptableojbect maybe ?
    [SerializeField] private Material[] playerMats;
    [SerializeField] private Transform model;
    
    [Header("Mouvement")]
    [SerializeField] private float maxAccel = 30f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float dashStrength = 1f;
    [SerializeField] private float dashLength = .5f;

    [Header("Animation")]
    [SerializeField] private float animIntensity = 2f;
    [SerializeField] private float animMaxAngle = 20f;
    
    // Private
    private Rigidbody _rb;
    private ParticleSystem _particleSystem;
    
    private Vector3 _velocity, _desiredVelocity, _inputMovement;

    private bool _dashing;
    private float _dashTimer;
    
    private bool _inputDash, _inputInteract;

    private float _animTimer;

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
        print("Dashing: " + _dashing);
        print("Timer: " + _dashTimer);
    }

    private void FixedUpdate()
    {
        Move();
        Dash();
    }

    // https://youtu.be/5tOOstXaIKE
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        _inputMovement = new Vector3(rawInput.x, 0, rawInput.y);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        _inputDash = context.action.triggered;
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        //inputInteract = context.ReadValue<bool>();
        _inputInteract = context.action.triggered;
    }

    private void GetInput()
    {
        _desiredVelocity = _inputMovement * maxSpeed;
    }
    
    private void Move()
    {
        if(_dashing) return;
        
        print("Moving");
        
        float maxSpeedChange = maxAccel * Time.deltaTime;
        
        _velocity = _rb.velocity;
        
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, maxSpeedChange);
        _velocity.z = Mathf.MoveTowards(_velocity.z, _desiredVelocity.z, maxSpeedChange);

        //transform.position += velocity * Time.deltaTime;
        _rb.velocity = _velocity;
    }

    private void Dash()
    {
        // Already dashing
        if(_dashing)
        {
            print("Currently dashing " + Time.time);
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
        
        print("Start dash " + Time.time);
        _rb.AddForce(_inputMovement * dashStrength, ForceMode.Impulse);
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

        model.forward = accel;
        float angle = accel.magnitude * animIntensity;

        Mathf.Clamp(0, animMaxAngle, angle);
        
        model.Rotate(-angle,0,0);
    }
}
