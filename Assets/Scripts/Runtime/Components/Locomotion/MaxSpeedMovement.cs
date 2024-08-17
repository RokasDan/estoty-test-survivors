using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal sealed class MaxSpeedMovement : IMovement
    {
        public void Move(Transform actorTransform, Vector2 direction, float speed)
        {
            actorTransform.Translate(direction.normalized * (speed * Time.deltaTime));
        }
    }
}
