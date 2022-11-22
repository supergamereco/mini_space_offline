using UnityEngine;

public static class ExtensionMethod
{
    public static void SetActive(this Transform transform, bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// Rotate to target to make x axis point at position.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="deltaTime"></param>
    public static void SmoothLookAt2D(this Transform from, Vector3 to, float deltaTime)
    {
        Vector3 direction = to - from.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion m_Quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        from.rotation = Quaternion.Slerp(from.rotation, m_Quaternion, deltaTime);
    }

    /// <summary>
    /// Rotate to target to make x axis point at position.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="direction"></param>
    public static void LookAt2D(this Transform from, Vector3 to)
    {
        Vector3 direction = to - from.position;
        from.LookAtDirection2D(direction);
    }

    /// <summary>
    /// Rotate to direction to make x axis point at direction.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="direction"></param>
    public static void LookAtDirection2D(this Transform from, Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        from.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static void RotateToTarget(this Rigidbody2D rigidbody2D, Vector2 targetPosition, Vector3 rotateAxis, float deltaTime)
    {
        Vector2 direction = targetPosition - rigidbody2D.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, rotateAxis).z;
        rigidbody2D.angularVelocity = rotateAmount * deltaTime;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string Ellipsis(this string message, int maxLength)
    {
        int half = maxLength / 2;
        if (maxLength < message.Length)
        {
            return string.Concat(message.Substring(0, half), "...", message.Substring(message.Length - half));
        }
        else
        {
            return message;
        }
    }
}
