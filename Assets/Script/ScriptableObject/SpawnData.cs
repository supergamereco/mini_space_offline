using System.Collections.Generic;
using NFT1Forge.OSY.Controller;
using UnityEngine;

namespace NFT1Forge.OSY.DataModel
{
    [CreateAssetMenu(fileName = "New Item", menuName = "OSY/Spawn Data")]
    public class SpawnData : ScriptableObject
    {
        public int id;
        public float SpawnChance; // Max = 1f
        public string PrefabName;
    }
}