using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    public Action<ushort> OnSelected;
    public ushort colorId;

    public void SetSelection(bool isSelected)
    {
        transform.GetChild(0).SetActive(isSelected);
    }
    /// <summary>
    /// Called on first frame
    /// </summary>
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnSelected?.Invoke(colorId));
    }
}
