﻿using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
using RokasDan.EstotyTestSurvivors.Runtime.Components.PlayerRotation;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal interface IEnemyActor: IMovement
    {
        public void DamageEnemy(int damage);
        public void KillEnemy();
        public void FlipEnemySprite();
        public void SetPlayerLocation(IPlayerActor playerActor);
        public void AddEnemySystem(EnemySystem system);
        public void AttackPlayer(IPlayerActor playerActor);
        public void DetectPlayer(ColliderEnteredArgs args);
        public void LosePlayer(ColliderExitedArgs args);
        public void PushEnemy(Vector2 force);
        public int GetRarityLevel();
        public Transform EnemyTransform { get; }
    }
}
