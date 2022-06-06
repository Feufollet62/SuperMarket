using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace _Script{
	public class navscript : MonoBehaviour
	{
		#region Variables
		[SerializeField] float timeService = 10f;
		#endregion

		#region Properties

		#endregion
		
		#region Builtin Methods 
		void Start()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.destination = GameObject.Find("Pos2").transform.position;
			StartCoroutine("TimeService");
		}


		void Update()
		{

		}

		IEnumerator TimeService()
        {
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			yield return new WaitForSeconds(timeService);
			agent.destination = GameObject.Find("Pos3").transform.position;
		}
		#endregion
	}
}