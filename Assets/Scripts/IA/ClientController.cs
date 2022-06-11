using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ClientType {Normal, Presse,Vieux, Riche}

namespace _Script{
	public class ClientController : MonoBehaviour
	{
		#region Variables

		public ClientType type = ClientType.Normal;
		// SO avec tous les modèles de joueur

		private int fileTarget;

		[SerializeField] float timeService = 20f;
		[SerializeField] int posClientRandom;
		[SerializeField] ClientData clientData;
		[SerializeField] float timeduClient;
		[SerializeField] GameManager gM;
		[SerializeField] List<Transform> posBaseClient;
		[SerializeField] int intPosBaseClient;

		public NavMeshAgent agent;

		#endregion

		#region Properties

		IEnumerator EnterShop()
		{
			agent.destination = GameObject.Find("Posentre").transform.position;
			yield return new WaitForSeconds(1f);


			StartCoroutine("TimeTravel");
		}
		
		IEnumerator TimeTravel()
		{
			/*
			if (gM.targetClient == 1)
			{
				agent.destination = gM.fileClient1[0].position;
				gM.fileClient1.Remove(gM.fileClient1[0]);
			}
			else if (gM.targetClient == 2)
			{
				agent.destination = gM.fileClient2[0].position;
				gM.fileClient2.Remove(gM.fileClient2[0]);
			}
			else if (gM.targetClient == 3)
			{
				agent.destination = gM.fileClient3[0].position;
				gM.fileClient3.Remove(gM.fileClient3[0]);
			}
			*/
			yield return new WaitForSeconds(timeService);
			//je vais attendre qu'une nouvelle queue se place
			StartCoroutine("InExitQueue");
		}
		IEnumerator InExitQueue()
		{
			/*
			if (gM.targetClient == 1)
			{
				gM.fileClient1.Add(posBaseClient[0]);
			}
			else if (gM.targetClient == 2)
			{
				gM.fileClient2.Add(posBaseClient[0]);
			}
			else if (gM.targetClient == 3)
			{
				gM.fileClient3.Add(posBaseClient[0]);
			}*/
			
			Destroy(gM.prefabUICommande);
			if (posClientRandom < 3)
			{
				agent.destination = GameObject.Find("Posexitqueue2").transform.position;
			}
			else
			{
				agent.destination = GameObject.Find("Posexitqueue").transform.position;
			}

			yield return new WaitForSeconds(1.2f);

			

			StartCoroutine("ExitShop");
		}

		IEnumerator ExitShop()
		{
			agent.destination = GameObject.Find("Pos5").transform.position;
			yield return new WaitForSeconds(1f);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("posPourCommande"))
			{
				gM.prefabUICommande.SetActive(true);
			}
		}

		public void SetupClient(ClientInfo infos, int target)
        {
			/*
			public ClientType type = ClientType.Normal;

			public Mesh model;
			public Material material;

			public Sprite sprite;

			public string name;
			public string description;
			public float time;*/

			type = infos.type;
			fileTarget = target;

			MeshFilter mFilter = GetComponent<MeshFilter>();
			MeshRenderer mRender = GetComponent<MeshRenderer>();

			mFilter.sharedMesh = infos.model;
			mRender.sharedMaterial = infos.material;

			gameObject.name = infos.name;
		}
		#endregion

		#region Builtin Methods 
		void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			gM = FindObjectOfType<GameManager>();

			//gM.files[fileTarget]

			/*
			if (fileTarget == 1)
            {
				posBaseClient[0] = gM.fileClient1[0];
			}
			else if (fileTarget == 2)
			{
				posBaseClient[0] = gM.fileClient2[0];
			}
			else if (fileTarget == 3)
			{
				posBaseClient[0] = gM.fileClient3[0];
			}*/

			//timeduClient = clientData.Time;
			StartCoroutine("EnterShop");
		}

        #endregion
    }
}