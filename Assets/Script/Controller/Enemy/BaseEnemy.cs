using System;
using NFT1Forge.OSY.Controller.Bullet;
using NFT1Forge.OSY.Controller.Gameplay;
using NFT1Forge.OSY.Controller.Interface;
using NFT1Forge.OSY.Controller.Weapon;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class BaseEnemy : BaseObjectController
    {
        [SerializeField] protected bool m_IsImmuneToDamage = default;
        [SerializeField] protected BaseWeapon[] m_WeaponArray = default;
        [SerializeField] private string m_ExplosionEffectName = default;
        [SerializeField] private Transform m_ExplosionTransform = default;
        [SerializeField] private bool m_IsByPassOnExitStage = false;
        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxShoot;

        public bool CanDeadByCollision = true;


        public IMoveable Moveable { get; private set; }
        public bool IsDestroy => Health <= 0;
        [HideInInspector] public float Health;

        protected float m_Attack;
        protected bool m_IsMoving = false;
        protected bool m_IsFiring = false;

        private Action<uint, Vector3> m_KillCallback;
        private ObjectType m_ObjectType;
        private uint m_Id;
        private ushort m_Score;
        private AudioSource m_Sfx;

        #region getter
        public ushort BaseScore => m_Score;
        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            Moveable = GetComponent<IMoveable>();
            m_Sfx = GetComponent<AudioSource>();
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        protected virtual void Update()
        {
            if (m_IsMoving)
                Moveable.OnMove();
            if (m_IsFiring)
            {
                for (int i = 0; i < m_WeaponArray.Length; i++)
                {
                    m_WeaponArray[i].RunWeapon();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attack"></param>
        /// <param name="health"></param>
        /// <param name="score"></param>
        /// <param name="objectType"></param>
        /// <param name="killCallback"></param>
        public virtual void GetReady(uint id, float attack, float health, ushort score, ObjectType objectType, Action<uint, Vector3> killCallback)
        {
            m_Id = id;
            m_ObjectType = objectType;
            m_KillCallback = killCallback;
            m_Attack = attack;
            Health = health;
            m_Score = score;
            gameObject.SetActive(true);

            if (GameplayController.I.PlayerTransform == null) return;

            if (Moveable is ISetTargetAble followTarget)
            {
                followTarget.SetTarget(GameplayController.I.PlayerTransform);
            }
            for (int i = 0; i < m_WeaponArray.Length; i++)
            {
                if (m_WeaponArray[i] is ISetTargetAble weaponTarget)
                {
                    weaponTarget.SetTarget(GameplayController.I.PlayerTransform);
                }
                if (m_WeaponArray[i] is ShootBulletWeapon shootBulletWeapon)
                {
                    shootBulletWeapon.OnBulletShoot = (bullet) =>
                    {
                        if (bullet.Moveable is IAimable aimBullet)
                        {
                            aimBullet.SetDirection((GameplayController.I.PlayerTransform.position - bullet.transform.position).normalized);
                        }
                        else if (bullet.Moveable is ISetTargetAble targetBullet)
                        {
                            targetBullet.SetTarget(GameplayController.I.PlayerTransform);
                        }
                        bullet.GetReady(m_Attack);
                    };
                }
            }
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
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void StateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                case GameState.WaitForBoss:
                case GameState.Boss:
                    m_IsMoving = true;
                    m_IsFiring = true;
                    break;

                case GameState.BossWarning:
                case GameState.BossKilled:
                case GameState.NextLevel:
                case GameState.GameOver:
                    m_IsMoving = true;
                    m_IsFiring = false;
                    break;

                default:
                    m_IsMoving = false;
                    m_IsFiring = false;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="isDead"></param>
        public virtual bool TakeDamage(float damage,bool isForce = false)
        {
            if (m_IsImmuneToDamage && !isForce) return false;

            Health -= damage;
            bool isDead = Health <= 0f;
            if (isDead)
            {
                DeadAction(true);
            }
            return isDead;
        }
        /// <summary>
        /// Dead without taking bullet
        /// </summary>
        public void ForceDead(bool isDeadByCollision)
        {
            DeadAction(isDeadByCollision);
        }
        /// <summary>
        /// Do the dead action
        /// </summary>
        protected void DeadAction(bool isDeadByCollision)
        {
            IsActive = true;
            if (isDeadByCollision)
            {
                m_KillCallback?.Invoke(m_Id, transform.position);
            }
            ToBeDone();
            ObjectPoolManager.I.ReturnToPool(this, m_ObjectType);
            CreateExplosionEffect();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateExplosionEffect()
        {
            if (!string.IsNullOrEmpty(m_ExplosionEffectName) && null != m_ExplosionTransform)
            {
                Transform spawnedEffect = ObjectPoolManager.I.GetObject(ObjectType.VFX, m_ExplosionEffectName, null);
                spawnedEffect.transform.position = m_ExplosionTransform.position;
                spawnedEffect.transform.rotation = m_ExplosionTransform.rotation;
            }
        }
        /// <summary>
        /// Game object going out of bound
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (m_IsByPassOnExitStage) return;

            if (other.CompareTag("Stage"))
            {
                ToBeDone();
                GameplayController.I.EnemyKilled(m_Id, false);
                ObjectPoolManager.I.ReturnToPool(this, m_ObjectType);
            }
        }
        /// <summary>
        /// Collider trigger by others collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsActive)
                return;

            if (other.CompareTag("PlayerBullet"))
            {
                BaseBullet bullet = other.GetComponent<BaseBullet>();
                if (bullet.IsActive)
                {
                    bool isDead = TakeDamage(bullet.Damage);
                    bullet.ToBeDone();
                    ObjectPoolManager.I.ReturnToPool(bullet, ObjectType.Bullet);
                    bullet.CreateHitParticle(isDead ? null : transform);
                }
            }
        }

    }
}
