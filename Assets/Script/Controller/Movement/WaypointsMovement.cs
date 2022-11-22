using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Movement
{
    public class WaypointsMovement : MonoBehaviour, IMoveable
    {
        private const float MINIMUN_DISTANCE = 0.1f;

        [SerializeField] private float m_MoveSpeed = default;
        [SerializeField] private Vector2[] m_WaypointArray = default;

        private int m_CurrentWaypointIndex;
        private Vector3 m_Position;
        private Vector2 m_CurrentWaypoint;

        public void OnMove()
        {
            if (IsReachCurrentWaypoint())
            {
                m_CurrentWaypointIndex++;
                if (TryGetWaypoint(m_CurrentWaypointIndex, out Vector2 newWaypoint))
                {
                    SetWaypoint(newWaypoint);
                }
                else
                {
                    ResetWaypoint();
                }
            }
            MoveTransformTowardCurrentWaypoint(transform);
        }

        public void ResetWaypoint()
        {
            m_CurrentWaypointIndex = 0;
            if (TryGetWaypoint(0, out Vector2 newWaypoint))
            {
                SetWaypoint(newWaypoint);
            }
        }

        public bool TryGetWaypoint(int index, out Vector2 waypoint)
        {
            bool canGet = IsIndexInOfBound(index);
            waypoint = Vector2.zero;
            if (canGet)
            {
                waypoint = GetWaypoint(index);
            }
            return canGet;
        }

        public Vector2 GetWaypoint(int index) => m_WaypointArray[index];

        public int GetWaypointLength() => m_WaypointArray.Length;

        public void SetWaypoint(Vector2 newWaypoint)
        {
            m_CurrentWaypoint = newWaypoint;
        }

        private void Awake()
        {
            ResetWaypoint();
        }

        private bool IsIndexInOfBound(int index) => index < GetWaypointLength() && 0 <= index;

        private bool IsReachCurrentWaypoint() => Vector2.Distance(m_Position, m_CurrentWaypoint) < MINIMUN_DISTANCE;

        private void MoveTransformTowardCurrentWaypoint(Transform transform)
        {
            m_Position = transform.position;
            m_Position = Vector2.MoveTowards(m_Position, m_CurrentWaypoint, m_MoveSpeed * Time.deltaTime);
            transform.position = m_Position;
        }
    }
}
