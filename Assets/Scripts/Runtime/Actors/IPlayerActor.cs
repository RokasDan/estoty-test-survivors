using System;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors
{
    internal interface IPlayerActor: IMovement
    {
        public void DamagePlayer(int damage);
        public void KillPlayer();
        public void PushPlayer(Vector2 force);
        public bool IsPlayerDead { get; }
        public Transform PlayerTransform { get; }
        public int MaxPlayerHealth { get; }
        public int CurrentPlayerHealth { get; }
        public int CurrentPlayerExperience { get; }
        public int BulletCount { get; }
        public int ScoreCount { get; }
        public event Action OnStatsChanged;
    }
}
