using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.stratodolphin.overworldrpg.Characters.Inventory
{
    public class Inventory
    {
        #region Private Variables
		/// <summary>
		/// The actual storage for this inventory.
		/// </summary>
        private List<Storable> _items = new List<Storable>();

		/// <summary>
		/// Determines whether or not this inventory is a covered inventory
		/// such as a box or bag (false if so), or if it is an open
		/// inventory that people can see inside of such as hands or the
		/// body clothes (true if so).
		/// </summary>
		private bool _isVisibleInventory = false;

        /// <summary>
        /// The game character that owns this inventory. If this is null,
        /// then that means this is just a storage spot, not an inventory
        /// of a person.
        /// </summary>
        protected GameCharacter _owner;
        #endregion

        #region Public Attributes
        /// <summary>
        /// The maximum number of <see cref="Storable"/> items that
        /// this inventory may hold.
        /// </summary>
        public int Limit;

        /// <summary>
        /// Designates whether or not a game character owns this inventory
        /// or if the inventory is simply a storage spot in the world.
        /// </summary>
        public bool IsCharacterInventoy { get { return this._owner != null; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for inventory. This will set the limit
        /// for this inventory to null.
        /// </summary>
		public Inventory() { }

		/// <summary>
		/// Constructor that sets whether or not this inventory is a
		/// visible inventory where people can see the contents of it
		/// or not. This will set the limit for this inventory to null.
		/// </summary>
		public Inventory(bool isVisible) { this._isVisibleInventory = isVisible; }

        /// <summary>
        /// Instantiates an Inventory that has a limit of the given
        /// amount in <paramref name="limit"/>. isVisible will be used
		/// to set the value of <see cref="_isVisibleInventory"/>.
        /// </summary>
        /// <param name="limit"></param>
		public Inventory(int limit, bool isVisible=false)
        {
            this.Limit = limit;
			this._isVisibleInventory = isVisible;
        }
        #endregion

        #region List Interface
        /// <summary>
        /// Returns the number of <see cref="Storable"/>s that are
        /// contained in this inventory.
        /// </summary>
        /// <returns></returns>
        public int count()
        {
            return this._items.Count();
        }

        /// <summary>
        /// <para>
        /// Determines if this inventory is full by comparing the
        /// count with the limit of this inventory.
        /// </para>
        /// <para>
        /// If this inventory is full, true is returned. Otherwise,
        /// false is returned.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public bool isFull()
        {
            return this._items.Count() >= this.Limit;
        }

        /// <summary>
        /// <para>
        /// Override of the base Add method.
        /// </para>
        /// <para>
        /// If this inventory is full, an <see cref="IndexOutOfRangeException"/>
        /// will be thrown and the item won't be added.
        /// </para>
        /// </summary>
        /// <param name="item"></param>
        public void add(Storable item)
        {
            if (this.isFull()) throw new IndexOutOfRangeException("This inventory is already full. Item not added.");

            this._items.Add(item);
			item.Owner = this;
			this.updateStorablesVisibility (item);
        }

        /// <summary>
        /// Loops through items and copies each <see cref="Storable"/>
        /// from items into this inventory. If this inventory is full
        /// at any point, copying will stop and only some of the items
        /// will be added.
        /// </summary>
        /// <param name="items"></param>
        public void add(List<Storable> items)
        {
            int i = items.Count() - 1;
            while (!this.isFull() && i >= 0)
            {
                Storable itemToAdd = items[i];
                this.add(itemToAdd);
                i--;
            }
        }

        /// <summary>
        /// <para>
        /// Loops through items and moves each <see cref="Storable"/>
        /// from items into this inventory.
        /// </para>
        /// <para>
        /// If this inventory is full
        /// at any point, moving will stop and only some of the items
        /// will be added. Each item moved from the given inventory to
        /// this inventory will be removed from the one passed in.
        /// </para>
        /// </summary>
        /// <param name="items"></param>
        public void add(Inventory inventory)
        {
            int i = inventory.count() - 1;
            while (!this.isFull() && i >= 0)
            {
                Storable itemToAdd = inventory.all()[i];
                this.add(itemToAdd);
                inventory.remove(i);
                i--;
            }
        }

        /// <summary>
        /// Removes the element at the given index (i).
        /// </summary>
        /// <param name="i"></param>
        public void remove(int i)
        {
            this._items.RemoveAt(i);
        }

        /// <summary>
        /// Returns all items in this inventory.
        /// </summary>
        /// <returns></returns>
        public List<Storable> all()
        {
            return this._items;
        }

        /// <summary>
        /// Returns a list of items that are in this inventory that are
        /// of the type designated in type. This uses the Type field on
        /// each item to compare to type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Storable> getItemsByType(int type)
        {
			List<Storable> itemsOfType = new List<Storable>(from item in this._items where item.Type == type select item);
			//Debug.Log ("My items: " + this.ToString () + " but filtered list: " + itemsOfType.Count.ToString ());
            return new List<Storable>(from item in this._items where item.Type == type select item);
        }

		/// <summary>
		/// Determines whether or not this inventory contains an item
		/// of the type in the parameter type.
		/// </summary>
		/// <returns><c>true</c>, if this inventory has an item of type,
		/// <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		public bool hasItemType(int type) {
			return this.getItemsByType (type).Count >= 1;
		}
		#endregion

		#region Other
		/// <summary>
		/// Removes the item from the screen if this inventory is not an open, visible inventory.
		/// Otherwise, this adds the item back on screen.
		/// </summary>
		/// <param name="item">Item.</param>
		protected void updateStorablesVisibility(Storable item) {
			if (this._isVisibleInventory) {
				item.gameObject.SetActive (true);
			} else {
				item.gameObject.SetActive (false);
			}
		}
		#endregion

		#region Utility Methods
		/// <summary>
		/// Returns a string representation of this inventory.
		/// </summary>
		/// <returns>The string.</returns>
		public override String ToString() {
			String val = "[";
			for (int i = 0; i < (this.all ().Count); i++) {
				val = val + (this.all () [i].ToString()) + ", ";
			}
			val = val + "]";
			return val;
		}
		#endregion
    }
}
