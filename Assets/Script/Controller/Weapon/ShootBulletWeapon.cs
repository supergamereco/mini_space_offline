using System;
using NFT1Forge.OSY.Controller.Bullet;
using NFT1Forge.OSY.Controller.Gameplay;
using NFT1Forge.OSY.Controller.Movement;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;

namespace NFT1Forge.OSY.Controller.Weapon
{
    public abstract class ShootBulletWeapon : BaseWeapon
    {
        public Action<BaseBullet> OnBulletShoot;

        public void ShootBullet(string bulletName)
        {
            BaseBullet bullet = ObjectPoolManager.I.GetObject<BaseBullet>(ObjectType.Bullet, $"Bullet/{bulletName}");
            bullet.transform.position = m_BulletSpawnPosition;
            bullet.transform.rotation = m_BulletSpawnRotation;
            if (bullet.Moveable is HomingMissileMovement)
            {
                ((HomingMissileMovement)bullet.Moveable).OnFuel();
            }
            OnBulletShoot?.Invoke(bullet);
            GameplayController.I.AddBullet(GameplayController.I.CurrentBulletId, bullet);
            GameplayController.I.CurrentBulletId++;
        }
    }
}
