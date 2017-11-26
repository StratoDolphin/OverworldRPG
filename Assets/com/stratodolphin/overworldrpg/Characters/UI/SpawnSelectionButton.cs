using UnityEngine;
using System.Collections;

namespace com.stratodolphin.overworldrpg.Characters.UI
{
    public class SpawnSelectionButton: MonoBehaviour
    {
        #region Public Attributes
        /// <summary>
        /// Index in <see cref="GameInfo.Bonfires"/> that this button
        /// script is a selection for.
        /// </summary>
        public int BonfireIndex;
        #endregion
    }
}