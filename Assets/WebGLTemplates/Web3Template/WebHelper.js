const METAMASK_POSSIBLE_ERRORS = 
{
    '-32700': 
    {
        standard: 'JSON RPC 2.0',
        message: 'Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text.',
    },
    '-32600': 
    {
        standard: 'JSON RPC 2.0',
        message: 'The JSON sent is not a valid Request object.',
    },
    '-32601': 
    {
        standard: 'JSON RPC 2.0',
        message: 'The method does not exist / is not available.',
    },
    '-32602': 
    {
        standard: 'JSON RPC 2.0',
        message: 'Invalid method parameter(s).',
    },
    '-32603': 
    {
        standard: 'JSON RPC 2.0',
        message: 'Internal JSON-RPC error.',
    },
    '-32000': 
    {
        standard: 'EIP-1474',
        message: 'Invalid input.',
    },
    '-32001': 
    {
        standard: 'EIP-1474',
        message: 'Resource not found.',
    },
    '-32002': 
    {
        standard: 'EIP-1474',
        message: 'Resource unavailable.',
    },
    '-32003': 
    {
        standard: 'EIP-1474',
        message: 'Transaction rejected.',
    },
    '-32004': 
    {
        standard: 'EIP-1474',
        message: 'Method not supported.',
    },
    '-32005': 
    {
        standard: 'EIP-1474',
        message: 'Request limit exceeded.',
    },
    '4001': 
    {
        standard: 'EIP-1193',
        message: 'User rejected the request.',
    },
    '4100': 
    {
        standard: 'EIP-1193',
        message: 'The requested account and/or method has not been authorized by the user.',
    },
    '4200': 
    {
        standard: 'EIP-1193',
        message: 'The requested method is not supported by this Ethereum provider.',
    },
    '4900': 
    {
        standard: 'EIP-1193',
        message: 'The provider is disconnected from all chains.',
    },
    '4901': 
    {
        standard: 'EIP-1193',
        message: 'The provider is disconnected from the specified chain.',
    },
}

function openPage(pageUrl) {
    window.location.href = pageUrl;
    //localStorage.setItem("unityInstance", osyUnityInstance)
}

function getAuthenticationType() {
    return authenticationType;
}

function getAuthenticationType() {
  return localStorage.getItem('authentication_type');
}

function getUUID() {
  return localStorage.getItem('uuid');
}

function getWalletAddress() {
  return localStorage.getItem('metamask_account');
}

function setApplicationStatus(){
  isAppReady = true
  osyUnityInstance.SendMessage(
    "ReceivedEventObject",
      "ApplicationReady",
      "Application is Ready!!"
  )
}

function sendWalletAddress() {
  osyUnityInstance.SendMessage(
      "ReceivedEventObject",
      "ReceiveWalletAddress",
      window.userWalletAddress
  )
}

function receiveMessageFromUnity(txt) {
  // Assign received from Unity message
  
}


function firebaseLogEvent(eventKey, eventJSONValue) {
  console.log("eventKey", eventKey)
  console.log("eventJSONValue", eventJSONValue)
}

async function showPlayerMissionStepMessage(type, data){
  console.log("Type from unity")
  console.log(type)
  console.log("Data from unity")
  console.log(data)

  let jsonData = JSON.parse(data);
  console.log(jsonData.player_mission_step_id)
  let playerMissionStepId = 7
  // let result = await setMissionStep(jsonData.player_mission_step_id)
  let result = await setMissionStep(playerMissionStepId)

  var value = {code:parseInt(type), message:JSON.stringify(result)};
  
  console.log("value")
  console.log(value)
  osyUnityInstance.SendMessage(
    '(singleton) NFT1Forge.OSY.System.WebBridgeSystem',
    'ReceiveWebMessage',
    JSON.stringify(value)
    );
}

function showPlayerMissionStepMessageById(currentMissionStepId){
  console.log("currentMissionStepId",currentMissionStepId)
}

async function isLogIn(loggedInStatus) {
  console.log("isLogin",parseInt(loggedInStatus))
  //localStorage.setItem("isLogin",loggedInStatus)
}

async function sendMessageToUnity() {
  const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' })
    .catch((e) => {
      console.error(e.message)
      return
    })
  if (!accounts) { return }
  window.userWalletAddress = accounts[0]
  var value = {code:10001, message:window.userWalletAddress};
  osyUnityInstance.SendMessage(
    "WebBridgeSystem",
    "ReceiveWebMessage",
    JSON.stringify(value)
  );
}

async function getUserNFTMetadata(endPoint, appSecret, url, userWalletAddress) {
  var jsonResult = ""
  const apiconfig = await axios.create({
      baseURL: endPoint,
      headers: {
        'Accept':'*/*',
        'X-NFT-1-APP-SECRET':appSecret,
        'Content-Type' : 'application/json'
      }
  })
  await apiconfig.get(url,
  {
      params: {
        owner: userWalletAddress + "_polygon"
      }
  })
  .then(response => {
      var metadata = []
      metadata = response.data.metadata
      var jsonResults = []

      for (var index in metadata) {
        var tokenMeta = metadata[index]
        var attribute = {}
        var attributes = tokenMeta.attributes
        for (var attributeIndex in attributes) {
          var fieldName = attributes[attributeIndex].fieldName.toLowerCase()
          attribute[fieldName] = attributes[attributeIndex].value
        }
        var mappedJson = { 
            tokenId: tokenMeta.tokenId,
            name: tokenMeta.name,
            description: tokenMeta.description,
            image: tokenMeta.image
        }
        Object.assign(mappedJson, attribute)
        jsonResults.push(mappedJson)
      }
      console.log(jsonResults)
      
  })
  .catch(err => {
      console.error(err)
  })
  return jsonResult
}

function mintSignature(ethers,nftClient,endPoint, appSecret, url, adminWalletId, contractAddress, tokenId, nonce , userWalletAddress){
  const apiInstance = axios.create({
    baseURL: endPoint,
    headers: {
      'Accept':'*/*',
      'X-NFT-1-APP-SECRET':appSecret,
      'Content-Type' : 'application/json'
    }
  })

  apiInstance.get(url, 
  {
    params:{
      walletId : adminWalletId,
      tokenId : tokenId,
      nonce: nonce,
      recipientAddress : userWalletAddress
    }
  })
  .then(response => {
    console.log('success\n')
    console.log(response.data)
    this.mintWithSignature(ethers,nftClient,contractAddress, tokenId, nonce , userWalletAddress,response.data)
  })
  .catch(err => {
    console.log("error\n")
    console.log(err.message);
  })
}

async function mintWithSignature(ethers,nftClient,contractAddress, tokenId, nonce , userAddress,signature){
  const provider = new ethers.providers.Web3Provider(window.ethereum);
  const signer = provider.getSigner();
  await nftClient.connectProvider(contractAddress, provider)
  nftClient.connectSigner(signer)
  nftClient.setWaitConfirmations(1)
  try{
    const result = await nftClient.mintWithSignature(userAddress, tokenId, nonce, signature)
    console.log(result)
  }catch(err){
    console.log(METAMASK_POSSIBLE_ERRORS[err.code].message)
  }
  
}

async function burnNFT(ethers,nftClient,contractAddress,tokenId){
  const provider = new ethers.providers.Web3Provider(window.ethereum);
  const signer = provider.getSigner();
  await nftClient.connectProvider(contractAddress, provider)
  nftClient.connectSigner(signer)
  nftClient.setWaitConfirmations(1) //1 for testnet, 5 for mainnet
  try{
    const result = await nftClient.burn(tokenId)
  }catch(err){
    console.log(METAMASK_POSSIBLE_ERRORS[err.code].message)
  }
}

async function lockNFT(ethers,nftClient,contractAddress,tokenId){
  const provider = new ethers.providers.Web3Provider(window.ethereum);
  const signer = provider.getSigner();
  await nftClient.connectProvider(contractAddress, provider)
  nftClient.connectSigner(signer)
  nftClient.setWaitConfirmations(1) //1 for testnet, 5 for mainnet

  const isLocked = await nftClient.locked(tokenId)
  console.log(isLocked)

  try{
    if(!isLocked){
      const resultLock = await nftClient.lock(tokenId)
      console.log(resultLock)
    }
  }catch(err){
    console.log(METAMASK_POSSIBLE_ERRORS[err.code].message)
  }
}

function withdrawSignature(ethers,nftClient,endPoint, appSecret, url, contractAddress, tokenId) {
  const apiInstance = axios.create({
    baseURL: endPoint,
    headers: {
      'Accept':'*/*',
      'X-NFT-1-APP-SECRET':appSecret,
      'Content-Type' : 'application/json'
    }
  })
    
  apiInstance.post(url)
    .then(response => {
      console.log('success\n')
      console.log(response.data)
      this.unlockNFT(ethers,nftClient,contractAddress, tokenId, response.data)
    })
    .catch(err => {
      console.log("error\n")
      console.log(err.message);
    })
}

async function unlockToken(ethers,nftClient,contractAddress, tokenId,signature){
  const provider = new ethers.providers.Web3Provider(window.ethereum);
  const signer = provider.getSigner();
  await nftClient.connectProvider(contractAddress, provider)
  nftClient.connectSigner(signer)
  nftClient.setWaitConfirmations(1)

  const resultVersion = await nftClient.tokenVersion(tokenId)

  const isLocked = await nftClient.locked(tokenId)
  console.log('locked status : ')
  console.log(isLocked)
  try{
    if(isLocked){
      const resultUnlock = await nftClient.unLock(tokenId,resultVersion,signature)
      console.log(resultUnlock)
    }
  }catch(err){
    console.log(METAMASK_POSSIBLE_ERRORS[err.code].message)
  }
}

function updateNFTMetadata(endPoint, appSecret, url, jsonMetadata) {
  const apiconfig = axios.create({
      baseURL: endPoint,
      headers: {
        'Accept':'*/*',
        'X-NFT-1-APP-SECRET':appSecret,
        'Content-Type' : 'application/json'
      }
    })
  apiconfig.patch(url, jsonMetadata)
  .then(response => {
      console.log("response update metadata: ", response)
  })
  .catch(err => {
      console.error(err)
  })
}

/***
 * data is json string 
 * {"active_pilot": 11,"active_spaceship": 24,"active_weapon": 33,"active_chest": 44,"active_pass": 55}
 */
function selectNFT(data){
  // do something with data
}

function pageAction(action, data){
      // do something with data
      console.log(action)
      console.log(data)
}

/***
 * contract id can read from json config mapping with type 
 * tokenId variable can send multiple value by "," e.g. 6,7 
 * 
 */
async function getUserNFTMetadataByTokenId(endPoint,appId, appSecret, contractId, tokenId) {
  var jsonResult = ""
  const url = endPoint + "/metadata/api/v1/apps/"+appId+"/contracts/"+contractId+"/metadata/tokens?tokenIds="+tokenId
  const apiconfig = await axios.create({
      baseURL: endPoint,
      headers: {
        'Accept':'*/*',
        'X-NFT-1-APP-SECRET':appSecret,
        'Content-Type' : 'application/json'
      }
  })
  await apiconfig.get(url)
  .then(response => {
      var metadata = []
      metadata = response.data.metadata
      var jsonResults = []

      for (var index in metadata) {
        var tokenMeta = metadata[index]
        var attribute = {}
        var attributes = tokenMeta.attributes
        for (var attributeIndex in attributes) {
          var fieldName = attributes[attributeIndex].fieldName.toLowerCase()
          attribute[fieldName] = attributes[attributeIndex].value
        }
        var mappedJson = { 
            tokenId: tokenMeta.tokenId,
            name: tokenMeta.name,
            description: tokenMeta.description,
            image: tokenMeta.image
        }
        Object.assign(mappedJson, attribute)
        jsonResults.push(mappedJson)
      }
      console.log(jsonResults)
      
  })
  .catch(err => {
      console.error(err)
  })
  return jsonResult
}

async function setMissionStep(missionStep) {
  let serverApiUrl = 'the_server_api_url'
  //let serverApiUrl = 'https://o9plmt3g.nft1.global'
  const apiconfig = await axios.create({
    baseURL: serverApiUrl,
    headers: {
      'Accept':'*/*',
      'Content-Type' : 'application/json'
    }
  })
  let playerMissionStepResult
  const url = "/mission/" + 'your_wallet_address'
  await apiconfig.post(url, {
    step: missionStep
  })
  .then(response => {
    console.log("response mission step: ", response.data)
    playerMissionStepResult = response.data
    if(playerMissionStepResult.success) {
      localStorage.setItem('mission_step', playerMissionStepResult.next_step)
    }
  })
  .catch(err => {
    console.error(err)
  })

  return playerMissionStepResult
}