using UnityEngine;
using System.Collections;

public class Part : MonoBehaviour {

	public enum HighlightDegree {
		None,
		Half,
		Full,
		Pale,
	}

	static readonly Color normalColor = new Color (0.5f, 0.5f, 0.5f);
	static readonly Color halfHighlightedColor = new Color (0.5f, 0.875f, 0.875f);
	static readonly Color highlightedColor = new Color (0.125f, 1, 0);
	static readonly Color paleColor = new Color (0.375f, 0.625f, 1);

	private SpringJoint springJoint;
	private Vector3 anchor = Vector3.zero;

	private CharacterJointMock StoredJoint { get; set; }
	private RigidbodyMock StoredRigidbody { get; set; }

	void Awake () {
		StoreJoint ();
		StoreRigidbody ();

		Transform anchorGO = transform.Find ("DefaultAnchorForSpringJoint");
		if (anchorGO) {
			anchor = anchorGO.transform.localPosition;
		}
	}

	#region Store

	void StoreJoint() {
		CharacterJoint original = GetComponent<CharacterJoint> ();
		if (original) {
			StoredJoint = new CharacterJointMock();
			StoredJoint.axis = original.axis;
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
			restored.anchor = Vector3.zero; //StoredJoint.anchor;
			restored.axis = StoredJoint.axis;
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

	public void ConnectToRigidbody(Rigidbody externalRigidbody, Vector3 externalAnchor) {

		RestoreRigidbody ();
		RestoreJoint ();

		anchor = transform.InverseTransformPoint (externalRigidbody.transform.TransformPoint (Vector3.zero));
		GetComponent<Rigidbody> ().isKinematic = false;
		springJoint = gameObject.AddComponent<SpringJoint> ();
		springJoint.autoConfigureConnectedAnchor = false;
		springJoint.connectedBody = externalRigidbody;
		springJoint.connectedAnchor = externalAnchor;
		springJoint.minDistance = 0;
		springJoint.maxDistance = 0.125f;
		springJoint.spring = 3;
		springJoint.anchor = anchor;
	}

	public void DisconnectFromRigidbody() {
		DestroyImmediate (springJoint);
		DestroyImmediate (GetComponent<Joint> ());
		DestroyImmediate (GetComponent<Rigidbody> ());
	}

	#endregion


	#region Highlight

	private HighlightDegree highlighted = HighlightDegree.None;
	public HighlightDegree Highlighted {

		set {
			if (highlighted == value) {
				return;
			}

			highlighted = value;
			Color tint = normalColor;
			if (highlighted == HighlightDegree.Half) {
				tint = halfHighlightedColor;
			} else if (highlighted == HighlightDegree.Full) {
				tint = highlightedColor;
			} else if (highlighted == HighlightDegree.Pale) {
				tint = paleColor;
			}
			LeanTween.color (gameObject, tint, 0.125f);
		}

		get {
			return highlighted;
		}

	}

	#endregion

	void OnDrawGizmos() {
		if (springJoint) {
			Gizmos.color = Color.white;
			Vector3 anchorPositionOnWorld = transform.TransformPoint(springJoint.anchor);
			Gizmos.DrawLine(anchorPositionOnWorld, springJoint.connectedBody.transform.TransformPoint (springJoint.connectedAnchor));
		}
	}
}



#region internal class

internal class CharacterJointMock {
	public Vector3 axis;
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

