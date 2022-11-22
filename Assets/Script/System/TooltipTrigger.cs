using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.Layer;
using UnityEngine;
using UnityEngine.EventSystems;

/* TODO
* Will be removed after version 27xx.00
*/
namespace NFT1Forge.OSY.Controller
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipSystem.Show(gameObject.name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }
    }
}
