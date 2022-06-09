using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace _Script{
	public class navscript : MonoBehaviour
	{
		#region Variables
		[SerializeField] float timeService = 20f;
		[SerializeField] int posClientRandom;
		[SerializeField] Transform[] posClient;
		[SerializeField] Transform posFinShop;
		[SerializeField] 
		private LayerMask layertest;
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
			agent.destination = posClient[posClientRandom].position;
			yield return new WaitForSeconds(timeService);
			StartCoroutine("InExitQueue");
		}
		IEnumerator InExitQueue()
		{
			int numberforlayer = 10;
			layertest = LayerMask.NameToLayer("ClientEndCommand");
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
			posClientRandom = Random.Range(0, posClient.Length);
			StartCoroutine("EnterShop");
		}

        #endregion
    }
}