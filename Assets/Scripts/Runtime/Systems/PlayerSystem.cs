﻿using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class PlayerSystem : MonoBehaviour, IPlayerSystem, IStartable
    {
        [Required]
        [SerializeField]
        private ActorPlayer actorPlayerPrefab;

        [Inject]
        private IObjectResolver objectResolver;
        private ActorPlayer actorPlayerInstance;

        public void Start()
        {
            if (!TryGetPlayer(out _))
            {
                actorPlayerInstance = objectResolver.Instantiate(
                    actorPlayerPrefab,
                    Vector3.zero,
                    Quaternion.identity
                );
            }
        }

        public bool TryGetPlayer(out IActorPlayer actorPlayer)
        {
            if (actorPlayerInstance == false)
            {
                actorPlayer = default;
                return false;
            }

            actorPlayer = actorPlayerInstance;
            return true;
        }
    }
}
