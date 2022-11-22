using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.System;
using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class PilotAvatar : BaseObjectController
    {
        [SerializeField] private SkinnedMeshRenderer m_Renderer;
        [SerializeField] private MeshRenderer m_HairRenderer;
        //Temporary solution : ran out of time
        [SerializeField] private List<Texture> m_BodyTextureList;

        /// <summary>
        /// Change hair material texture
        /// </summary>
        /// <param name="colorIndex"></param>
        public void SetHeadColor(HairColor hairColor)
        {
            m_HairRenderer.material.mainTexture = m_BodyTextureList[(int)hairColor - 1];
        }
        /// <summary>
        /// Change body material texture
        /// </summary>
        /// <param name="costumeColor"></param>
        public void SetBodyColor(CostumeColor costumeColor)
        {
            m_Renderer.material.mainTexture = m_BodyTextureList[(int)costumeColor - 1];
        }
    }
}
