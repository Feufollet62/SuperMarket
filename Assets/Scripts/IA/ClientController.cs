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

		private bool premiereVerif = false;
		private bool FirstPlace = false;
		private bool SecondPlace = false;
		#endregion

		#region Builtin Methods 
		void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			gM = FindObjectOfType<GameManager>();

			fileTarget = gM.targetClient;
			
			StartCoroutine("EnterShop");
		}

        private void Update()
        {
            if (!gM.files[fileTarget].positions[0].prise && !FirstPlace)
            {
				SecondPlace = false;
				StartCoroutine(InQueue());
			}
		}

        #endregion

        #region CustomFunction

        IEnumerator EnterShop()
		{
			agent.destination = GameObject.Find("EntreeExt").transform.position;
			yield return new WaitForSeconds(1f);


			StartCoroutine("TimeTravel");
		}

		IEnumerator TimeTravel()
		{	
			if (!gM.files[fileTarget].positions[0].prise && !premiereVerif)
			{
				agent.destination = gM.files[fileTarget].positions[0].pos.position;
				gM.files[fileTarget].positions[0].prise = true;
				FirstPlace = true;
			}
			else if (gM.files[fileTarget].positions[0].prise && !gM.files[fileTarget].positions[1].prise && !premiereVerif)
			{
				agent.destination = gM.files[fileTarget].positions[1].pos.position;
				gM.files[fileTarget].positions[1].prise = true;
				SecondPlace = true;
			}
            if (false)
            {
				premiereVerif = true;
			}
			


			yield return new WaitForSeconds(timeService);
			
            //je vais attendre qu'une nouvelle queue se place
            if (FirstPlace)
            {
				StartCoroutine("InExitQueue");
            }
			
		}
		IEnumerator InQueue()
        {
			yield return new WaitForSeconds(1f);

			agent.destination = gM.files[fileTarget].positions[0].pos.position;
			gM.files[fileTarget].positions[1].prise = false;

			StartCoroutine(TimeTravel());
		}
		IEnumerator InExitQueue()
		{
			gM.files[fileTarget].positions[0].prise = false;
			gM.nbClientAct--; //<-- marche seulement si tout est bon 
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
			agent.destination = GameObject.Find("Sortie").transform.position;
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
	}
}