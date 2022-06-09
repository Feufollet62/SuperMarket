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

			StartCoroutine("TimeService");
		}
		
		IEnumerator TimeService()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = gM.posClient[posClientRandom].position;
			gM.posClient.Remove(gM.posClient[posClientRandom]);
			
			yield return new WaitForSeconds(timeService);
			StartCoroutine("InExitQueue");
		}
		IEnumerator InExitQueue()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			if (posClientRandom < 3)
			{
				agent.destination = GameObject.Find("Posexitqueue2").transform.position;
			}
			else
			{
				agent.destination = GameObject.Find("Posexitqueue").transform.position;
			}

			yield return new WaitForSeconds(1f);

			gM.posClient.Add(posBaseClient[0]);

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
			if (other.CompareTag("FinRun"))
			{
				Debug.Log("Je vais partir");
			}
		}
		#endregion

		#region Builtin Methods 
		void Start()
		{
			gM = FindObjectOfType<GameManageur>();
			posClientRandom = Random.Range(0, gM.posClient.Count);
			posBaseClient[0] = gM.posClient[posClientRandom];
			timeduClient = clientData.Time;
			StartCoroutine("EnterShop");
		}

        #endregion
    }
}