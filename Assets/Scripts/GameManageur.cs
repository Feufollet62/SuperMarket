using System.Collections.Generic;
using UnityEngine;


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
	public class GameManageur : MonoBehaviour
	{
		#region Variables
		[SerializeField] private GameObject screenWin;
		[SerializeField] public int score;
		[SerializeField] public bool activation = false;

		[SerializeField] Transform posSpawn;
		[SerializeField] public int nbClientAct;

		public File[] files;

		public GameObject ClientPrefab;
		public List<ClientController> Clients;

		[SerializeField] private float limitSpawn;
		[SerializeField] private float cooldownSpawn = 4f;
		public GameObject UIcommand;
		public GameObject UIemplacement;
		public Transform BaseCommande;
		public int targetClient;
		public ClientData cData;
		#endregion

		#region Builtin Methods
		void Start()
		{
			nbClientAct = 1;
			
		}


		void Update()
		{
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
			if (score == 20)
			{
				screenWin.SetActive(true);
				activation = true;
			}
        }

		void ClientSpawner()
        {
			limitSpawn -= Time.deltaTime;
			if (limitSpawn < 0)
			{
				if (nbClientAct <= 6)
				{
					ClientController newClient = Instantiate(ClientPrefab, posSpawn.position, transform.rotation).GetComponent<ClientController>();
					newClient.transform.parent = transform;

					int typeClient = Random.Range(0, cData.clientInfos.Length);
					targetClient = Random.Range(1, 4);

					newClient.SetupClient(cData.clientInfos[typeClient], targetClient);

					GameObject newClientUI = Instantiate(UIcommand, UIemplacement.transform.position, UIemplacement.transform.rotation);
					newClientUI.transform.parent = UIemplacement.transform;
					newClientUI.transform.position = BaseCommande.position;
					
					limitSpawn = cooldownSpawn;
					nbClientAct++;

					

					BaseCommande.transform.position = new Vector3(BaseCommande.position.x + 130, BaseCommande.position.y, BaseCommande.position.z);
				}
			}
		}
		#endregion
	}
}