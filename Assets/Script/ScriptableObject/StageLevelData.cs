using System.Collections.Generic;
using UnityEngine;

namespace NFT1Forge.OSY.DataModel
{
    [CreateAssetMenu(fileName = "New Item", menuName = "OSY/Stage Level")]
    public class StageLevelData : ScriptableObject
    {
        public string LevelName;
        public Material MaterialSkybox;
        public List<SpawnData> EnemySpawnData;
        public List<SpawnData> ItemSpawnData;
        public List<SpawnData> HazardSpawnData;
        public SpawnData BossSpawnData;
        public SpawnData ChestSpawnData;
        public SpawnData GoldSpawnData;
    }
}
