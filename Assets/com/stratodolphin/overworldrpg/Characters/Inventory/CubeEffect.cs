using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.stratodolphin.overworldrpg.Characters.Inventory
{
	public class CubeEffect : MonoBehaviour, EffectApplier {

		#region Public Attributes
		public float DecAmount;
		#endregion

		#region Public Methods
		/// <summary>
		/// Increases the characters
		/// <see cref="GameCharacter._hitPoints"/> by the amount
		/// stored in <see cref="IncAmount"/>. If the
		/// amount is negative, the characters health will
		/// be decreased by that amount.
		/// </summary>
		/// <param name="_character"></param>
		protected void Apply(GameCharacter character)
		{
			character.decreaseHealth(this.DecAmount);
		}

		/// <summary>
		/// Applies this item and then removes it from the game.
		/// This is an implementation of <see cref="EffectApplier.Use(GameCharacter)"/>.
		/// </summary>
		/// <param name="character"></param>
		public void Use(GameCharacter character)
		{
			this.Apply(character);
			Destroy(this.gameObject);
		}

		/// <summary>
		/// Denotes whether or not this item will be used as soon
		/// as it is picked up, or if it can be saved for later use.
		/// This is an implementation of <see cref="EffectApplier.IsStored()"/>
		/// and returns true.
		/// </summary>
		/// <returns></returns>
		public bool IsStored() { return false; }
		#endregion
	}
}
