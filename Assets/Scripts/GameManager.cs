using System.Collections.Generic;
using _Script;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class File
{
	public PositionFile[] positions;

	// Idéalement ne spawn personne si toutes les places sont prises
	public PositionFile GetBestPos()
	{
		foreach (PositionFile pos in positions)
		{
			if (!pos.occupied) return pos;
		}

		return null; // On devrait jamais en arriver là
	}
}

[System.Serializable]
public class PositionFile
{
	public Transform transform;
	public ClientController currentClient;
	public bool canOrder = false;
	public bool occupied = false;
}

namespace _Script{
	public class GameManager : MonoBehaviour
	{
		#region Variables
		
		[Header("Files d'attente et spawning")]
		public GameObject clientPrefab;
		public ClientData clientData;
		
		public List<ClientController> clientList; // Utiliser ça stp
		
		[SerializeField] private Transform clientSpawnPos;

		public File[] files;
		
		public Transform sortieQueue1;
		public Transform sortieQueue2;
		public Transform porteEntree;
		public Transform porteSortie;
		public Transform despawn;

		[SerializeField] private float limitSpawn;
		[SerializeField] private float cooldownSpawn = 4f;

		[Header("UI")]
		[SerializeField] private GameObject prefabScreenWin;
		[SerializeField] private GameObject prefabScreenInGame;
		
		public int score;
		private float time;
		
		[SerializeField] private float dureePartie;
		[SerializeField] private Text textScore;
		
		public List<GameObject> listeUI;

		// Différence entre ces deux là ?
		public GameObject prefabUIEmplacement;
		public Transform uICommande;
		
		#endregion

		#region Builtin Methods
		private void Start()
		{
			
		}
		
		private void Update()
		{
			time += Time.deltaTime;
			
			ClientSpawner();
			
			if (!(time >= dureePartie)) return;
			WinLevel();
		}
		#endregion
		
		#region Properties
		private void WinLevel()
		{
			textScore.text = score.ToString();
			prefabScreenInGame.SetActive(false);
			prefabScreenWin.SetActive(true);
		}
		public void ButtonRestartGame()
        {
			SceneManager.LoadScene("Niveau_1");
        }
		public void ButtonBackMenu()
		{
			SceneManager.LoadScene("MenuSelectLvl");
		}

		private void ClientSpawner()
        {
			limitSpawn -= Time.deltaTime;
			
			if (!(limitSpawn < 0) || clientList.Count > 6) return;
			
			int typeClient = Random.Range(0, clientData.clientInfos.Length);
			
			ClientController newClient = Instantiate(clientPrefab, clientSpawnPos.position, transform.rotation).GetComponent<ClientController>();
			clientList.Add(newClient);
			
			newClient.transform.parent = transform;
			newClient.SetupClient(clientData.clientInfos[typeClient], GetBestPos());
					
			limitSpawn = cooldownSpawn;
        }
		
		// C'est pas opti la comp avec Null
		private PositionFile GetBestPos()
		{
			PositionFile bestPos = null;
			
			foreach (File file in files)
			{
				PositionFile tempPos = file.GetBestPos();
				if (tempPos != null) 
				{
					bestPos = tempPos;
					break;
				}
			}

			return bestPos;
		}
		
		#endregion
	}
}