using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables;
using RokasDan.EstotyTestSurvivors.Runtime.Data.CollectibleTable;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class CollectibleSystem : MonoBehaviour, ICollectibleSystem
    {
        [Inject]
        private IObjectResolver objectResolver;
        private readonly List<ICollectable> activeCollectables = new List<ICollectable>();

        private Vector2 RandomizeSpawnPosition (Vector2 center, float radius)
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
            var distance = Random.Range(0f, radius);
            var x = center.x + distance * Mathf.Cos(angle);
            var y = center.y + distance * Mathf.Sin(angle);
            return new Vector2(x, y);
        }

        public void SpawnCollectables(CollectibleTableData collectibleTable, Vector2 position, float spreadRadius)
        {
            foreach (var collectible in collectibleTable.collectibles)
            {
                if (Random.value <= collectible.dropChance)
                {
                    int quantity = Random.Range(collectible.minQuantity, collectible.maxQuantity + 1);
                    for (int i = 0; i < quantity; i++)
                    {
                        var randomPosition = RandomizeSpawnPosition(position, spreadRadius);
                        var instantiatedPrefab = objectResolver.Instantiate(collectible.collectiblePrefab, randomPosition, Quaternion.identity);
                        var spawnedCollectible = instantiatedPrefab.GetComponent<ICollectable>();
                        if (spawnedCollectible != null)
                        {
                            TrackCollectables(spawnedCollectible);
                        }
                    }
                }
            }
        }

        public void TrackCollectables(ICollectable collectable)
        {
            activeCollectables.Add(collectable);
        }

        public void UntrackCollectables(ICollectable collectable)
        {
            activeCollectables.Remove(collectable);
        }

        public void DestroyAllCollectables()
        {
            if (activeCollectables.Count > 0)
            {
                foreach (var collectable in activeCollectables)
                {
                    UntrackCollectables(collectable);
                    collectable.DestroyCollectable();
                }
            }
        }

        public List<ICollectable> ActiveCollectibles => activeCollectables;
    }
}
