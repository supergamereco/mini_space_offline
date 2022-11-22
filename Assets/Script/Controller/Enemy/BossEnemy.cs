using System;
using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.DataModel;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class BossEnemy : BaseEnemy
    {
        public event Action<float> OnHpChanged;
        public event Action<float> OnSetHp;
        public event Action OnKilled;
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attack"></param>
        /// <param name="health"></param>
        /// <param name="score"></param>
        /// <param name="objectType"></param>
        /// <param name="killCallback"></param>
        public override void GetReady(uint id, float attack, float health, ushort score, ObjectType objectType, Action<uint, Vector3> killCallback)
        {
            OnSetHp?.Invoke(health);
            base.GetReady(id, attack, health, score, objectType, killCallback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="isDead"></param>
        public override bool TakeDamage(float damage,bool isForce = false)
        {
            if (m_IsImmuneToDamage) return false;

            Health -= damage;
            OnHpChanged?.Invoke(Health);
            bool isDead = Health <= 0f;
            if (isDead)
            {
                OnKilled?.Invoke();
                DeadAction(true);
            }
            return isDead;
        }

    }
}