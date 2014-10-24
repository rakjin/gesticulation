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

	public override string ToString ()
	{
		return string.Format ("pose.RootPosition = new Vector3({0}, {1}, {2});\npose.RootRotation = new Quaternion({3}, {4}, {5}, {6});\npose.Chest = new Quaternion({7}, {8}, {9}, {10});\npose.Head = new Quaternion({11}, {12}, {13}, {14});\npose.ThighL = new Quaternion({15}, {16}, {17}, {18});\npose.ThighR = new Quaternion({19}, {20}, {21}, {22});\npose.ShinL = new Quaternion({23}, {24}, {25}, {26});\npose.ShinR = new Quaternion({27}, {28}, {29}, {30});\npose.FootL = new Quaternion({31}, {32}, {33}, {34});\npose.FootR = new Quaternion({35}, {36}, {37}, {38});\npose.ShoulderL = new Quaternion({39}, {40}, {41}, {42});\npose.ShoulderR = new Quaternion({43}, {44}, {45}, {46});\npose.UpperArmL = new Quaternion({47}, {48}, {49}, {50});\npose.UpperArmR = new Quaternion({51}, {52}, {53}, {54});\npose.ForearmL = new Quaternion({55}, {56}, {57}, {58});\npose.ForearmR = new Quaternion({59}, {60}, {61}, {62});\npose.HandL = new Quaternion({63}, {64}, {65}, {66});\npose.HandR = new Quaternion({67}, {68}, {69}, {70});\n",
		                      RootPosition.x, RootPosition.y, RootPosition.z,
		                      RootRotation.x, RootRotation.y, RootRotation.z, RootRotation.w,
		                      Chest.x, Chest.y, Chest.z, Chest.w,
		                      Head.x, Head.y, Head.z, Head.w,
		                      ThighL.x, ThighL.y, ThighL.z, ThighL.w,
		                      ThighR.x, ThighR.y, ThighR.z, ThighR.w,
		                      ShinL.x, ShinL.y, ShinL.z, ShinL.w,
		                      ShinR.x, ShinR.y, ShinR.z, ShinR.w,
		                      FootL.x, FootL.y, FootL.z, FootL.w,
		                      FootR.x, FootR.y, FootR.z, FootR.w,
		                      ShoulderL.x, ShoulderL.y, ShoulderL.z, ShoulderL.w,
		                      ShoulderR.x, ShoulderR.y, ShoulderR.z, ShoulderR.w,
		                      UpperArmL.x, UpperArmL.y, UpperArmL.z, UpperArmL.w,
		                      UpperArmR.x, UpperArmR.y, UpperArmR.z, UpperArmR.w,
		                      ForearmL.x, ForearmL.y, ForearmL.z, ForearmL.w,
		                      ForearmR.x, ForearmR.y, ForearmR.z, ForearmR.w,
		                      HandL.x, HandL.y, HandL.z, HandL.w,
		                      HandR.x, HandR.y, HandR.z, HandR.w);
	}

}
