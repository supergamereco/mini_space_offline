using UnityEngine;

public class RotateUI : MonoBehaviour
{
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private int m_RotateDirection;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        float rotation = m_RotateDirection * Time.deltaTime * m_RotationSpeed;
        transform.Rotate(0f, 0f, rotation);
    }
}
