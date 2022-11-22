using System.Collections;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Gameplay
{
    public class GameplayMissile : MonoBehaviour
    {
        private SpecialWeaponDataModel m_SpecialWeapon;
        private Transform m_SpawnPoint;

        /// <summary>
        /// Set basic data needed for operation
        /// </summary>
        /// <param name="data"></param>
        /// <param name="spawnPoint"></param>
        public void SetData(SpecialWeaponDataModel data, Transform spawnPoint)
        {
            m_SpecialWeapon = data;
            m_SpawnPoint = spawnPoint;
        }
        /// <summary>
        /// FIRE
        /// </summary>
        /// <returns></returns>
        public IEnumerator OpenFire()
        {
            for (int i = 0; i < m_SpecialWeapon.firingspeed; i++)
            {
                SpawnMissile();
                yield return new WaitForSeconds(0.25f);
            }
        }
        /// <summary>
        /// Spawn a missile object
        /// </summary>
        private void SpawnMissile()
        {
            PlayerHomingMissile bullet = ObjectPoolManager.I.GetObject<PlayerHomingMissile>(ObjectType.Bullet, "Bullet/PlayerHomingMissile");
            Vector3 randomPos = new Vector3(Random.Range(0f, 1f), Random.Range(2f, 8f), 0);
            bullet.transform.position = m_SpawnPoint.position + randomPos;
            bullet.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            bullet.GetReady(m_SpecialWeapon.damage);
        }
    }
}
