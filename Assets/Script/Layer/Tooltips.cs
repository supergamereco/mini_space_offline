using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* TODO
* Will be removed after version 27xx.00
*/
public class Tooltips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;
    private readonly int wrapLimit = 0;
    private int headerLength;
    private int contentLength;

    public void SetText(string content, string header)
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.transform.SetActive(false);
        }
        else
        {
            headerField.transform.SetActive(true);
        }
        headerField.text = header;
        contentField.text = content;

        headerLength = headerField.text.Length;
        contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > wrapLimit || contentLength > wrapLimit) ? true : false;
    }

}
