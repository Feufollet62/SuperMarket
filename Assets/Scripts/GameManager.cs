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
		public ClientData clientDataSO;
		public List<ClientController> currentClients;

		[SerializeField] private Transform clientSpawnPos;
		public File[] files;

		[SerializeField] private float limitSpawn;
		[SerializeField] private float cooldownSpawn = 4f;
		
		public int nbClientAct; // <-- à remplacer par la taille de currentClients

		[Header("UI")]
		//Score
		[SerializeField] private GameObject prefabScreenWin;
		[SerializeField] private GameObject prefabScreenInGame;
		public int score;
		private bool activation = false; // <<- Potentiellement remplaçable par screenWinPrefab.activeSelf
		private float time;
		public float timeGame;
		public Text textScore;


		public GameObject prefabUICommande;
		public List<GameObject> listeUI;

		// Différence entre ces deux là ?
		public GameObject prefabUIEmplacement;
		public Transform baseCommande;
		
		public int targetClient; // à remplacer
		
		#endregion

		#region Builtin Methods
		void Start()
		{
			nbClientAct = 1;
			time = timeGame;
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
			if (limitSpawn < 0)
			{
				if (nbClientAct <= 6)
				{
					ClientController newClient = Instantiate(clientPrefab, clientSpawnPos.position, transform.rotation).GetComponent<ClientController>();
					newClient.transform.parent = transform;

					int typeClient = Random.Range(0, clientDataSO.clientInfos.Length);
					targetClient = Random.Range(0, files.Length);

                    while (files[targetClient].positions[0].prise && files[targetClient].positions[1].prise)
                    {
						targetClient = Random.Range(0, files.Length);
					}

					newClient.SetupClient(clientDataSO.clientInfos[typeClient], targetClient);

					/*
					GameObject newClientUI = Instantiate(prefabUICommande, prefabUIEmplacement.transform.position, prefabUIEmplacement.transform.rotation);
					newClientUI.transform.parent = prefabUIEmplacement.transform;
					newClientUI.transform.position = baseCommande.position;
					baseCommande.transform.position = new Vector3(baseCommande.position.x + 130, baseCommande.position.y, baseCommande.position.z);
					*/
					limitSpawn = cooldownSpawn;
					nbClientAct++;
				}
			}
		}
		#endregion
	}
}