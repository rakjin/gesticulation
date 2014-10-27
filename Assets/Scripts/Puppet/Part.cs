using UnityEngine;
using System.Collections;

public class Part : MonoBehaviour {

	static readonly Color normalColor = new Color (0.5f, 0.5f, 0.5f);
	static readonly Color highlightedColor = new Color (0.125f, 1, 0);

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
			StoredJoint.breakForce = original.breakForce;
			StoredJoint.breakTorque = original.breakTorque;
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
			StoredRigidbody.isKinematic = original.isKinematic;
			StoredRigidbody.mass = original.mass;
			StoredRigidbody.maxAngularVelocity = original.maxAngularVelocity;
			StoredRigidbody.position = original.position;
			StoredRigidbody.rotation = original.rotation;
			StoredRigidbody.sleepAngularVelocity = original.sleepAngularVelocity;
			StoredRigidbody.sleepVelocity = original.sleepVelocity;
			StoredRigidbody.useGravity = original.useGravity;
			StoredRigidbody.velocity = original.velocity;
			DestroyImmediate(original);
		}
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
	}

	void OnMouseExit () {
		Highlighted = false;
	}

	#endregion
}



#region internal class

internal class CharacterJointMock {
	public Vector3 anchor;
	public bool autoConfigureConnectedAnchor;
	public Vector3 axis;
	public float breakForce;
	public float breakTorque;
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
	public bool isKinematic;
	public float mass;
	public float maxAngularVelocity;
	public Vector3 position;
	public Quaternion rotation;
	public float sleepAngularVelocity;
	public float sleepVelocity;
	public bool useGravity;
	public Vector3 velocity;
}

#endregion

