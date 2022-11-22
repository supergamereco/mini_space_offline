using System.Collections.Generic;
using NFT1Forge.OSY.Controller.Enemy;
using NFT1Forge.OSY.Controller.Movement;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class HazardSpawner : Spawner
    {
        [SerializeField] private float m_AsteroidMinSpeed = default;
        [SerializeField] private float m_AsteroidMaxSpeed = default;
        [SerializeField] private PositionZone m_AsteroidTargetZone = default;

        private readonly List<SpawnData> m_SpawnDataList = new List<SpawnData>();
        private uint m_CurrentHazardId = 10000;
        private MonsterMaster m_MonsterMaster;

        private Vector3 m_SpawnPosition;
        private Vector3 m_SpawnTopPosition;
        private Vector3 m_SpawnDownPosition;
        private Vector3 m_SpawnRightPosition;

        /// <summary>
        /// Called on first frame
        /// </summary>
        protected override void Start()
        {
            base.Start();
            m_MonsterMaster = DatabaseSystem.I.GetMetadata<MonsterMaster>();
            float cameraPosition = 100f;
            Vector3 spawnTopPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Random.Range(0f, Screen.width),
                    Screen.height + 10f,
                    cameraPosition)
                );
            Vector3 spawnDownPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Random.Range(0f, Screen.width),
                    - 10f,
                    cameraPosition)
                );
            Vector3 spawnRightPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Screen.width + 10f,
                    Random.Range(0f, Screen.height),
                    cameraPosition)
                );
            m_SpawnTopPosition = spawnTopPosition;
            m_SpawnDownPosition = spawnDownPosition;
            m_SpawnRightPosition = spawnRightPosition;
        }
        /// <summary>
        /// Set enemy list
        /// </summary>
        /// <param name="spawnList"></param>
        public void SetHazardSpawnData(List<SpawnData> spawnList)
        {
            m_SpawnDataList.Clear();
            m_SpawnDataList.AddRange(spawnList);
        }
        /// <summary>
        /// Instantiate hazard object into scene
        /// </summary>
        protected override void Spawn()
        {
            int spawnPoint = Random.Range(0, 3);
            if (spawnPoint == 0)
            {
                m_SpawnPosition = m_SpawnTopPosition;
            }
            else if (spawnPoint == 1)
            {
                m_SpawnPosition = m_SpawnDownPosition;
            }
            else if (spawnPoint == 2)
            {
                m_SpawnPosition = m_SpawnRightPosition;
            }

            if (0 >= m_SpawnDataList.Count)
                return;
            SpawnData data = RandomHazard();
            int index = m_MonsterMaster.monster_master.FindIndex(a => a.id.Equals(data.id));

            BaseEnemy hazard = ObjectPoolManager.I.GetObject<BaseEnemy>(ObjectType.Hazard, $"{ObjectType.Hazard}/{data.PrefabName}");
            ushort score = (ushort)Mathf.RoundToInt(m_MonsterMaster.monster_master[index].score * m_ScoreMultiplier);
            hazard.GetReady(m_CurrentHazardId,
                m_MonsterMaster.monster_master[index].attack * m_AttackMultiplier,
                m_MonsterMaster.monster_master[index].armor * m_ArmorMultiplier,
                score,
                ObjectType.Hazard, AfterDeath);
            hazard.transform.position = m_SpawnPosition;
            hazard.transform.rotation = Random.rotation;
            float randomX = Random.Range(m_AsteroidTargetZone.SpawnXPositionMin, m_AsteroidTargetZone.SpawnXPositionMax);
            float randomY = Random.Range(m_AsteroidTargetZone.SpawnYPositionMin, m_AsteroidTargetZone.SpawnYPositionMax);
            Vector3 targetPosion = new Vector3(randomX, randomY, 0);
            hazard.transform.LookAt2D(targetPosion);
            if (hazard.Moveable is ForwardMovement forwardMovement)
            {
                forwardMovement.SetSpeed(Random.Range(m_AsteroidMinSpeed, m_AsteroidMaxSpeed));
            }
            GameplayController.I.AddHazard(m_CurrentHazardId, hazard);
            m_CurrentHazardId++;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SpawnData RandomHazard()
        {
            return m_SpawnDataList[Random.Range(0, m_SpawnDataList.Count)];
        }
        /// <summary>
        /// Called after hazard was destroyed
        /// </summary>
        /// <param name="score"></param>
        protected void AfterDeath(uint id, Vector3 position)
        {
            GameplayController.I.EnemyKilled(id, true);
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(new Vector3(m_AsteroidTargetZone.SpawnXPositionMax, m_AsteroidTargetZone.SpawnYPositionMax),
                            new Vector3(m_AsteroidTargetZone.SpawnXPositionMax, m_AsteroidTargetZone.SpawnYPositionMin));
            Gizmos.DrawLine(new Vector3(m_AsteroidTargetZone.SpawnXPositionMin, m_AsteroidTargetZone.SpawnYPositionMax),
                            new Vector3(m_AsteroidTargetZone.SpawnXPositionMin, m_AsteroidTargetZone.SpawnYPositionMin));
            Gizmos.DrawLine(new Vector3(m_AsteroidTargetZone.SpawnXPositionMax, m_AsteroidTargetZone.SpawnYPositionMin),
                            new Vector3(m_AsteroidTargetZone.SpawnXPositionMin, m_AsteroidTargetZone.SpawnYPositionMin));
            Gizmos.DrawLine(new Vector3(m_AsteroidTargetZone.SpawnXPositionMax, m_AsteroidTargetZone.SpawnYPositionMax),
                            new Vector3(m_AsteroidTargetZone.SpawnXPositionMin, m_AsteroidTargetZone.SpawnYPositionMax));
        }
#endif
    }
}