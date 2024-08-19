﻿using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.CollectibleTable
{
    [CreateAssetMenu(fileName = "CollectibleTableData", menuName = "ScriptableObjects/CollectibleTableData")]
    internal sealed class CollectibleTableData : ScriptableObject
    {
        public List<CollectibleDrop> collectibles;

        [System.Serializable]
        public class CollectibleDrop
        {
            [Required]
            public GameObject collectiblePrefab;
            [Range(0f, 1f)]
            public float dropChance = 1f;
            [Min(1)]
            public int minQuantity = 1;
            [Min(1)]
            public int maxQuantity = 1;
        }
    }
}