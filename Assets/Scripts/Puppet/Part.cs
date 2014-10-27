using UnityEngine;
using System.Collections;

public class Part : MonoBehaviour {

	static readonly Color normalColor = new Color (0.5f, 0.5f, 0.5f);
	static readonly Color highlightedColor = new Color (0.125f, 1, 0);

	private SpringJoint springJoint;
	private CharacterJointMock StoredJoint { get; set; }
	private RigidbodyMock StoredRigidbody { get; set; }

	void Awake () {
		StoreJoint ();
		StoreRigidbody ();
	}

	#region Store

	void StoreJoint() {
		CharacterJoint original = GetComponent<CharacterJoint> ();
		if (original) {
			StoredJoint = new CharacterJointMock();
			StoredJoint.anchor = original.anchor;
			StoredJoint.autoConfigureConnectedAnchor = original.autoConfigureConnectedAnchor;
			StoredJoint.axis = original.axis;
			StoredJoint.connectedAnchor = original.connectedAnchor;
			StoredJoint.connectedBody = original.connectedBody;
			StoredJoint.highTwistLimit = original.highTwistLimit;
			StoredJoint.lowTwistLimit = original.lowTwistLimit;
			StoredJoint.swing1Limit = original.swing1Limit;
			StoredJoint.swing2Limit = original.swing2Limit;
			StoredJoint.swingAxis = original.swingAxis;
			DestroyImmediate(original);
		}
	}

	void StoreRigidbody() {
		Rigidbody original = GetComponent<Rigidbody> ();
		if(original) {
			StoredRigidbody = new RigidbodyMock();
			StoredRigidbody.angularDrag = original.angularDrag;
			StoredRigidbody.angularVelocity = original.angularVelocity;
			StoredRigidbody.centerOfMass = original.centerOfMass;
			StoredRigidbody.constraints = original.constraints;
			StoredRigidbody.drag = original.drag;
			StoredRigidbody.mass = original.mass;
			StoredRigidbody.maxAngularVelocity = original.maxAngularVelocity;
			StoredRigidbody.position = original.position;
			StoredRigidbody.rotation = original.rotation;
			StoredRigidbody.sleepAngularVelocity = original.sleepAngularVelocity;
			StoredRigidbody.sleepVelocity = original.sleepVelocity;
			DestroyImmediate(original);
		}
	}

	#endregion


	#region Restore

	void RestoreJoint() {
		if (StoredJoint != null) {
			CharacterJoint restored = gameObject.AddComponent<CharacterJoint>();
			restored.anchor = StoredJoint.anchor;
			restored.autoConfigureConnectedAnchor = StoredJoint.autoConfigureConnectedAnchor;
			restored.axis = StoredJoint.axis;
			restored.connectedAnchor = StoredJoint.connectedAnchor;
			restored.connectedBody = StoredJoint.connectedBody;
			restored.highTwistLimit = StoredJoint.highTwistLimit;
			restored.lowTwistLimit = StoredJoint.lowTwistLimit;
			restored.swing1Limit = StoredJoint.swing1Limit;
			restored.swing2Limit = StoredJoint.swing2Limit;
			restored.swingAxis = StoredJoint.swingAxis;
			restored.breakForce = Mathf.Infinity;
			restored.breakTorque = Mathf.Infinity;
		}
	}

	void RestoreRigidbody() {
		if (StoredRigidbody != null) {
			Rigidbody restored = gameObject.AddComponent<Rigidbody>();
			restored.angularDrag = StoredRigidbody.angularDrag;
			restored.angularVelocity = StoredRigidbody.angularVelocity;
			restored.centerOfMass = StoredRigidbody.centerOfMass;
			restored.constraints = StoredRigidbody.constraints;
			restored.drag = StoredRigidbody.drag;
			restored.mass = StoredRigidbody.mass;
			restored.maxAngularVelocity = StoredRigidbody.maxAngularVelocity;
			restored.position = StoredRigidbody.position;
			restored.rotation = StoredRigidbody.rotation;
			restored.sleepAngularVelocity = StoredRigidbody.sleepAngularVelocity;
			restored.sleepVelocity = StoredRigidbody.sleepVelocity;
			restored.useGravity = false;
			restored.isKinematic = false;
		}
	}

	#endregion


	#region Connect to / Disconnect from external rigidbody

	void ConnectToRigidbody(Rigidbody externalRigidbody) {

		RestoreRigidbody ();
		RestoreJoint ();

		GetComponent<Rigidbody> ().isKinematic = false;
		springJoint = gameObject.AddComponent<SpringJoint> ();
		springJoint.connectedBody = externalRigidbody;
		springJoint.minDistance = 0.0625f;
		springJoint.maxDistance = 0.125f;
		springJoint.spring = 10;
	}

	void DisconnectFromRigidbody() {
		DestroyImmediate (springJoint);
		DestroyImmediate (GetComponent<Joint> ());
		DestroyImmediate (GetComponent<Rigidbody> ());
	}

	#endregion


	#region Highlight

	private bool highlighted = false;
	public bool Highlighted {

		set {
			if (highlighted == value) {
				return;
			}

			highlighted = value;
			Color tint = (highlighted ? highlightedColor : normalColor);
			LeanTween.color (gameObject, tint, 0.125f);
		}

		get {
			return highlighted;
		}

	}

	void OnMouseEnter () {
		Highlighted = true;
		ConnectToRigidbody (Controller.Instance.FixedSphere.rigidbody);
	}

	void OnMouseExit () {
		Highlighted = false;
		DisconnectFromRigidbody ();
	}

	#endregion
}



#region internal class

internal class CharacterJointMock {
	public Vector3 anchor;
	public bool autoConfigureConnectedAnchor;
	public Vector3 axis;
	public Vector3 connectedAnchor;
	public Rigidbody connectedBody;
	public SoftJointLimit highTwistLimit;
	public SoftJointLimit lowTwistLimit;
	public SoftJointLimit swing1Limit;
	public SoftJointLimit swing2Limit;
	public Vector3 swingAxis;
}

internal class RigidbodyMock {
	public float angularDrag;
	public Vector3 angularVelocity;
	public Vector3 centerOfMass;
	public RigidbodyConstraints constraints;
	public float drag;
	public float mass;
	public float maxAngularVelocity;
	public Vector3 position;
	public Quaternion rotation;
	public float sleepAngularVelocity;
	public float sleepVelocity;
}

#endregion

