using System;
using System.Collections;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer.Gameplay
{
    public class LayerGameResult : BaseLayer
    {
        [SerializeField] private Image m_ImagePilot;
        [SerializeField] private Image m_ImageSpaceship;
        [SerializeField] private Image m_ImageChest;
        //[SerializeField] private Image m_ImageBoss;
        [SerializeField] private Button m_ButtonDone;
        [SerializeField] private TextMeshProUGUI m_TextScore;
        [SerializeField] private TextMeshProUGUI m_TextGoldCollected;
        [SerializeField] private TextMeshProUGUI m_TextPilotAbility;
        [SerializeField] private TextMeshProUGUI m_TextGold;
        [SerializeField] private TextMeshProUGUI m_TextBossKilled;
        [SerializeField] private TextMeshProUGUI m_TextEnemyKilled;
        [SerializeField] private Transform m_PassProgressBar;
        [SerializeField] private Transform m_ImageChestTransform;
        [SerializeField] private Image m_ChestProgress;
        [SerializeField] private Image m_WeaponProgress;
        [SerializeField] private Image m_PassProgress;
        [SerializeField] private Transform m_WeaponProgressContainer;
        [SerializeField] private Transform m_ChestProgressContainer;

        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxOk;

        public Action OnDone;

        private AudioSource m_Sfx;

        /// <summary>
        /// Initialize layer
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            m_Sfx = GetComponent<AudioSource>();
            m_ButtonDone.onClick.AddListener(OnButtonDone);
        }
        /// <summary>
        /// ButtonDone clicked callback
        /// </summary>
        private void OnButtonDone()
        {
            StartCoroutine(Done());
        }
        /// <summary>
        /// Done process
        /// </summary>
        /// <returns></returns>
        private IEnumerator Done()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);
            yield return new WaitForSeconds(0.6f);
            OnDone?.Invoke();
        }
        /// <summary>
        /// Set displaying data
        /// </summary>
        /// <param name="score"></param>
        /// <param name="totalGold"></param>
        public void SetData(uint score, uint collectedGold, uint totalGold, uint enemyKilled, uint bossKilled, bool isChestCollected
            , bool isEvolvedChest, bool isEvolvedSpecialWeapon)
        {
            m_ImageChestTransform.SetActive(isChestCollected);
            ChestDataModel chest = InventorySystem.I.CreateChestData("1", "Common chest");
            AssetCacheManager.I.SetSprite(m_ImageChest, chest.GetImagePath());
            PilotDataModel pilot = InventorySystem.I.GetActivePilot();
            AssetCacheManager.I.SetSprite(m_ImagePilot, pilot.GetProfileImagePath());
            SpaceshipDataModel spaceship = InventorySystem.I.GetActiveSpaceship();
            AssetCacheManager.I.SetSprite(m_ImageSpaceship, spaceship.GetImagePath());
            m_TextScore.text = score.ToString("###,###,##0");
            m_TextGoldCollected.text = collectedGold.ToString("###,###,##0");
            m_TextPilotAbility.text = $"{pilot.goldbooster:F2}%"; // pilot.goldbooster.ToString("##0.00") + "%";
            m_TextGold.text = totalGold.ToString("###,###,##0");
            m_TextEnemyKilled.text = enemyKilled.ToString("###,###,##0");
            m_TextBossKilled.text = bossKilled.ToString("###,###,##0");
            if (DataModel.GameMode.Survival == GameplayDataManager.I.CurrentGameMode)
                m_PassProgressBar.SetActive(true);
            else
                m_PassProgressBar.SetActive(false);
            ChestDataModel activedChest = InventorySystem.I.GetActiveChest();
            if (null != activedChest)
            {
                m_ChestProgressContainer.SetActive(true);
                ChestMasterData chestMaster = DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master.Find(
                        a => a.name.Equals(activedChest.name)
                    );
                m_ChestProgress.fillAmount = (isEvolvedChest) ? 1: (float)activedChest.missionprogress / chestMaster.mission;
            }
            SpecialWeaponDataModel activedWeapon = InventorySystem.I.GetActiveWeapon();
            if (null != activedWeapon)
            {
                m_WeaponProgressContainer.SetActive(true);
                SpecialWeaponMasterData weaponMaster = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master.Find(
                        a => a.name.Equals(activedWeapon.name)
                    );
                m_WeaponProgress.fillAmount = (isEvolvedSpecialWeapon)? 1 : (float)activedWeapon.missionprogress / weaponMaster.evo_mission;
            }
            //PassDataModel activedPass = ?;
            //if (null != activedPass)
            //{
            //    PassMasterData passMaster = DatabaseSystem.I.GetMetadata<PassMaster>().pass_master.Find(
            //            a => a.name.Equals(activedPass.name)
            //        );
            //    m_PassProgress.fillAmount = activedPass.playcount / passMaster.max_play;
            //    InventorySystem.I.ResetActivePass();
            //}
            if (isChestCollected)
                LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_RECEIVED_CHEST"));
        }

    }
}