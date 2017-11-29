using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.stratodolphin.overworldrpg.Characters.Spawning;
using com.stratodolphin.overworldrpg.Characters.UI;

namespace com.stratodolphin.overworldrpg.Characters
{
	public class GameLogic : MonoBehaviour {

		#region Spawning
        /// <summary>
        /// Getter for the UI canvas that allows user to spawn at a
        /// chosen campfire.
        /// </summary>
        /// <remarks>
        /// I cache it so that, if it is disactive when it's hidden, it
        /// can still be retreived.
        /// </remarks>
        private static SpawnUIViewModel _spawnUI;
        public static SpawnUIViewModel SpawnUI
        {
            get {
                if (_spawnUI != null) { return _spawnUI; }
                SpawnUIViewModel ui = GameObject.Find("SpawnUICanvas").GetComponent<SpawnUIViewModel>();
                _spawnUI = ui;
                return _spawnUI;
            }
        }

        /// <summary>
        /// Shows or hides the Spawn UI Canvas.
        /// </summary>
        /// <param name="visibility"></param>
        public static void toggleSpawnUIVisibility()
        {
            if (SpawnUI.gameObject.activeSelf)
            {
                SpawnUI.hide();
            } else
            {
                SpawnUI.show();
            }
        }
		#endregion

		#region Pausing
		/// <summary>
		/// Getter for the UI canvas that allows user to pause at any
		/// time.
		/// </summary>
		/// <remarks>
		/// I cache it so that, if it is disactive when it's hidden, it
		/// can still be retreived.
		/// </remarks>
		private static PauseUIViewModel _pauseUI;
		public static PauseUIViewModel PauseUI
		{
			get {
				if (_pauseUI != null) { return _pauseUI; }
				PauseUIViewModel ui = GameObject.Find("PauseUICanvas").GetComponent<PauseUIViewModel>();
				_pauseUI = ui;
				return _pauseUI;
			}
		}

		/// <summary>
		/// Shows or hides the Pause UI Canvas.
		/// </summary>
		/// <param name="visibility"></param>
		public static void togglePauseUIVisibility() {
			if (PauseUI.gameObject.activeSelf)
			{
				PauseUI.hide();
			} else
			{
				PauseUI.show();
			}
		}
		#endregion
		// Use this for initialization
		void Start () {
			GameInfo.initialize ();
            SpawnUI.initialize();
			PauseUI.initialize ();
		}
		
		// Update is called once per frame
		void Update () { }
	}
}