using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class CameraSystem : MonoBehaviour, ICameraSystem
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private float dampening = 0;

        [Inject]
        private IPlayerSystem playerSystem;

        private Vector3 velocity = Vector3.zero;

        public void FollowPlayer(IPlayerActor player)
        {
            var playerPosition = player.PlayerTransform.position;
            var offsetTarget = new Vector3(playerPosition.x, playerPosition.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, offsetTarget, ref velocity, dampening);
        }

        private void Update()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                FollowPlayer(player);
            }
        }
    }
}
