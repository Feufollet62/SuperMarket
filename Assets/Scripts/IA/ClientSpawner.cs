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
		public int aleaspawn;
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
				if (FindObjectOfType<GameManageur>().nbClientAct <= 6)
				{
					GameObject newClient = Instantiate(Client[0], transform.position, transform.rotation);
					newClient.transform.parent = transform;
					GameObject newClientUI = Instantiate(UIcommand, UIemplacement.transform.position, UIemplacement.transform.rotation);
					newClientUI.transform.parent = UIemplacement.transform;
					newClientUI.transform.position = BaseCommande.position;
					limitSpawn = cooldownSpawn;
					FindObjectOfType<GameManageur>().nbClientAct++;
					aleaspawn = Random.Range(1, 3);
					BaseCommande.transform.position = new Vector3(BaseCommande.position.x + 130, BaseCommande.position.y, BaseCommande.position.z);
				}
			}
		}
		#endregion

		#region Properties

		#endregion
	}
}