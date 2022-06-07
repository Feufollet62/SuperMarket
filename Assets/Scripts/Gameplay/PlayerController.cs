using System;
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

    [Header("Animation")]
    [SerializeField] private float animIntensity = 2f;
    [SerializeField] private float animMaxAngle = 20f;
    
    // Private
    private Rigidbody rb;
    private Vector3 velocity, desiredVelocity;
    
    private Vector3 inputMovement;
    private bool inputDash, inputInteract;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerManager manager = PlayerManager.Instance;
        
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
        Move();
    }

    // https://youtu.be/5tOOstXaIKE
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        inputMovement = new Vector3(rawInput.x, 0, rawInput.y);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        //inputDash = context.ReadValue<bool>();
        inputDash = context.action.triggered;
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        //inputInteract = context.ReadValue<bool>();
        inputInteract = context.action.triggered;
    }

    private void GetInput()
    {
        desiredVelocity = inputMovement * maxSpeed;
    }
    
    private void Move()
    {
        float maxSpeedChange = maxAccel * Time.deltaTime;
        
        velocity = rb.velocity;
        
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        //transform.position += velocity * Time.deltaTime;
        rb.velocity = velocity;
    }

    private void Animate()
    {
        if(inputMovement == Vector3.zero)
        {
            model.Rotate(0,0,0);
            return;
        }
        
        // On retrouve l'accélération
        Vector3 accel = inputMovement*maxSpeed - velocity;

        model.forward = accel;
        float angle = accel.magnitude * animIntensity;

        Mathf.Clamp(0, animMaxAngle, angle);
        
        model.Rotate(-angle,0,0);
    }
}
