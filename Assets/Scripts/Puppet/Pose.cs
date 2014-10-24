using UnityEngine;
using System.Collections;

public class Pose {

	public Vector3 RootPosition { get; set; }
	public Quaternion RootRotation { get; set; }

	public Quaternion Chest { get; set; }
	public Quaternion Head { get; set; }

	public Quaternion ThighL { get; set; }
	public Quaternion ThighR { get; set; }
	public Quaternion ShinL { get; set; }
	public Quaternion ShinR { get; set; }
	public Quaternion FootL { get; set; }
	public Quaternion FootR { get; set; }

	public Quaternion ShoulderL { get; set; }
	public Quaternion ShoulderR { get; set; }
	public Quaternion UpperArmL { get; set; }
	public Quaternion UpperArmR { get; set; }
	public Quaternion ForearmL { get; set; }
	public Quaternion ForearmR { get; set; }
	public Quaternion HandL { get; set; }
	public Quaternion HandR { get; set; }

}
