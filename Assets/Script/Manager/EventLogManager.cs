using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using UnityEngine;

public class EventLogManager : Singleton<EventLogManager>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    public void SimpleEvent(string eventName)
    {
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
            WebBridgeUtils.FirebaseLogEvent(eventName, string.Empty);
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void PilotUpgrade(PilotDataModel data)
    {
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
        {
            PilotUpgradeModel model = new PilotUpgradeModel()
            {
                pilot_name = data.name,
                pilot_level = data.level
            };
            WebBridgeUtils.FirebaseLogEvent("game_demo_pilot_upgrade_click", JsonUtility.ToJson(model));
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.Parameter[] levelUpParameters = {
            new Firebase.Analytics.Parameter("pilot_name", data.name),
            new Firebase.Analytics.Parameter("pilot_level", data.level)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("game_demo_pilot_upgrade_click",
        levelUpParameters);
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SpaceshipUpgrade(SpaceshipDataModel data)
    {
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
        {
            SpaceshipUpgradeModel model = new SpaceshipUpgradeModel()
            {
                spaceship_name = data.name,
                spaceship_level = data.level
            };
            WebBridgeUtils.FirebaseLogEvent("game_demo_spaceship_upgrade_click", JsonUtility.ToJson(model));
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.Parameter[] levelUpParameters = {
            new Firebase.Analytics.Parameter("spaceship_name", data.name),
            new Firebase.Analytics.Parameter("spaceship_level", data.level)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("game_demo_spaceship_upgrade_click",
        levelUpParameters);
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="name"></param>
    /// <param name="level"></param>
    public void ItemActivate(string eventName, string name, ushort level = 0)
    {
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
        {
            ItemActivateModel model = new ItemActivateModel()
            {
                name = name,
                level = level
            };
            WebBridgeUtils.FirebaseLogEvent(eventName, JsonUtility.ToJson(model));
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.Parameter[] levelUpParameters = {
            new Firebase.Analytics.Parameter("name", name),
            new Firebase.Analytics.Parameter("level", level)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName,
        levelUpParameters);
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nftType"></param>
    /// <param name="nftName"></param>
    public void OpenChestComplete(ObjectListModel data)
    {
        string nftType = string.Empty;
        string nftName = string.Empty;

        if (null != data.pilot && 0 < data.pilot.Count)
        {
            nftType = "Pilot";
            nftName = data.pilot[0].name;
        }
        if (null != data.spaceship && 0 < data.spaceship.Count)
        {
            nftType = "Spaceship";
            nftName = data.spaceship[0].name;
        }
        if (null != data.chest && 0 < data.chest.Count)
        {
            nftType = "Chest";
            nftName = data.chest[0].name;
        }
        if (null != data.survival_pass && 0 < data.survival_pass.Count)
        {
            nftType = "Pass";
            nftName = data.survival_pass[0].name;
        }
        if (null != data.special_weapon && 0 < data.special_weapon.Count)
        {
            nftType = "Special Weapon";
            nftName = data.special_weapon[0].name;
        }
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
        {
            OpenChestEventModel model = new OpenChestEventModel()
            {
                nft_type = nftType,
                nft_name = nftName
            };
            WebBridgeUtils.FirebaseLogEvent("open_chest_completed_click_in_game", JsonUtility.ToJson(model));
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.Parameter[] levelUpParameters = {
            new Firebase.Analytics.Parameter("nft_type", nftType),
            new Firebase.Analytics.Parameter("nft_name", nftName)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("open_chest_completed_click_in_game",
        levelUpParameters);
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stageType"></param>
    public void PlayGame(string stageType)
    {
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
        {
            PlayGameEventModel model = new PlayGameEventModel()
            {
                stage_type = stageType
            };
            WebBridgeUtils.FirebaseLogEvent("game_demo_play_stage_start", JsonUtility.ToJson(model));
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.Parameter[] levelUpParameters = {
            new Firebase.Analytics.Parameter("stage_type", stageType)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("game_demo_play_stage_start",
        levelUpParameters);
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stageType"></param>
    /// <param name="score"></param>
    /// <param name="gold"></param>
    /// <param name="gotChest"></param>
    /// <param name="enemyKilled"></param>
    /// <param name="bossKilled"></param>
    public void FinishGame(string stageType, uint score, uint gold, bool gotChest, ushort enemyKilled, ushort bossKilled)
    {
#if UNITY_WEBGL
        if (BuildType.WebPlayer == SystemConfig.BuildType)
        {
            FinishGameEventModel model = new FinishGameEventModel()
            {
                stage_type = stageType,
                score = score,
                gold = gold,
                got_chest = gotChest,
                enemy_killed = enemyKilled,
                boss_killed = bossKilled
            };
            WebBridgeUtils.FirebaseLogEvent("game_demo_play_stage_finish", JsonUtility.ToJson(model));
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        Firebase.Analytics.Parameter[] levelUpParameters = {
            new Firebase.Analytics.Parameter("stage_type", stageType),
            new Firebase.Analytics.Parameter("score", score),
            new Firebase.Analytics.Parameter("gold", gold),
            new Firebase.Analytics.Parameter("got_chest", gotChest.ToString()),
            new Firebase.Analytics.Parameter("enemy_killed", enemyKilled),
            new Firebase.Analytics.Parameter("boss_killed", bossKilled),
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("game_demo_play_stage_finish",
        levelUpParameters);
#endif
    }

}
