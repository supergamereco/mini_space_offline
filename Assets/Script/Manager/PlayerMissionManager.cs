using System.Collections;
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
        yield return null;