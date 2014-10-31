using UnityEngine;
using System.Collections;

public class Pose {

	private static Pose defaultPose;
	public static Pose DefaultPose () {
		if (defaultPose == null) {
			Pose pose = new Pose ();
			pose.RootPosition = new Vector3(0f, 0f, 0f);
			pose.RootRotation = new Quaternion(3.090862E-08f, 0.7071068f, 0.7071068f, -3.090862E-08f);
			pose.Chest = new Quaternion(0f, 0f, 0f, 1f);
			pose.Head = new Quaternion(0f, 0f, 0f, 1f);
			pose.ThighL = new Quaternion(0f, 0f, 0f, 1f);
			pose.ThighR = new Quaternion(0f, 0f, 0f, 1f);
			pose.ShinL = new Quaternion(0f, -1.862645E-09f, 0f, 1f);
			pose.ShinR = new Quaternion(0f, 0f, 0f, 1f);
			pose.FootL = new Quaternion(0f, 1.862645E-09f, 0f, 1f);
			pose.FootR = new Quaternion(0f, 0f, 0f, 1f);
			pose.ShoulderL = new Quaternion(0f, 0f, 0f, 1f);
			pose.ShoulderR = new Quaternion(0f, 0f, 0f, 1f);
			pose.UpperArmL = new Quaternion(0f, 0f, 0f, 1f);
			pose.UpperArmR = new Quaternion(0f, 0f, 0f, 1f);
			pose.ForearmL = new Quaternion(4.656613E-10f, 0f, 0f, 1f);
			pose.ForearmR = new Quaternion(0f, 0f, 0f, 1f);
			pose.HandL = new Quaternion(-4.656613E-10f, 0f, 0f, 1f);
			pose.HandR = new Quaternion(0f, 0f, 0f, 1f);
			defaultPose = pose;
		}
		return defaultPose;
	}

	public static Pose RandomPose00 () {
		Pose pose = new Pose ();
		pose.RootPosition = new Vector3(0.7597921f, 0.3172723f, 0.2748378f);
		pose.RootRotation = new Quaternion(0.06465621f, 0.7142513f, 0.6713315f, -0.1870259f);
		pose.Chest = new Quaternion(-0.08464119f, 0.03550453f, -0.02175937f, 0.995541f);
		pose.Head = new Quaternion(0.2477382f, 0.006086576f, -0.1288139f, 0.9602061f);
		pose.ThighL = new Quaternion(0.006947418f, -0.2190897f, -0.2259428f, 0.9491583f);
		pose.ThighR = new Quaternion(-0.04065934f, -0.1623949f, -0.1929759f, 0.966817f);
		pose.ShinL = new Quaternion(0.02249193f, -1.179602E-05f, -3.974847E-06f, 0.999747f);
		pose.ShinR = new Quaternion(-0.04334604f, 2.734348E-06f, -2.682194E-06f, 0.9990602f);
		pose.FootL = new Quaternion(-0.2501879f, 0.04194485f, 0.1599378f, 0.9539742f);
		pose.FootR = new Quaternion(0.3259747f, -0.05703522f, 0.1626393f, 0.9295353f);
		pose.ShoulderL = new Quaternion(0.2516475f, 0.1645909f, -0.1661115f, 0.9391434f);
		pose.ShoulderR = new Quaternion(-0.2103826f, -0.2536181f, -0.02343704f, 0.943858f);
		pose.UpperArmL = new Quaternion(-0.05970342f, -0.04668266f, -0.0955112f, 0.9925392f);
		pose.UpperArmR = new Quaternion(0.2481305f, 0.07853668f, -0.2760275f, 0.9252416f);
		pose.ForearmL = new Quaternion(1.832829E-05f, -0.23736f, 8.471246E-06f, 0.9714218f);
		pose.ForearmR = new Quaternion(-1.636114E-05f, 0.3483752f, -1.965423E-05f, 0.9373552f);
		pose.HandL = new Quaternion(-0.322214f, -0.102178f, 0.1219392f, 0.9332034f);
		pose.HandR = new Quaternion(0.06961224f, 0.05110344f, 0.066039f, 0.9940731f);
		return pose;
	}

	public static Pose RandomPose01 () {
		Pose pose = new Pose ();
		pose.RootPosition = new Vector3(0f, 0f, 0f);
		pose.RootRotation = new Quaternion(3.090862E-08f, 0.7071068f, 0.7071068f, -3.090862E-08f);
		pose.Chest = new Quaternion(0f, 0f, 0f, 1f);
		pose.Head = new Quaternion(0f, 0f, 0f, 1f);
		pose.ThighL = new Quaternion(0.005532235f, -0.02290104f, 0.05089671f, 0.9984261f);
		pose.ThighR = new Quaternion(-0.06209144f, 0.05373791f, 0.1568021f, 0.9842104f);
		pose.ShinL = new Quaternion(0f, -1.862645E-09f, 0f, 1f);
		pose.ShinR = new Quaternion(0f, 0f, 0f, 1f);
		pose.FootL = new Quaternion(0f, 1.862645E-09f, 0f, 1f);
		pose.FootR = new Quaternion(0f, 0f, 0f, 1f);
		pose.ShoulderL = new Quaternion(0f, 0f, 0f, 1f);
		pose.ShoulderR = new Quaternion(0f, 0f, 0f, 1f);
		pose.UpperArmL = new Quaternion(0.8194493f, -0.5333355f, -0.1656313f, -0.1289277f);
		pose.UpperArmR = new Quaternion(0f, 0f, 0f, 1f);
		pose.ForearmL = new Quaternion(0.0005442592f, -0.09039186f, 0.00136947f, -0.9959052f);
		pose.ForearmR = new Quaternion(-0.0007446893f, 0.07563742f, 0.002194274f, 0.9971328f);
		pose.HandL = new Quaternion(-0.1979808f, 0.1140546f, -0.3329598f, 0.9148405f);
		pose.HandR = new Quaternion(0f, 0f, 0f, 1f);
		return pose;
	}

	public static Pose RandomPose02 () {
		Pose pose = new Pose ();
		pose.RootPosition = new Vector3(-0.03287354f, -0.04304898f, 0.01300705f);
		pose.RootRotation = new Quaternion(0.03379956f, 0.712796f, 0.6997254f, -0.0341163f);
		pose.Chest = new Quaternion(0f, 0f, 0f, 1f);
		pose.Head = new Quaternion(0.2257549f, 0.0720173f, 0.1312093f, 0.9626175f);
		pose.ThighL = new Quaternion(-0.6528271f, 0.1613901f, 0.3262869f, 0.6643093f);
		pose.ThighR = new Quaternion(-0.2931835f, 0.2043805f, 0.01784036f, 0.9337847f);
		pose.ShinL = new Quaternion(0.7365193f, 0.01632268f, 0.002005719f, 0.6762167f);
		pose.ShinR = new Quaternion(0.6629125f, 0.01393302f, 0.002564537f, 0.7485629f);
		pose.FootL = new Quaternion(0f, 1.862645E-09f, 0f, 1f);
		pose.FootR = new Quaternion(0f, 0f, 0f, 1f);
		pose.ShoulderL = new Quaternion(0f, 0f, 0f, 1f);
		pose.ShoulderR = new Quaternion(0f, 0f, 0f, 1f);
		pose.UpperArmL = new Quaternion(0.6424512f, 0.7632013f, -0.02069663f, 0.06596938f);
		pose.UpperArmR = new Quaternion(-0.9244344f, -0.3347354f, -0.03047058f, 0.1801244f);
		pose.ForearmL = new Quaternion(-0.0002626195f, 0.1020376f, -6.446832E-05f, -0.9947805f);
		pose.ForearmR = new Quaternion(0f, 0f, 0f, 1f);
		pose.HandL = new Quaternion(-4.656613E-10f, 0f, 0f, 1f);
		pose.HandR = new Quaternion(0f, 0f, 0f, 1f);
		return pose;
	}

	public static Pose RandomPose03 () {
		Pose pose = new Pose ();
		pose.RootPosition = new Vector3(1.060621f, -1.018352f, 0.01803527f);
		pose.RootRotation = new Quaternion(0.5457732f, -0.462261f, -0.6225307f, -0.3176506f);
		pose.Chest = new Quaternion(0.48029f, -0.1917225f, -0.2853253f, 0.8069408f);
		pose.Head = new Quaternion(0.005715903f, 0.2485494f, -0.2458528f, -0.9368815f);
		pose.ThighL = new Quaternion(-0.6008956f, 0.3827541f, 0.1839159f, 0.6771992f);
		pose.ThighR = new Quaternion(-0.170979f, 0.0673325f, -0.2273544f, -0.9563172f);
		pose.ShinL = new Quaternion(0.7035684f, 0.01996845f, -0.002416419f, 0.7103429f);
		pose.ShinR = new Quaternion(-0.7162444f, 0.01727534f, -0.008588521f, -0.6975829f);
		pose.FootL = new Quaternion(0f, 1.862645E-09f, 0f, 1f);
		pose.FootR = new Quaternion(0f, 0f, 0f, 1f);
		pose.ShoulderL = new Quaternion(0f, 0f, 0f, 1f);
		pose.ShoulderR = new Quaternion(0f, 0f, 0f, 1f);
		pose.UpperArmL = new Quaternion(-0.7026323f, -0.3541844f, 0.4991129f, -0.3629705f);
		pose.UpperArmR = new Quaternion(0.7595598f, 0.6201896f, -0.1913438f, -0.04267785f);
		pose.ForearmL = new Quaternion(4.656613E-10f, 0f, 0f, 1f);
		pose.ForearmR = new Quaternion(0f, 0f, 0f, 1f);
		pose.HandL = new Quaternion(-0.2456737f, 0.1950154f, -0.2374973f, 0.9193522f);
		pose.HandR = new Quaternion(0f, 0f, 0f, 1f);
		return pose;
	}

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
