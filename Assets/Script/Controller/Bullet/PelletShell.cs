using System.Collections;
using NFT1Forge.OSY.Controller.Gameplay;
using NFT1Forge.OSY.Controller.Movement;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;
using Serializable = System.SerializableAttribute;

namespace NFT1Forge.OSY.Controller.Bullet
{
    public class PelletShell : BaseBullet
    {
        [Serializable]
        public class Pellet
        {
            public float PelletOutTime;
            public BaseBullet[] Bullets;
        }

        [SerializeField] private GameObject m_PelletVisualize = default;
        [SerializeField] private string m_BulletName = default;
        [SerializeField] private Pellet[] m_PelletInShellArray = default;

        private bool m_IsPelletReleased;
        private float m_PelletOutCountdown;
        private int m_CurrentPelletIndex;
        private Pellet m_CurrentPellet;

        public void ResetPellet()
        {
            m_IsPelletReleased = false;
            m_CurrentPelletIndex = 0;
        }

        protected override void Update()
        {
            if (!m_IsPelletReleased)
            {
                base.Update();

                if (10f > Camera.main.WorldToScreenPoint(transform.position).x && IsActive)
                {
                    StartCoroutine(ReleasePellets());
                    m_IsPelletReleased = true;
                }
            }
        }

        private void OnEnable()
        {
            m_PelletVisualize.SetActive(true);
            ResetPellet();
        }

        private IEnumerator ReleasePellets()
        {
            m_PelletVisualize.SetActive(false);
            CreateHitParticle(null);
            while (m_PelletInShellArray.Length > m_CurrentPelletIndex)
            {
                m_CurrentPellet = m_PelletInShellArray[m_CurrentPelletIndex];

                m_PelletOutCountdown += Time.deltaTime;
                if (m_PelletOutCountdown > m_CurrentPellet.PelletOutTime)
                {
                    BaseBullet[] bulletArray = m_CurrentPellet.Bullets;
                    for (int i = 0; i < bulletArray.Length; i++)
                    {
                        BaseBullet bullet = ObjectPoolManager.I.GetObject<BaseBullet>(ObjectType.Bullet, $"Bullet/{m_BulletName}");
                        bullet.GetReady(Damage);
                        bullet.transform.position = bulletArray[i].transform.position;
                        bullet.transform.rotation = bulletArray[i].transform.rotation;
                        if (bullet.Moveable is ForwardMovement forwardMovement &&
                            bulletArray[i].Moveable is ForwardMovement forwardMovementInArray)
                        {
                            forwardMovement.SetSpeed(forwardMovementInArray.MoveSpeed);
                        }
                        GameplayController.I.AddBullet(GameplayController.I.CurrentBulletId, bullet);
                        GameplayController.I.CurrentBulletId++;
                    }
                    m_PelletOutCountdown = 0;
                    m_CurrentPelletIndex++;
                }
                yield return null;
            }
            Done();
            ObjectPoolManager.I.ReturnToPool(this, ObjectType.Bullet);
        }
    }
}
