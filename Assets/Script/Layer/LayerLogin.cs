using System;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerLogin : BaseLayer
    {
        [SerializeField] private Button m_ButtonLogin;
        [SerializeField] private TMP_InputField m_InputUser;
        [SerializeField] private TMP_InputField m_InputPass;

        private PlayerInputAction m_PlayerInputAction;

        public Action OnLoginSuccess;

        /// <summary>
        /// Initializing layer
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_ButtonLogin.onClick.AddListener(OnButtonLogin);
            m_PlayerInputAction = new PlayerInputAction();
            m_PlayerInputAction.Player.Disable();
            m_PlayerInputAction.Mainmenu.Enable();
            m_PlayerInputAction.Mainmenu.LogIn_Input_Tab.started += InputTab;
            m_PlayerInputAction.Mainmenu.Login_Input_Enter.started += InputEnter;
            m_InputUser.Select();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void InputTab(InputAction.CallbackContext context)
        {
            if (m_InputUser.isFocused)
                m_InputPass.Select();
            else
                m_InputUser.Select();
        }
        /// <summary>s
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void InputEnter(InputAction.CallbackContext context)
        {
            OnButtonLogin();
        }
        /// <summary>
        /// On login button clicked
        /// </summary>
        private void OnButtonLogin()
        {
            LayerSystem.I.ShowSoftLoadingScreen();
            //StartCoroutine(AuthenticationSystem.I.LoginCorporate(m_InputUser.text, m_InputPass.text, (isSuccess) =>
            //{
            //    LayerSystem.I.HideSoftLoadingScreen();
            //    if (isSuccess)
            //    {
            //        OnLoginSuccess();
            //        m_PlayerInputAction.Mainmenu.Disable();
            //    }
            //    else
            //    {
            //        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_LOGIN_FAILED"));
            //    }
            //}));
            OnLoginSuccess();
            m_PlayerInputAction.Mainmenu.Disable();
        }
    }
}
