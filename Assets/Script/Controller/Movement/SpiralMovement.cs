using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Movement
{
    public class SpiralMovement : MonoBehaviour, IMoveable
    {
        [SerializeField] private Vector2 m_Velocity = default;
        [SerializeField] private float m_OffsetTimer = default;
        [SerializeField] private bool m_InvertXDirection = default;
        [SerializeField] private bool m_InvertYDirection = default;
        [Tooltip("The higher spiral speed will constrict width and height of spiral.")]
        [SerializeField] private float m_SpiralSpeed = 3;
        [SerializeField] private float m_Width = 15;
        [SerializeField] private float m_Height = 10;
        [SerializeField] private bool m_IsLimitMovement;
        [SerializeField] private float m_LimitValue;

        private float m_Cos => m_InvertXDirection ? -Mathf.Cos(m_Timer) : Mathf.Cos(m_Timer);
        private float m_Sin => m_InvertYDirection ? -Mathf.Sin(m_Timer) : Mathf.Sin(m_Timer);
        private float m_Timer;
        private Vector3 m_Position;

        public void OnMove()
        {
            m_Timer += Time.deltaTime * m_SpiralSpeed;
            m_Position = transform.position;

            m_Position += (Vector3)m_Velocity * Time.deltaTime;
            m_Position.x += m_Cos * m_Width * Time.deltaTime;
            m_Position.y += m_Sin * m_Height * Time.deltaTime;
            m_Position.z = 0;

            transform.position = m_Position;
            if (m_IsLimitMovement)
                LimitMovement();
        }

        private void LimitMovement()
        {
            float xRatio = Camera.main.WorldToScreenPoint(transform.position).x / Screen.width;
            if ((0 > m_Velocity.x && m_LimitValue > xRatio) ||
                (0 < m_Velocity.x && 0.9f < xRatio))
            {
                m_Velocity.x = -m_Velocity.x;
            }
        }

        public void SetVelocity(Vector2 velocity)
        {
            m_Velocity = velocity;
        }

        private void Start()
        {
            m_Timer += m_OffsetTimer;
        }
    }
}
