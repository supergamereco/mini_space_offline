mergeInto(LibraryManager.library, {
   SendMessageToPage: function (text) {
      var convertedText = Pointer_stringify(text);
      receiveMessageFromUnity(convertedText);
   },

   SendEventToPage: function (event) {
      var convertedText = Pointer_stringify(event);
      receiveEventFromUnity(convertedText);
   },

   SetAppReady: function () {
     isAppReady = true;
   },

   IsPageReady: function () {
     return isPageReady;
   },

   GetAuthenticationType: function () {
     //authenticationType;
     return getAuthenticationType();
   },

  GetPageUUID: function () {
    var returnStr = getUUID();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  GetPageWalletAddress: function () {
    var returnStr = getWalletAddress();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  GetUserNFTMetadata: function (endPoint, appSecret, url, userWalletAddress) {
    getUserNFTMetadata(Pointer_stringify(endPoint), 
      Pointer_stringify(appSecret), 
      Pointer_stringify(url), 
      Pointer_stringify(userWalletAddress));
  },

  MintSignature: function () {
    var signature = mintSignature();
    return signature;
  },
  
  MintNFTToken: function (userAddress, tokenId, nonce, signature) {
    var tansactionHash = mintWithSignature(userAddress, tokenId, nonce, signature);
    return tansactionHash;
  },

  UpdateNFTMetadata: function (endPoint, appSecret, url, jsonMetadata) {
    updateNFTMetadata(Pointer_stringify(endPoint), 
      Pointer_stringify(appSecret), 
      Pointer_stringify(url), 
      Pointer_stringify(jsonMetadata));
  },

  SelectNFT: function (data) {
    selectNFT(Pointer_stringify(data));
  },

  PageAction: function (action, data) {
    pageAction(Pointer_stringify(action), Pointer_stringify(data));
  },

  ShowPlayerMissionStepMessage: function (type, data) {
    showPlayerMissionStepMessage(Pointer_stringify(type),Pointer_stringify(data));
  },

  ShowPlayerMissionStepMessageById: function (currentMissionStepId) {
    showPlayerMissionStepMessageById(Pointer_stringify(currentMissionStepId));
  },

  IsLogIn: function (loggedInStatus, currentMissionStepId) {
    isLogIn(Pointer_stringify(loggedInStatus), Pointer_stringify(currentMissionStepId));
  },


  FirebaseLogEvent: function (eventKey, eventJSONValue) {
    firebaseLogEvent(Pointer_stringify(eventKey),Pointer_stringify(eventJSONValue));
  },

  TestFunction: function () {
    testFunction();
  },

  PostNFTMetadata: function(endPoint, appSecret, url, jsonMetadata) {
    postNFTMetadata(Pointer_stringify(endPoint), 
      Pointer_stringify(appSecret), 
      Pointer_stringify(url), 
      Pointer_stringify(jsonMetadata));
  },

  GetMobileStatus: function() {
    return getMobileStatus();
  },

  GetServerId: function() {
    return getServerId();
  }
});