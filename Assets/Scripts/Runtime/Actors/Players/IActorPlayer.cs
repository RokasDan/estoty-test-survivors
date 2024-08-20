using System;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Players
{
    internal interface IActorPlayer
    {
        public void DamagePlayer(int damage);
        public void PushPlayer(Vector2 force);
        public bool IsPlayerDead { get; }
        public Transform PlayerTransform { get; }
        public BaseProjectileActor ProjectileActor { get; set; }
        public int CurrentPlayerAmmo { get; set; }
        public int MaxPlayerHealth { get; set; }
        public int CurrentPlayerHealth { get; set; }
        public int MaxPlayerExperience { get; set; }
        public int CurrentPlayerExperience { get; set; }
        public int CurrentScoreCount { get; set; }
        public Action OnStatsChanged { get; set; }
        public float PlayerSpeed { get; set; }
        public float PlayerFireRate { get; set; }
    }
}
