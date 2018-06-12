using UnityEngine;
using System.Collections;

// Extract from Dev'lusion

namespace HermesUtilities{
	public class GameLight : MonoBehaviour {

		public float velocity = 2;
		public float time = 0;
		private bool status;
		public bool isFlashing;
		public bool isDisable;
		public Light objLight;


		void Start () {
			if (velocity < 0) velocity = 0;
			if (time < 0) time = 0;
		}

		void Update () {
			if (isDisable) {
				changeStatus (false);
				return;
			}

			if (isFlashing) {
				flashing ();
				return;
			}

			changeStatus (true);
		}

		private void changeStatus (bool status){
			objLight.enabled = status;
		}

		private void flashing () {
			time += Time.deltaTime;

			if (velocity <= time) {
				status = !status;
				changeStatus (status);
				time = 0;
			}
		}
	}
}