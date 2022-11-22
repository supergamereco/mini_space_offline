using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.Controller;
using NFT1Forge.OSY.Manager;
using UnityEngine;

namespace NFT1Forge.OSY.Controller.VFX
{
    public class ReturnVFXToPool : BaseObjectController
    {
        private ParticleSystem m_Particle;
        private float m_Timer;

        private void Awake()
        {
            m_Particle = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            m_Timer = m_Particle.main.duration;
        }

        private void Update()
        {
            m_Timer -= Time.deltaTime;
            if (0 > m_Timer)
            {
                ObjectPoolManager.I.ReturnToPool(this, DataModel.ObjectType.VFX);
            }
        }
    }
}
