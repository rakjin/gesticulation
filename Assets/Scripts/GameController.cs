using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public Transform Sphere { get; private set; }

	Transform sphereContainer;
	Poser defaultPoser;

	Controller controller = new Controller();
	Vector3 controllerPosition = new Vector3(0, 2, 0);

	// Use this for initialization
	void Start () {

		Instance = this;

		Debug.Log (controller);

		sphereContainer = GameObject.Find ("/SphereContainer").transform;
		Sphere = GameObject.Find ("/SphereContainer/Sphere").transform;

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		defaultPoser = defaultPuppet.GetComponent<Poser> ();

		defaultPoser.Pose (Pose.RandomPose01 (), 3f);
	}
	
	// Update is called once per frame
	void Update () {
		sphereContainer.Rotate (new Vector3 (0, 0.25f, 0));
	}

	void OnDrawGizmos() {
		if (controller == null) {
			return;
		}

		Frame frame = controller.Frame ();
		if (frame.Hands.Count > 0) {
			Vector3 palmPosition = frame.Hands[0].PalmPosition.ToUnityScaled();
			Vector3 thumbPosition = frame.Hands[0].Fingers[0].StabilizedTipPosition.ToUnityScaled();
			Vector3 indexPosition = frame.Hands[0].Fingers[1].StabilizedTipPosition.ToUnityScaled();
			palmPosition *= 20;
			thumbPosition *= 20;
			indexPosition *= 20;
			palmPosition -= controllerPosition;
			thumbPosition -= controllerPosition;
			indexPosition -= controllerPosition;
			Gizmos.DrawWireSphere(palmPosition, 1);
			Gizmos.DrawWireSphere(thumbPosition, 0.3f);
			Gizmos.DrawWireSphere(indexPosition, 0.3f);
		}
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 200, 30), "(un)select")) {
			defaultPoser.Head.Highlighted = !defaultPoser.Head.Highlighted;
			defaultPoser.Chest.Highlighted = !defaultPoser.Chest.Highlighted;
		}
	}

}
