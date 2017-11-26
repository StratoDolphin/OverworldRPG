using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;
using System;
using System.Reflection;

namespace com.stratodolphin.overworldrpg.Characters.Spawning
{
	public class Bonfire : MonoBehaviour {

		#region Spawning
		/// <summary>
		/// Returns the <see cref="Quaternion"/> rotation that the player to be spawned
		/// should have upon spawning.
		/// </summary>
		/// <returns>The spawn rotation.</returns>
		protected Quaternion getSpawnRotation() {
			Quaternion rotation = Quaternion.LookRotation (this.gameObject.transform.position);
			return rotation;
		}

		/// <summary>
		/// Returns the position that the player to be spawned should have upon spawning.
		/// </summary>
		/// <returns>The spawn location.</returns>
		protected Vector3 getSpawnLocation() {
			Vector3 position = this.gameObject.transform.position;
			position.x -= 5;
			return position;
		}

		/// <summary>
		/// Spawns the specified player at this campfire. This method will instantiate
		/// the player on the game map, set the Main player of the game <see cref="GameInfo.MainPlayer"/>
		/// to this player and set the IsAlive attribute on player to true.
		/// </summary>
		/// <param name="player">Player.</param>
		public void spawn(GameObject player) {
			Debug.Log ("Spawning.");
			Vector3 position = this.getSpawnLocation ();
			Quaternion rotation = this.getSpawnRotation ();

			// Instantiate is a clone funtion, not a creation function. We want to
			// control the cloned object, not player because player is not actually
			// on the map.
			GameObject instantiatedPlayer = Instantiate (player, position, rotation);
			GameInfo.setMainPlayer (instantiatedPlayer.GetComponent<GamePlayer>());
			GameInfo.MainPlayer.IsAlive = true;
		}

		/// <summary>
		/// Spawns the specified player at this campfire. This method will set the
		/// position and rotation of the player on the game map.
		/// </summary>
		/// <param name="player">Player.</param>
		public void respawn(GamePlayer player) {
			Debug.Log ("Respawning. " + player.IsAlive.ToString());
			if (player.IsAlive == false) {
				return;
			} else {
				Vector3 position = this.getSpawnLocation ();
				Quaternion rotation = this.getSpawnRotation ();

				Debug.Log ("Setting position.");
				player.setPosition (position, rotation);
			}
		}
		#endregion

		// Use this for initialization
		void Start () {}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}