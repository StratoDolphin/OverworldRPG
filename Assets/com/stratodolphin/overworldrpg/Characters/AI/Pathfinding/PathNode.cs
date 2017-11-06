using UnityEngine;
using System.Collections;

namespace Assets.com.stratodolphin.overworldrpg.Characters.AI.Pathfinding
{
    public class PathNode
    {
        #region Private Variables
        /// <summary>
        /// Position on the grid that this node represents.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Denotes whether or not a the pathfinder can
        /// consider walking through this point. If false,
        /// this node will never be evaluated as a possible
        /// node to walk through.
        /// </summary>
        public bool Passable;

        /// <summary>
        /// Distance from the start node to this node. This
        /// is the length of a straight line from start to
        /// this.
        /// </summary>
        public float G;

        /// <summary>
        /// Distance from the end node to this node. This
        /// is the length of a straight line from this to
        /// end.
        /// </summary>
        public float H;

        /// <summary>
        /// <para>
        /// Estimated distance cost of this node.
        /// </para>
        /// <para>
        /// Sum of the magnitudes of the vector between start
        /// and this plus the the vector bwetween this and end.
        /// For simplicity's sake, this is the sum of <see cref="G"/>
        /// and <see cref="H"/>.
        /// </para>
        /// </summary>
        public float F;

        /// <summary>
        /// Determines whether or not this node has already been
        /// evaluated. If this is <see cref="PathState.Closed"/>,
        /// then it should not be evaluated a second time as it
        /// must have already been evaluated.
        /// </summary>
        public PathState State;

        /// <summary>
        /// The <see cref="PathNode"/> that this node is reached by
        /// which is directly before this node. This allows us to
        /// retrace the path from then end to the start via the chain
        /// of these nodes. You can think of this as a Linked List of
        /// nodes where the link is via this variable.
        /// </summary>
        public PathNode ParentNode;
        #endregion
    }

    public enum PathState {
        Untested,
        Open,
        Closed
    }
}
