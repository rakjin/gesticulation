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
		return string.Format ("pose.RootPosition = new Vector3({0}f, {1}f, {2}f);\npose.RootRotation = new Quaternion({3}f, {4}f, {5}f, {6}f);\npose.Chest = new Quaternion({7}f, {8}f, {9}f, {10}f);\npose.Head = new Quaternion({11}f, {12}f, {13}f, {14}f);\npose.ThighL = new Quaternion({15}f, {16}f, {17}f, {18}f);\npose.ThighR = new Quaternion({19}f, {20}f, {21}f, {22}f);\npose.ShinL = new Quaternion({23}f, {24}f, {25}f, {26}f);\npose.ShinR = new Quaternion({27}f, {28}f, {29}f, {30}f);\npose.FootL = new Quaternion({31}f, {32}f, {33}f, {34}f);\npose.FootR = new Quaternion({35}f, {36}f, {37}f, {38}f);\npose.ShoulderL = new Quaternion({39}f, {40}f, {41}f, {42}f);\npose.ShoulderR = new Quaternion({43}f, {44}f, {45}f, {46}f);\npose.UpperArmL = new Quaternion({47}f, {48}f, {49}f, {50}f);\npose.UpperArmR = new Quaternion({51}f, {52}f, {53}f, {54}f);\npose.ForearmL = new Quaternion({55}f, {56}f, {57}f, {58}f);\npose.ForearmR = new Quaternion({59}f, {60}f, {61}f, {62}f);\npose.HandL = new Quaternion({63}f, {64}f, {65}f, {66}f);\npose.HandR = new Quaternion({67}f, {68}f, {69}f, {70}f);\n",
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
