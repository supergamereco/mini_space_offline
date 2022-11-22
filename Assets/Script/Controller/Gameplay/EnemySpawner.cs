using System.Collections.Generic;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private Transform m_BossSpawnPoint;

        private Vector3 m_DefaultEnemyRotation;
        private readonly List<SpawnData> m_SpawnDataList = new List<SpawnData>();
        private SpawnData m_BossSpawnData;
        private readonly List<float> m_AccumulateChance = new List<float>();
        private Vector3 m_DefaultItemRotation = Vector3.zero;
        private readonly List<SpawnData> m_ItemSpawnDataList = new List<SpawnData>();
        private SpawnData m_ChestSpawnData;
        private readonly List<float> m_ItemAccumulateChance = new List<float>();
        private SpawnData m_GoldSpawnData;
        private uint m_CurrentEnemyId = 0;
        private uint m_CurrentItemId = 0;
        private MonsterMaster m_MonsterMaster;
        private bool m_IsMissionChestDrop = false;
        private bool m_IsItemDrop = false;
        private Vector3 m_SpawnPosition;
        private Vector3 m_BossSpawnPosition;
        private float m_MinSpawnPosition;
        private float m_MaxSpawnPosition;

        /// <summary>
        /// Called on first frame
        /// </summary>
        protected override void Start()
        {
            base.Start();
            m_DefaultEnemyRotation = Quaternion.identity.eulerAngles;
            m_DefaultEnemyRotation.y = 180f;
            m_MonsterMaster = DatabaseSystem.I.GetMetadata<MonsterMaster>();

            float cameraPosition = 100f;
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Screen.width,
                    Screen.height,
                    cameraPosition)
                );
            Vector3 minSpawnPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Screen.width,
                    10,
                    cameraPosition)
                );
            Vector3 bossSpawnPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Screen.width,
                    Screen.height/2,
                    cameraPosition)
                );
            m_MinSpawnPosition = minSpawnPosition.y;
            m_MaxSpawnPosition = spawnPosition.y - 3;
            m_SpawnPosition = spawnPosition;
            m_BossSpawnPosition = bossSpawnPosition;
        }
        /// <summary>
        /// Set enemy list
        /// </summary>
        /// <param name="spawnList"></param>
        public void SetEnemySpawnData(List<SpawnData> spawnList)
        {
            m_SpawnDataList.Clear();
            m_AccumulateChance.Clear();
            m_SpawnDataList.AddRange(spawnList);
            float accumRate = 0f;
            for (int i = 0; i < m_SpawnDataList.Count; i++)
            {
                accumRate += m_SpawnDataList[i].SpawnChance;
                m_AccumulateChance.Add(accumRate);
            }
        }
        /// <summary>
        /// Set boss spawn data
        /// </summary>
        /// <param name="data"></param>
        public void SetBossSpawnData(SpawnData data)
        {
            m_BossSpawnData = data;
        }
        /// <summary>
        /// Set drop item list
        /// </summary>
        /// <param name="itemList"></param>
        public void SetItemSpawnData(List<SpawnData> itemList)
        {
            m_ItemSpawnDataList.Clear();
            m_ItemAccumulateChance.Clear();
            m_ItemSpawnDataList.AddRange(itemList);
            float accumRate = 0f;
            for (int i = 0; i < m_ItemSpawnDataList.Count; i++)
            {
                accumRate += m_ItemSpawnDataList[i].SpawnChance;
                m_ItemAccumulateChance.Add(accumRate);
            }
        }
        /// <summary>
        /// Set chest spawn data
        /// </summary>
        /// <param name="data"></param>
        public void SetChestSpawnData(SpawnData data)
        {
            m_ChestSpawnData = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetGoldSpawnData(SpawnData data)
        {
            m_GoldSpawnData = data;
        }
        /// <summary>
        /// Spawn an enemy into scene
        /// </summary>
        protected override void Spawn()
        {
            SpawnData data = RandomEnemy();
            if (null == data)
                return;
            int index = m_MonsterMaster.monster_master.FindIndex(a => a.id.Equals(data.id));
            BaseEnemy enemy = ObjectPoolManager.I.GetObject<BaseEnemy>(ObjectType.Enemy, $"{ObjectType.Enemy}/{data.PrefabName}");
            m_SpawnPosition.y = Random.Range(m_MinSpawnPosition, m_MaxSpawnPosition);
            enemy.transform.position = m_SpawnPosition;
            enemy.transform.rotation = Quaternion.Euler(m_DefaultEnemyRotation);
            ushort score = (ushort)Mathf.RoundToInt(m_MonsterMaster.monster_master[index].score * m_ScoreMultiplier);
            enemy.GetReady(m_CurrentEnemyId,
                m_MonsterMaster.monster_master[index].attack * m_AttackMultiplier,
                m_MonsterMaster.monster_master[index].armor * m_ArmorMultiplier,
                score,
                ObjectType.Enemy, AfterDeath);
            GameplayController.I.AddEnemy(m_CurrentEnemyId, enemy);
            m_CurrentEnemyId++;
        }
        /// <summary>
        /// Called after enemy die
        /// </summary>
        /// <param name="score"></param>
        protected void AfterDeath(uint id, Vector3 position)
        {
            int amounts = Random.Range(1, 5);
            for (int i = 0; i < amounts; i++)
            {
                float distanceX = Random.Range(-8f, 8f);
                float distanceY = Random.Range(-8f, 8f);
                int direction = Random.Range(-1, 1);
                if (i == 0)
                {
                    distanceX = 0f;
                    distanceY = 0f;
                }
                float itemChance = Random.Range(0.001f, 1f);
                float chestChance = Random.Range(0.001f, 1f);
                if (itemChance <= GameplayDataManager.I.ItemDropRate && m_IsItemDrop == false)
                    SpawnItem(distanceX, distanceY, direction, position, "item");

                else if(itemChance >= GameplayDataManager.I.ItemDropRate)
                    SpawnItem(distanceX, distanceY, direction, position, "gold");

                else if(chestChance <= GameplayDataManager.I.ChestDropRate && m_IsMissionChestDrop == false)
                    SpawnItem(distanceX, distanceY, direction, position, "chest");
            }
            m_IsItemDrop = false;
            GameplayController.I.EnemyKilled(id, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected BaseItem SpawnItem(float distanceX, float distanceY, int direction, Vector3 position, string itemtype)
        {
            SpawnData data = RandomItem();
            if (itemtype == "gold")
                data = m_GoldSpawnData;
            if (itemtype == "chest")
                data = m_ChestSpawnData;
            BaseItem item = ObjectPoolManager.I.GetObject<BaseItem>(ObjectType.Item, $"{ObjectType.Item}/{data.PrefabName}");
            position.x = position.x + distanceX;
            position.y = position.y + distanceY;
            if (position.y < m_MinSpawnPosition)
                position.y = m_MinSpawnPosition;
            else if (position.y > m_MaxSpawnPosition)
                position.y = m_MaxSpawnPosition;
            item.transform.rotation = Quaternion.Euler(m_DefaultItemRotation);
            item.GetReady(GameplayController.I.CatchItem, position);
            GameplayController.I.AddItem(m_CurrentItemId, item);
            if (itemtype == "item")
                m_IsItemDrop = true;
            if (itemtype == "chest")
                m_IsMissionChestDrop = true;
            m_CurrentItemId++;
            return item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SpawnData RandomEnemy()
        {
            float chance = Random.Range(0.001f, 1f);
            for (int i = 0; i < m_AccumulateChance.Count; i++)
            {
                if (chance <= m_AccumulateChance[i])
                    return m_SpawnDataList[i];
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SpawnData RandomItem()
        {
            float chance = Random.Range(0.001f, 1f);
            for (int i = 0; i < m_ItemAccumulateChance.Count; i++)
            {
                if (chance <= m_ItemAccumulateChance[i])
                {
                    return m_ItemSpawnDataList[i];
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SpawnBoss()
        {
            BossEnemy boss = ObjectPoolManager.I.GetObject<BossEnemy>(ObjectType.Enemy, $"{ObjectType.Enemy}/{m_BossSpawnData.PrefabName}");
            int index = m_MonsterMaster.monster_master.FindIndex(a => a.id.Equals(m_BossSpawnData.id));

            boss.name = m_MonsterMaster.monster_master[index].name;
            boss.transform.SetPositionAndRotation(m_BossSpawnPosition, Quaternion.Euler(m_DefaultEnemyRotation));
            ushort score = (ushort)Mathf.RoundToInt(m_MonsterMaster.monster_master[index].score * m_ScoreMultiplier);
            boss.GetReady(0,
                m_MonsterMaster.monster_master[index].attack * m_AttackMultiplier,
                m_MonsterMaster.monster_master[index].armor * m_ArmorMultiplier,
                score,
                ObjectType.Enemy, AfterBossDeath);
            GameplayController.I.SetBoss(boss);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        private void AfterBossDeath(uint id, Vector3 position)
        {
            GameplayController.I.BossKilled();
        }
    }
}
