using System.Collections;
using UnityEngine;

namespace NFT1Forge.OSY.System
{
    public class BaseController : MonoBehaviour
    {
        /// <summary>
        /// Initialize current controller
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Initialize()
        {
            yield return null;
        }
        /// <summary>
        /// Shutdown current controller
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Shutdown()
        {
            yield return null;
        }
    }
}