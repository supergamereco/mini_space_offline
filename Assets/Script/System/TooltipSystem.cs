using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using UnityEngine;

/* TODO
* Will be removed after version 27xx.00
*/
public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    [SerializeField] public Tooltips tooltip;

    public void Awake()
    {
        current = this;
    }

    public static void Show(string localKey)
    {
        TooltipMasterData master = DatabaseSystem.I.GetMetadata<TooltipMaster>().tooltips.Find(
                a => a.key.Equals(localKey));
        current.tooltip.SetText(master.description, master.title);
        current.tooltip.transform.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.transform.SetActive(false);
    }
}

