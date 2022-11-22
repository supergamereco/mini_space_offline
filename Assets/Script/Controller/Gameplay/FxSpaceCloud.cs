using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSpaceCloud : MonoBehaviour
{
    [SerializeField] private Vector3 m_MoveVector = new Vector3 (0.1f, 0f, 0f);

    

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        transform.Translate(m_MoveVector * Time.deltaTime);
    }
}
