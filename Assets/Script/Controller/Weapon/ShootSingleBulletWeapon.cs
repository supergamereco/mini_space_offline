using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.Controller.Bullet;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.Weapon
{
    public class ShootSingleBulletWeapon : ShootBulletWeapon
    {
        [SerializeField] private BaseBullet bullet = default;

        public override void Shoot()
        {
            ShootBullet(bullet.name);
        }
    }
}
