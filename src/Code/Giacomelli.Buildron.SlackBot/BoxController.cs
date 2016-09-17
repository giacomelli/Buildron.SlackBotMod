using Buildron.Domain.Builds;
using UnityEngine;

namespace Giacomelli.Buildron.SlackBot
{
	public class BoxController : MonoBehaviour
	{
		#region Properties
		public float MinX = -5;
		public float MaxX = 5;

		public float MinY = 25;
		public float MaxY = 35;

		public float MinZ = -5;
		public float MaxZ = -1;
		#endregion

		#region Methods
		private void Awake()
		{
			transform.position = new Vector3(
				Random.Range(MinX, MaxX),
				Random.Range(MinY, MaxY),
				Random.Range(MinZ, MaxZ));
			
			gameObject.AddComponent<Rigidbody>();
		}

		public void SetModel(IBuild build)
		{
			Color color;

			if (build.IsFailed())
			{
				color = Color.red;
			}
			else if (build.IsQueued())
			{
				color = Color.blue;
			}
			else if (build.IsRunning())
			{
				color = Color.yellow;
			}
			else
			{
				color = Color.green;
			}

			GetComponent<Renderer>().material.color = color;
		}
		#endregion
	}
}