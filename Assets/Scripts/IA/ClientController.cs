using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ClientType {Normal, Presse,Vieux, Riche}

public enum ActPos {pos1, pos2, apartir}

namespace _Script{
	public class ClientController : MonoBehaviour
	{
		#region Variables

		public ClientType type = ClientType.Normal;
		public ActPos ActPos = ActPos.pos1;

		// SO avec tous les modèles de joueur

		private int fileTarget;

		[SerializeField] float timeService = 20f;
		[SerializeField] int posClientRandom;
		[SerializeField] ClientData clientData;
		[SerializeField] GameManager gM;
		[SerializeField] IdVerif verifID;
		[SerializeField] int intPosBaseClient;

		public NavMeshAgent agent;

		private bool premiereVerif = false;
		private bool FirstPlace = false;
		private bool SecondPlace = false;

		public GameObject prefabUICommande;
		//public GameObject prefabUIEmplacement;
		public Transform baseCommande;
		
		public float iDaleatoire;
		public GameObject[] listeObject;
		public bool iDEgal;
		#endregion

		#region Builtin Methods 
		void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			gM = FindObjectOfType<GameManager>();
			verifID = FindObjectOfType<IdVerif>();

			iDaleatoire = Random.Range(0, listeObject.Length);

			fileTarget = gM.targetClient;
			
			StartCoroutine("EnterShop");
		}

        private void Update()
        {
            if (!gM.files[fileTarget].positions[0].prise && ActPos == ActPos.pos2)
            {
				StartCoroutine(InQueue());
			}
		}

        #endregion

        #region CustomFunction

        IEnumerator EnterShop()
		{
			agent.destination = GameObject.Find("EntreeExt").transform.position;
			yield return new WaitForSeconds(3f);

			StartCoroutine("TimeTravel");
		}

		IEnumerator TimeTravel()
		{	
			if (!gM.files[fileTarget].positions[0].prise && !premiereVerif)
			{
				agent.destination = gM.files[fileTarget].positions[0].pos.position;
				gM.files[fileTarget].positions[0].prise = true;
				ActPos = ActPos.pos1;
				FirstPlace = true;
			}
			else if (gM.files[fileTarget].positions[0].prise && !gM.files[fileTarget].positions[1].prise && !premiereVerif)
			{
				agent.destination = gM.files[fileTarget].positions[1].pos.position;
				gM.files[fileTarget].positions[1].prise = true;
				ActPos = ActPos.pos2;
				SecondPlace = true;
			}
			premiereVerif = true;


			
            if (FirstPlace)
            {
				yield return new WaitForSeconds(5f);
				StartCoroutine(EnCommande());
			}
			
		}
		IEnumerator InQueue()
        {
			agent.destination = gM.files[fileTarget].positions[0].pos.position;
			gM.files[fileTarget].positions[1].prise = false;
			gM.files[fileTarget].positions[0].prise = true;
			ActPos = ActPos.pos1;
			FirstPlace = true;

			yield return new WaitForSeconds(3f);

			StartCoroutine(EnCommande());
		}
		
		IEnumerator EnCommande()
        {
			prefabUICommande.GetComponent<CommandUI>().textNameClient.text = gameObject.name.ToString();
			prefabUICommande.GetComponent<CommandUI>().timeClient.text = timeService.ToString();
			Sprite sprite = prefabUICommande.GetComponent<CommandUI>().imageObjet;
			sprite = listeObject[(int)iDaleatoire].GetComponent<ObjectDefinition>().imageObjet;

			GameObject newClientUI = Instantiate(prefabUICommande, baseCommande.transform.position, baseCommande.transform.rotation);
			newClientUI.transform.parent = GameObject.Find("Content").transform;
			newClientUI.transform.position = GameObject.Find("BaseDeCommande").transform.position;
			
			verifID.clientsWait.Add(gameObject.GetComponent<ClientController>());
			if (iDEgal)
			{
				//StartCoroutine(InExitQueue());
			}

			yield return new WaitForSeconds(timeService);

			//StartCoroutine(InExitQueue());
			InExitQueue();
		}

		public void InExitQueue()
		{
			verifID.clientsWait.Remove(gameObject.GetComponent<ClientController>());
			ActPos = ActPos.apartir;
			gM.files[fileTarget].positions[0].prise = false;
			
			//Destroy(gM.prefabUICommande);
			if (posClientRandom < 3)
			{
				agent.destination = GameObject.Find("Posexitqueue2").transform.position;
			}
			else
			{
				agent.destination = GameObject.Find("Posexitqueue").transform.position;
			}

			//yield return new WaitForSeconds(1.2f);

			StartCoroutine("ExitShop");
		}

		IEnumerator ExitShop()
		{
			agent.destination = GameObject.Find("EntreeExt").transform.position;
			yield return new WaitForSeconds(3f);
			StartCoroutine(DestroyClient());
		}
		IEnumerator DestroyClient()
        {
			agent.destination = GameObject.Find("Sortie").transform.position;
			gM.nbClientAct--; //<-- marche seulement si tout est bon 

			yield return new WaitForSeconds(4f);

			Destroy(gameObject);
		}
		/*
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("posPourCommande"))
			{
				gM.prefabUICommande.SetActive(true);
			}
		}*/

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
			//gM.namePrefabUICommande.text = infos.name.ToString();
			timeService = infos.time;
			//gM.timePrefabUICommande.text = infos.time.ToString();
			//gM.spritePrefabUICommande = infos.sprite;
		}
		#endregion
	}
}