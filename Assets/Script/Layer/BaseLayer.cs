using UnityEngine;

namespace NFT1Forge.OSY.Layer
{
    public class BaseLayer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void ShowLayer()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void HideLayer()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Delete()
        {
            Destroy(gameObject);
        }
    }
}
