using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
	//public Transform cam { get; set; }

	//public float maxHeading;//how far can the head cam turn in either direction, from the cars forward vector
	//public float maxRightStickHeading;//how far can the right stick input add to the existing angle

	//public float camLerpSpeed;//used in the transform.forward = Lerp... line, should be less than .5f
	//public float stickHeldInfluenceTime;//holding the left stick will influence how fast the cam lerp happens until this long in seconds
	//public float stickNotHeldInfluenceTime;//similar to above, but this is used to make the camera lerp back forward slower and smoother

	//void Awake()
	//{

	//	//find our main camera, since we cannot assign it before runtime
	//	cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
	//}
	//void Update()
	//{
	//	//the first person camera follows the rotation and position of the FPCamHandle (this transform) but is not a child so it's rotation is independant
	//	//the camera's look will LERP toward the direction of the car's velocity over time based on how long the left stick is held in that direction
	//	//the camera look can also be freely influenced by the right stick to allow the player to look around independantly

	//	Vector3 newCamDir;
	//	if (true)   //if the car is travelling backwards just ignore all the cool calculations we were gonna do
	//	{
	//		//determine the signed angle between the car's forward direction and its velocity vector
	//		float sign = Mathf.Sign(transform.root.InverseTransformPoint(transform.root.position + carController.rb.velocity).x);
	//		float rotationAngle = sign * Vector3.Angle(transform.root.forward, carController.rb.velocity);
	//		//modify that angle
	//		rotationAngle *= StickHoldTimeInfluence(); //angle the camera into the turn more based on how long the left stick is held
	//		rotationAngle *= Mathf.Clamp(carController.speedInMph / 30, 0f, 3f);
	//		rotationAngle += maxRightStickHeading * BSDInput.SquaredInput("2ndStickX"); //add the squared influence from the right stick to independantly control the head
	//		rotationAngle = Mathf.Clamp(rotationAngle, -maxHeading, maxHeading); //clamp the rotation so the camera doesn't turn more than the Max heading
	//																			 //create a new direction vector that is rotated rotationAngle degrees from the car's forward direction
	//		newCamDir = Quaternion.AngleAxis(rotationAngle, Vector3.up) * transform.root.forward;
	//	}
	//	else
	//	{
	//		newCamDir = transform.root.forward;     //clean and simple for when the car is moving backwards
	//	}
	//	//now LERP our rotation towards our newCamDir 
	//	transform.forward = Vector3.Lerp(transform.forward, newCamDir, camLerpSpeed);

	//	//update the cam's position and rotation based off our position and rotation
	//	cam.position = transform.position;
	//	cam.forward = transform.forward;


	//	//draw vectors for debugging
	//	Debug.DrawRay(transform.position, newCamDir, Color.red);
	//	Debug.DrawRay(transform.position, carController.rb.velocity, Color.magenta);
	//	Debug.DrawRay(transform.position, transform.root.forward, Color.yellow);
	//	Debug.DrawRay(transform.position, transform.forward, Color.cyan);
	//}


	////this will lerp up to 1 based on how long the left stick has been held, 
	////then lerp back down to 0 based on how long the stick hasn't been held
	//float StickHoldTimeInfluence()
	//{
	//	float HeldInfluence = (BSDInput.horizontalAxisHeldFor / stickHeldInfluenceTime) * Mathf.Abs(BSDInput.SquaredInput("Horizontal"));
	//	float NotHeldInfluence = (BSDInput.horizontalAxisNotHeldFor / stickNotHeldInfluenceTime);

	//	return Mathf.Clamp01(HeldInfluence) + (1 - Mathf.Clamp01(NotHeldInfluence));
	//}
}
