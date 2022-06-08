using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script{
	public class ClientSpawner : MonoBehaviour
	{
		#region Variables
		public GameObject[] Client;
		[SerializeField] private float limitSpawn;
		[SerializeField] private float cooldownSpawn = 4f;
		//[SerializeField] private int canSpawn;
		#endregion

		#region Builtin Methods
		void Start()
        {
			limitSpawn = cooldownSpawn;
			//canSpawn = 1;
		}

		void Update()
		{

			/*limitSpawn -= Time.deltaTime;
			if (limitSpawn < 0 && canSpawn <= 2)
			{
                for (int i = 0; i < slime.Length; i++)
                {
					GameObject enemy = Instantiate(slime[i], transform.position, transform.rotation);
					enemy.transform.parent = transform;
					limitSpawn = cooldownSpawn;
					canSpawn++;
				}
			}*/

			/*****************************************************/

			/*limitSpawn -= Time.deltaTime;
			if (limitSpawn < 0)
			{
				if (FindObjectOfType<PlayerManager>().score != 5)
                {
					GameObject enemySimple = Instantiate(slime[0], transform.position, transform.rotation);
					enemySimple.transform.parent = transform;
					limitSpawn = cooldownSpawn;
                }
                else
                {
					GameObject enemyBoss = Instantiate(slime[1], transform.position, transform.rotation);
					enemyBoss.transform.parent = transform;
					limitSpawn = cooldownSpawn;
				}
			}*/

			/*****************************************************/

			limitSpawn -= Time.deltaTime;
			if (limitSpawn < 0)
			{
				if (FindObjectOfType<GameManager>().ClientContent <= 300)
				{
					GameObject enemyPrefab;
					enemyPrefab = Client[0];
					GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
					newEnemy.transform.parent = transform;
					limitSpawn = cooldownSpawn;
					FindObjectOfType<GameManager>().ClientContent++;
				}
			}
		}
		#endregion

		#region Properties

		#endregion
	}
}