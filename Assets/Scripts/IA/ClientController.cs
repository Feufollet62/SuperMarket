using System.Collections;	
using UnityEngine;
using UnityEngine.AI;

public enum ClientType {Normal, Presse, Vieux, Riche}

public enum Position {Pos1, Pos2, APartir}

namespace _Script{
	public class ClientController : MonoBehaviour
	{
		#region Variables

		public ClientType type = ClientType.Normal;
		public Position PosActuelle = Position.Pos1;

		// SO avec tous les modèles de joueur

		private int fileTarget;

		[SerializeField] private float timeService = 20f;
		//[SerializeField] private int fileChoiseClient;
		private GameManager manager;
		private IdVerif verifID;

		private NavMeshAgent agent;

		private bool _premiereVerif = false;
		private bool _firstPlace = false;

		public GameObject prefabUICommande;
		private GameObject newClientUI;

		public int iDAleatoire;
		public ObjectData[] listeObject; // Part dans game manager
		public bool iDEgal;
		
		#endregion

		#region Builtin Methods 
		void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			manager = FindObjectOfType<GameManager>();
			verifID = FindObjectOfType<IdVerif>();

			iDAleatoire = Random.Range(0, listeObject.Length);

			fileTarget = manager.targetClient;
			
			StartCoroutine(nameof(EnterShop));
		}

        private void Update()
        {
            if (!manager.files[fileTarget].positions[0].occupied && PosActuelle == Position.Pos2)
            {
				StartCoroutine(InQueue());
			}
		}

        #endregion

        #region CustomFunction

        IEnumerator EnterShop() // Stop aux coroutines
		{
			agent.destination = manager.porteEntree.position;
			yield return new WaitForSeconds(3f);

			StartCoroutine(nameof(TimeTravel));
		}

		IEnumerator TimeTravel()
		{	
			if (!manager.files[fileTarget].positions[0].occupied && !_premiereVerif)
			{
				agent.destination = manager.files[fileTarget].positions[0].pos.position;
				manager.files[fileTarget].positions[0].occupied = true;
				PosActuelle = Position.Pos1;
				_firstPlace = true;
			}
			else if (manager.files[fileTarget].positions[0].occupied && !manager.files[fileTarget].positions[1].occupied && !_premiereVerif)
			{
				agent.destination = manager.files[fileTarget].positions[1].pos.position;
				manager.files[fileTarget].positions[1].occupied = true;
				PosActuelle = Position.Pos2;
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
			agent.destination = manager.files[fileTarget].positions[0].pos.position;
			manager.files[fileTarget].positions[1].occupied = false;
			manager.files[fileTarget].positions[0].occupied = true;
			PosActuelle = Position.Pos1;
			_firstPlace = true;

			yield return new WaitForSeconds(3f);

			StartCoroutine(PasserCommande());
		}
		
		IEnumerator PasserCommande()
        {
	        CommandUI ui = Instantiate(prefabUICommande, manager.uICommande.position , manager.uICommande.rotation).GetComponent<CommandUI>();

			ui.textNameClient.text = gameObject.name;
			ui.timeClient.text = timeService.ToString();
			ui.afficheObjet.sprite = listeObject[iDAleatoire].image;

			ui.gameObject.SetActive(true);
			ui.transform.parent = manager.prefabUIEmplacementContent.transform;
			ui.gameObject.transform.position = Vector3.zero;
			ui.gameObject.transform.position = manager.uICommande.position;
			newClientUI = ui.gameObject;


			manager.uICommande.position += Vector3.right * 130; // C'est quoi ce 130 ?
			manager.listeUI.Add(ui.gameObject);

			verifID.clientsWait.Add(this);

			yield return new WaitForSeconds(timeService);
			
			ExitQueue();
		}

		public void ExitQueue()
		{
			verifID.clientsWait.Remove(this);
			PosActuelle = Position.APartir;
			manager.files[fileTarget].positions[0].occupied = false;
			
            for (int i = 1; i < manager.listeUI.Count; i++)
            {
				manager.listeUI[i].transform.position = new Vector3(manager.listeUI[i].transform.position.x - 130, manager.listeUI[i].transform.position.y, manager.listeUI[i].transform.position.z);
			}
            
			manager.listeUI.Remove(newClientUI);
			manager.uICommande.transform.position = new Vector3(manager.uICommande.position.x - 130, manager.uICommande.position.y, manager.uICommande.position.z);
			newClientUI.SetActive(false);

			
			if (fileTarget > 2) // nani the fuck ?
			{
				agent.destination = manager.sortieQueue2.position;
			}
			else
			{
				agent.destination = manager.sortieQueue1.position;
			}
			
			StartCoroutine(ExitShop());
		}

		IEnumerator ExitShop()
		{
			agent.destination = manager.porteSortie.position;
			yield return new WaitForSeconds(3f);
			StartCoroutine(DestroyClient());
		}
		IEnumerator DestroyClient()
        {
			agent.destination = manager.despawn.position;
			manager.clientList.Remove(this);

			yield return new WaitForSeconds(4f);

			Destroy(gameObject);
		}

		public void SetupClient(ClientInfo info, int target)
		{
			type = info.type;
			fileTarget = target;

			MeshFilter mFilter = GetComponent<MeshFilter>();
			MeshRenderer mRender = GetComponent<MeshRenderer>();

			mFilter.sharedMesh = info.model;
			mRender.sharedMaterial = info.material;

			gameObject.name = info.name;
			timeService = info.time;
		}
		#endregion
	}
}