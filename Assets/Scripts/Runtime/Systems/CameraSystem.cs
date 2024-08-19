using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
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

        public void FollowPlayer(IActorPlayer actorPlayer)
        {
            var playerPosition = actorPlayer.PlayerTransform.position;
            var offsetTarget = new Vector3(playerPosition.x, playerPosition.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, offsetTarget, ref velocity, dampening);
        }

        public bool TryGetCamera(out Camera targetCamera)
        {
            if (mainCamera == false)
            {
                targetCamera = default;
                return false;
            }

            targetCamera = mainCamera;
            return true;
        }

        private void FixedUpdate()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                FollowPlayer(player);
            }
        }

        public Vector2 GetOutsideCameraPosition(int offset)
        {
            if (!mainCamera)
            {
                return Vector2.zero;
            }
            var cameraBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
            var cameraTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.transform.position.z));
            var  minX = cameraBottomLeft.x;
            var maxX = cameraTopRight.x;
            var minY = cameraBottomLeft.y;
            var maxY = cameraTopRight.y;
            var side = Random.Range(0, 4);
            Vector2 spawnPosition;
            switch (side)
            {
                case 0: // Top
                    spawnPosition = new Vector2(Random.Range(minX, maxX), maxY - offset);
                    break;
                case 1: // Bottom
                    spawnPosition = new Vector2(Random.Range(minX, maxX), minY + offset);
                    break;
                case 2: // Left
                    spawnPosition = new Vector2(minX + offset, Random.Range(minY, maxY));
                    break;
                case 3: // Right
                    spawnPosition = new Vector2(maxX - offset, Random.Range(minY, maxY));
                    break;
                default:
                    spawnPosition = Vector2.zero;
                    break;
            }
            return spawnPosition;
        }
    }
}
