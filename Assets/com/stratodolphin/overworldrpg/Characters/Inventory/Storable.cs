using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.stratodolphin.overworldrpg.Characters.Inventory
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

        /// <summary>
        /// Denotes that this item is an effect item
        /// and therefore will not persist in the inventory.
        /// </summary>
        public const int TYPE_EFFECT = 3;
        #endregion

        #region Private Variables
        /// <summary>
        /// Inventory in which this <see cref="Storable"/> is stored.
        /// There can only be one inventory that contains this item.
        /// </summary>
        protected Inventory _owner;

        /// <summary>
        /// Application wrapper for this <see cref="Storable"/>.
        /// </summary>
        protected EffectApplier effectApplier;
        #endregion

        #region Public Attributes
        /// <summary>
        /// Type of item that this <see cref="Storable"/> is. The options
        /// for this property are the constants in this class that
        /// start with "TYPE_".
        /// </summary>
        public int Type;

		/// <summary>
		/// Name of the item. This name will show up when 
		/// the user looks at his/her inventory
		/// </summary>
		public string Name;

		public Inventory Owner {
			get { return this._owner; }
			set { this._owner = value; }
		}

        /// <summary>
        /// Accessor for <see cref="effectApplier"/>.
        /// </summary>
        public EffectApplier EffectApplierAccessor { get { return this.effectApplier; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Applies any actions needed when a character picks up this
        /// item. For example, if the item applies an immediate effect,
        /// that effect application is called.
        /// </summary>
        /// <param name="character"></param>
        public void ApplyActionsOnPickup(GameCharacter character)
        {
            if (this.Type == Storable.TYPE_EFFECT)
            {
                if (!this.effectApplier.IsStored())
                {
                    this.effectApplier.Use(character);
                }
            }
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Returns a string representation of this storable item..
        /// </summary>
        /// <returns>The string.</returns>
        public override String ToString() {
			return this.Name;
		}
        #endregion

        #region Frames
        public void Start()
        {
            try
            {
                this.effectApplier = this.gameObject.GetComponent<EffectApplier>();
            } catch (NullReferenceException e) { this.effectApplier = null; }
        }
        #endregion
    }
}
