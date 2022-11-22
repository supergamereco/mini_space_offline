using NFT1Forge.OSY.Controller.Gameplay;
using NFT1Forge.OSY.Controller.Interface;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Bullet
{
    public class BaseBullet : BaseObjectController
    {
        public IMoveable Moveable;

        [SerializeField] private string m_HitEffectName = default;
        [SerializeField] private Transform m_HitEffectSpawnTransform = default;

        public float Damage { get; private set; }

        private bool m_IsMoveable = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Moveable = GetComponent<IMoveable>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void StateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                case GameState.WaitForBoss:
                case GameState.BossWarning:
                case GameState.Boss:
                case GameState.BossKilled:
                case GameState.NextLevel:
                case GameState.GameOver:
                    m_IsMoveable = true;
                    break;

                default:
                    m_IsMoveable = false;
                    break;
            }
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        protected virtual void Update()
        {
            if (m_IsMoveable)
                Moveable.OnMove();
        }
        /// <summary>
        /// 
        /// </summary>
        public void GetReady(float damage)
        {
            Damage = damage;
            GameplayController.I.OnGameStateChanged += StateChanged;
            StateChanged(GameplayController.I.CurrentGameState);
            Ready();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ToBeDone()
        {
            GameplayController.I.OnGameStateChanged -= StateChanged;
            Done();
        }
        /// <summary>
        /// VFX
        /// </summary>
        /// <param name="hitTransform"></param>
        public void CreateHitParticle(Transform hitTransform)
        {
            Transform bullet = ObjectPoolManager.I.GetObject(ObjectType.VFX, m_HitEffectName, hitTransform);
            bullet.transform.position = m_HitEffectSpawnTransform.position;
            bullet.transform.rotation = m_HitEffectSpawnTransform.rotation;
        }
        /// <summary>
        /// Enable or disable game object
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        /// <summary>
        /// 
        /// </summary>
        public void ForceDead()
        {
            IsActive = true;
            ToBeDone();
            ObjectPoolManager.I.ReturnToPool(this, ObjectType.Bullet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Stage"))
            {
                ToBeDone();
                ObjectPoolManager.I.ReturnToPool(this, ObjectType.Bullet);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Ray(transform.position, transform.right * 5));
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
#endif
    }
}
