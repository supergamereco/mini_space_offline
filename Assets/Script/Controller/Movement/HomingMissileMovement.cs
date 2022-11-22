using System;
using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Movement
{
    public class HomingMissileMovement : MonoBehaviour, IMoveable, ISetTargetAble
    {
        [SerializeField] private float m_MoveSpeed = 1f;
        [SerializeField] private float m_RotateSpeed = 1f;
        [SerializeField] private float m_Fuel = 10f;
        [SerializeField] private bool m_IsUseFuel = false;

        public Action OnTargetLost;

        private Transform m_Target;
        private float m_CurrentFuel;

        /// <summary>
        /// Fill fuel for rotating
        /// </summary>
        public void OnFuel()
        {
            m_CurrentFuel = m_Fuel;
        }
        /// <summary>
        /// Making move
        /// </summary>
        /// <param name="transform"></param>
        public void OnMove()
        {
            if (null == m_Target || !m_Target.gameObject.activeInHierarchy)
            {
                OnTargetLost?.Invoke();
            }
            else
            {
                Rotate();
            }
            transform.position += m_MoveSpeed * Time.deltaTime * transform.forward;
        }
        /// <summary>
        /// Rotate toward target
        /// </summary>
        private void Rotate()
        {
            if (m_IsUseFuel && m_CurrentFuel < 0f)
                return;
            m_CurrentFuel -= Time.deltaTime;
            Vector3 targetDirection = m_Target.position - transform.position;
            float singleStep = m_RotateSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by singleStep
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            newDirection.z = 0f; //lock z axis
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        /// <summary>
        /// Set target to move to
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            m_Target = target;
        }
    }
}
