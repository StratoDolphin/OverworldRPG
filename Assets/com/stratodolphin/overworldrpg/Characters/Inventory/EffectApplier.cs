using UnityEngine;
using System.Collections;

namespace com.stratodolphin.overworldrpg.Characters.Inventory
{
    /// <summary>
    /// Applies an effect on a given player.
    /// </summary>
    public interface EffectApplier
    {
        /// <summary>
        /// Denotes whether or not this item will be used as soon
        /// as it is picked up, or if it can be saved for later use.
        /// This should just simply return true or false.
        /// </summary>
        /// <returns></returns>
        bool IsStored();

        /// <summary>
        /// Applies this item and then removes it from the game.
        /// </summary>
        /// <param name="character"></param>
        void Use(GameCharacter character);
    }
}