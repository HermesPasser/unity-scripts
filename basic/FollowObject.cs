using UnityEngine;

namespace HermesUtilities{
	public class FollowObject : MonoBehaviour {
		public Transform objectForFollow;
		public float velocity = 1;
		
		void LateUpdate () {
			if (objectForFollow != null)
				transform.position = Vector3.Lerp(transform.position, new Vector3(objectForFollow.position.x, objectForFollow.position.y, transform.position.z), velocity);
		}
	}
}