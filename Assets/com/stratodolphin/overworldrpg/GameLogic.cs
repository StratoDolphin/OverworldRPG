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
		/// game map. This will return the <see cref="GamePlayer"/>
		/// script that is on the players gameobject.
		/// </summary>
		protected static GamePlayer createMainPlayerMeta() {
			GameObject player = (GameObject) Resources.Load ("prefabs/MainPlayer");
			Debug.Log (player);
			GamePlayer playerScript = player.GetComponent<GamePlayer> ();
			return playerScript;
		}

		public static void checkSpawning() {
			if (GameInfo.MainPlayer != null && GameInfo.MainPlayer.IsAlive) {
				return;
			}

			foreach (Bonfire fire in GameInfo.Bonfires) {
				if (Input.GetKeyDown (fire.SpawnKey)) {
					fire.spawn (createMainPlayerMeta ());
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