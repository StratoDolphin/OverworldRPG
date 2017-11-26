using UnityEngine;
using System.Collections;

namespace com.stratodolphin.overworldrpg.Characters.Weaponry
{
    public class Weapon : MonoBehaviour
    {
        #region Public Attributes
        /// <summary>
        /// Amount of damage this item will do when it deals
        /// a blow to an enemy. This amount will be removed
        /// from that enemy's hitpoints.
        /// </summary>
        public float Strength;
        #endregion
    }
}