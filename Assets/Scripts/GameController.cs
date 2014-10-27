using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public Transform Sphere { get; private set; }

	Poser defaultPoser;

	Controller controller = new Controller();
	Vector3 controllerPosition = new Vector3(0, -0.5f, 0);
	float controllerScale = 10;

	// Use this for initialization
	void Start () {

		Instance = this;

		Sphere = GameObject.Find ("/Sphere").transform;

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		defaultPoser = defaultPuppet.GetComponent<Poser> ();

		//defaultPoser.Pose (Pose.RandomPose01 (), 3f);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnDrawGizmos() {
		if (controller == null) {
			return;
		}

		Frame frame = controller.Frame ();
		if (frame.Hands.Count > 0) {
			Vector3 palmPosition = frame.Hands[0].PalmPosition.ToUnityScaled();
			Vector3 thumbPosition = frame.Hands[0].Fingers[0].TipPosition.ToUnityScaled();
			Vector3 indexPosition = frame.Hands[0].Fingers[1].TipPosition.ToUnityScaled();
			palmPosition *= controllerScale;
			thumbPosition *= controllerScale;
			indexPosition *= controllerScale;
			palmPosition += controllerPosition;
			thumbPosition += controllerPosition;
			indexPosition += controllerPosition;

			if (Vector3.Distance(indexPosition, thumbPosition) < 0.25f) {
				Gizmos.color = Color.red;
				Sphere.position = (thumbPosition + indexPosition) * 0.5f;
			}

			Gizmos.DrawWireSphere(palmPosition, 0.25f);
			Gizmos.DrawWireSphere(thumbPosition, 0.125f);
			Gizmos.DrawWireSphere(indexPosition, 0.125f);
		}
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 200, 30), "(un)select")) {
			defaultPoser.Head.Highlighted = !defaultPoser.Head.Highlighted;
			defaultPoser.Chest.Highlighted = !defaultPoser.Chest.Highlighted;
		}
	}

}
