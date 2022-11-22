using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Movement
{
    public class BouncingMovement : MonoBehaviour, IMoveable
    {
        [SerializeField] private float m_MoveSpeed;
        private Vector3 m_MoveDirection;
        private Vector3 m_Position;

        /// <summary>
        /// Called on first frame
        /// </summary>
        private void Start()
        {
            Vector2 direction2D = Random.insideUnitCircle.normalized;
            m_MoveDirection.x = 0f > direction2D.x ? direction2D.x : -direction2D.x;
            m_MoveDirection.y = direction2D.y;
            m_MoveDirection.z = 0f;
        }
        /// <summary>
        /// Move
        /// </summary>
        public void OnMove()
        {
            m_Position = transform.position;
            m_Position += m_MoveSpeed * Time.deltaTime * m_MoveDirection;
            transform.position = m_Position;
            CheckReflect();
        }
        /// <summary>
        /// Reflect movement when get too close to the edge of screen
        /// </summary>
        private void CheckReflect()
        {
            if ((0f > m_MoveDirection.y && 10f > Camera.main.WorldToScreenPoint(transform.position).y) ||
                (0f < m_MoveDirection.y && Screen.height - 10f < Camera.main.WorldToScreenPoint(transform.position).y))
                m_MoveDirection.y = -m_MoveDirection.y;
            if ((0f > m_MoveDirection.x && 10f > Camera.main.WorldToScreenPoint(transform.position).x) ||
                (0f < m_MoveDirection.x && Screen.width - 10f < Camera.main.WorldToScreenPoint(transform.position).x))
                m_MoveDirection.x = -m_MoveDirection.x;
        }
    }
}
