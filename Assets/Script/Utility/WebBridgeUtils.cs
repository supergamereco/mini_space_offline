#if UNITY_WEBGL
using System;
using System.Runtime.InteropServices;

public static class WebBridgeUtils
{
    public static string localEndPoint = "http://localhost:60649";
    public static string oneSyncEndPointPreStaging = "https://api.1sync-staging.nft1.global";
    public static string appId = "polygon:mumbai:BYdXIxk_bSXRwm_RpTy2L";
    public static string appSecret = "polygon:mumbai:BYdXIxk_bSXRwm_RpTy2L:eqvAJjwpp6xB0Vim";

    public static string generateNonce() {
        Guid uuid = Guid.NewGuid();
        return uuid.ToString();
    }

    [DllImport("__Internal")]
    public static extern void SendMessageToPage(string text);

    [DllImport("__Internal")]
    private static extern void SendEventToPage(string eventText);

    [DllImport("__Internal")]
    public static extern void SetAppReady();

    [DllImport("__Internal")]
    public static extern bool IsPageReady();

    [DllImport("__Internal")]
    public static extern int GetAuthenticationType();

    [DllImport("__Internal")]
    public static extern string GetPageUUID();

    [DllImport("__Internal")]
    public static extern string GetPageWalletAddress();

    [DllImport("__Internal")]
    public static extern string GetUserNFTMetadata(string endPoint, string appSecret, string url, string userWalletAddress);

    [DllImport("__Internal")]
    public static extern string SelectNFT(string data);

    [DllImport("__Internal")]
    public static extern string PageAction(string action, string data);

    [DllImport("__Internal")]
    public static extern string MintSignature();

    [DllImport("__Internal")]
    public static extern string MintNFTToken();

    [DllImport("__Internal")]
    public static extern string UpdateNFTMetadata(string endPoint, string appSecret, string url, string jsonMetadata);

    [DllImport("__Internal")]
    public static extern string TestFunction();

    [DllImport("__Internal")]
    public static extern string PostNFTMetadata(string endPoint, string appSecret, string url, string jsonMetadata);

    [DllImport("__Internal")]
    public static extern int GetMobileStatus();

    [DllImport("__Internal")]
    public static extern int GetServerId();

    [DllImport("__Internal")]
    public static extern string ShowPlayerMissionStepMessageById(string currentMissionStepId);

    [DllImport("__Internal")]
    public static extern string IsLogIn(string loggedInStatus, string missionStep);

    [DllImport("__Internal")]
    public static extern string FirebaseLogEvent(string eventKey ,string eventJSONValue);

    public static string getUserNFTMetadataUrl(string contractID) {
        return $"/metadata/api/v1/apps/{appId}/contracts/{contractID}/metadata";
    }

    public static string updateNFTMetadataUrl(string contractID) {
        return $"/metadata/api/v1/apps/{appId}/contracts/{contractID}/metadata";
    }

    public static string postNFTMetadataUrl(string contractID)
    {
        return $"/metadata/api/v1/apps/{appId}/contracts/{contractID}/metadata";
    }
}
#endif
