using UnityEngine;

namespace NFT1Forge.OSY.Controller
{
    public class BaseObjectController : MonoBehaviour
    {
        [HideInInspector] public bool IsActive { get; protected set; } = false;

        protected bool m_IsReady = false;

        /// <summary>
        /// Set this object to be ready for action
        /// </summary>
        protected void Ready()
        {
            m_IsReady = true;
            IsActive = true;
            gameObject.SetActive(true);
        }
        /// <summary>
        /// This object is done for
        /// </summary>
        protected void Done()
        {
            m_IsReady = false;
            IsActive = false;
        }
    }
}
