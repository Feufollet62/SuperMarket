using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace _Script{
	public class ClientController : MonoBehaviour
	{
		#region Variables
		[SerializeField] float timeService = 20f;
		[SerializeField] int posClientRandom;
		[SerializeField] ClientData clientData;
		[SerializeField] ClientSpawner clientSpawner;
		[SerializeField] float timeduClient;
		[SerializeField] GameManageur gM;
		[SerializeField] List<Transform> posBaseClient;
		[SerializeField] int intPosBaseClient;
		#endregion

		#region Properties

		IEnumerator EnterShop()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Posentre").transform.position;
			yield return new WaitForSeconds(1f);

			StartCoroutine("TimeTravel");
		}
		
		IEnumerator TimeTravel()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			
			
			if (clientSpawner.aleaspawn == 1)
			{
				agent.destination = gM.fileClient1[0].position;
				gM.fileClient1.Remove(gM.fileClient1[0]);
			}
			else if (clientSpawner.aleaspawn == 2)
			{
				agent.destination = gM.fileClient2[0].position;
				gM.fileClient2.Remove(gM.fileClient2[0]);
			}
			else if (clientSpawner.aleaspawn == 3)
			{
				agent.destination = gM.fileClient3[0].position;
				gM.fileClient3.Remove(gM.fileClient3[0]);
			}

			yield return new WaitForSeconds(timeService);
			//je vais attendre qu'une nouvelle queue se place
			//StartCoroutine("InExitQueue");
		}
		IEnumerator InExitQueue()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			if (clientSpawner.aleaspawn == 1)
			{
				gM.fileClient1.Add(posBaseClient[0]);
			}
			else if (clientSpawner.aleaspawn == 2)
			{
				gM.fileClient2.Add(posBaseClient[0]);
			}
			else if (clientSpawner.aleaspawn == 3)
			{
				gM.fileClient3.Add(posBaseClient[0]);
			}
			
			Destroy(clientSpawner.UIcommand);
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
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Pos5").transform.position;
			yield return new WaitForSeconds(1f);
			exitShop();
		}

		void exitShop()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Pos5").transform.position;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("posPourCommande"))
			{
				clientSpawner.UIcommand.SetActive(true);
			}
		}
		#endregion

		#region Builtin Methods 
		void Start()
		{
			gM = FindObjectOfType<GameManageur>();
            if (clientSpawner.aleaspawn == 1)
            {
				posBaseClient[0] = gM.fileClient1[0];
			}
			else if (clientSpawner.aleaspawn == 2)
			{
				posBaseClient[0] = gM.fileClient2[0];
			}
			else if(clientSpawner.aleaspawn == 3)
			{
				posBaseClient[0] = gM.fileClient3[0];
			}

			timeduClient = clientData.Time;
			StartCoroutine("EnterShop");
		}

        #endregion
    }
}