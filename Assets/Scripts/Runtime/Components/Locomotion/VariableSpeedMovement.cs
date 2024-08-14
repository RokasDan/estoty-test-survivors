using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal sealed class VariableSpeedMovement : IMovement
    {
        public void Move(Transform transform, Vector2 direction, float speed)
        {
            transform.Translate(direction * (speed * Time.deltaTime));
        }
    }
}
