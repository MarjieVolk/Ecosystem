using UnityEngine;
using System.Collections;

public class BespokeCameraController : MonoBehaviour {

	public Vector3 target;
	public float xSpeed;
	public float ySpeed;
	public float zoomSpeed;

	public float minDistance, maxDistance;
	public float minYAngle, maxYAngle;

    public float panSpeed;

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

		Vector3 translation = new Vector3(0, 0, 0);

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			translation += new Vector3(transform.forward.x, 0, transform.forward.z);
        }

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			translation += Vector3.Cross(transform.forward, new Vector3(0, 1, 0));
        }

		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			translation += new Vector3(-transform.forward.x, 0, -transform.forward.z);
        }

		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			translation += Vector3.Cross(new Vector3(0, 1, 0), transform.forward);
        }

		translation.Normalize();
		transform.position += translation * panSpeed;
		target += translation * panSpeed;
	}
}
