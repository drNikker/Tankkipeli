using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullRagdollMode : MonoBehaviour {

    CharacterJoint joint;

    public void RagdollMode()
    {
        joint = GetComponent<CharacterJoint>();
        SoftJointLimit lowLimits = joint.lowTwistLimit;
        SoftJointLimit highLimits = joint.highTwistLimit;
        lowLimits.limit = -100;
        highLimits.limit = 100;
        joint.lowTwistLimit = lowLimits;
        joint.highTwistLimit = highLimits;
    }

}
