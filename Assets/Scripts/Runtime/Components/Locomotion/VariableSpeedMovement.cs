using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal sealed class VariableSpeedMovement : IMovement
    {
        public void Move(Transform actorTransform, Vector2 direction, float speed)
        {
            actorTransform.Translate(direction * (speed * Time.deltaTime));
        }
    }
}
