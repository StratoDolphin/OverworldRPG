using UnityEngine;
using System.Collections;

namespace com.stratodolphin.overworldrpg.Characters.UI
{
    public class UIPlayerControls : MonoBehaviour
    {
        #region Spawn UI
        /// <summary>
        /// The key that can be pressed to toggle the visibility of
        /// the spawn ui.
        /// </summary>
        public KeyCode SpawnUIToggle = KeyCode.M;

        /// <summary>
        /// Checks to see if the spawn ui has been toggled by
        /// the user. If so, toggle it via
        /// <see cref="GameLogic.toggleSpawnUIVisibility()"/>.
        /// </summary>
        protected void checkSpawnUIToggle()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (GameInfo.MainPlayer != null && GameInfo.MainPlayer.BonfireLocation != null)
                {
                    GameLogic.toggleSpawnUIVisibility();
                }
            }
        }
		#endregion

		#region Pause UI
		/// <summary>
		/// The key that can be pressed to toggle the visibility of
		/// the spawn ui.
		/// </summary>
		public KeyCode PauseUIToggle = KeyCode.P;

		/// <summary>
		/// Checks to see if the pause ui has been toggled by
		/// the user. If so, toggle it via
		/// <see cref="GameLogic.togglePauseUIVisibility()"/>.
		/// </summary>
		protected void checkPauseUIToggle()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				if (GameInfo.MainPlayer != null)
				{
					GameLogic.togglePauseUIVisibility();
				}
			}
		}
        #endregion

		#region Inv UI

		/// <summary>
		/// Checks to see if the pause ui has been toggled by
		/// the user. If so, toggle it via
		/// <see cref="GameLogic.togglePauseUIVisibility()"/>.
		/// </summary>
		protected void checkInvUIToggle()
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				if (GameInfo.MainPlayer != null)
				{
					GameLogic.toggleInvUIVisibility();
				}
			}
		}
		#endregion


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            checkSpawnUIToggle();
			checkPauseUIToggle ();
			checkInvUIToggle ();
        }
    }
}