using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        protected Vector3 m_BulletSpawnPosition => m_WeaponTransform.position;
        protected Quaternion m_BulletSpawnRotation => m_WeaponTransform.rotation;

        [SerializeField] protected Transform m_WeaponTransform = default;
        [SerializeField] private float m_ShootInterval = default;
        [SerializeField] private float m_WeaponInitializeTime = default;

        private float m_ShootCountdown;

        public virtual void RunWeapon()
        {
            m_ShootCountdown += Time.deltaTime;
            if (IsWeaponReady())
            {
                Shoot();
                m_ShootCountdown = 0;
            }
        }

        public abstract void Shoot();

        private void Awake()
        {
            m_ShootCountdown -= m_WeaponInitializeTime;
        }

        private bool IsWeaponReady() => m_ShootCountdown >= m_ShootInterval;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Ray(m_BulletSpawnPosition, m_WeaponTransform.right * 5));
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
#endif
    }
}
