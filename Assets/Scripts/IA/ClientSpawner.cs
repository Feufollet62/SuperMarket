using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script{
	public class ClientSpawner : MonoBehaviour
	{
		#region Variables
		public GameObject[] Client;
		[SerializeField] private float limitSpawn;
		[SerializeField] private float cooldownSpawn = 4f;
		public GameObject UIcommand;
		public GameObject UIemplacement;
		public Transform BaseCommande;
		//[SerializeField] private int canSpawn;
		#endregion

		#region Builtin Methods
		void Start()
        {
			limitSpawn = cooldownSpawn;
		}

		void Update()
		{

			limitSpawn -= Time.deltaTime;
			if (limitSpawn < 0)
			{
				if (FindObjectOfType<GameManager>().ClientContent <= 6)
				{
					GameObject clientPrefab;
					clientPrefab = Client[0];
					GameObject newClient = Instantiate(clientPrefab, transform.position, transform.rotation);
					newClient.transform.parent = transform;
					GameObject newClientUI = Instantiate(UIcommand, UIemplacement.transform.position, UIemplacement.transform.rotation);
					newClientUI.transform.parent = UIemplacement.transform;
					newClientUI.transform.position = BaseCommande.position;
					limitSpawn = cooldownSpawn;
					FindObjectOfType<GameManager>().ClientContent++;
					BaseCommande.transform.position = new Vector3(BaseCommande.position.x + 130, BaseCommande.position.y, BaseCommande.position.z);
				}
			}
		}
		#endregion

		#region Properties

		#endregion
	}
}