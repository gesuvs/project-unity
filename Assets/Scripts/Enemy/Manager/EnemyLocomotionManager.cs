using System;
using UnityEngine;

namespace Enemy.Manager
{
	public class EnemyLocomotionManager : MonoBehaviour
	{
		private EnemyManager EnemyManager;

		private LayerMask detectionLayer;

		private void Awake()
		{
			EnemyManager = GetComponent<EnemyManager>();
		}

		public void HandleDetection()
		{
			var colliders = Physics
				.OverlapSphere(
					transform.position, 
					EnemyManager.detectionRadius, 
					detectionLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				
			}
			
		}
	}
}