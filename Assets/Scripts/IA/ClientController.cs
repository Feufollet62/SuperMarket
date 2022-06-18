using System.Collections;	
using UnityEngine;
using UnityEngine.AI;

public enum ClientType {Normal, Presse, Vieux, Riche}

public enum ActPos {Pos1, Pos2, APartir}

namespace _Script{
	public class ClientController : MonoBehaviour
	{
		#region Variables

		public ClientType type = ClientType.Normal;
		public ActPos ActPos = ActPos.Pos1;

		// SO avec tous les modèles de joueur

		private int fileTarget;

		[SerializeField] private float timeService = 20f;
		[SerializeField] private int posClientRandom;
		[SerializeField] private ClientData clientData;
		[SerializeField] private GameManager gM;
		[SerializeField] private IdVerif verifID;

		private NavMeshAgent agent;

		private bool _premiereVerif = false;
		private bool _firstPlace = false;

		public GameObject prefabUICommande;
		private GameObject newClientUI;

		public int iDAleatoire;
		public GameObject[] listeObject;
		public bool iDEgal;
		#endregion

		#region Builtin Methods 
		void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			gM = FindObjectOfType<GameManager>();
			verifID = FindObjectOfType<IdVerif>();

			iDAleatoire = Random.Range(0, listeObject.Length);

			fileTarget = gM.targetClient;
			
			StartCoroutine("EnterShop");
		}

        private void Update()
        {
            if (!gM.files[fileTarget].positions[0].prise && ActPos == ActPos.Pos2)
            {
				StartCoroutine(InQueue());
			}
		}

        #endregion

        #region CustomFunction

        IEnumerator EnterShop()
		{
			agent.destination = GameObject.Find("EntreeExt").transform.position; // Aled
			yield return new WaitForSeconds(3f);

			StartCoroutine(nameof(TimeTravel));
		}

		IEnumerator TimeTravel()
		{	
			if (!gM.files[fileTarget].positions[0].prise && !_premiereVerif)
			{
				agent.destination = gM.files[fileTarget].positions[0].pos.position;
				gM.files[fileTarget].positions[0].prise = true;
				ActPos = ActPos.Pos1;
				_firstPlace = true;
			}
			else if (gM.files[fileTarget].positions[0].prise && !gM.files[fileTarget].positions[1].prise && !_premiereVerif)
			{
				agent.destination = gM.files[fileTarget].positions[1].pos.position;
				gM.files[fileTarget].positions[1].prise = true;
				ActPos = ActPos.Pos2;
			}
			
			_premiereVerif = true;
			
            if (_firstPlace)
            {
				yield return new WaitForSeconds(5f);
				StartCoroutine(PasserCommande());
			}
		}
		IEnumerator InQueue()
        {
			agent.destination = gM.files[fileTarget].positions[0].pos.position;
			gM.files[fileTarget].positions[1].prise = false;
			gM.files[fileTarget].positions[0].prise = true;
			ActPos = ActPos.Pos1;
			_firstPlace = true;

			yield return new WaitForSeconds(3f);

			StartCoroutine(PasserCommande());
		}
		
		IEnumerator PasserCommande()
        {
	        CommandUI ui = Instantiate(prefabUICommande, gM.baseCommande.transform.position , gM.baseCommande.transform.rotation).GetComponent<CommandUI>();

			ui.textNameClient.text = gameObject.name;
			ui.timeClient.text = timeService.ToString();
			ui.afficheObjet.sprite = listeObject[iDAleatoire].GetComponent<Interactible>().imageObjet;

			ui.gameObject.SetActive(true);
			ui.transform.parent = gM.prefabUIEmplacement.transform;
			ui.transform.position = gM.baseCommande.position;
			
			gM.baseCommande.transform.position = new Vector3(gM.baseCommande.position.x + 130, gM.baseCommande.position.y, gM.baseCommande.position.z);
			gM.listeUI.Add(newClientUI);

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
			ActPos = ActPos.APartir;
			gM.files[fileTarget].positions[0].prise = false;
			
            for (int i = 1; i < gM.listeUI.Count; i++)
            {
				gM.listeUI[i].transform.position = new Vector3(gM.listeUI[i].transform.position.x - 130, gM.listeUI[i].transform.position.y, gM.listeUI[i].transform.position.z);
			}
			gM.listeUI.Remove(newClientUI);
			gM.baseCommande.transform.position = new Vector3(gM.baseCommande.position.x - 130, gM.baseCommande.position.y, gM.baseCommande.position.z);
			newClientUI.SetActive(false);

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