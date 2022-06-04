using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerID {Player1, Player2, Player3, Player4}

public class PlayerController : MonoBehaviour
{
    // Info joueur
    public PlayerID id = PlayerID.Player1;
    [SerializeField] private Material[] playerMats;
    
    // Variables mouvement
    [SerializeField] private float maxAccel = 10f;
    [SerializeField] private float maxSpeed = 10f;
    
    // Private
    Vector3 velocity;
    
    private Vector3 inputMovement;
    private bool inputDash;
    private bool inputInteract;

    private void Start()
    {
        // Assigne les bons matériaux aux bons joueurs
        GetComponentInChildren<MeshRenderer>().material = playerMats[(int)id];
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
    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 rawInput = value.ReadValue<Vector2>();
        print(rawInput.ToString("f3"));
        inputMovement = new Vector3(rawInput.x, 0, rawInput.y);
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        inputDash = value.ReadValueAsButton();
    }
    
    public void OnInteract(InputAction.CallbackContext value)
    {
        inputInteract = value.ReadValueAsButton();
    }
}
