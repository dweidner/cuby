using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	public float size = 1f;

	protected Transform target;
	protected bool isAnimating;

	public void Start() {
		
		target = transform.FindChild ("Target");
		
		if (!target)
			Debug.LogError("Could not find target element");
		
	}

	public void Update () {

		if (!isAnimating && Input.GetKeyDown("up")) {
			isAnimating = true;
			target.Translate (0, -.5f * size, .5f * size);
			StartCoroutine(RotateAroundEdge(target.position, Vector3.right));
		}
		if (!isAnimating && Input.GetKeyDown("down")) {
			isAnimating = true;
			target.Translate(0, -.5f * size, -.5f * size);  
			StartCoroutine(RotateAroundEdge(target.position, Vector3.left));  
		}
		if (!isAnimating && Input.GetKeyDown("left")) {
			isAnimating = true;
			target.Translate(-.5f * size, -.5f * size, 0);  
			StartCoroutine(RotateAroundEdge(target.position, Vector3.forward)); 
		}
		if (!isAnimating && Input.GetKeyDown("right")) {
			isAnimating = true;
			target.Translate(.5f * size, -.5f * size, 0);
			StartCoroutine(RotateAroundEdge(target.position, Vector3.back));
		}

	}

	public IEnumerator RotateAroundEdge(Vector3 point, Vector3 axis, float end = 90f) {

		int iterations = 30;
		float angle = end / iterations;

		for (int i = 1; i <= iterations; i++) {
			transform.RotateAround(point, axis, angle);
			yield return new WaitForSeconds(0.0033333f);
		}

		target.position = transform.position;

		// Restrict movement to xz layer
		Vector3 position = transform.position;
		position.y = .5f * size;
		transform.position = position;

		// Clamp angles
		Vector3 euler = transform.eulerAngles;
		euler.x = Mathf.Round (euler.x / 90f) * 90f;
		euler.y = Mathf.Round (euler.x / 90f) * 90f;
		euler.z = Mathf.Round (euler.x / 90f) * 90f;
		transform.eulerAngles = euler;

		// Stop animation
		isAnimating = false;

//		GameObject dimensionController = GameObject.Find ("DimensionController");
//		dimensionController.GetComponent<DimensionController> ().UpdateDimension ();

	}

}
