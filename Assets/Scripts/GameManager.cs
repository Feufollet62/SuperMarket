using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class File
{
	public PositionFile[] positions;
}

[System.Serializable]
public class PositionFile
{
	public Transform pos;
	public bool prise = false;
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

		[SerializeField] private float limitSpawn;
		[SerializeField] private float cooldownSpawn = 4f;

		[Header("UI")]
		//Score
		[SerializeField] private GameObject prefabScreenWin;
		[SerializeField] private GameObject prefabScreenInGame;
		public int score;
		private bool activation = false; // <<- Potentiellement remplaçable par screenWinPrefab.activeSelf
		private float time;
		public float dureePartie;
		public Text textScore;
		
		public List<GameObject> listeUI;

		// Différence entre ces deux là ?
		public GameObject prefabUIEmplacement;
		public Transform baseCommande;
		
		public int targetClient; // à remplacer
		
		#endregion

		#region Builtin Methods
		void Start()
		{
			time = dureePartie;
		}
		
		void Update()
		{
			time -= Time.deltaTime;
			if (activation == false)
			{
				WinLevel();
			}
			ClientSpawner();
		}
		#endregion

		#region Properties
		void WinLevel()
        {
			if (time <= 0)
			{
				textScore.text = score.ToString();
				prefabScreenInGame.SetActive(false);
				prefabScreenWin.SetActive(true);
				activation = true;
			}
        }
		public void ButtonRestartGame()
        {
			SceneManager.LoadScene("Niveau_1");
        }
		public void ButtonBackMenu()
		{
			SceneManager.LoadScene("MenuSelectLvl");
		}

		void ClientSpawner()
        {
			limitSpawn -= Time.deltaTime;
			
			if (!(limitSpawn < 0) || clientList.Count > 6) return;
			
			ClientController newClient = Instantiate(clientPrefab, clientSpawnPos.position, transform.rotation).GetComponent<ClientController>();
			clientList.Add(newClient);
			
			newClient.transform.parent = transform;

			int typeClient = Random.Range(0, clientData.clientInfos.Length);
			targetClient = Random.Range(0, files.Length);

			while (files[targetClient].positions[0].prise && files[targetClient].positions[1].prise)
			{
				targetClient = Random.Range(0, files.Length);
			}

			newClient.SetupClient(clientData.clientInfos[typeClient], targetClient);
					
			limitSpawn = cooldownSpawn;
        }
		#endregion
	}
}