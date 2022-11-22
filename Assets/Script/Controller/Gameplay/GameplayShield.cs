using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.Controller.Bullet;
using NFT1Forge.OSY.Controller.Enemy;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class GameplayShield : MonoBehaviour
    {
        public float MaxFuel => m_MaxFuel;
        public float CurrentFuel {get; private set;}

        [SerializeField] private GameObject m_ShieldVisualize = default;
        [SerializeField] private Collider2D m_ShieldCollider = default;
        [Tooltip("Avoid do damage to enemy that out of screen")]
        [SerializeField] private float m_DamagePerSec = default;
        [Tooltip("Determine how long laser can activate")]
        [SerializeField] private float m_MaxFuel = default;
        [SerializeField] private float m_FuelRegenerationRate = default;
        [SerializeField] private string m_HitEffectName = default;

        private readonly List<BaseEnemy> m_EnemyList = new List<BaseEnemy>();
        private bool m_IsShieldClosed;
        private string m_SpecialWeaponName = default;
        private Vector3 m_ShieldRange = new Vector3(1f, 1f, 1f);

        private void FixedUpdate()
        {
            if (m_IsShieldClosed)
            {
                CurrentFuel += CurrentFuel >= MaxFuel ? 0 : m_FuelRegenerationRate * Time.deltaTime;
                return;
            }

            CurrentFuel -= Time.deltaTime;
            if (0 >= CurrentFuel)
            {
                CloseShield();
                return;
            }

            if (m_SpecialWeaponName == "Bardeen Reactor")
            {
                m_ShieldCollider.transform.localScale = m_ShieldCollider.transform.localScale + (m_ShieldRange * Time.deltaTime * 1.3f);
                if(m_ShieldCollider.transform.localScale.x >= 2.75f)
                {
                    m_ShieldCollider.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                }
                
            }

            for (int i = 0; i < m_EnemyList.Count; i++)
            {
                if (null == m_EnemyList[i] || m_EnemyList[i].IsDestroy || !m_EnemyList[i].IsActive)
                {
                    m_EnemyList.RemoveAt(i);
                    i--;
                    continue;
                }
                
                if (null != m_EnemyList[i])
                {
                    CreateHitParticle(m_EnemyList[i].transform);
                    m_EnemyList[i].TakeDamage(m_DamagePerSec * Time.deltaTime, true);
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

        public void ResetShield()
        {
            SpecialWeaponDataModel weapon = InventorySystem.I.GetActiveWeapon();
            m_SpecialWeaponName = weapon.name;
            CurrentFuel = MaxFuel;
            m_ShieldCollider.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            CloseShield();
        }

        public bool IsReady() => CurrentFuel >= m_MaxFuel;

        public void OpenShiled()
        {
            m_ShieldVisualize.SetActive(true);
            m_ShieldCollider.enabled = true;
            m_IsShieldClosed = false;
        }

        public void CloseShield()
        {
            m_ShieldVisualize.SetActive(false);
            m_ShieldCollider.enabled = false;
            m_IsShieldClosed = true;
            m_EnemyList.Clear();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                BaseEnemy enemy = other.GetComponent<BaseEnemy>();
                if (null != enemy)
                {
                    m_EnemyList.Add(enemy);
                }
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                BaseBullet bullet = other.GetComponent<BaseBullet>();
                ObjectPoolManager.I.ReturnToPool(bullet, ObjectType.Bullet);
                bullet.CreateHitParticle(transform);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                BaseEnemy enemy = other.GetComponent<BaseEnemy>();
                if (null != enemy)
                {
                    m_EnemyList.Remove(enemy);
                }
            }
        }
    }
}
