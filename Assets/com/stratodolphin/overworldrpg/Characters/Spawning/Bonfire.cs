using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.com.stratodolphin.overworldrpg.Characters.Spawning
{
	public class Bonfire : MonoBehaviour {

		#region Spawning
		protected void spawn() {
			Vector3 position = this.gameObject.transform.position;
			Quaternion rotation = Quaternion.LookRotation (position);
			Debug.Log ("loading character.");
			Instantiate (Resources.Load ("com/stratodalphin/overworldrpg/Characters/Prefabs/MainPlayer.prefab"), position, rotation);
		}
		#endregion

		// Use this for initialization
		void Start () {
			spawn ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}