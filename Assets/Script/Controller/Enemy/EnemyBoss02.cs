using UnityEngine;

namespace NFT1Forge.OSY.Controller.Enemy
{
    public class EnemyBoss02 : BossEnemy
    {
        [SerializeField] private float m_ShootRotateSpeed = default;
        [SerializeField] private float m_MoveDuration = default;
        [SerializeField] private float m_FiringDuration = default;

        private float m_MoveTimer;
        private float m_FiringTimer;

        protected override void Update()
        {
            if (m_MoveTimer < m_MoveDuration && m_IsMoving)
            {
                Moveable.OnMove();
                m_MoveTimer += Time.deltaTime;
            }
            else if (m_IsFiring)
            {
                if (m_FiringTimer < m_FiringDuration)
                {
                    for (int i = 0; i < m_WeaponArray.Length; i++)
                    {
                        m_WeaponArray[i].RunWeapon();
                    }
                    m_FiringTimer += Time.deltaTime;
                    transform.Rotate(m_ShootRotateSpeed * Time.deltaTime * Vector3.forward);
                }
                else
                {
                    m_MoveTimer = 0;
                    m_FiringTimer = 0;
                }
            }  
        }
    }
}
