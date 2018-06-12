using UnityEngine;

namespace HermesUtilities.Movimentation{
	public class BasicTopDownMovimentationPC : MonoBehaviour {
		private Rigidbody2D rbody;
		public float speed = 3f;
	
		void Start(){
			rbody = GetComponent<Rigidbody2D> ();
		}

		void Update(){
			Vector2 mov = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (mov != Vector2.zero) 
				rbody.MovePosition (rbody.position + mov * speed * Time.deltaTime);
		}
	}
}