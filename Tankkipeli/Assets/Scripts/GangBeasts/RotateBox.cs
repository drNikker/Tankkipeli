using UnityEngine;
using System.Collections;

public class RotateBox : MonoBehaviour {

	private Rigidbody rb;
	public float rotateSpeed;

	Vector3 rotationLeft;
	Vector3 rotationRight;

	//public ConfigurableJoint Joint;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{

		rotationLeft.Set (0f, -rotateSpeed, 0f);
		rotationRight.Set (0f, rotateSpeed, 0f);

		rotationLeft = -rotationLeft.normalized * -rotateSpeed;
		rotationRight = rotationRight.normalized * rotateSpeed;

		Quaternion deltaRotationLeft = Quaternion.Euler (rotationLeft * Time.fixedDeltaTime);
		Quaternion deltaRotationRight = Quaternion.Euler (rotationRight * Time.fixedDeltaTime);
	
		if (Input.GetKey (KeyCode.A)) {
			rb.MoveRotation (rb.rotation * deltaRotationLeft);
		}

		if (Input.GetKey (KeyCode.D)) {
			rb.MoveRotation (rb.rotation * deltaRotationRight);
		} 

		/*if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) 
		{
			Joint.angularXMotion = ConfigurableJointMotion.Locked;
			Joint.angularZMotion = ConfigurableJointMotion.Locked;
		}
		else
		{
			Joint.angularXMotion = ConfigurableJointMotion.Free;
			Joint.angularZMotion = ConfigurableJointMotion.Free;
		}*/
	}

}
