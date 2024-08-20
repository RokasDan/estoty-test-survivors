using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.LootTables
{
    [CreateAssetMenu(fileName = "New CollectibleTableData", menuName = "Collectibles/CollectibleTable")]
    internal sealed class CollectibleTableData : ScriptableObject
    {
        public List<CollectibleDrop> collectibles;

        [System.Serializable]
        public class CollectibleDrop
        {
            [Required]
            [SerializeField]
            private GameObject collectiblePrefab;

            [Range(0f, 1f)]
            [SerializeField]
            private float dropChance = 1f;

            [Min(1)]
            [SerializeField]
            private int minQuantity = 1;

            [Min(1)]
            [SerializeField]
            public int maxQuantity = 1;

            public GameObject CollectiblePrefab
            {
                get { return collectiblePrefab; }
            }

            public float DropChance
            {
                get { return dropChance; }
            }

            public int MinQuantity
            {
                get { return minQuantity; }
            }

            public int MaxQuantity
            {
                get { return maxQuantity; }
            }
        }
    }
}
