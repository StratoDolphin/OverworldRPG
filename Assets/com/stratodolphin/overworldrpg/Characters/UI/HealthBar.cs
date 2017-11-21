using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;

/// <summary>
/// Script that controlls the health bar for a given character.
/// </summary>
public class HealthBar : MonoBehaviour {

	#region Protected Variables
	/// <summary>
	/// The percent of health that this player has left.
	/// </summary>
	protected float _healthPercent;
	#endregion

	#region UI Actions
	/// <summary>
	/// Makes this health bar face the main players camera.
	/// </summary>
	protected void faceMainCamera() {
		if (GameInfo.MainPlayer != null) {
			//transform.LookAt (GameInfo.MainPlayer.MainCamera.transform);
		}
	}

	/// <summary>
	///  Refreshes the health bar on the screen to match the current health.
	/// </summary>
	/// <param name="npcHealth">Npc health.</param>
	protected void refreshHealthBarDisplay() {
		//npcHealth has to be a value between 0 and 1; max health has scale of 1
		Transform actualHealthBar = this.transform.Find("HealthBar").Find("Bar");
		actualHealthBar.localScale = new Vector3 (this._healthPercent, actualHealthBar.localScale.y, actualHealthBar.localScale.z);
	}

	/// <summary>
	/// Sets the health percent and refreshes the health bar display on the screen.
	/// </summary>
	/// <param name="healthPercent">Health percent.</param>
	public void setHealthPercent(float healthPercent) {
		this._healthPercent = healthPercent;
		this.refreshHealthBarDisplay ();
	}
	#endregion

	// Update is called once per frame
	void Update () {
		this.faceMainCamera ();
	}
}
