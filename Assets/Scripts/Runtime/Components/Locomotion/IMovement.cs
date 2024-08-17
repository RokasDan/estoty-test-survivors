using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal interface IMovement
    {
        public void Move(Transform actorTransform, Vector2 direction, float speed);
    }
}
