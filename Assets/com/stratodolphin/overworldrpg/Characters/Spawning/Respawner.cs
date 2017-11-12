using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters;

namespace Assets.com.stratodolphin.overworldrpg.Characters.Spawning
{
	public class Respawner : MonoBehaviour {

		// Use this for initialization
		void OnTriggerEnter(Collider otherObject) {
			//Debug.Log ("Enter!");
			if (otherObject.gameObject.GetInstanceID () == GameInfo.MainPlayer.gameObject.GetInstanceID ()) {
				//Debug.Log ("Setting bonfire");
				((GamePlayer)otherObject.GetComponent<GamePlayer> ()).BonfireLocation = this.GetComponentInParent<Bonfire> ();
			}
		}

		void OnTriggerExit(Collider otherObject) {
			//Debug.Log ("Exit");
			if (otherObject.gameObject.GetInstanceID () == GameInfo.MainPlayer.gameObject.GetInstanceID ()) {
				((GamePlayer)otherObject.GetComponent<GamePlayer> ()).BonfireLocation = null;
			}
		}
	}
}