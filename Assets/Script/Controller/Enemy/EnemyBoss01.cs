using NFT1Forge.OSY.Controller.Weapon;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Enemy
{
    public class EnemyBoss01 : BossEnemy
    {
        //BOSS01 - DEEP SPACE - Big Bee
        [SerializeField] protected BaseWeapon[] m_WeaponArray2;
        [SerializeField] private float m_MinTranformTime = 5f;
        [SerializeField] private float m_MaxTranformTime = 8f;
        [SerializeField] private float m_TransformDuration = 1f;
        [SerializeField] private Transform m_PivotTransform;
        [SerializeField] private float m_WingWeaponRotation;
        [SerializeField] private Collider2D[] m_ColliderArray1;
        [SerializeField] private Collider2D[] m_ColliderArray2;
        private enum Behaviour { TailWeapon, WingWeapon, Transform }
        private Behaviour m_CurrentBehaviour = Behaviour.TailWeapon;
        private Behaviour m_TargetBehaviour;
        private float m_TransformTime = 0f;
        private float m_CurrentTime = 0f;
        private Vector3 m_CurrentRotation;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            m_TransformTime = Random.Range(m_MinTranformTime, m_MaxTranformTime);
            for (int i = 0; i < m_WeaponArray2.Length; i++)
            {
                if (m_WeaponArray2[i] is ShootBulletWeapon shootBulletWeapon)
                {
                    shootBulletWeapon.OnBulletShoot = (bullet) =>
                    {
                        bullet.GetReady(m_Attack);
                    };
                }
            }
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        protected override void Update()
        {
            if (m_IsMoving)
                Moveable.OnMove();
            CheckBehaviour();
            if (m_IsFiring)
            {
                if (Behaviour.TailWeapon == m_CurrentBehaviour)
                {
                    for (int i = 0; i < m_WeaponArray.Length; i++)
                    {
                        m_WeaponArray[i].RunWeapon();
                    }
                }
                else
                {
                    for (int i = 0; i < m_WeaponArray2.Length; i++)
                    {
                        m_WeaponArray2[i].RunWeapon();
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckBehaviour()
        {
            m_CurrentTime += Time.deltaTime;
            if (Behaviour.Transform == m_CurrentBehaviour)
            {
                Transformation();
            }
            else
            {
                if (m_CurrentTime > m_TransformTime)
                {
                    SwitchBehaviour();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void SwitchBehaviour()
        {
            m_CurrentTime = 0f;
            if (Behaviour.TailWeapon == m_CurrentBehaviour)
            {
                m_TargetBehaviour = Behaviour.WingWeapon;
            }
            else
            {
                m_TargetBehaviour = Behaviour.TailWeapon;
            }
            m_CurrentBehaviour = Behaviour.Transform;
            m_IsFiring = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Transformation()
        {
            if (m_CurrentTime > m_TransformDuration)
            {
                m_IsFiring = true;
                m_CurrentBehaviour = m_TargetBehaviour;

                for (int i = 0; i < m_ColliderArray1.Length; i++)
                {
                    m_ColliderArray1[i].enabled = Behaviour.TailWeapon == m_CurrentBehaviour;
                }
                for (int i = 0; i < m_ColliderArray2.Length; i++)
                {
                    m_ColliderArray2[i].enabled = Behaviour.WingWeapon == m_CurrentBehaviour;
                }
                gameObject.SetActive(true);
                m_CurrentTime = 0f;
            }
            else
            {
                m_CurrentRotation = m_PivotTransform.rotation.eulerAngles;
                if (Behaviour.WingWeapon == m_TargetBehaviour)
                {
                    m_CurrentRotation.x = Mathf.Lerp(0, 90, m_CurrentTime / m_TransformDuration);
                }
                else
                {
                    m_CurrentRotation.x = Mathf.Lerp(90, 0, m_CurrentTime / m_TransformDuration);
                }
                m_PivotTransform.rotation = Quaternion.Euler(m_CurrentRotation);
            }
        }

    }
}
