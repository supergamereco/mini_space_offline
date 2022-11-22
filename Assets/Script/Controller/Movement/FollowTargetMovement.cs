using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class FollowTargetMovement : MonoBehaviour, IMoveable, ISetTargetAble
    {
        private const float BACK_TO_DEFAULT_ROTATION_SPEED = 3;

        [SerializeField] protected bool m_IsMagnetItem = default;
        [Tooltip("Is velocity will depend on object local axis?")]
        [SerializeField] protected bool m_IsLocalMovement = default;
        [SerializeField] protected Vector2 m_Velocity = default;
        [Tooltip("Will object rotation back to default rotation after follow target?")]
        [SerializeField] protected bool m_IsBackToDefaultRotation = default;
        [SerializeField] protected float m_FollowSpeed = default;
        [SerializeField] protected float m_DetectRange = default;
        [SerializeField] protected float m_RotationSpeed = 1f;
        [Tooltip("How long missile can rotate toward target")]
        [SerializeField] protected float m_FollowFuel = default;

        private float m_RotateFuelUsed;
        private float m_MagneticTimeLeft = 6f;

        protected Transform m_TargetTransform;
        protected Vector3 m_Position;
        protected Quaternion m_DefaultRotation;
        protected Rigidbody2D m_Rigidbody2D;

        /// <summary>
        /// Called on first frame
        /// </summary>
        private void Start()
        {
            TryGetComponent<Rigidbody2D>(out m_Rigidbody2D);
            m_DefaultRotation = transform.rotation;
        }
        /// <summary>
        /// Move transform forward by default. If detect player in range,
        /// will try to get close player until undetected player or destroyed.
        /// </summary>
        /// <param name="transform"></param>
        public virtual void OnMove()
        {
            m_Position = transform.position;
            if (IsDetectTarget() && m_RotateFuelUsed < m_FollowFuel)
            {
                m_RotateFuelUsed += Time.deltaTime;
                FollowTarget(transform, m_TargetTransform);
            }
            else
            {
                if (m_IsLocalMovement)
                {
                    transform.position += transform.right * m_Velocity.x * Time.deltaTime;
                    transform.position += transform.up * m_Velocity.y * Time.deltaTime;
                }
                else
                {
                    transform.position += (Vector3)m_Velocity * Time.deltaTime;
                }

                if (m_IsBackToDefaultRotation)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, m_DefaultRotation, Time.deltaTime * BACK_TO_DEFAULT_ROTATION_SPEED);
                }
                m_Rigidbody2D.angularVelocity = 0;
            }

            if (m_IsMagnetItem)
            {
                m_MagneticTimeLeft -= Time.deltaTime;
                if (m_MagneticTimeLeft <= 0)
                {
                    m_DetectRange = 0f;
                }
            }
        }
        /// <summary>
        /// Set target transform to follow
        /// </summary>
        /// <param name="transform"></param>
        public void SetTarget(Transform targetObject)
        {
            m_TargetTransform = targetObject.transform;
        }
        /// <summary>
        /// This object get close to position or not.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool IsGetClose(Vector3 targetPosition) =>
            Vector2.Distance(targetPosition, m_Position) < m_DetectRange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="followerTransform"></param>
        /// <param name="targetTransform"></param>
        protected virtual void FollowTarget(Transform followerTransform, Transform targetTransform)
        {
            followerTransform.position = Vector3.MoveTowards(followerTransform.position, targetTransform.position, m_FollowSpeed * Time.deltaTime);
            m_Rigidbody2D.RotateToTarget(targetTransform.position, -followerTransform.right, m_RotationSpeed * Time.deltaTime);
        }
        /// <summary>
        /// Detects the set target?
        /// </summary>
        /// <returns></returns>
        private bool IsDetectTarget()
        {
            if (null != m_TargetTransform)
            {
                Vector3 followingPosition = m_TargetTransform.position;
                return IsGetClose(followingPosition);
            }
            return false;
        }
        /// <summary>
        /// Magnet Item
        /// </summary>
        /// <returns></returns>
        public void OnMagnetic()
        {
            m_MagneticTimeLeft = 6.0f;
            m_DetectRange = 200f;
            m_FollowSpeed = 25f;
        }
        /// <summary>
        /// Magnet Item
        /// </summary>
        /// <returns></returns>
        public void OnClearItem()
        {
            m_MagneticTimeLeft = 2.0f;
            m_DetectRange = 700f;
            m_FollowSpeed = 2400f;
        }

        private void OnEnable()
        {
            m_RotateFuelUsed = 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_DetectRange);
        }
#endif
    }
}
