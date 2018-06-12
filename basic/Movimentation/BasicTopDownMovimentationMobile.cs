using UnityEngine;

namespace HermesUtilities.Movimentation{
	public class BasicTopDownMovimentationMobile : MonoBehaviour {
		private Rigidbody2D rbody;
		public float speed = 3f;

		public float x;
		public float y;
		public float width;
		public float height;
		void Start(){
			rbody = GetComponent<Rigidbody2D> ();
		}
		

		void OnGUI(){
			while (GUI.RepeatButton (new Rect (x, y - height * 2, width, height), "^"))
				movimentationMobile (0f, 1f, Vector2.up * speed);
			while (GUI.RepeatButton (new Rect(x,y, width, height), "V"))
				movimentationMobile (0f, -1f, -Vector2.up* speed);
			while (GUI.RepeatButton (new Rect(width ,y-height, width, height), ">"))
				movimentationMobile (1f, 0f, -Vector2.left* speed);
			while (GUI.RepeatButton (new Rect (0, y - height, width, height), "<"))
				movimentationMobile (-1f, 0f, Vector2.left* speed);
		}

		private void movimentationMobile(float x, float y, Vector2 dir){
			rbody.MovePosition (rbody.position + dir* Time.deltaTime);
		}
	}
}