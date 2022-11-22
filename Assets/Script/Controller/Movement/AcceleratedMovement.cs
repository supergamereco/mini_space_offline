using NFT1Forge.OSY.Controller.Interface;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Movement
{
    public class AcceleratedMovement : MonoBehaviour, IMoveable
    {   
        [SerializeField] private Vector2 m_MaxVelocity = default;
        [SerializeField] private AnimationCurve m_AccelerateCurvePattern = default;

        private float m_CurveDeltaTime;
        private Vector3 m_CurrentVelocity;

        public void OnMove()
        {
            m_CurveDeltaTime += Time.deltaTime;
            m_CurrentVelocity =  m_AccelerateCurvePattern.Evaluate(m_CurveDeltaTime) * m_MaxVelocity;
            transform.position += m_CurrentVelocity * Time.deltaTime;
        }
    }
}
