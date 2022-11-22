using System.Collections;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.JsonModel;
namespace NFT1Forge.OSY.System
{
    public class MintingSystem : Singleton<MintingSystem>
    {
        private NftMetadataModel m_CurrentNft;

        /// <summary>
        /// Call API minting item
        /// </summary>
        //public IEnumerator CallAPIMinting(NftMetadataModel nft)
        //{
        //    m_CurrentNft = nft;
        //    CoroutineWithData api = new CoroutineWithData(this, ApiService.I.Minting(m_CurrentNft.token_id));
        //    yield return api.coroutine;
        //    BaseResponseModel response = (BaseResponseModel)api.result;
        //    if (response.success)
        //    {
        //        ShowWaitingDialog();
        //    }
        //    else
        //    {
        //        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
        //    }
        //}
        /// <summary>
        /// Call API cancel when waiting for minting
        /// </summary>
        private void CallAPIMintStatus()
        {
            StartCoroutine(CallAPIGetMintingStatus());
        }
        /// <summary>
        /// Call API cancel when waiting for minting
        /// </summary>
        //private IEnumerator CallAPICancelMinting()
        //{
        //    if (null == m_CurrentNft)
        //        yield break;

        //    CancelInvoke();
        //    CoroutineWithData api = new CoroutineWithData(this, ApiService.I.CancelMinting(m_CurrentNft.token_id));
        //    yield return api.coroutine;
        //    BaseResponseModel response = (BaseResponseModel)api.result;
        //    if (response.success)
        //    {
        //        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_MINT_CANCEL"));
        //    }
        //    else
        //    {
        //        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
        //    }
        //}
        /// <summary>
        /// Call API check the status minting
        /// </summary>
        private IEnumerator CallAPIGetMintingStatus()
        {
            //if (null == m_CurrentNft)
            //{
            //    LayerSystem.I.ShowPopupDialog("Error : 3000001");
            //    yield break;
            //}
            //CoroutineWithData api = new CoroutineWithData(this, ApiService.I.GetMintingStatus(m_CurrentNft.token_id));
            //yield return api.coroutine;
            //GetMintingStatusResponseModel response = (GetMintingStatusResponseModel)api.result;
            //if (response.success)
            //{
            //    switch (response.data.status)
            //    {
            //        case 1:
            //            m_CurrentNft = null;
            //            CancelInvoke();
            //            LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_MINT_FAILED"));
            //            break;
            //        case 2:
            //            break;
            //        case 3:
            //            InventorySystem.I.ChangeToNft(m_CurrentNft);
            //            m_CurrentNft = null;
            //            CancelInvoke();
            //            LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_MINT_SUCCESS"));
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    m_CurrentNft = null;
            //    CancelInvoke();
            //    LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
            //}
            yield return null;
        }
        /// <summary>
        /// Show mint waiting dialog
        /// </summary>
        /// <param name="nft"></param>
        public void ShowWaitingDialog(NftMetadataModel nft = null)
        {
            //if (null != nft)
            //    m_CurrentNft = nft;
            //LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_MINT_WAITING"),
            //        () => StartCoroutine(CallAPICancelMinting()),
            //        m_CurrentNft.image,
            //        LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_CANCEL"));
            //InvokeRepeating(nameof(CallAPIMintStatus), SystemConfig.TimingGetMintingStatus, SystemConfig.TimingGetMintingStatus);
        }
    }
}
