using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyController : MonoBehaviour {
	public Transform root;
	Dictionary<string, int> kinectJointIDX;
//    public enum JointType : int
//    {
//        HipCenter = 0,
//        Spine = 1,
//        Neck = 2,
//        Head = 3,
//        ShoulderLeft = 4,
//        ElbowLeft = 5,
//        WristLeft = 6,
//        HandLeft = 7,
//        ShoulderRight = 8,
//        ElbowRight = 9,
//        WristRight = 10,
//        HandRight = 11,
//        HipLeft = 12,
//        KneeLeft = 13,
//        AnkleLeft = 14,
//        FootLeft = 15,
//        HipRight = 16,
//        KneeRight = 17,
//        AnkleRight = 18,
//        FootRight = 19,
//        SpineShoulder = 20,
//        HandTipLeft = 21,
//        ThumbLeft = 22,
//        HandTipRight = 23,
//        ThumbRight = 24,
//        Count = 25
//    }

	// Use this for initialization
	void Start () {   
		kinectJointIDX = new Dictionary<string, int>(25);
		kinectJointIDX.Add("Hips", 0);
		kinectJointIDX.Add("Spine1", 1);
		kinectJointIDX.Add("Neck", 2);
		//need to adjust
		kinectJointIDX.Add("LeftEye", 3);
		kinectJointIDX.Add("LeftArm", 4);
		kinectJointIDX.Add("LeftForeArm", 5);
		kinectJointIDX.Add("LeftHand", 6);
		kinectJointIDX.Add("RightArm", 8);
		kinectJointIDX.Add("RightForeArm", 9);
		kinectJointIDX.Add("RightHand", 10);
		kinectJointIDX.Add("LeftUpLeg", 12);
		kinectJointIDX.Add("LeftLeg", 13);
		kinectJointIDX.Add("LeftFoot", 14);
		kinectJointIDX.Add("LeftToeBase", 15);
		kinectJointIDX.Add("RightUpLeg", 16);
		kinectJointIDX.Add("RightLeg", 17);
		kinectJointIDX.Add("RightFoot", 18);
		kinectJointIDX.Add("RightToeBase", 19);
		kinectJointIDX.Add("LeftHandThumb3", 22);
		kinectJointIDX.Add("RightHandThumb3", 24);
	}

	void UpdateJoints(Transform parent, Vector3 totalTrans) {
		foreach (Transform child in parent) {
			//Debug.Log(child.name);
			if (kinectJointIDX.ContainsKey(child.name)) {
				//Transform JointTransform = BodySourceView.JointTransforms[kinectJointIDX[child.name]];
				Windows.Kinect.JointOrientation JointOrientation = 
					BodySourceView.JointOrientations[kinectJointIDX[child.name]];

				//Vector3 kinectPos = (JointTransform != null) ? JointTransform.localPosition : child.position;
				Quaternion kinectRot = (JointOrientation != null) ? 
					new Quaternion(JointOrientation.Orientation.X,
					               JointOrientation.Orientation.Y,
					               JointOrientation.Orientation.Z,
					               JointOrientation.Orientation.W) : child.rotation;

				//kinectRot = Quaternion.Inverse(parent.rotation) * kinectRot;
				Vector3 eulerRot = kinectRot.eulerAngles;
				eulerRot = new Vector3(eulerRot.z,eulerRot.y,eulerRot.x);
				kinectRot = Quaternion.Euler(eulerRot);
//				Vector3 rotateVec = kinectPos-parent.position;
//				kinectRot = Quaternion.LookRotation(rotateVec, Vector3.up);
				//kinectPos.z = -kinectPos.z;
				//Debug.Log(child.name + kinectPos);
				//child.position = kinectPos;// - totalTrans;
				child.localRotation = kinectRot;
				//Debug.Log(rotateVec);
			}
			else {
				child.rotation = parent.rotation;
			}
			UpdateJoints(child, child.position + totalTrans);
		}
	}
	// Update is called once per frame
	void Update () {        
        if (BodySourceView.bodyTracked)
        {
			Vector3 RootPosition = BodySourceView.JointTransforms[kinectJointIDX["Hips"]].localPosition;
			//RootPosition.z = -RootPosition.z;
			root.position = RootPosition;
			UpdateJoints(root, root.position);
        }         
	}
}
