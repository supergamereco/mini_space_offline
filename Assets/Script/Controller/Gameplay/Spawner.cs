using NFT1Forge.OSY.DataModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public abstract partial class Spawner : MonoBehaviour
    {
        [SerializeField] protected Transform m_ObjectHolder;
        [SerializeField] private float m_MinSpawnInterval = 2f;
        [SerializeField] private float m_MaxSpawnInterval = 5f;
        [SerializeField] private PositionZone[] m_SpawnPositionZoneArray = default;

        protected float m_AttackMultiplier = 1f;
        protected float m_ArmorMultiplier = 1f;
        protected float m_ScoreMultiplier = 1f;

        private float m_CurrentTime = 0f;
        private float m_NextSpawnTime = 0f;
        private bool m_IsSpawning = false;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Start()
        {
            m_NextSpawnTime = Random.Range(m_MinSpawnInterval, m_MaxSpawnInterval);
            GameplayController.I.OnGameStateChanged += StateChanged;
        }
        /// <summary>
        /// Set multiplier for enemy spawned
        /// </summary>
        public void SetMultiplier(float attack, float armor, float score)
        {
            m_AttackMultiplier = attack;
            m_ArmorMultiplier = armor;
            m_ScoreMultiplier = score;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void StateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    m_IsSpawning = true;
                    break;

                default:
                    m_IsSpawning = false;
                    break;
            }
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (m_IsSpawning)
            {
                m_CurrentTime += Time.deltaTime;
                if (m_CurrentTime > m_NextSpawnTime)
                {
                    m_NextSpawnTime = Random.Range(m_MinSpawnInterval, m_MaxSpawnInterval);
                    m_CurrentTime = 0;
                    Spawn();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetSpawnPoint()
        {
            Vector3 newPosition = new Vector3();
            PositionZone selectedZone = m_SpawnPositionZoneArray[Random.Range(0, m_SpawnPositionZoneArray.Length)];
            newPosition.x = Random.Range(selectedZone.SpawnXPositionMin, selectedZone.SpawnXPositionMax);
            newPosition.y = Random.Range(selectedZone.SpawnYPositionMin, selectedZone.SpawnYPositionMax);
            return newPosition;
        }

        /// <summary>
        /// Add object into scene
        /// </summary>
        protected abstract void Spawn();

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < m_SpawnPositionZoneArray.Length; i++)
            {
                PositionZone zone = m_SpawnPositionZoneArray[Random.Range(0, m_SpawnPositionZoneArray.Length)];
                if (null == zone) return;
                Gizmos.DrawLine(new Vector3(zone.SpawnXPositionMax, zone.SpawnYPositionMax),
                                new Vector3(zone.SpawnXPositionMax, zone.SpawnYPositionMin));
                Gizmos.DrawLine(new Vector3(zone.SpawnXPositionMin, zone.SpawnYPositionMax),
                                new Vector3(zone.SpawnXPositionMin, zone.SpawnYPositionMin));
                Gizmos.DrawLine(new Vector3(zone.SpawnXPositionMax, zone.SpawnYPositionMin),
                                new Vector3(zone.SpawnXPositionMin, zone.SpawnYPositionMin));
                Gizmos.DrawLine(new Vector3(zone.SpawnXPositionMax, zone.SpawnYPositionMax),
                                new Vector3(zone.SpawnXPositionMin, zone.SpawnYPositionMax));
            }
        }
#endif
    }
}
