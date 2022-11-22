using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Movement
{
    /// <summary>
    /// Move forward depend on local x axis of transform.
    /// </summary>
    public class ForwardMovement : MonoBehaviour, IMoveable
    {
        public float MoveSpeed => m_MoveSpeed;

        [SerializeField] private bool m_IsInvertDirection = default;
        [SerializeField] private float m_MoveSpeed = default;

        private Vector3 m_Position;
        public virtual void OnMove()
        {
            m_Position = transform.position;
            m_Position += GetForwardDirection() * m_MoveSpeed * Time.deltaTime;
            m_Position.z = 0;
            transform.position = m_Position;
        }

        public void SetSpeed(float moveSpeed)
        {
            m_MoveSpeed = moveSpeed;
        }

        protected Vector3 GetForwardDirection() =>
            m_IsInvertDirection ? -transform.right : transform.right;
    }
}
