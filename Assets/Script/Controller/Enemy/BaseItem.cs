using System;
using NFT1Forge.OSY.Controller.Gameplay;
using NFT1Forge.OSY.Controller.Interface;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class BaseItem : BaseObjectController
    {
        [SerializeField] private ItemType m_ItemType;

        public IMoveable Moveable { get; private set; }

        private Action<ItemType> m_CatchCallback;
        private bool m_IsMoving = false;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            Moveable = GetComponent<IMoveable>();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (m_IsMoving)
                Moveable.OnMove();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="score"></param>
        /// <param name="catchCallback"></param>
        public void GetReady(Action<ItemType> catchCallback, Vector3 position)
        {
            m_CatchCallback = catchCallback;
            gameObject.SetActive(true);
            transform.position = position;

            if (GameplayController.I.PlayerTransform == null) return;

            if (Moveable is ISetTargetAble followTarget)
            {
                followTarget.SetTarget(GameplayController.I.PlayerTransform);
            }
            GameplayController.I.OnGameStateChanged += StateChanged;
            StateChanged(GameplayController.I.CurrentGameState);
            Ready();
        }
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
                case GameState.BossWarning:
                case GameState.Boss:
                case GameState.BossKilled:
                case GameState.NextLevel:
                    m_IsMoving = true;
                    break;

                default:
                    m_IsMoving = false;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Stage"))
            {
                ObjectPoolManager.I.ReturnToPool(this, ObjectType.Item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsActive)
                return;

            if (other.CompareTag("Player"))
            {
                IsActive = false;
                m_CatchCallback?.Invoke(m_ItemType);
                ObjectPoolManager.I.ReturnToPool(this, ObjectType.Item);
            }
        }
    }
}
