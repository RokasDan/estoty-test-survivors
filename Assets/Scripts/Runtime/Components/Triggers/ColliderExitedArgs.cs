using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers
{
    internal readonly struct ColliderExitedArgs
    {
        public Collider2D Collider { get; }

        public ColliderExitedArgs(Collider2D collider)
        {
            Collider = collider;
        }
    }
}
