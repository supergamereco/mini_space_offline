using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Weapon
{
    public class TrackTargetWeapon : ShootSingleBulletWeapon, ISetTargetAble
    {
        [SerializeField] private float m_RotateSpeed = default;

        private Transform m_TargetTransform;
        private Vector3 m_NewDirection;
        private Vector3 m_TargetDirection;
        private float m_SpeedDelta;
        private float m_Angle;
        private Quaternion m_Quaternion;

        public override void RunWeapon()
        {
            base.RunWeapon();
            if (null != m_TargetTransform)
            {
                m_SpeedDelta = m_RotateSpeed * Time.deltaTime;
                m_WeaponTransform.SmoothLookAt2D(m_TargetTransform.position, m_SpeedDelta);
            }
        }

        public void SetTarget(Transform targetObject)
        {
            m_TargetTransform = targetObject.transform;
        }
    }
}
