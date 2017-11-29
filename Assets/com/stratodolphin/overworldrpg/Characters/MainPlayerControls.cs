using UnityEngine;
using System.Collections;

public class MainPlayerControls : MonoBehaviour
{
	#region Control Constants
	/// <summary>
	/// The forward control key.
	/// </summary>
	public KeyCode Forward = KeyCode.W;

	/// <summary>
	/// The backward control key.
	/// </summary>
	public KeyCode Backward = KeyCode.S;

	/// <summary>
	/// The left control key.
	/// </summary>
	public KeyCode Left = KeyCode.A;

	/// <summary>
	/// The right control key.
	/// </summary>
	public KeyCode Right = KeyCode.D;
	#endregion

	#region Public Attributes
	/// <summary>
	/// The Main Player script that this controls.
	/// </summary>
	public GamePlayer PlayerScript;
	#endregion

	#region Control Actions
	protected void checkInput() {
		if (Input.GetKeyDown (Forward)) {
			move (Forward);
		}
		if (Input.GetKeyDown (Backward)) {
			move (Backward);
		}
		if (Input.GetKeyDown (Left)) {
			move (Left);
		}
		if (Input.GetKeyDown (Right)) {
			move (Right);
		}

		if (Input.GetKeyUp (Forward)) {
			stopMoving ();
		}
		if (Input.GetKeyUp (Backward)) {
			stopMoving ();
		}
		if (Input.GetKeyUp (Left)) {
			stopMoving ();
		}
		if (Input.GetKeyUp (Right)) {
			stopMoving ();
		}
	}

	protected void move(KeyCode directionDesire) {
		Vector3 directionToMove = this.getPointInDirection (directionDesire);
		this.PlayerScript.setDesireToApproach (directionToMove, true);
	}

	protected void stopMoving() {
		this.PlayerScript.setDesireToMove (false);
	}
	#endregion

	protected Vector3 getPointInDirection(KeyCode direction) {
		Transform cameraTransform = this.transform.Find ("Camera");
		Vector3 endPoint = cameraTransform.forward;

		if (direction == Forward) {
			endPoint = cameraTransform.forward;
		} else if (direction == Backward) {
			endPoint = -1 * (cameraTransform.forward);
		} else if (direction == Left) {
			endPoint = -1 * (cameraTransform.right);
		} else if (direction == Right) {
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
		checkInput ();
	}
}

