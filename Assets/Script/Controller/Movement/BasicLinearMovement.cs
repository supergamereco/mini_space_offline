using UnityEngine;
using NFT1Forge.OSY.Controller.Interface;

namespace NFT1Forge.OSY.Controller.Movement
{
    public class BasicLinearMovement : MonoBehaviour, IMoveable, IAimable
    {
        [SerializeField] private float m_MoveSpeed = 1f;

        private Vector3 m_Movement = default;

        /// <summary>
        /// Move transform and have speed based on vector.
        /// </summary>
        /// <param name="transform"></param>
        public void OnMove()
        {
            transform.position += m_Movement * Time.deltaTime * m_MoveSpeed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void SetDirection(Vector3 direction)
        {
            m_Movement = direction;
        }
    }
}
