using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
	public enum Edge {
		None = 0,
		Front = 1,
		Back = 2,
		Left = 3,
		Right = 4,
	}

	public delegate void AnimationAction();

	public static event AnimationAction OnStart;
	public static event AnimationAction OnDone;

	public Edge edge = Edge.None;
	public float size = 1f;
	public float speed = 3f;
	public float start = 0f;
	public float end = 90f;

	protected bool isAnimating = false;
	protected float angle = 0f;

	public void OnEnable() {
		InputController.OnMove += HandleOnMove;
	}

	public void OnDisable() {
		InputController.OnMove -= HandleOnMove;
	}
	
	public void Update() {
		
		if (isAnimating)
			UpdateAnimation (edge, start, end);
		
	}

	public void StartAnimation(Edge e, float from = 0f, float to = 90f) {

		if (e != Edge.None && !isAnimating) {

			edge = e;
			start = from;
			end = to;
			isAnimating = true;

			Debug.Log ("Start");

			if (OnStart != null)
				OnStart();
		}

	}

	public void UpdateAnimation(Edge e, float from = 0f, float to = 90f) {
		
		// Run animation. Trigger event once done.
		if (isAnimating && RotateAroundEdge (e, start, end) >= 1f) {
			StopAnimation();
		}
		
	}

	public void StopAnimation() {

		if (isAnimating) {

			isAnimating = false;

			angle = 0f;
			edge = Edge.None;

			// Fix vertical position
			transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

			// Clamp angles
			var euler = transform.eulerAngles;
			euler.x = Mathf.Round(euler.x / end) * end;
			euler.y = Mathf.Round(euler.y / end) * end;
			euler.z = Mathf.Round(euler.z / end) * end;
			transform.eulerAngles = euler;
			
			if (OnDone != null)
				OnDone();

		}

	}

	protected void HandleOnMove (InputController.Movement dir) {

		if (!isAnimating) {

			switch (dir) {
				case InputController.Movement.Forward:
					StartAnimation(Edge.Front);
					break;
				case InputController.Movement.Back:
					StartAnimation(Edge.Back);
					break;
				case InputController.Movement.Left:
					StartAnimation(Edge.Left);
					break;
				case InputController.Movement.Right:
					StartAnimation(Edge.Right);
					break;
			}

		}

	}

	protected Vector3 EdgeToDirection(Edge edge) {

		switch (edge) {
			case Edge.Front:
				return Vector3.back;
			case Edge.Back:
				return Vector3.forward;
			case Edge.Left:
				return Vector3.right;
			case Edge.Right:
				return Vector3.left;
			default:
				return Vector3.zero;
		}

	}
	
	protected float RotateAroundEdge(Edge e, float from = 0f, float to = 90f) {
		
		// Move pivot point to the edge of the cube
		Vector3 xz = EdgeToDirection (e);
		Vector3 pivot = transform.position + .5f * size * xz + .5f * size * Vector3.down;

		// Rotate entity and calculate current angle
		float a = Mathf.Ceil (Mathf.LerpAngle (from, to, speed * Time.deltaTime));
		transform.RotateAround (pivot, xz, a);
		angle += a;

		// Return the progress of the animation
		return Mathf.Min (angle / to, 1f);
		
	}

}
