using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.Controller.Enemy;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class GameplayLaser : MonoBehaviour
    {
        public float MaxFuel => m_MaxFuel;
        public float CurrentFuel {get; private set;}

        [SerializeField] private GameObject m_LaserVisualize = default;
        [SerializeField] private Collider2D m_LaserCollider = default;
        [Tooltip("Avoid do damage to enemy that out of screen")]
        [SerializeField] private float m_LimitLaserXPos = default;
        [SerializeField] private float m_DamagePerSec = default;
        [Tooltip("Determine how long laser can activate")]
        [SerializeField] private float m_MaxFuel = default;
        [SerializeField] private float m_FuelRegenerationRate = default;
        [SerializeField] private string m_HitEffectName = default;

        private List<BaseEnemy> enemyList = new List<BaseEnemy>();
        private bool m_IsLaserClosed;

        private void Awake()
        {
            ResetLaser();
        }

        private void FixedUpdate()
        {
            if (m_IsLaserClosed)
            {
                CurrentFuel += CurrentFuel >= MaxFuel ? 0 : m_FuelRegenerationRate * Time.deltaTime;
                return;
            }

            CurrentFuel -= Time.deltaTime;
            if (0 >= CurrentFuel)
            {
                CloseLaser();
                return;
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] == null || enemyList[i].IsDestroy || !enemyList[i].IsActive)
                {
                    enemyList.RemoveAt(i);
                    i--;
                    continue;
                }
                
                if (enemyList[i].transform.position.x < m_LimitLaserXPos)
                {
                    CreateHitParticle(enemyList[i].transform);
                    enemyList[i].TakeDamage(m_DamagePerSec * Time.deltaTime, true);
                }
            }
        }
        /// <summary>
        /// VFX
        /// </summary>
        /// <param name="hitTransform"></param>
        public void CreateHitParticle(Transform hitTransform)
        {
            Transform bullet = ObjectPoolManager.I.GetObject(ObjectType.VFX, m_HitEffectName, transform);
            bullet.transform.position = hitTransform.position;
            bullet.transform.rotation = hitTransform.rotation;
        }

        public void ResetLaser()
        {
            CurrentFuel = MaxFuel;
            CloseLaser();
        }

        public bool IsReady() => CurrentFuel >= m_MaxFuel;

        public void OpenLaser()
        {
            m_LaserVisualize.SetActive(true);
            m_LaserCollider.enabled = true;
            m_IsLaserClosed = false;
        }

        public void CloseLaser()
        {
            m_LaserVisualize.SetActive(false);
            m_LaserCollider.enabled = false;
            m_IsLaserClosed = true;
            enemyList.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                BaseEnemy enemy = other.GetComponent<BaseEnemy>();
                if (null != enemy)
                {
                    enemyList.Add(enemy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                BaseEnemy enemy = other.GetComponent<BaseEnemy>();
                if (null != enemy)
                {
                    enemyList.Remove(enemy);
                }
            }
        }
    }
}
