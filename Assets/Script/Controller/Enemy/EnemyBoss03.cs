using UnityEngine;

namespace NFT1Forge.OSY.Controller.Enemy
{
    public class EnemyBoss03 : BossEnemy
    {
        [SerializeField] private Vector3 m_StartRotation = default;

        /// <summary>
        /// Called on first frame
        /// </summary>
        private void Start()
        {
            transform.rotation = Quaternion.Euler(m_StartRotation);
        }
    }
}