using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlScheme {ControllerFull, ControllerHalfL, ControllerHalfR, KeyboardFull, KeyboardHalfL, KeyboardHalfR, None}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance; // Singleton
    
    [SerializeField] private ControlScheme[] controlSchemes = new ControlScheme[4]; // Remplir ce truc dans les menus avant de lancer la scene
    public PlayerController[] currentPlayers = new PlayerController[4];
    
    public Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;
    
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < controlSchemes.Length; i++)
        {
            ControlScheme scheme = controlSchemes[i];

            // Which pad is solo and which isn't ?
            switch (scheme)
            {
                case ControlScheme.ControllerFull:
                    CreateNewPlayer(Gamepad.all[0]);
                    break;
                case ControlScheme.ControllerHalfL: case ControlScheme.ControllerHalfR:
                    CreateNewPlayer(Gamepad.all[0]);
                    break;
                case ControlScheme.KeyboardFull:
                    CreateNewPlayer(Keyboard.current);
                    break;
                case ControlScheme.KeyboardHalfL: case ControlScheme.KeyboardHalfR:
                    CreateNewPlayer(Keyboard.current);
                    break;
                case ControlScheme.None:
                    break;
            }
            
            void CreateNewPlayer(InputDevice device)
            {
                PlayerController newPlayer = PlayerInput.Instantiate(playerPrefab, controlScheme: scheme.ToString(), pairWithDevice: device).GetComponent<PlayerController>();
                newPlayer.id = i;
                newPlayer.transform.position = spawnPoints[i].position;

                currentPlayers[i] = newPlayer;
            }
        }
    }
}
