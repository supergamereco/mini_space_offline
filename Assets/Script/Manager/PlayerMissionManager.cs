using System.Collections;using NFT1Forge.OSY.API;using NFT1Forge.OSY.DataModel;using NFT1Forge.OSY.JsonModel;using NFT1Forge.OSY.System;using UnityEngine;public class PlayerMissionManager : Singleton<PlayerMissionManager>{    public ushort PlayerMissionStep { get; private set; }    /// <summary>    /// Update mission step    /// </summary>    /// <param name="step"></param>    public void UpdateMission(ushort step)    {        PlayerMissionStep = step;    }    /// <summary>    /// Send mission complete to server    /// </summary>    /// <param name="missionStep"></param>    public IEnumerator MissionCompleted(ushort missionStep)    {
        //CoroutineWithData api = new CoroutineWithData(this, ApiService.I.UpdateMissionStep(missionStep));
        //yield return api.coroutine;
        //UpdateMissionResponseModel response = (UpdateMissionResponseModel)api.result;
        //        if (response.success)
        //        {
        //            PlayerMissionStep = response.data.next_step;
        //#if UNITY_WEBGL
        //            if (BuildType.WebPlayer == SystemConfig.BuildType)
        //            {
        //                WebBridgeUtils.ShowPlayerMissionStepMessageById($"{PlayerMissionStep}");
        //            }
        //#endif
        //            yield return true;
        //        }
        //        else
        //        {
        //            LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
        //            yield return false;
        //        }
        yield return null;    }    /// <summary>    /// Process message recieved from web3    /// </summary>    /// <param name="message"></param>    public void ProcessUpdateMessage(string message)    {        PlayerMissionStepResponseModel playerMissionStepResponseModel = JsonUtility.FromJson<PlayerMissionStepResponseModel>(message);        PlayerMissionStep = playerMissionStepResponseModel.data.next_step;    }}
