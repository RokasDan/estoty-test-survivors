using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class PlayerSystem : MonoBehaviour, IPlayerSystem, IStartable
    {
        [Inject]
        private IObjectResolver objectResolver;

        [Required]
        [SerializeField]
        private PlayerActor playerActorPrefab;

        private PlayerActor playerInstance;

        public void Start()
        {
            if (!TryGetPlayer(out var player))
            {
                playerInstance = objectResolver.Instantiate(
                    playerActorPrefab,
                    Vector3.zero,
                    Quaternion.identity
                );
            }
        }

        public bool TryGetPlayer(out IPlayerActor player)
        {
            if (playerInstance == false)
            {
                player = default;
                return false;
            }

            player = playerInstance;
            return true;
        }
    }
}
