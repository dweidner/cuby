using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	
	public enum Movement {
		None,
		Forward,
		Back,
		Left,
		Right
	}

	public delegate void MoveAction(Movement dir);

	public static event MoveAction OnMove;
	public static event MoveAction OnMoveLeft;
	public static event MoveAction OnMoveRight;
	public static event MoveAction OnMoveForward;
	public static event MoveAction OnMoveBack;

	public void Update () {

		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			MoveBack();
		} 

		else if (Input.GetKeyUp (KeyCode.RightArrow)) {
			MoveRight();
		} 

		else if (Input.GetKeyUp (KeyCode.UpArrow)) {
			MoveForward();
		} 

		else if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			MoveLeft();
		}
	
	}

	public void MoveLeft() {

		if (OnMove != null)
				OnMove(Movement.Left);

		if (OnMoveLeft != null) 
			OnMoveLeft (Movement.Left);

	}

	public void MoveRight() {

		if (OnMove != null)
			OnMove(Movement.Right);

		if (OnMoveRight != null) 
			OnMoveRight (Movement.Right);

	}

	public void MoveForward() {

		if (OnMove != null)
			OnMove(Movement.Forward);

		if (OnMoveForward != null) 
			OnMoveForward (Movement.Forward);

	}

	public void MoveBack() {

		if (OnMove != null)
			OnMove(Movement.Back);;

		if (OnMoveBack != null) 
			OnMoveBack (Movement.Back);

	}
}
