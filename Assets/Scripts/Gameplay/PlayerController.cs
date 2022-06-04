using System;
using UnityEngine;

public enum PlayerID {Player1, Player2, Player3, Player4}

public class PlayerController : MonoBehaviour
{
    // Info joueur
    public PlayerID id = PlayerID.Player1;
    [SerializeField] private Material[] playerMats;
    
    // Variables mouvement
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 10f;

    private void Start()
    {
        GetComponent<MeshRenderer>().material = playerMats[(int)id];
    }

    private void Update()
    {
        
    }
}
