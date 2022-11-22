using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTesterController : MonoBehaviour
{
    [SerializeField] private Transform m_Player;
    [SerializeField] private Transform[] m_TargetArray;

    private PlayerInputAction m_PlayerInputAction;

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
    {
        m_PlayerInputAction = new PlayerInputAction();
        m_PlayerInputAction.Player.Enable();
        m_PlayerInputAction.Player.SpecialWeapon.started += FireSpecialWeapon;
    }
    /// <summary>
    /// FIRE
    /// </summary>
    private void FireSpecialWeapon(InputAction.CallbackContext context)
    {
        PlayerHomingMissile bullet = ObjectPoolManager.I.GetObject<PlayerHomingMissile>(ObjectType.Bullet, "Bullet/PlayerHomingMissile");
        Vector3 randomPos = new Vector3(Random.Range(0f,1f), Random.Range(0f, 5f), 0);
        bullet.transform.position = m_Player.position + randomPos;
        bullet.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        bullet.GetReady(1f);
    }
}
