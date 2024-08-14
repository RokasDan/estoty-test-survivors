using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal interface IMovement
    {
        public void Move(Transform transform, Vector2 direction, float speed);
    }
}
