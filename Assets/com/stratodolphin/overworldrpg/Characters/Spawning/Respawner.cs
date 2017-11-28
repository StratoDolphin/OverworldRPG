using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;

namespace com.stratodolphin.overworldrpg.Characters.Spawning
{
	public class Respawner : MonoBehaviour {

        /// <summary>
        /// Marks this bonfire as activated so that the main
        /// player can spawn or respawn at it.
        /// </summary>
        protected void activateBonfireIfNew()
        {
            Bonfire bonfire = this.GetComponentInParent<Bonfire>();
            bonfire.IsActivated = true;
            GameLogic.SpawnUI.refresh();
        }

		// Use this for initialization
		void OnTriggerEnter(Collider otherObject) {
			//Debug.Log ("Enter!");
			if (GameInfo.MainPlayer == null) {
				return;
			}

			if (otherObject.gameObject.GetInstanceID () == GameInfo.MainPlayer.gameObject.GetInstanceID ()) {
                //Debug.Log ("Setting bonfire");
                ((GamePlayer)otherObject.GetComponent<GamePlayer>()).BonfireLocation = this.GetComponentInParent<Bonfire>();
                activateBonfireIfNew();
			}
		}

		void OnTriggerExit(Collider otherObject) {
			//Debug.Log ("Exit");
			if (GameInfo.MainPlayer == null) {
				return;
			}

			if (otherObject.gameObject.GetInstanceID () == GameInfo.MainPlayer.gameObject.GetInstanceID ()) {
				((GamePlayer)otherObject.GetComponent<GamePlayer> ()).BonfireLocation = null;
                GameLogic.SpawnUI.hide();
			}
		}
	}
}