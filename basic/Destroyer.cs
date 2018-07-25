using UnityEngine;

namespace HermesUtilities{
	public class Destroyer : MonoBehaviour {
		public float time;
		private float currentTime;
		
		void Start () {
			if (time < 0) time = 0;
		}

		void Update () {
			currentTime += Time.deltaTime;
			
			if (currentTime >= time && transform.gameObject != null) 
				Destroy(gameObject);
		}
	}
}