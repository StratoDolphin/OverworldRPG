using UnityEngine;
using System.Collections;

public class MainPlayerControls : MonoBehaviour
{
	#region Control Constants
	/// <summary>
	/// The forward control key.
	/// </summary>
	public static KeyCode MyForward = KeyCode.W;

	/// <summary>
	/// The backward control key.
	/// </summary>
	public static KeyCode MyBackward = KeyCode.S;

	/// <summary>
	/// The left control key.
	/// </summary>
	public static KeyCode MyLeft = KeyCode.A;

	/// <summary>
	/// The right control key.
	/// </summary>
	public static KeyCode MyRight = KeyCode.D;

	/// <summary>
	/// The swing sword control.
	/// </summary>
	public static KeyCode SwingSword = KeyCode.Mouse0;
	#endregion

	bool [] buttonsDown = new bool[] {false, false, false, false}; //top, right, bottom, left (clockwise from top)
	KeyCode [] directionKeyCodes = new KeyCode[] {MyForward, MyRight, MyBackward, MyLeft};

	bool forwardDown = false;
	bool rightDown = false;
	bool backDown = false;
	bool leftDown = false;

	#region Public Attributes
	/// <summary>
	/// The Main Player script that this controls.
	/// </summary>
	public GamePlayer PlayerScript;
	#endregion

	#region Protected Attributes

	#endregion

	#region Control Actions
	protected void checkInputForMovement() {
		if (Input.GetKeyDown (MyForward)) {
			buttonsDown[0] = true;
			//move (Forward);
		}
		if (Input.GetKeyDown (MyRight)) {
			buttonsDown[1] = true;
			//move (Right);
		}
		if (Input.GetKeyDown (MyBackward)) {
			buttonsDown[2] = true;
			//move (Backward);
		}
		if (Input.GetKeyDown (MyLeft)) {
			buttonsDown[3] = true;
			//move (Left);
		}

		if (Input.GetKeyUp (MyForward)) {
			buttonsDown[0] = false;
			//stopMoving ();
		}
		if (Input.GetKeyUp (MyRight)) {
			buttonsDown[1] = false;
			//stopMoving ();
		}
		if (Input.GetKeyUp (MyBackward)) {
			buttonsDown[2] = false;
			//stopMoving ();
		}
		if (Input.GetKeyUp (MyLeft)) {
			buttonsDown[3] = false;
			//stopMoving ();
		}

		Vector3 finalMoveTarget = Vector3.zero;
		bool movementWanted = false;
		//loop over buttonDown array and combine vectors into a big vector
		for (int i = 0; i < 4; i++) {
			if (buttonsDown [i] == true) {
				Vector3 tempPoint = getPointInDirection (directionKeyCodes [i]);
				finalMoveTarget += tempPoint;
				movementWanted = true;
			}
		}
		//Debug.Log (finalMoveTarget);
		if (movementWanted) {
			//when no buttons are being pressed, a moveTarget of (0,0,0) is passed in and
			//the player tries to walk to that coodinate. This cancels that.
			move (finalMoveTarget);
		} else {
			this.PlayerScript.setDesireToMove (false);
		}

	}

	protected void move(Vector3 moveTarget) {
	//protected void move(KeyCode directionDesire) {
		//Vector3 directionToMove = this.getPointInDirection (directionDesire);
		//this.PlayerScript.setDesireToApproach (directionToMove, true);
		this.PlayerScript.setDesireToApproach(moveTarget, true);
	}

	protected void checkInputActions() {
		if (Input.GetKeyDown (SwingSword)) {
			this.startSwinging ();
		}

		if (Input.GetKeyUp (SwingSword)) {
			this.stopSwinging ();
		}
	}

	protected void startSwinging() {
		this.PlayerScript.setDesireToSwing ();
	}

	protected void stopSwinging() {
		this.PlayerScript.setDesireStopSwinging ();
	}
	#endregion


	//we are no longer gonna use this method
	protected Vector3 getPointInDirection(KeyCode direction) {
		Transform cameraTransform = this.transform.Find ("Camera");
		Vector3 endPoint = cameraTransform.forward;

		if (direction == MyForward) {
			endPoint = cameraTransform.forward;
		} else if (direction == MyBackward) {
			endPoint = -1 * (cameraTransform.forward);
		} else if (direction == MyLeft) {
			endPoint = -1 * (cameraTransform.right);
		} else if (direction == MyRight) {
			endPoint = cameraTransform.right;
		}

		endPoint.y = transform.position.y;
		Ray directionRay = new Ray(transform.position, endPoint);
		return directionRay.GetPoint (100.00f);
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkInputForMovement ();
		checkInputActions ();
	}
}

