using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Smooth Follow")]
public class FollowTarget : MonoBehaviour {

	public Transform target;
	public float distance = 3f;
	public float height = 2f;
	public float hDamping = 2f;
	public float rDamping = 0;

	public void Start () {

		if (!target) {
			Debug.Log ("No target selected for the Camera");
		}

	}
	
	public void LateUpdate () {

		float desiredAngle = target.eulerAngles.y;
		float desiredHeight = target.position.y + height;
		desiredHeight = height;

		float a = transform.eulerAngles.y;
		float h = transform.position.y;

		a = Mathf.LerpAngle (a, desiredAngle, rDamping * Time.deltaTime);
		h = Mathf.Lerp (h, desiredHeight, hDamping * Time.deltaTime);

		Vector3 position = target.position;
		position -= Quaternion.Euler (0, a, 0) * Vector3.forward * distance;
		position.y = h;

		transform.position = position;
		transform.LookAt (new Vector3(target.position.x, 0, target.position.z));
	
	}
}
