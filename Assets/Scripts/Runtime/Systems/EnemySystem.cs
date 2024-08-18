using System.Collections.Generic;
using System.Linq;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.EnemySystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class EnemySystem : IEnemySystem, IStartable, ITickable
    {

        [Inject]
        private IObjectResolver objectResolver;
        private readonly EnemySystemSettings enemySystemSettings;
        private readonly IPlayerSystem playerSystem;
        private readonly ICameraSystem cameraSystem;
        private float spawnAreaWidth;
        private float enemySpawnRate;
        private float spawnTimer;
        private int maxEnemyCount;
        private readonly List<EnemyActor> aliveEnemies = new List<EnemyActor>();

        public EnemySystem(IPlayerSystem playerSystem, EnemySystemSettings enemySystemSettings, ICameraSystem cameraSystem)
        {
            this.enemySystemSettings = enemySystemSettings;
            this.playerSystem = playerSystem;
            this.cameraSystem = cameraSystem;
        }

        public void Start()
        {
            spawnAreaWidth = enemySystemSettings.spawnAreaWidth;
            enemySpawnRate = enemySystemSettings.enemySpawnRate;
            maxEnemyCount = enemySystemSettings.initialEnemyCount;
            spawnTimer = enemySpawnRate;
        }

        public void Tick()
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                var enemyPrefab = GetEnemyPrefab();
                SpawnEnemy(enemyPrefab);
                spawnTimer = enemySpawnRate;
            }
        }

        public void SpawnEnemy(EnemyActor enemyPrefab)
        {
            if (enemyPrefab == null || aliveEnemies.Count >= maxEnemyCount)
            {
                return;
            }
            var spawnPosition = GetRandomSpawnPosition();
            var enemyActor = objectResolver.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            TrackEnemy(enemyActor);
            if (playerSystem.TryGetPlayer(out var player))
            {
                enemyActor.AddEnemySystem(this);
                enemyActor.SetPlayerLocation(player);
            }
        }

        public void TrackEnemy(EnemyActor enemyActor)
        {
            aliveEnemies.Add(enemyActor);
        }

        public void UntrackEnemy(EnemyActor enemyActor)
        {
            aliveEnemies.Remove(enemyActor);
        }

        public Vector2 GetRandomSpawnPosition()
        {
            if (cameraSystem.TryGetCamera(out var targetCamera))
            {
                if (targetCamera == null)
                {
                    return Vector2.zero;
                }
                var cameraPosition = targetCamera.transform.position;
                var cameraHeight = 2f * targetCamera.orthographicSize;
                var cameraWidth = cameraHeight * targetCamera.aspect;
                var randomX = Random.Range(-cameraWidth - spawnAreaWidth, cameraWidth + spawnAreaWidth) + cameraPosition.x;
                var randomY = Random.Range(-cameraHeight - spawnAreaWidth, cameraHeight + spawnAreaWidth) + cameraPosition.y;
                return new Vector2(randomX, randomY);
            }
            return Vector2.zero;
        }

        public EnemyActor GetEnemyPrefab()
        {
            var enemyPrefabs = enemySystemSettings.enemyPrefabs;
            if (enemyPrefabs.Count == 1)
            {
                return enemyPrefabs.FirstOrDefault();
            }
            var totalWeight = enemyPrefabs.Sum(e => 1.0f / e.GetRarityLevel());
            var randomWeight = Random.Range(0, totalWeight);
            var cumulativeWeight = 0.0f;
            foreach (var enemyPrefab in enemyPrefabs)
            {
                cumulativeWeight += 1.0f / enemyPrefab.GetRarityLevel();
                if (randomWeight < cumulativeWeight)
                {
                    return enemyPrefab;
                }
            }
            return enemyPrefabs.FirstOrDefault();
        }
    }
}
