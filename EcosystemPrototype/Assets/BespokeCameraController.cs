using UnityEngine;
using System.Collections;

public class BespokeCameraController : MonoBehaviour {

	public Vector3 target;
	public float xSpeed;
	public float ySpeed;
	public float zoomSpeed;

	public float minDistance, maxDistance;
	public float minYAngle, maxYAngle;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//rotate
		if (Input.GetMouseButton(1)) {
			float x = Input.GetAxis("Mouse X");
			transform.RotateAround(target, Vector3.up, x * xSpeed);
			
			float y = Input.GetAxis("Mouse Y");
			float nextYAngle = transform.eulerAngles.x - y * ySpeed;
			if(nextYAngle < maxYAngle && nextYAngle > minYAngle)
				transform.RotateAround(target, transform.right, -y * ySpeed);
		}

		//zoom
		float z = Input.GetAxis ("Mouse ScrollWheel");
		Vector3 difference = target - transform.position;
		Vector3 direction = new Vector3 (difference.x, difference.y, difference.z);
		direction.Normalize ();
		direction *= zoomSpeed * z;

		//clamp the zoom
		float nextDistance = (target - (transform.position + direction)).magnitude;
		if (nextDistance < minDistance || nextDistance > maxDistance)
						return;

		transform.position += direction;
	}
}
