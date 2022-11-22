using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NFT1Forge.OSY.System
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationMap : MonoBehaviour
    {
        [SerializeField] private string m_Key;

        private void OnEnable()
        {
            TextMeshProUGUI textField = GetComponent<TextMeshProUGUI>();
            textField.font = LocalizationSystem.I.GetFont();
            if(!string.IsNullOrEmpty(m_Key))
            {
                string value = LocalizationSystem.I.GetLocalizeValue(m_Key);
                textField.text = value;
            }
        }
    }
}
