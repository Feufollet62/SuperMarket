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
		#endregion

		#region Properties

		#endregion

		#region Builtin Methods 
		void Start()
		{
			posClientRandom = Random.Range(0, posClient.Length);
			//NavMeshAgent agent = GetComponent<NavMeshAgent>();
			//agent.destination = GameObject.Find("Posentree").transform.position;
			//agent.destination = GameObject.Find("Pos2").transform.position;
			//agent.destination = posClient[posClientRandom].position;
			StartCoroutine("EnterShop");
		}

		IEnumerator EnterShop()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Posentre").transform.position;
			yield return new WaitForSeconds(1f);
			
			StartCoroutine("InShop");
		}
		IEnumerator InShop()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = posFinShop.position;
			yield return new WaitForSeconds(0.4f);

			StartCoroutine("TimeService");
		}

		IEnumerator TimeService()
        {
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = posClient[posClientRandom].position;
			yield return new WaitForSeconds(timeService);
			StartCoroutine("InShopExit");
		}

		IEnumerator InShopExit()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = posFinShop.position;
			yield return new WaitForSeconds(4f);

			StartCoroutine("ExitShop");
		}
		IEnumerator ExitShop()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Posentre").transform.position;
			yield return new WaitForSeconds(1f);
			exitShop();
		}

		void exitShop()
        {
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Pos5").transform.position;
		}
		#endregion
	}
}