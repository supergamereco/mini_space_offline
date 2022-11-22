using System;
using System.Collections;
using System.Collections.Generic;
using NFT1Forge.OSY.API;
using NFT1Forge.OSY.DataModel;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.Manager;
using NFT1Forge.OSY.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFT1Forge.OSY.Layer
{
    public class LayerInventory : BaseLayer
    {
        public Action OnClosed;
        public Action OnForceUpdate;

        [SerializeField] private Transform m_ItemHolder = default;
        [SerializeField] private RawImage m_ShowSelectItemImage = default;
        [SerializeField] private Button m_CloseButton = default;

        [Header("Infomation Panel")]
        [SerializeField] private TextMeshProUGUI m_ItemNameText = default;
        [SerializeField] private TextMeshProUGUI m_DescriptionText = default;
        [SerializeField] private TextMeshProUGUI m_FirstSpecialDescriptionText = default;
        [SerializeField] private TextMeshProUGUI m_SecondSpecialDescriptionText = default;
        [SerializeField] private TextMeshProUGUI m_ThirdSpecialDescriotionText = default;
        [SerializeField] private TextMeshProUGUI m_TextUid;
        [SerializeField] private TextMeshProUGUI m_TextGold;
        [SerializeField] private Transform m_Sstar;
        [SerializeField] private Transform m_Astar;
        [SerializeField] private Transform m_Bstar;
        [SerializeField] private Transform m_NftIconOn;
        [SerializeField] private Transform m_ActiveIconOn;
        [SerializeField] private Transform m_NftIconOff;
        [SerializeField] private Transform m_ActiveIconOff;
        [SerializeField] private Button m_ButtonNftIconOn = default;
        [SerializeField] private Button m_ButtonActiveIconOn = default;
        [SerializeField] private Button m_ButtonNftIconOff = default;
        [SerializeField] private Button m_ButtonActiveIconOff = default;
        [SerializeField] private Button m_ButtonBoundIconOff = default;

        [Header("Button")]
        [SerializeField] private Button m_ButtonActivate = default;
        [SerializeField] private Button m_ButtonOpen = default;
        [SerializeField] private Button m_ButtonMint = default;

        [Header("Mint item")]
        [SerializeField] private Transform m_PanelMintConfirmation;
        [SerializeField] private Button m_ButtonMintConfirm;
        [SerializeField] private Button m_ButtonMintCancel;

        [Header("Open Chest")]
        [SerializeField] private Transform m_PanelOpenChest;
        [SerializeField] private Button m_ButtonOpenConfirm = default;
        [SerializeField] private Button m_ButtonOpenCancel = default;

        [Header("Progress")]
        [SerializeField] private Transform m_ProgressBarTransform;
        [SerializeField] private TextMeshProUGUI m_TextProgress = default;
        [SerializeField] private Image m_ImageProgressBar = default;

        [Header("SFX")]
        [SerializeField] private AudioClip m_SfxOk;
        [SerializeField] private AudioClip m_SfxCancel;

        private readonly List<InventorySlot> m_ItemList = new List<InventorySlot>();
        private NftMetadataModel m_SelectedItem;
        private NFTCollectionType m_SelectedType;
        private AudioSource m_Sfx;

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            m_Sfx = GetComponent<AudioSource>();
            m_CloseButton.onClick.AddListener(OnCloseButton);
            m_ButtonActivate.onClick.AddListener(() => StartCoroutine(OnActiveButton()));
            m_ButtonOpen.onClick.AddListener(OnOpenButton);
            m_ButtonOpenConfirm.onClick.AddListener(OnConfirmButton);
            m_ButtonOpenCancel.onClick.AddListener(OnCancelButton);
            m_ButtonMint.onClick.AddListener(OnMintButton);
            m_ButtonMintConfirm.onClick.AddListener(OnConfirmMintButton);
            m_ButtonMintCancel.onClick.AddListener(OnCancleMintButton);
            m_ButtonNftIconOn.onClick.AddListener(OnNftIconOnButton);
            m_ButtonActiveIconOn.onClick.AddListener(OnActiveIconOnButton);
            m_ButtonNftIconOff.onClick.AddListener(OnNftIconOffButton);
            m_ButtonActiveIconOff.onClick.AddListener(OnActiveIconOffButton);
            m_ButtonBoundIconOff.onClick.AddListener(OnBoundIconOffButton);

            string uidText = "UID"; //We might have wallet address / UID in the future
            m_TextUid.text = $"{uidText}: {AuthenticationSystem.I.WalletAddress.Ellipsis(10)}";

            OnSoftCurrency1Changed(InventorySystem.I.SoftCurrency1);
            InventorySystem.I.OnSoftCurrency1Changed += OnSoftCurrency1Changed;
            InventorySystem.I.OnItemActivated += OnItemActivated;
            InventorySystem.I.OnItemUpdated += OnItemUpdate;
            InventorySystem.I.OnInventoryRefresh += OnInventoryRefresh;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateInventoryList()
        {
            ObjectPoolManager.I.SetGlobalObjectHolder(m_ItemHolder);

            for (int i = 0; i < InventorySystem.I.ChestCount; i++)
            {
                CreateItemSlot(InventorySystem.I.GetChestByIndex(i));
            }
            for (int i = 0; i < InventorySystem.I.PassCount; i++)
            {
                CreateItemSlot(InventorySystem.I.GetPassByIndex(i));
            }
            for (int i = 0; i < InventorySystem.I.WeaponCount; i++)
            {
                CreateItemSlot(InventorySystem.I.GetWeaponByIndex(i));
            }
            if (0 < m_ItemList.Count)
            {
                OnItemSelect(m_ItemList[0].TokenId, m_ItemList[0].Type);
            }
            else
            {
                m_ShowSelectItemImage.enabled = false;
                m_DescriptionText.font = LocalizationSystem.I.GetFont();
                m_DescriptionText.text = LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_EMPTY_ITEM_DISCRIPTION");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateSlot()
        {
            int j = 0;
            for (int i = 0; i < InventorySystem.I.ChestCount; i++)
            {
                ChestDataModel chest = InventorySystem.I.GetChestByIndex(i);
                bool isSelected = chest.token_id == m_SelectedItem.token_id;
                m_ItemList[j].Setup(chest.token_id, chest.name, chest.GetImagePath(), chest.is_nft, chest.is_active, isSelected, OnItemSelect, NFTCollectionType.Chest);
                j++;
            }
            for (int i = 0; i < InventorySystem.I.PassCount; i++)
            {
                PassDataModel pass = InventorySystem.I.GetPassByIndex(i);
                bool isActive = false;//pass.tokenId == InventorySystem.I.activeObject.active_pass;
                bool isSelected = pass.token_id == m_SelectedItem.token_id;
                m_ItemList[j].Setup(pass.token_id, pass.name, pass.GetImagePath(), pass.is_nft, isActive, isSelected, OnItemSelect, NFTCollectionType.Pass);
                j++;
            }
            for (int i = 0; i < InventorySystem.I.WeaponCount; i++)
            {
                SpecialWeaponDataModel weapon = InventorySystem.I.GetWeaponByIndex(i);
                bool isSelected = weapon.token_id == m_SelectedItem.token_id;
                m_ItemList[j].Setup(weapon.token_id, weapon.name, weapon.GetImagePath(), weapon.is_nft, weapon.is_active, isSelected, OnItemSelect, NFTCollectionType.SpecialWeapon);
                j++;
            }
            if (0 == m_ItemList.Count)
            {
                m_ShowSelectItemImage.enabled = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void CreateItemSlot(NftMetadataModel data)
        {
            InventorySlot slot = ObjectPoolManager.I.GetObject<InventorySlot>(ObjectType.UI, "Layer/InventorySlot");
            slot.transform.localScale = Vector3.one;
            string imagePath = "";
            NFTCollectionType nftType = NFTCollectionType.Chest;
            var type = data.GetType();

            if (typeof(ChestDataModel) == type)
            {
                imagePath = ((ChestDataModel)data).GetImagePath();
            }
            else if (typeof(PassDataModel) == type)
            {
                imagePath = ((PassDataModel)data).GetImagePath();
                nftType = NFTCollectionType.Pass;
            }
            else if (typeof(SpecialWeaponDataModel) == type)
            {
                imagePath = ((SpecialWeaponDataModel)data).GetImagePath();
                nftType = NFTCollectionType.SpecialWeapon;
            }

            slot.Setup(data.token_id, data.name, imagePath, data.is_nft, data.is_active, false, OnItemSelect, nftType);
            m_ItemList.Add(slot);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenId"></param>
        private void OnItemSelect(string tokenId, NFTCollectionType type)
        {
            if (null != m_Sfx && m_Sfx.isActiveAndEnabled)
                m_Sfx.PlayOneShot(m_SfxOk);
            m_SelectedType = type;
            ActiveObjectRequestModel activeModel = new ActiveObjectRequestModel("0", "0", "0", "0", "0");
            PageActionType pageActionType = PageActionType.SelectMissionChest;
            if (NFTCollectionType.Chest == type)
            {
                ChestDataModel selected = InventorySystem.I.GetChest(tokenId);
                if (null != selected)
                {
                    m_SelectedItem = selected;
                    LoadChestData(selected);
                    activeModel = new ActiveObjectRequestModel("0", "0", "0", tokenId, "0");
                    pageActionType = PageActionType.SelectMissionChest;
                }
                EventLogManager.I.SimpleEvent("game_demo_nft_info_mission_chest_click_in_game");
            }
            else if (NFTCollectionType.Pass == type)
            {
                PassDataModel selected = InventorySystem.I.GetPass(tokenId);
                if (null != selected)
                {
                    m_SelectedItem = selected;
                    LoadPassData(selected);
                    activeModel = new ActiveObjectRequestModel("0", "0", "0", "0", tokenId);
                    pageActionType = PageActionType.SelectSurvivalPass;
                }
                EventLogManager.I.SimpleEvent("game_demo_nft_info_survival_pass_click_in_game");
            }
            else if (NFTCollectionType.SpecialWeapon == type)
            {
                SpecialWeaponDataModel selected = InventorySystem.I.GetSpecialWeapon(tokenId);
                if (null != selected)
                {
                    m_SelectedItem = selected;
                    LoadWeaponData(selected);
                    activeModel = new ActiveObjectRequestModel("0", "0", tokenId, "0", "0");
                    if ("God" == selected.GetMaster().rank)
                    {
                        pageActionType = PageActionType.SelectGodWeapon;
                    }
                    else
                    {
                        pageActionType = PageActionType.SelectSpecialWeapon;
                    }
                }
                EventLogManager.I.SimpleEvent("game_demo_nft_info_special_weapon_click_in_game");
            }
            UpdateSlot();
            m_ButtonMint.gameObject.SetActive(!m_SelectedItem.is_nft);
            m_ButtonActivate.gameObject.SetActive(!m_SelectedItem.is_active && NFTCollectionType.Pass != type);
#if UNITY_WEBGL
            if (BuildType.WebPlayer == SystemConfig.BuildType)
            {
                WebBridgeUtils.SelectNFT(JsonUtility.ToJson(activeModel));
                WebBridgeUtils.PageAction(((int)pageActionType).ToString(), "");
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private IEnumerator OnActiveButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);
            if (m_SelectedItem is ChestDataModel chest)
            {
                StartCoroutine(InventorySystem.I.SetActiveChest(chest.token_id));
                if (5 == PlayerMissionManager.I.PlayerMissionStep)
                {
                    CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(5));
                    yield return api.coroutine;
                    if (!(bool)api.result)
                    {
                        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                    }
                }
            }
            else if (m_SelectedItem is SpecialWeaponDataModel weapon)
            {
                StartCoroutine(InventorySystem.I.SetActiveWeapon(weapon.token_id));
                if (13 == PlayerMissionManager.I.PlayerMissionStep)
                {
                    CoroutineWithData api = new CoroutineWithData(this, PlayerMissionManager.I.MissionCompleted(13));
                    yield return api.coroutine;
                    if (!(bool)api.result)
                    {
                        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_FAILED_UPDATE_DATA"));
                    }
                }
            }
            UpdateSlot();
            if (m_SelectedItem is ChestDataModel selectedChest)
            {
                //ChestDataModel selected = InventorySystem.I.GetChest(m_SelectedItem.token_id);
                //if (null != selected)
                //{
                //    LoadChestData(selected);
                //}
                LoadChestData(selectedChest);
            }
            else if (m_SelectedItem is PassDataModel selectedPass)
            {
                //PassDataModel selected = InventorySystem.I.GetPass(m_SelectedItem.token_id);
                //if (null != selected)
                //{
                //    LoadPassData(selected);
                //}
                LoadPassData(selectedPass);
            }
            else if (m_SelectedItem is SpecialWeaponDataModel selectedSpecialWeapon)
            {
                //SpecialWeaponDataModel selected = InventorySystem.I.GetSpecialWeapon(m_SelectedItem.token_id);
                //if (null != selected)
                //{
                //    LoadWeaponData(selected);
                //}
                LoadWeaponData(selectedSpecialWeapon);
            }
            m_ButtonActivate.gameObject.SetActive(!m_SelectedItem.is_active);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnOpenButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);
            m_PanelOpenChest.SetActive(true);
            EventLogManager.I.SimpleEvent("open_chest_click_in_game");
        }
        /// <summary>
        /// Confirm to open chest
        /// </summary>
        private void OnConfirmButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxOk);
            //EventLogManager.I.SimpleEvent("open_chest_confirm_click_in_game");
            StartCoroutine(CallApiOpenBox());
        }
        /// <summary>
        /// Call api and process the result
        /// </summary>
        /// <returns></returns>
        private IEnumerator CallApiOpenBox()
        {
            yield return null;
            int randType = UnityEngine.Random.Range(0, 4);
            switch (randType)
            {
                case 0: //pilot
                    int randPilot = UnityEngine.Random.Range(0, 5);
                    PilotMasterData pilotMaster = DatabaseSystem.I.GetMetadata<PilotMaster>().pilot_master[randPilot];
                    PilotDataModel pilot = new PilotDataModel();
                    pilot.token_id = Guid.NewGuid().ToString();
                    pilot.name = pilotMaster.name;
                    pilot.level = 1;
                    pilot.SyncMasterData();
                    InventorySystem.I.AddPilot(pilot);
                    LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  { pilot.name}", urlImage: pilot.GetProfileImagePath());
                    break;

                case 1: //spaceship
                    int randSpaceship = UnityEngine.Random.Range(0, 5);
                    SpaceshipMasterData spaceshipMaster = DatabaseSystem.I.GetMetadata<SpaceshipMaster>().spaceship_master[randSpaceship];
                    SpaceshipDataModel spaceship = new SpaceshipDataModel();
                    spaceship.token_id = Guid.NewGuid().ToString();
                    spaceship.name = spaceshipMaster.name;
                    spaceship.level = 1;
                    spaceship.SyncMasterData();
                    InventorySystem.I.AddSpaceship(spaceship);
                    LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {spaceship.name}", urlImage: spaceship.GetImagePath());
                    break;

                case 2: //pass
                    PassMasterData passMaster = DatabaseSystem.I.GetMetadata<PassMaster>().pass_master[1];
                    PassDataModel pass = new PassDataModel();
                    pass.token_id = Guid.NewGuid().ToString();
                    pass.name = passMaster.name;
                    pass.SyncMasterData();
                    InventorySystem.I.AddPass(pass);
                    LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {pass.name}", urlImage: pass.GetImagePath());
                    break;

                case 3: //weapon
                    int randWeapon = UnityEngine.Random.Range(0, 3);
                    randWeapon = randWeapon + randWeapon + 1;
                    SpecialWeaponMasterData specialWeaponMaster = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master[randWeapon];
                    SpecialWeaponDataModel specialWeapon = new SpecialWeaponDataModel();
                    specialWeapon.token_id = Guid.NewGuid().ToString();
                    specialWeapon.name = specialWeaponMaster.name;
                    specialWeapon.SyncMasterData();
                    InventorySystem.I.AddSpecialWeapon(specialWeapon);
                    LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {specialWeapon.name}", urlImage: specialWeapon.GetImagePath());
                    break;
            }
            m_PanelOpenChest.SetActive(false);
            InventorySystem.I.RemoveChest(m_SelectedItem.token_id);
            if (m_SelectedItem.is_active)
                OnForceUpdate?.Invoke();
            ClearItemList();
            UpdateInventoryList();

            //    CoroutineWithData api = new CoroutineWithData(this, ApiService.I.OpenChest(m_SelectedItem));
            //    yield return api.coroutine;
            //    OpenChestResponseModel response = (OpenChestResponseModel)api.result;
            //    LayerSystem.I.HideSoftLoadingScreen();
            //    m_PanelOpenChest.SetActive(false);
            //    if (response.success)
            //    {
            //        string nftType = string.Empty;
            //        string nftName = string.Empty;
            //        InventorySystem.I.AddInventoryList(response.data);
            //        InventorySystem.I.RemoveChest(m_SelectedItem.token_id);
            //        if (m_SelectedItem.is_active)
            //            OnForceUpdate?.Invoke();
            //        ClearItemList();
            //        UpdateInventoryList();
            //        EventLogManager.I.OpenChestComplete(response.data);
            //        if (0 < response.data.pilot.Count)
            //        {
            //            LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  { response.data.pilot[0].name}", urlImage: response.data.pilot[0].GetProfileImagePath());
            //        }
            //        if (0 < response.data.spaceship.Count)
            //        {
            //            LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {response.data.spaceship[0].name}", urlImage: response.data.spaceship[0].GetImagePath());
            //        }
            //        if (0 < response.data.chest.Count)
            //        {
            //            LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {response.data.chest[0].name}", urlImage: response.data.chest[0].GetImagePath());
            //        }
            //        if (0 < response.data.survival_pass.Count)
            //        {
            //            LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {response.data.survival_pass[0].name}", urlImage: response.data.survival_pass[0].GetImagePath());
            //        }
            //        if (0 < response.data.special_weapon.Count)
            //        {
            //            LayerSystem.I.ShowPopupDialog($"{LocalizationSystem.I.GetLocalizeValue("POPUP_DIALOG_YOU_GOT_ITEM")}  {response.data.special_weapon[0].name}", urlImage: response.data.special_weapon[0].GetImagePath());
            //        }
            //    }
            //    else
            //    {
            //        LayerSystem.I.ShowPopupDialog(LocalizationSystem.I.GetErrorMassage($"{response.error.status}"));
            //    }
        }
        /// <summary>
        /// ButtonCancel clicked callback
        /// </summary>
        private void OnCancelButton()
        {
            if (null != m_Sfx)
                m_Sfx.PlayOneShot(m_SfxCancel);
            m_PanelOpenChest.SetActive(false);
            EventLogManager.I.SimpleEvent("open_chest_cancel_click_in_game");
        }
        /// <summary>
        /// ButtonMint clicked callback
        /// </summary>
        private void OnMintButton()
        {
            if (null != m_SelectedItem)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                m_PanelMintConfirmation.SetActive(true);
            }
            else
            {
                Debug.LogError("Invalid pilot uid");
            }
        }
        /// <summary>
        /// ButtonMintConfirm clicked callback
        /// </summary>
        private void OnConfirmMintButton()
        {
            //if (null != m_SelectedItem)
            //{
            //    if (null != m_Sfx)
            //        m_Sfx.PlayOneShot(m_SfxOk);
            //    m_PanelMintConfirmation.SetActive(false);
            //    StartCoroutine(MintingSystem.I.CallAPIMinting(m_SelectedItem));
            //}
            //else
            //{
            //    Debug.LogError("Invalid pilot uid");
            //}
        }
        /// <summary>
        /// ButtonMintCancel clicked callback
        /// </summary>
        private void OnCancleMintButton()
        {
            if (null != m_SelectedItem)
            {
                if (null != m_Sfx)
                    m_Sfx.PlayOneShot(m_SfxOk);
                m_PanelMintConfirmation.SetActive(false);
            }
            else
            {
                Debug.LogError("Invalid pilot uid");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void LoadChestData(ChestDataModel item)
        {
            m_ItemNameText.gameObject.SetActive(true);
            m_ShowSelectItemImage.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_FirstSpecialDescriptionText.gameObject.SetActive(true);
            m_SecondSpecialDescriptionText.gameObject.SetActive(true);

            ChestMasterData chestMaster = DatabaseSystem.I.GetMetadata<ChestMaster>().chest_master.Find(
                    a => a.name.Equals(item.name)
                );
            ChestMasterDisplayData chestMasterDisplay = DatabaseSystem.I.GetMetadata<ChestMasterDisplay>().chest_master_display.Find(
                    a => a.name.Equals(item.name)
                );
            m_ItemNameText.text = item.name;
            string imageUrl = $"{SystemConfig.BaseAssetPath}rendered/{chestMaster.image}";
            AssetCacheManager.I.SetTextureToRawImage(m_ShowSelectItemImage, imageUrl);

            if (item.rank.Equals("Silver"))
            {
                m_Bstar.SetActive(true);
                m_Astar.SetActive(true);
                m_Sstar.SetActive(false);
            }
            else if (item.rank.Equals("Gold"))
            {
                m_Bstar.SetActive(true);
                m_Astar.SetActive(true);
                m_Sstar.SetActive(true);
            }

            m_DescriptionText.text = chestMasterDisplay.description;
            if (item.rank.Equals("Gold"))
                m_FirstSpecialDescriptionText.text = " - ";
            else
                m_FirstSpecialDescriptionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_MISSION_TYPE")} : {chestMaster.mission_type}";
            m_SecondSpecialDescriptionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_MISSION_TARGET")} : {chestMaster.mission}";
            if (0 < chestMaster.mission)
            {
                m_ProgressBarTransform.SetActive(true);
                float value = item.missionprogress / (float)chestMaster.mission;
                m_ImageProgressBar.fillAmount = value;
                m_ThirdSpecialDescriotionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_MISSION_PROGRESS")} : {item.missionprogress}";
                m_TextProgress.text = $"{item.missionprogress} / {chestMaster.mission}";
            }
            else
            {
                m_ProgressBarTransform.SetActive(false);
            }
            m_ButtonOpen.gameObject.SetActive(true);
            m_NftIconOn.SetActive(item.is_nft);
            m_NftIconOff.SetActive(!item.is_nft);
            m_ActiveIconOn.SetActive(item.is_active);
            m_ActiveIconOff.SetActive(!item.is_active);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void LoadPassData(PassDataModel item)
        {
            m_ItemNameText.gameObject.SetActive(true);
            m_ShowSelectItemImage.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_FirstSpecialDescriptionText.gameObject.SetActive(true);
            m_SecondSpecialDescriptionText.gameObject.SetActive(true);

            PassMasterData passMaster = DatabaseSystem.I.GetMetadata<PassMaster>().pass_master.Find(
                    a => a.name.Equals(item.name)
                );
            PassMasterDisplayData passMasterDisplay = DatabaseSystem.I.GetMetadata<PassMasterDisplay>().pass_master_display.Find(
                    a => a.name.Equals(item.name)
                );
            m_ItemNameText.text = item.name;
            string imageUrl = $"{SystemConfig.BaseAssetPath}rendered/{passMaster.image}";
            AssetCacheManager.I.SetTextureToRawImage(m_ShowSelectItemImage, imageUrl);

            m_Bstar.SetActive(true);
            m_Astar.SetActive(true);
            m_Sstar.SetActive(true);

            m_DescriptionText.text = passMasterDisplay.description;
            m_FirstSpecialDescriptionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SREEN_BEST_RECORD")} : {item.bestrecord}";
            m_SecondSpecialDescriptionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_MAX_PLAY")} : {item.maxplay}";
            if (0 < passMaster.max_play)
            {
                m_ProgressBarTransform.SetActive(true);
                float value = item.playcount / (float)passMaster.max_play;
                m_ImageProgressBar.fillAmount = value;
                m_ThirdSpecialDescriotionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_PLAY_COUNT")} : {item.playcount}";
                m_TextProgress.text = $"{item.playcount} / {passMaster.max_play}";
            }
            else
            {
                m_ProgressBarTransform.SetActive(false);
            }
            m_ButtonOpen.gameObject.SetActive(false);
            m_NftIconOn.SetActive(item.is_nft);
            m_NftIconOff.SetActive(!item.is_nft);
            m_ActiveIconOn.SetActive(item.is_active);
            m_ActiveIconOff.SetActive(!item.is_active);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void LoadWeaponData(SpecialWeaponDataModel item)
        {
            m_ItemNameText.gameObject.SetActive(true);
            m_ShowSelectItemImage.gameObject.SetActive(true);
            m_DescriptionText.gameObject.SetActive(true);
            m_FirstSpecialDescriptionText.gameObject.SetActive(true);
            m_SecondSpecialDescriptionText.gameObject.SetActive(true);

            SpecialWeaponMasterData weaponMaster = DatabaseSystem.I.GetMetadata<SpecialWeaponMaster>().weapon_master.Find(
                    a => a.name.Equals(item.name)
                );
            SpecialWeaponMasterDisplayData weaponMasterDisplay = DatabaseSystem.I.GetMetadata<SpecialWeaponMasterDisplay>().weapon_master_display.Find(
                    a => a.name.Equals(item.name)
                );
            m_ItemNameText.text = item.name;
            string imageUrl = $"{SystemConfig.BaseAssetPath}rendered/{weaponMaster.image}";
            AssetCacheManager.I.SetTextureToRawImage(m_ShowSelectItemImage, imageUrl);

            if (item.rank.Equals("Special"))
            {
                m_Bstar.SetActive(true);
                m_Astar.SetActive(true);
                m_Sstar.SetActive(false);
            }
            else if (item.rank.Equals("God"))
            {
                m_Bstar.SetActive(true);
                m_Astar.SetActive(true);
                m_Sstar.SetActive(true);
            }

            m_DescriptionText.text = weaponMasterDisplay.description;
            m_FirstSpecialDescriptionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_MISSION_TYPE")} : { item.evotype}";
            m_SecondSpecialDescriptionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_FIRE_DAMAGE")} : {item.damage}";

            if (0 < weaponMaster.evo_mission)
            {
                m_ProgressBarTransform.SetActive(true);
                float value = item.missionprogress / (float)weaponMaster.evo_mission;
                m_ImageProgressBar.fillAmount = value;
                m_ThirdSpecialDescriotionText.text = $"{LocalizationSystem.I.GetLocalizeValue("INVENTORY_SCREEN_MISSION_PROGRESS")} : {weaponMaster.evo_type} {item.missionprogress}/{weaponMaster.evo_mission}";
                m_TextProgress.text = $"{item.missionprogress} / {weaponMaster.evo_mission}";
            }
            else
            {
                m_ProgressBarTransform.SetActive(false);
            }
            m_ButtonOpen.gameObject.SetActive(false);
            m_NftIconOn.SetActive(item.is_nft);
            m_NftIconOff.SetActive(!item.is_nft);
            m_ActiveIconOn.SetActive(item.is_active);
            m_ActiveIconOff.SetActive(!item.is_active);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnCloseButton()
        {
            ClearItemList();
            OnClosed?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnNftIconOnButton()
        {
            TooltipMasterData master = DatabaseSystem.I.GetMetadata<TooltipMaster>().tooltips.Find(
                a => a.key.Equals("NFT_ON"));
            LayerSystem.I.ShowPopupDialog(master.description);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnActiveIconOnButton()
        {
            TooltipMasterData master = DatabaseSystem.I.GetMetadata<TooltipMaster>().tooltips.Find(
                a => a.key.Equals("ACTIVE_ON"));
            LayerSystem.I.ShowPopupDialog(master.description);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnNftIconOffButton()
        {
            TooltipMasterData master = DatabaseSystem.I.GetMetadata<TooltipMaster>().tooltips.Find(
                a => a.key.Equals("NFT_OFF"));
            LayerSystem.I.ShowPopupDialog(master.description);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnActiveIconOffButton()
        {
            TooltipMasterData master = DatabaseSystem.I.GetMetadata<TooltipMaster>().tooltips.Find(
                a => a.key.Equals("ACTIVE_OFF"));
            LayerSystem.I.ShowPopupDialog(master.description);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnBoundIconOffButton()
        {
            TooltipMasterData master = DatabaseSystem.I.GetMetadata<TooltipMaster>().tooltips.Find(
                a => a.key.Equals("BOUND_OFF"));
            LayerSystem.I.ShowPopupDialog(master.description);
        }
        /// <summary>
        /// Clear all slot in inventory
        /// </summary>
        private void ClearItemList()
        {
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                m_ItemList[i].Reset();
                ObjectPoolManager.I.ReturnToPool(m_ItemList[i], ObjectType.UI);
            }
            m_ItemList.Clear();
        }
        /// <summary>
        /// Delete layer
        /// </summary>
        public override void Delete()
        {
            InventorySystem.I.OnSoftCurrency1Changed -= OnSoftCurrency1Changed;
            InventorySystem.I.OnItemActivated -= OnItemActivated;
            base.Delete();
        }

        #region Event Listener
        /// <summary>
        /// Subscribe to InventorySystem event
        /// </summary>
        /// <param name="balance"></param>
        private void OnSoftCurrency1Changed(uint balance)
        {
            m_TextGold.text = $"{balance:###,###,##0}";
        }
        /// <summary>
        /// Subscribe to InventorySystem event
        /// </summary>
        /// <param name="tokenId"></param>
        private void OnItemActivated(string tokenId, NFTCollectionType type)
        {
            if (null != m_SelectedItem)
                m_ActiveIconOn.SetActive(m_SelectedItem.token_id == tokenId);
        }
        /// <summary>
        /// Subscribe to InventorySystem event item update
        /// </summary>
        /// <param name="tokenId"></param>
        private void OnItemUpdate(NftMetadataModel nft)
        {
            if (nft.GetType() == m_SelectedItem.GetType() && nft.token_id == m_SelectedItem.token_id)
                OnItemSelect(m_SelectedItem.token_id, m_SelectedType);
        }
        /// <summary>
        /// Subscribe to Inventorysystem
        /// </summary>
        private void OnInventoryRefresh()
        {
            ClearItemList();
            UpdateInventoryList();
        }
        #endregion
    }
}
