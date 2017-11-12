using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.com.stratodolphin.overworldrpg.Characters.Spawning;

namespace com.stratodolphin.overworldrpg.Characters
{
	public class GameLogic : MonoBehaviour {

		#region Spawning
		/// <summary>
		/// Creates the main player without actually placing it on the
		/// game map. This will return the GameObject of the player.
		/// </summary>
		protected static GameObject createMainPlayerMeta() {
			GameObject player = (GameObject) Resources.Load ("prefabs/MainPlayer");
			return player;
		}

		/// <summary>
		/// If a key on this frame was pressed that corresponds to a certain campfire,
		/// The game will spawn the main player at that campfire. This will only do
		/// that if the main player has not already been spawned.
		/// </summary>
		public static void checkSpawning() {
			bool toRespawn = false;
			if (GameInfo.MainPlayer != null && GameInfo.MainPlayer.IsAlive) {
				toRespawn = true;
			}

			foreach (Bonfire fire in GameInfo.Bonfires) {
				if (Input.GetKeyDown (fire.SpawnKey)) {
					if (toRespawn) {
						fire.respawn (GameInfo.MainPlayer);
					} else {
						fire.spawn (createMainPlayerMeta ());
					}
					return;
				}
			}
		}
		#endregion

		// Use this for initialization
		void Start () {
			GameInfo.initialize ();
		}
		
		// Update is called once per frame
		void Update () {
			checkSpawning ();
		}
	}
}