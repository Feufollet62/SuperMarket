using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // Info joueur
    public int id;
    [SerializeField] private Material[] playerMats;
    
    // Variables mouvement
    [SerializeField] private float maxAccel = 30f;
    [SerializeField] private float maxSpeed = 8f;
    
    // Private
    Vector3 velocity;
    
    private Vector3 inputMovement;
    private bool inputDash;
    private bool inputInteract;

    private void Start()
    {
        PlayerManager manager = PlayerManager.Instance;
        
        // Assigne les bons matériaux aux bons joueurs
        GetComponentInChildren<MeshRenderer>().material = playerMats[id];
    }

    private void Update()
    {
        Vector3 desiredVelocity = inputMovement * maxSpeed;
        float maxSpeedChange = maxAccel * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        transform.position += velocity * Time.deltaTime;
    }

    // https://youtu.be/5tOOstXaIKE
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        print(rawInput.ToString("f3"));
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
}
