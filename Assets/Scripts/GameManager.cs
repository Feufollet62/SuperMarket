using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Script{
	public class GameManager : MonoBehaviour
	{
		#region Variables
		[SerializeField] private GameObject screenWin;
		[SerializeField] public int score;
		[SerializeField] public bool activation = false;
		[SerializeField] public int ClientContent;
		public List<GameObject> positionUsed;
		[SerializeField]public  Transform[] posClient;
		#endregion

		#region Builtin Methods
		void Start()
		{
			ClientContent = 1;
			positionUsed = new List<GameObject>();
		}


		void Update()
		{
			if (activation == false)
			{
				WinLevel();
			}
		}
		#endregion

		#region Properties
		void WinLevel()
        {
			if (score == 20)
			{
				screenWin.SetActive(true);
				activation = true;
			}
        }
		#endregion
	}
}