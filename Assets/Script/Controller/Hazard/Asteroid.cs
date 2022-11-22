using UnityEngine;

namespace NFT1Forge.OSY.Controller.Enemy
{
    public class Asteroid : BaseEnemy
    {
        [SerializeField] private Transform m_RotateTransform = default;
        [SerializeField] private float m_RotateMinSpeed = default;
        [SerializeField] private float m_RotateMaxSpeed = default;
        public string PrefabName;

        private Vector3 m_RandomRotationVector;
        private Quaternion m_RandomRotation;
        private float m_RotateSpeed;

        protected override void Awake()
        {
            base.Awake();
            m_RotateTransform.rotation = Random.rotation;
            m_RandomRotation = Random.rotation;
            m_RotateSpeed = GetRandomRotateSpeed();
        }

        protected override void Update()
        {
            base.Update();
            m_RandomRotationVector = new Vector3(m_RandomRotation.x, m_RandomRotation.y, m_RandomRotation.z);
            m_RotateTransform.Rotate(m_RandomRotationVector * m_RotateSpeed * Time.deltaTime);
        }

        private float GetRandomRotateSpeed() => Random.Range(m_RotateMinSpeed, m_RotateMaxSpeed);
    }
}
