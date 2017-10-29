using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.com.stratodolphin.overworldrpg.Characters
{
    public class Storable : MonoBehaviour
    {
        #region Constant Variables
        /// <summary>
        /// Denotes that this item is a melee weapon.
        /// </summary>
        public const int TYPE_MELEE = 1;

        /// <summary>
        /// Denotes that this item is a ranged weapon.
        /// </summary>
        public const int TYPE_RANGE = 2;
        #endregion

        #region Private Variables
        /// <summary>
        /// Inventory in which this <see cref="Storable"/> is stored.
        /// There can only be one inventory that contains this item.
        /// </summary>
        protected Inventory _owner;
        #endregion

        #region Public Attributes
        /// <summary>
        /// Type of item that this <see cref="Storable"/> is. The options
        /// for this property are the constants in this class that
        /// start with "TYPE_".
        /// </summary>
        public int Type;

		public Inventory Owner {
			get { return this._owner; }
			set { this._owner = value; }
		}
        #endregion
    }
}
