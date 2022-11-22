using System;
using System.Collections.Generic;
using NFT1Forge.OSY.Controller.Bullet;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class GameplaySpaceship : BaseObjectController
    {
        [SerializeField] private float m_MoveSpeed = 5f;
        [SerializeField] private Transform m_SpaceshipModelHolder;
        [SerializeField] private Transform m_NormalWeaponTransform;
        [SerializeField] private GameplayLaser m_Laser;
        [SerializeField] private GameplayShield m_Shield;
        [SerializeField] private List<Transform> m_BossPart;

        public event Action<float> OnSpecialWeaponTimeChanged;
        public event Action OnSpecialFired;

        public bool IsRapidFire = false;
        public bool IsMagnet = false;
        public float RapidFireDuration;
        public float MagnetDuration;

        private StarterAssetsInputs m_StarterInput;
        private Transform m_SpaceshipModel;
        private float m_NormalWeaponInterval;
        private float m_RapidNormalWeaponInterval;
        private float m_CurrentNormalWeaponInterval;
        private PlayerInputAction m_PlayerInputAction;
        private Vector3 m_CurrentPos;
        private float m_CurrentNormalWeaponTime = 0f;
        private float m_CurrentSpecialWeaponTime = 0f;
        private float m_SpecialWeaponChargeTime;
        private float m_SpecialWeaponChargeSpeed = 1f;
        private bool m_IsSpecialWeaponReady = true;
        private bool m_IsMovable = false;
        private bool m_IsFiring = false;
        private float m_RapidFireTime;
        private float m_MagnetTime;
        private float m_NormalWeaponDamage = 0f;
        private bool m_IsMoveOut = false;
        private bool m_HasSpecialWeapon = false;
        private SpecialWeaponDataModel m_SpecialWeapon;
        private GameplayMissile m_Missile;

        private AudioSource m_Sfx;
        private Vector3 m_limitPotision;

        /// <summary>
        /// Setup component before this object can be used
        /// </summary>
        /// <param name="controller"></param>
        public void Setup(float attack, float specialWeaponChargeTime, float specialWeaponChargeSpeed, Action<StarterAssetsInputs> callback)
        {
            float cameraPosition = 100f;
            Vector3 limitPotision = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Screen.width,
                    Screen.height,
                    cameraPosition)
                );
            m_limitPotision = limitPotision;
            m_Sfx = GetComponent<AudioSource>();
            SpaceshipMasterData spaceshipMaster = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master.Find(
                a => a.name.Equals(InventorySystem.I.GetActiveSpaceship().name)
                );
            LoadSpaceshipModel(spaceshipMaster.asset_path);
            ShowBossPart();
            m_PlayerInputAction = new PlayerInputAction();
            m_PlayerInputAction.Player.Enable();
            m_PlayerInputAction.Player.SpecialWeapon.started += FireSpecialWeapon;

            m_NormalWeaponInterval = GameplayDataManager.I.NormalWeaponInterval;
            m_RapidNormalWeaponInterval = GameplayDataManager.I.RapidNormalWeaponInterval;
            m_CurrentNormalWeaponInterval = m_NormalWeaponInterval;
            RapidFireDuration = GameplayDataManager.I.ItemRapidFireValue;
            MagnetDuration = GameplayDataManager.I.ItemMagnetValue;

            m_SpecialWeapon = InventorySystem.I.GetActiveWeapon();
            if (null != m_SpecialWeapon)
            {
                m_HasSpecialWeapon = true;
                m_IsSpecialWeaponReady = true;
                if (m_SpecialWeapon.name == "Meson Blaster" || m_SpecialWeapon.name == "Anti-Matter Blaster")
                    m_Laser.ResetLaser();
                if (m_SpecialWeapon.name == "Dynamic Resistor" || m_SpecialWeapon.name == "Bardeen Reactor")
                    m_Shield.ResetShield();
                m_SpecialWeaponChargeTime = specialWeaponChargeTime;
                m_SpecialWeaponChargeSpeed = specialWeaponChargeSpeed;
                m_CurrentSpecialWeaponTime = 0;
                OnSpecialWeaponTimeChanged?.Invoke(m_SpecialWeaponChargeTime);
            }
            else
            {
                m_IsSpecialWeaponReady = false;
            }
            m_NormalWeaponDamage = attack;
            GameplayController.I.OnGameStateChanged += StateChanged;
            m_StarterInput = GetComponent<StarterAssetsInputs>();
            callback?.Invoke(m_StarterInput);
            Ready();
        }
        /// <summary>
        /// Reset to start position
        /// </summary>
        public void Reset()
        {
            transform.position = Vector3.zero;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void StateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Loading:
                case GameState.GetReady:
                    m_IsMovable = false;
                    m_IsFiring = false;
                    m_IsMoveOut = false;
                    break;

                case GameState.Playing:
                case GameState.WaitForBoss:
                case GameState.BossWarning:
                case GameState.Boss:
                    m_IsMovable = true;
                    m_IsFiring = true;
                    m_IsMoveOut = false;
                    break;

                case GameState.BossKilled:
                    m_IsMovable = true;
                    m_IsFiring = false;
                    m_IsMovable = false;
                    break;

                case GameState.NextLevel:
                    m_IsMovable = false;
                    m_IsFiring = false;
                    m_IsMoveOut = true;
                    break;

                default:
                    m_IsMovable = false;
                    m_IsFiring = false;
                    m_IsMoveOut = false;
                    break;
            }
        }
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (m_IsMovable)
                Move();
            if (m_IsFiring)
            {
                m_CurrentNormalWeaponTime += Time.deltaTime;
                if (m_CurrentNormalWeaponTime > m_CurrentNormalWeaponInterval)
                {
                    m_CurrentNormalWeaponTime = 0f;
                    FireNormalWeapon();
                }
                if (IsRapidFire)
                {
                    m_RapidFireTime += Time.deltaTime;
                    if (m_RapidFireTime > RapidFireDuration)
                    {
                        IsRapidFire = false;
                        m_CurrentNormalWeaponInterval = m_NormalWeaponInterval;
                    }
                }
                if (IsMagnet)
                {
                    m_MagnetTime += Time.deltaTime;
                    if (m_MagnetTime > MagnetDuration)
                    {
                        IsMagnet = false;
                    }
                }
                ChargeSpecialWeapon();
                CheckJoyControlSpecialWeapon();
            }
            if (m_IsMoveOut)
            {
                MoveFastForward();
            }
        }
        /// <summary>
        /// Read input and move spaceship
        /// </summary>
        private void Move()
        {
            //WASD
            Vector2 inputVector = m_PlayerInputAction.Player.Movement.ReadValue<Vector2>();
            if (inputVector.Equals(Vector2.zero))
            {
                //Arrow
                inputVector = m_PlayerInputAction.Player.MovementArrow.ReadValue<Vector2>();
            }
            if (inputVector.Equals(Vector2.zero))
            {
                //Joystick
                if (m_StarterInput.move != Vector2.zero)
                {
                    inputVector = m_StarterInput.move.normalized;
                }
            }
            m_CurrentPos = transform.position;
            m_CurrentPos.x = Mathf.Clamp(m_CurrentPos.x + (inputVector.x * Time.deltaTime * m_MoveSpeed), -m_limitPotision.x + 10, m_limitPotision.x - 10);
            m_CurrentPos.y = Mathf.Clamp(m_CurrentPos.y + (inputVector.y * Time.deltaTime * m_MoveSpeed), -m_limitPotision.y + 5, m_limitPotision.y - 2);

            transform.position = m_CurrentPos;
        }
        /// <summary>
        /// Move forward very fast 
        /// </summary>
        private void MoveFastForward()
        {
            m_CurrentPos = transform.position;
            m_CurrentPos.x += Time.deltaTime * m_MoveSpeed * 3f;
            transform.position = m_CurrentPos;
        }
        /// <summary>
        /// 
        /// </summary>
        private void FireNormalWeapon()
        {
            PlayerBullet1 bullet = ObjectPoolManager.I.GetObject<PlayerBullet1>(ObjectType.Bullet, "Bullet/PlayerBullet1");
            bullet.transform.position = m_NormalWeaponTransform.position;
            bullet.GetReady(m_NormalWeaponDamage);
            if (null != m_Sfx)
                m_Sfx.Play();
        }
        /// <summary>
        /// 
        /// </summary>
        public void BoostFireRate()
        {
            m_CurrentNormalWeaponInterval = m_RapidNormalWeaponInterval;
            IsRapidFire = true;
            m_RapidFireTime = 0f;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Magnetic()
        {
            IsMagnet = true;
            m_MagnetTime = 0f;
        }
        /// <summary>
        /// Increase time for special weapon. If time is more than weapon charge time then weapon is ready
        /// </summary>
        private void ChargeSpecialWeapon()
        {
            if (m_IsSpecialWeaponReady || !m_HasSpecialWeapon)
                return;
            m_CurrentSpecialWeaponTime += Time.deltaTime * m_SpecialWeaponChargeSpeed;
            if (m_CurrentSpecialWeaponTime > m_SpecialWeaponChargeTime)
            {
                m_IsSpecialWeaponReady = true;
                m_CurrentSpecialWeaponTime = 0;
            }
            else
            {
                OnSpecialWeaponTimeChanged?.Invoke(m_CurrentSpecialWeaponTime);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckJoyControlSpecialWeapon()
        {
            if (!m_IsSpecialWeaponReady)
                return;

            if (m_StarterInput.fire)
            {
                DoFireSpecialWeapon();
            }
        }
        /// <summary>
        /// Firing special weapon
        /// </summary>
        /// <param name="context">Callback context from input system</param>
        private void FireSpecialWeapon(InputAction.CallbackContext context)
        {
            if (m_IsFiring && m_IsSpecialWeaponReady)
            {
                DoFireSpecialWeapon();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DoFireSpecialWeapon()
        {
            OnSpecialFired?.Invoke();
            if (m_SpecialWeapon.name == "Meson Blaster" || m_SpecialWeapon.name == "Anti-Matter Blaster")
            {
                m_Laser.OpenLaser();
            }
            else if (m_SpecialWeapon.name == "Plasma Rocket" || m_SpecialWeapon.name == "Hell's Scream")
            {
                if (null == m_Missile)
                {
                    m_Missile = gameObject.AddComponent<GameplayMissile>();
                    m_Missile.SetData(m_SpecialWeapon, m_NormalWeaponTransform);
                }
                StartCoroutine(m_Missile.OpenFire());
            }
            else if (m_SpecialWeapon.name == "Dynamic Resistor" || m_SpecialWeapon.name == "Bardeen Reactor")
            {
                m_Shield.OpenShiled();
            }
            m_IsSpecialWeaponReady = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                BaseEnemy enemy = other.GetComponent<BaseEnemy>();
                if (enemy.CanDeadByCollision)
                    enemy.ForceDead(false);
                GameplayController.I.DeductEnergy(enemy.Health);
                enemy.CreateExplosionEffect();
            }
            else if (other.CompareTag("EnemyBullet"))
            {
                BaseBullet bullet = other.GetComponent<BaseBullet>();
                ObjectPoolManager.I.ReturnToPool(bullet, ObjectType.Bullet);
                GameplayController.I.DeductEnergy(bullet.Damage);
                bullet.CreateHitParticle(transform);
            }
        }
        /// <summary>
        /// Load spaceship 3D model
        /// </summary>
        private void LoadSpaceshipModel(string prefabName)
        {
            m_SpaceshipModel = ObjectPoolManager.I.GetObject(ObjectType.PlayerSpaceShip, prefabName, m_SpaceshipModelHolder);
            m_SpaceshipModel.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        /// <summary>
        /// Show boss part
        /// </summary>
        private void ShowBossPart()
        {
            SpaceshipDataModel spaceship = InventorySystem.I.GetActiveSpaceship();
            int index = m_BossPart.FindIndex(a => a.name.Equals(spaceship.bosspart));
            for (int i = 0; i < m_BossPart.Count; i++)
            {
                m_BossPart[i].SetActive(i == index);
            }
        }
        /// <summary>
        /// Return spaceship 3D model to pool
        /// </summary>
        public void DeActivate()
        {
            ObjectPoolManager.I.ReturnToPool(m_SpaceshipModel, ObjectType.PlayerSpaceShip);
            m_PlayerInputAction.Player.SpecialWeapon.started -= FireSpecialWeapon;
        }
    }
}
