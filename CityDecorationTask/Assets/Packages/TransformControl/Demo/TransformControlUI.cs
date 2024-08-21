using UnityEngine;

namespace mattatz.TransformControl.Demo {

	public class TransformControlUI : MonoBehaviour {

		[SerializeField] TransformControl control;
        private void Start()
        {
			TryGetComponent<TransformControl>(out control);
        }
        void Update() {
            control?.Control();

			if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Mouse1))
			{
				OnModeChanged(1);
			}

			if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.Mouse1))
			{
				OnModeChanged(0);
			}

			if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Mouse1))
			{
				OnModeChanged(2);
			}

		}

		public void OnModeChanged(int index) {
			switch(index) {
			case 0:
				control.mode = TransformControl.TransformMode.Translate;
				break;
			case 1:
				control.mode = TransformControl.TransformMode.Rotate;
				break;
			case 2:
				control.mode = TransformControl.TransformMode.Scale;
				break;
			}
		}

		public void OnCoordinateChanged(int index) {
			switch(index) {
			case 0:
				control.global = false;
				break;
			case 1:
				control.global = true;
				break;
			}
		}

	}

}
