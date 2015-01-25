using UnityEngine;
using System.Collections;

public delegate void PlayerEventHandler(PlayerEventArgs e);

public class PlayerEventArgs : System.EventArgs {
	
	private GameObject entity;
	
	private string movement;
	private bool canceled;
	
	public GameObject GameObject
	{
		get { return entity; }
	}
	
	public Vector3 Position
	{
		get { return entity.transform.position; }
	}
	
	public Vector3 NormalizedPosition
	{
		get { return NormalizePosition(this.Position); }
	}
	
	public string Movement
	{
		get { return movement; }
		set { movement = value; }
	}
	
	public bool IsCanceled 
	{
		get { return canceled; }
	}
	
	public PlayerEventArgs(GameObject e) {
		entity = e;
	}
	
	public void Cancel() {
		canceled = true;
	}
	
	protected Vector3 NormalizePosition(Vector3 position) {
		position.x = Mathf.Round (position.x / .5f) * .5f;
		position.y = Mathf.Round (position.y / .5f) * .5f;
		position.z = Mathf.Round (position.z / .5f) * .5f;
		return position;
	}
	
}

public class PlayerController : MonoBehaviour {
	
	public static event PlayerEventHandler OnMove;
	public static event PlayerEventHandler OnAnimationStart;
	public static event PlayerEventHandler OnAnimationDone;
	
	public float size = 1f;
	
	protected Transform target;
	protected bool isAnimating;
	
	public void Start() {
		
		target = transform.FindChild ("Target");
		
		if (!target) {
			Debug.LogError("Could not find target element");
		}
		
	}
	
	public void Update () {
		
		HandleInput ();
		
	}
	
	protected void HandleInput() {
		
		if (!isAnimating && Input.GetKeyDown("up")) {
			isAnimating = true;
			Move ("up");
		}
		
		if (!isAnimating && Input.GetKeyDown("down")) {
			isAnimating = true;
			Move ("down");
		}
		
		if (!isAnimating && Input.GetKeyDown("left")) {
			isAnimating = true;
			Move ("left");
		}
		
		if (!isAnimating && Input.GetKeyDown("right")) {
			isAnimating = true;
			Move ("right");
		}
		
	}
	
	public bool Move(string direction) {
		
		// Allow components to cancel movement
		if (OnMove != null) {
			
			PlayerEventArgs e = new PlayerEventArgs(gameObject);
			e.Movement = direction;
			
			OnMove(e);
			
			// Return early
			if (e.IsCanceled) {
				isAnimating = false;
				return false;
			}
			
		}
		
		// Start the correct animation
		isAnimating = true;
		
		switch (direction) {
		case "up":
			AnimateMoveUp();
			break;
		case "down":
			AnimateMoveDown();
			break;
		case "left":
			AnimateMoveLeft();
			break;
		case "right":
			AnimateMoveRight();
			break;
		}
		
		return true;
		
	}
	
	protected void AnimateMoveLeft() {
		
		target.Translate(-.5f * size, -.5f * size, 0);  
		StartCoroutine(RotateAroundEdge(target.position, Vector3.forward)); 
		
	}
	
	protected void AnimateMoveRight() {
		
		target.Translate(.5f * size, -.5f * size, 0);
		StartCoroutine(RotateAroundEdge(target.position, Vector3.back));
		
	}
	
	protected void AnimateMoveUp() {
		
		target.Translate (0, -.5f * size, .5f * size);
		StartCoroutine(RotateAroundEdge(target.position, Vector3.right));
		
	}
	
	protected void AnimateMoveDown() {
		
		target.Translate(0, -.5f * size, -.5f * size);  
		StartCoroutine(RotateAroundEdge(target.position, Vector3.left)); 
		
	}
	
	protected IEnumerator RotateAroundEdge(Vector3 point, Vector3 axis, float end = 90f) {
		
		StartAnimation ();
		
		int iterations = 30;
		float angle = end / iterations;
		
		for (int i = 1; i <= iterations; i++) {
			transform.RotateAround(point, axis, angle);
			yield return new WaitForSeconds(0.0033333f);
		}
		
		target.position = transform.position;
		
		// Normalize position
		Vector3 position = transform.position;
		position.x = Mathf.Round (position.x / (.5f * size)) * (.5f * size);
		position.y = Mathf.Round (position.y / (.5f * size)) * (.5f * size);
		position.z = Mathf.Round (position.z / (.5f * size)) * (.5f * size);
		transform.position = position;
		
		// Clamp angles
		Vector3 euler = transform.eulerAngles;
		euler.x = Mathf.Round (euler.x / 90f) * 90f;
		euler.y = Mathf.Round (euler.x / 90f) * 90f;
		euler.z = Mathf.Round (euler.x / 90f) * 90f;
		transform.eulerAngles = euler;
		
		// Stop animation
		isAnimating = false;
		
		FinishAnimation ();
		
	}
	
	protected void StartAnimation() {
		
		// Notify game components
		if (OnAnimationStart != null)
			OnAnimationStart (new PlayerEventArgs(gameObject));
		
	}
	
	protected void FinishAnimation() {
		
		
		// Notify game components
		if (OnAnimationDone != null)
			OnAnimationDone (new PlayerEventArgs(gameObject));
		
		audio.Play ();

	}
	
}