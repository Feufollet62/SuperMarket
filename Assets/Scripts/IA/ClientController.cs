using System.Collections;	
using UnityEngine;
using UnityEngine.AI;

public enum ClientType {Normal, Presse, Vieux, Riche}

namespace _Script{
	public class ClientController : MonoBehaviour
	{
		#region Variables
		
		[SerializeField] private float timeService = 20f;
		[SerializeField] private int posClientRandom;
		private GameManager manager;
		private IdVerif verifID;

		private NavMeshAgent agent;

		private bool _premiereVerif = false;
		private bool _firstPlace = false;

		public GameObject prefabUICommande;
		private GameObject newClientUI;

		public int idCommande;
		public ObjectData[] listeObject; // Part dans game manager

		private PositionFile posActuelle;

		// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		private int fileTarget = 0;
		
		#endregion

		#region Builtin Methods 
		private void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
			manager = FindObjectOfType<GameManager>();
			verifID = FindObjectOfType<IdVerif>();

			idCommande = Random.Range(0, listeObject.Length);
			
			StartCoroutine(nameof(EnterShop));
		}

        private void Update()
        {
            if (!posActuelle.occupied && !posActuelle.canOrder) // Normalement on peut tej ça si le manager nous assigne
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
			PositionFile pos0 = manager.files[fileTarget].positions[0];
			PositionFile pos1 = manager.files[fileTarget].positions[1];
			
			if (!pos0.occupied && !_premiereVerif)
			{
				GoToPosition(pos0);
				_firstPlace = true;
			}
			else if (pos0.occupied && !pos1.occupied && !_premiereVerif)
			{
				GoToPosition(pos1);
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
			agent.destination = manager.files[fileTarget].positions[0].transform.position;
			manager.files[fileTarget].positions[1].occupied = false;
			manager.files[fileTarget].positions[0].occupied = true;
			_firstPlace = true;

			yield return new WaitForSeconds(3f);

			StartCoroutine(PasserCommande());
		}
		
		IEnumerator PasserCommande()
        {
	        CommandUI ui = Instantiate(prefabUICommande, manager.uICommande.position , manager.uICommande.rotation).GetComponent<CommandUI>();

			ui.textNameClient.text = gameObject.name;
			ui.timeClient.text = timeService.ToString();
			ui.afficheObjet.sprite = listeObject[idCommande].image;

			ui.gameObject.SetActive(true);
			ui.transform.parent = manager.prefabUIEmplacement.transform;
			ui.transform.position = manager.uICommande.position;
			
			manager.uICommande.position += Vector3.right * 130; // C'est quoi ce 130 ?
			manager.listeUI.Add(newClientUI);

			yield return new WaitForSeconds(timeService);
			
			ExitQueue();
		}

		public void ExitQueue()
		{
			//verifID.clientsWait.Remove(this);
			manager.files[fileTarget].positions[0].occupied = false;
			
            for (int i = 1; i < manager.listeUI.Count; i++)
            {
				manager.listeUI[i].transform.position = new Vector3(manager.listeUI[i].transform.position.x - 130, manager.listeUI[i].transform.position.y, manager.listeUI[i].transform.position.z);
			}
            
			manager.listeUI.Remove(newClientUI);
			manager.uICommande.transform.position = new Vector3(manager.uICommande.position.x - 130, manager.uICommande.position.y, manager.uICommande.position.z);
			newClientUI.SetActive(false);

			if (posClientRandom < 3) // nani the fuck ?
			{
				agent.destination = manager.sortieQueue2.position;
			}
			else
			{
				agent.destination = manager.sortieQueue1.position;
			}
			
			StartCoroutine("ExitShop");
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

		public void SetupClient(ClientInfo info, PositionFile target)
		{
			GoToPosition(target);

			MeshFilter mFilter = GetComponent<MeshFilter>();
			MeshRenderer mRender = GetComponent<MeshRenderer>();

			mFilter.sharedMesh = info.model;
			mRender.sharedMaterial = info.material;

			gameObject.name = info.name;
			timeService = info.time;
		}

		public void GoToPosition(PositionFile pos)
		{
			if(pos.occupied) return;

			// On part de notre place actuelle et on la marque comme vide
			posActuelle.occupied = false;

			// On recup la nouvelle position
			posActuelle = pos;
			pos.currentClient = this;
			pos.occupied = true;
			
			agent.destination = pos.transform.position;
		}
		
		#endregion
	}
}